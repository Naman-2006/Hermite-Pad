using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using HermitePad.Core;

namespace HermitePad.Tools
{
    public class BezierTool
    {
        private readonly InkCanvas _inkCanvas;
        private BezierPath? _currentPath;
        private BezierNode? _selectedNode;
        private readonly List<UIElement> _visualNodes;
        private readonly Path _previewPath;
        private bool _isDraggingHandle = false;

        public BezierTool(InkCanvas inkCanvas)
        {
            _inkCanvas = inkCanvas;
            _visualNodes = new List<UIElement>();
            _currentPath = new BezierPath();
            
            // Create preview path
            _previewPath = new Path
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection { 5, 3 }
            };
        }

        public void OnMouseDown(Point position)
        {
            if (_currentPath == null)
            {
                _currentPath = new BezierPath();
            }

            // Add new node
            var node = new BezierNode(position, NodeType.Smooth);
            _currentPath.Nodes.Add(node);
            
            // Create visual representation
            AddNodeVisual(position);
            UpdatePreviewPath();
        }

        public void OnMouseMove(Point position)
        {
            if (_currentPath != null && _currentPath.Nodes.Count > 0)
            {
                // Update preview for next segment
                UpdatePreviewPath();
            }
        }

        public void OnMouseUp(Point position)
        {
            // Finalize current segment
        }

        private void AddNodeVisual(Point position)
        {
            var ellipse = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = Brushes.Red,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            
            Canvas.SetLeft(ellipse, position.X - 4);
            Canvas.SetTop(ellipse, position.Y - 4);
            
            _inkCanvas.Children.Add(ellipse);
            _visualNodes.Add(ellipse);
        }

        private void UpdatePreviewPath()
        {
            if (_currentPath == null || _currentPath.Nodes.Count < 2)
                return;

            var geometry = new PathGeometry();
            var figure = new PathFigure
            {
                StartPoint = _currentPath.Nodes[0].Position
            };

            for (int i = 1; i < _currentPath.Nodes.Count; i++)
            {
                var node = _currentPath.Nodes[i];
                
                if (node.ControlPoint1.HasValue && node.ControlPoint2.HasValue)
                {
                    // Bezier curve segment
                    figure.Segments.Add(new BezierSegment(
                        node.ControlPoint1.Value,
                        node.ControlPoint2.Value,
                        node.Position,
                        true));
                }
                else
                {
                    // Straight line segment
                    figure.Segments.Add(new LineSegment(node.Position, true));
                }
            }

            geometry.Figures.Add(figure);
            _previewPath.Data = geometry;
            
            if (!_inkCanvas.Children.Contains(_previewPath))
            {
                _inkCanvas.Children.Add(_previewPath);
            }
        }

        public void AddControlPoint(Point position, BezierNode node, bool isFirst)
        {
            if (isFirst)
            {
                node.ControlPoint1 = position;
            }
            else
            {
                node.ControlPoint2 = position;
            }
            UpdatePreviewPath();
        }

        public void SetNodeType(BezierNode node, NodeType type)
        {
            node.Type = type;
            UpdatePreviewPath();
        }

        public BezierPath? GetCurrentPath()
        {
            return _currentPath;
        }

        public void FinishPath()
        {
            _currentPath = null;
            _visualNodes.Clear();
        }

        public void ClearVisuals()
        {
            foreach (var visual in _visualNodes)
            {
                _inkCanvas.Children.Remove(visual);
            }
            _visualNodes.Clear();
            
            if (_inkCanvas.Children.Contains(_previewPath))
            {
                _inkCanvas.Children.Remove(_previewPath);
            }
        }
    }
}
