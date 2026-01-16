using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private readonly List<UIElement> _visualElements;
        private readonly Path _previewPath;
        private bool _isDraggingHandle = false;
        private bool _isCreatingNode = false;
        private Point _dragStartPoint;
        private Ellipse? _dragHandleVisual;
        private Line? _handleLine1;
        private Line? _handleLine2;

        public BezierTool(InkCanvas inkCanvas)
        {
            _inkCanvas = inkCanvas;
            _visualElements = new List<UIElement>();
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

            // Start creating a new node
            _isCreatingNode = true;
            _dragStartPoint = position;
            
            // Add new node at click position
            var node = new BezierNode(position, NodeType.Smooth);
            _currentPath.Nodes.Add(node);
            
            // Create visual representation
            AddNodeVisual(position);
        }

        public void OnMouseMove(Point position)
        {
            if (_isCreatingNode && _currentPath != null && _currentPath.Nodes.Count > 0)
            {
                // Calculate control handle position based on drag
                var lastNode = _currentPath.Nodes[_currentPath.Nodes.Count - 1];
                Vector handleVector = position - _dragStartPoint;
                
                // Set control points symmetrically around the anchor point
                lastNode.ControlPoint1 = _dragStartPoint - handleVector;
                lastNode.ControlPoint2 = _dragStartPoint + handleVector;
                
                // Update visual handles
                UpdateHandleVisuals(lastNode);
                UpdatePreviewPath();
            }
        }

        public void OnMouseUp(Point position)
        {
            if (_isCreatingNode)
            {
                _isCreatingNode = false;
                
                // Finalize the control points
                if (_currentPath != null && _currentPath.Nodes.Count > 0)
                {
                    var lastNode = _currentPath.Nodes[_currentPath.Nodes.Count - 1];
                    Vector handleVector = position - _dragStartPoint;
                    
                    // Only set control points if there was actual dragging
                    if (handleVector.Length > 5)
                    {
                        lastNode.ControlPoint1 = _dragStartPoint - handleVector;
                        lastNode.ControlPoint2 = _dragStartPoint + handleVector;
                        UpdateHandleVisuals(lastNode);
                    }
                    else
                    {
                        // Small drag or click - create straight segment
                        lastNode.ControlPoint1 = null;
                        lastNode.ControlPoint2 = null;
                    }
                    
                    UpdatePreviewPath();
                }
            }
        }

        private void AddNodeVisual(Point position)
        {
            var ellipse = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Red,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };
            
            Canvas.SetLeft(ellipse, position.X - 5);
            Canvas.SetTop(ellipse, position.Y - 5);
            
            _inkCanvas.Children.Add(ellipse);
            _visualElements.Add(ellipse);
        }

        private void UpdateHandleVisuals(BezierNode node)
        {
            // Remove old handle visuals
            if (_handleLine1 != null)
            {
                _inkCanvas.Children.Remove(_handleLine1);
                _visualElements.Remove(_handleLine1);
            }
            if (_handleLine2 != null)
            {
                _inkCanvas.Children.Remove(_handleLine2);
                _visualElements.Remove(_handleLine2);
            }
            if (_dragHandleVisual != null)
            {
                _inkCanvas.Children.Remove(_dragHandleVisual);
                _visualElements.Remove(_dragHandleVisual);
            }

            // Add new handle visuals if control points exist
            if (node.ControlPoint1.HasValue)
            {
                _handleLine1 = new Line
                {
                    X1 = node.Position.X,
                    Y1 = node.Position.Y,
                    X2 = node.ControlPoint1.Value.X,
                    Y2 = node.ControlPoint1.Value.Y,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                _inkCanvas.Children.Add(_handleLine1);
                _visualElements.Add(_handleLine1);

                var handle1 = new Ellipse
                {
                    Width = 6,
                    Height = 6,
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(handle1, node.ControlPoint1.Value.X - 3);
                Canvas.SetTop(handle1, node.ControlPoint1.Value.Y - 3);
                _inkCanvas.Children.Add(handle1);
                _visualElements.Add(handle1);
            }

            if (node.ControlPoint2.HasValue)
            {
                _handleLine2 = new Line
                {
                    X1 = node.Position.X,
                    Y1 = node.Position.Y,
                    X2 = node.ControlPoint2.Value.X,
                    Y2 = node.ControlPoint2.Value.Y,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                _inkCanvas.Children.Add(_handleLine2);
                _visualElements.Add(_handleLine2);

                var handle2 = new Ellipse
                {
                    Width = 6,
                    Height = 6,
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(handle2, node.ControlPoint2.Value.X - 3);
                Canvas.SetTop(handle2, node.ControlPoint2.Value.Y - 3);
                _inkCanvas.Children.Add(handle2);
                _visualElements.Add(handle2);
            }
        }

        private void UpdatePreviewPath()
        {
            if (_currentPath == null || _currentPath.Nodes.Count < 1)
                return;

            var geometry = new PathGeometry();
            var figure = new PathFigure
            {
                StartPoint = _currentPath.Nodes[0].Position
            };

            for (int i = 1; i < _currentPath.Nodes.Count; i++)
            {
                var prevNode = _currentPath.Nodes[i - 1];
                var node = _currentPath.Nodes[i];
                
                // Use Bezier curve if control points exist
                if (prevNode.ControlPoint2.HasValue && node.ControlPoint1.HasValue)
                {
                    figure.Segments.Add(new BezierSegment(
                        prevNode.ControlPoint2.Value,
                        node.ControlPoint1.Value,
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
            _currentPath = new BezierPath();
        }

        public void ClearVisuals()
        {
            foreach (var visual in _visualElements)
            {
                _inkCanvas.Children.Remove(visual);
            }
            _visualElements.Clear();
            
            if (_inkCanvas.Children.Contains(_previewPath))
            {
                _inkCanvas.Children.Remove(_previewPath);
            }
            
            _handleLine1 = null;
            _handleLine2 = null;
            _dragHandleVisual = null;
        }
    }
}
