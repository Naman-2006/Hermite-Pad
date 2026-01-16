using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using HermitePad.Core;
using HermitePad.Tools;

namespace HermitePad
{
    public partial class MainWindow : Window
    {
        private readonly CanvasManager _canvasManager;
        private readonly UndoRedoManager _undoRedoManager;
        private ToolMode _currentTool = ToolMode.Ink;
        private BezierTool? _bezierTool;
        private LassoTool? _lassoTool;
        
        public MainWindow()
        {
            InitializeComponent();
            
            _canvasManager = new CanvasManager(MainInkCanvas);
            _undoRedoManager = new UndoRedoManager();
            
            InitializeInkCanvas();
            SetupEventHandlers();
            UpdateToolUI();
        }

        private void InitializeInkCanvas()
        {
            // Enable pressure sensitivity
            MainInkCanvas.UseCustomCursor = true;
            
            // Set up default drawing attributes
            var drawingAttributes = new DrawingAttributes
            {
                Color = Colors.Black,
                Width = 2,
                Height = 2,
                FitToCurve = true,
                IgnorePressure = false,
                StylusTip = StylusTip.Ellipse
            };
            
            MainInkCanvas.DefaultDrawingAttributes = drawingAttributes;
            MainInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void SetupEventHandlers()
        {
            MainInkCanvas.StrokeCollected += OnStrokeCollected;
            MainInkCanvas.StrokeErasing += OnStrokeErasing;
            MainInkCanvas.SelectionChanged += OnSelectionChanged;
            
            // Zoom and pan
            MainInkCanvas.MouseWheel += OnMouseWheel;
            MainInkCanvas.MouseMove += OnMouseMove;
            MainInkCanvas.MouseLeftButtonDown += OnMouseLeftButtonDown;
            MainInkCanvas.MouseLeftButtonUp += OnMouseLeftButtonUp;
            
            // Keyboard shortcuts
            this.KeyDown += OnKeyDown;
        }

        private void OnStrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            _undoRedoManager.AddAction(new AddStrokeAction(MainInkCanvas, e.Stroke));
        }

        private void OnStrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {
            _undoRedoManager.AddAction(new RemoveStrokeAction(MainInkCanvas, e.Stroke));
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (_currentTool == ToolMode.Lasso && MainInkCanvas.GetSelectedStrokes().Count > 0)
            {
                UpdateSelectionInfo();
            }
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                // Zoom
                double zoomFactor = e.Delta > 0 ? 1.1 : 0.9;
                _canvasManager.Zoom(zoomFactor, e.GetPosition(MainInkCanvas));
                e.Handled = true;
            }
        }

        private bool _isPanning = false;
        private Point _lastPanPoint;

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Space || _currentTool == ToolMode.Pan)
            {
                _isPanning = true;
                _lastPanPoint = e.GetPosition(this);
                MainInkCanvas.CaptureMouse();
                e.Handled = true;
            }
            else if (_currentTool == ToolMode.Bezier)
            {
                _bezierTool?.OnMouseDown(e.GetPosition(MainInkCanvas));
                e.Handled = true;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(this);
                Vector delta = currentPoint - _lastPanPoint;
                _canvasManager.Pan(delta);
                _lastPanPoint = currentPoint;
            }
            else if (_currentTool == ToolMode.Bezier)
            {
                _bezierTool?.OnMouseMove(e.GetPosition(MainInkCanvas));
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isPanning)
            {
                _isPanning = false;
                MainInkCanvas.ReleaseMouseCapture();
            }
            else if (_currentTool == ToolMode.Bezier)
            {
                _bezierTool?.OnMouseUp(e.GetPosition(MainInkCanvas));
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.Z:
                        Undo();
                        e.Handled = true;
                        break;
                    case Key.Y:
                        Redo();
                        e.Handled = true;
                        break;
                    case Key.S:
                        SaveDocument();
                        e.Handled = true;
                        break;
                    case Key.O:
                        OpenDocument();
                        e.Handled = true;
                        break;
                }
            }
        }

        private void UpdateSelectionInfo()
        {
            var selectedStrokes = MainInkCanvas.GetSelectedStrokes();
            SelectionInfoText.Text = $"Selected: {selectedStrokes.Count} stroke(s)";
        }

        private void UpdateToolUI()
        {
            // Reset all button states
            InkButton.FontWeight = FontWeights.Normal;
            EraserButton.FontWeight = FontWeights.Normal;
            LassoButton.FontWeight = FontWeights.Normal;
            BezierButton.FontWeight = FontWeights.Normal;
            
            // Highlight current tool
            switch (_currentTool)
            {
                case ToolMode.Ink:
                    InkButton.FontWeight = FontWeights.Bold;
                    MainInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    break;
                case ToolMode.Eraser:
                    EraserButton.FontWeight = FontWeights.Bold;
                    MainInkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;
                case ToolMode.Lasso:
                    LassoButton.FontWeight = FontWeights.Bold;
                    MainInkCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;
                case ToolMode.Bezier:
                    BezierButton.FontWeight = FontWeights.Bold;
                    MainInkCanvas.EditingMode = InkCanvasEditingMode.None;
                    _bezierTool = new BezierTool(MainInkCanvas);
                    break;
            }
            
            ToolStatusText.Text = $"Tool: {_currentTool}";
        }

        // Tool button handlers
        private void InkButton_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = ToolMode.Ink;
            UpdateToolUI();
        }

        private void EraserButton_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = ToolMode.Eraser;
            UpdateToolUI();
        }

        private void LassoButton_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = ToolMode.Lasso;
            _lassoTool = new LassoTool(MainInkCanvas);
            UpdateToolUI();
        }

        private void BezierButton_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = ToolMode.Bezier;
            UpdateToolUI();
        }

        private void ConvertToMathButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStrokes = MainInkCanvas.GetSelectedStrokes();
            if (selectedStrokes.Count > 0)
            {
                var mathConverter = new MathConverter();
                var result = mathConverter.ConvertToMath(selectedStrokes);
                
                if (result != null)
                {
                    _canvasManager.AddMathObject(result);
                    MainInkCanvas.Strokes.Remove(selectedStrokes);
                    _undoRedoManager.AddAction(new ConvertToMathAction(MainInkCanvas, selectedStrokes, result));
                    
                    MessageBox.Show($"Converted to math:\nLaTeX: {result.LaTeX}\nMathML: {result.MathML}", 
                        "Math Conversion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Clear all content?", "Clear Canvas", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                var oldStrokes = MainInkCanvas.Strokes.Clone();
                MainInkCanvas.Strokes.Clear();
                _canvasManager.ClearAll();
                _undoRedoManager.AddAction(new ClearCanvasAction(MainInkCanvas, oldStrokes));
            }
        }

        private void SaveDocument()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Hermite Pad Document (*.hpd)|*.hpd|All files (*.*)|*.*",
                DefaultExt = ".hpd"
            };

            if (dialog.ShowDialog() == true)
            {
                var document = new Document
                {
                    Strokes = MainInkCanvas.Strokes,
                    MathObjects = _canvasManager.GetMathObjects(),
                    BezierPaths = _canvasManager.GetBezierPaths()
                };
                
                DocumentSerializer.Save(document, dialog.FileName);
                StatusText.Text = $"Saved: {dialog.FileName}";
            }
        }

        private void OpenDocument()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Hermite Pad Document (*.hpd)|*.hpd|All files (*.*)|*.*",
                DefaultExt = ".hpd"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var document = DocumentSerializer.Load(dialog.FileName);
                    MainInkCanvas.Strokes = document.Strokes;
                    _canvasManager.LoadMathObjects(document.MathObjects);
                    _canvasManager.LoadBezierPaths(document.BezierPaths);
                    StatusText.Text = $"Opened: {dialog.FileName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveDocument();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenDocument();
        }

        private void ExportPdfButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF Document (*.pdf)|*.pdf",
                DefaultExt = ".pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var exporter = new PdfExporter();
                    exporter.Export(MainInkCanvas, _canvasManager, dialog.FileName);
                    StatusText.Text = $"Exported to PDF: {dialog.FileName}";
                    MessageBox.Show("PDF exported successfully!", "Export Complete", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting PDF: {ex.Message}", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            Undo();
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            Redo();
        }

        private void Undo()
        {
            if (_undoRedoManager.CanUndo)
            {
                _undoRedoManager.Undo();
                StatusText.Text = "Undo";
            }
        }

        private void Redo()
        {
            if (_undoRedoManager.CanRedo)
            {
                _undoRedoManager.Redo();
                StatusText.Text = "Redo";
            }
        }

        private void LayersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LayersList.SelectedItem is Layer layer)
            {
                _canvasManager.SetActiveLayer(layer);
            }
        }

        private void AddLayerButton_Click(object sender, RoutedEventArgs e)
        {
            var layer = _canvasManager.AddLayer();
            LayersList.Items.Add(layer);
            LayersList.SelectedItem = layer;
        }

        private void RemoveLayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (LayersList.SelectedItem is Layer layer)
            {
                _canvasManager.RemoveLayer(layer);
                LayersList.Items.Remove(layer);
            }
        }
    }

    public enum ToolMode
    {
        Ink,
        Eraser,
        Lasso,
        Bezier,
        Pan
    }
}
