using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HermitePad.Core
{
    public class CanvasManager
    {
        private readonly InkCanvas _inkCanvas;
        private readonly List<Layer> _layers;
        private Layer _activeLayer;
        private readonly List<MathObject> _mathObjects;
        private readonly List<BezierPath> _bezierPaths;
        private ScaleTransform? _scaleTransform;
        private TranslateTransform? _translateTransform;

        public CanvasManager(InkCanvas inkCanvas)
        {
            _inkCanvas = inkCanvas;
            _layers = new List<Layer> { new Layer("Default Layer") };
            _activeLayer = _layers[0];
            _mathObjects = new List<MathObject>();
            _bezierPaths = new List<BezierPath>();
            
            InitializeTransforms();
        }

        private void InitializeTransforms()
        {
            _scaleTransform = new ScaleTransform(1.0, 1.0);
            _translateTransform = new TranslateTransform(0, 0);
            
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(_scaleTransform);
            transformGroup.Children.Add(_translateTransform);
            
            _inkCanvas.RenderTransform = transformGroup;
        }

        public void Zoom(double factor, Point center)
        {
            if (_scaleTransform == null || _translateTransform == null)
                return;
                
            double newScaleX = _scaleTransform.ScaleX * factor;
            double newScaleY = _scaleTransform.ScaleY * factor;
            
            // Limit zoom range
            if (newScaleX < 0.1 || newScaleX > 10.0) return;
            
            // Adjust translate to zoom toward center
            _translateTransform.X = center.X - (center.X - _translateTransform.X) * factor;
            _translateTransform.Y = center.Y - (center.Y - _translateTransform.Y) * factor;
            
            _scaleTransform.ScaleX = newScaleX;
            _scaleTransform.ScaleY = newScaleY;
        }

        public void Pan(Vector delta)
        {
            if (_translateTransform != null)
            {
                _translateTransform.X += delta.X;
                _translateTransform.Y += delta.Y;
            }
        }

        public Layer AddLayer()
        {
            var layer = new Layer($"Layer {_layers.Count + 1}");
            _layers.Add(layer);
            return layer;
        }

        public void RemoveLayer(Layer layer)
        {
            if (_layers.Count > 1)
            {
                _layers.Remove(layer);
                if (_activeLayer == layer)
                {
                    _activeLayer = _layers[0];
                }
            }
        }

        public void SetActiveLayer(Layer layer)
        {
            // Save current strokes to current layer
            if (_activeLayer != null && _inkCanvas.Strokes.Count > 0)
            {
                _activeLayer.Strokes.Clear();
                foreach (var stroke in _inkCanvas.Strokes)
                {
                    _activeLayer.Strokes.Add(stroke);
                }
            }
            
            _activeLayer = layer;
            
            // Load strokes from new active layer
            _inkCanvas.Strokes.Clear();
            if (layer.Strokes != null)
            {
                foreach (var stroke in layer.Strokes)
                {
                    _inkCanvas.Strokes.Add(stroke);
                }
            }
        }

        public Layer GetActiveLayer() => _activeLayer;

        public void AddMathObject(MathObject mathObject)
        {
            _mathObjects.Add(mathObject);
            _activeLayer.MathObjects.Add(mathObject);
        }

        public void AddBezierPath(BezierPath bezierPath)
        {
            _bezierPaths.Add(bezierPath);
            _activeLayer.BezierPaths.Add(bezierPath);
        }

        public List<MathObject> GetMathObjects() => _mathObjects;
        public List<BezierPath> GetBezierPaths() => _bezierPaths;

        public void LoadMathObjects(List<MathObject> mathObjects)
        {
            _mathObjects.Clear();
            _mathObjects.AddRange(mathObjects);
        }

        public void LoadBezierPaths(List<BezierPath> bezierPaths)
        {
            _bezierPaths.Clear();
            _bezierPaths.AddRange(bezierPaths);
        }

        public void ClearAll()
        {
            _mathObjects.Clear();
            _bezierPaths.Clear();
            foreach (var layer in _layers)
            {
                layer.Strokes.Clear();
                layer.MathObjects.Clear();
                layer.BezierPaths.Clear();
            }
        }
    }

    public class Layer
    {
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public System.Windows.Ink.StrokeCollection Strokes { get; set; }
        public List<MathObject> MathObjects { get; set; }
        public List<BezierPath> BezierPaths { get; set; }

        public Layer(string name)
        {
            Name = name;
            IsVisible = true;
            Strokes = new System.Windows.Ink.StrokeCollection();
            MathObjects = new List<MathObject>();
            BezierPaths = new List<BezierPath>();
        }

        public override string ToString() => Name;
    }

    public class MathObject
    {
        public string LaTeX { get; set; } = string.Empty;
        public string MathML { get; set; } = string.Empty;
        public Point Position { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class BezierPath
    {
        public List<BezierNode> Nodes { get; set; }
        public bool IsClosed { get; set; }
        public double StrokeThickness { get; set; }
        public Color StrokeColor { get; set; }

        public BezierPath()
        {
            Nodes = new List<BezierNode>();
            StrokeThickness = 2.0;
            StrokeColor = Colors.Black;
        }
    }

    public class BezierNode
    {
        public Point Position { get; set; }
        public Point? ControlPoint1 { get; set; }
        public Point? ControlPoint2 { get; set; }
        public NodeType Type { get; set; }

        public BezierNode(Point position, NodeType type = NodeType.Smooth)
        {
            Position = position;
            Type = type;
        }
    }

    public enum NodeType
    {
        Smooth,
        Corner
    }
}
