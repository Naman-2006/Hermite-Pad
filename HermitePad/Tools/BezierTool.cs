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
        private bool _isCreatingNode = false;
        private Point _dragStartPoint;
        private Line? _handleLine1;
        private Line? _handleLine2;
        private Ellipse? _handle1Visual;
        private Ellipse? _handle2Visual;

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
                
                // Update visual handles efficiently (reuse existing visuals)
                UpdateHandleVisualsEfficiently(lastNode);
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
                        UpdateHandleVisualsEfficiently(lastNode);
                    }
                    else
                    {
                        // Small drag or click - create straight segment
                        lastNode.ControlPoint1 = null;
                        lastNode.ControlPoint2 = null;
                        ClearHandleVisuals();
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

        private void UpdateHandleVisualsEfficiently(BezierNode node)
        {
            // Update existing visuals instead of recreating them
            if (node.ControlPoint1.HasValue)
            {
                if (_handleLine1 == null)
                {
                    _handleLine1 = new Line
                    {
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1
                    };
                    _inkCanvas.Children.Add(_handleLine1);
                    _visualElements.Add(_handleLine1);
                }
                
                _handleLine1.X1 = node.Position.X;
                _handleLine1.Y1 = node.Position.Y;
                _handleLine1.X2 = node.ControlPoint1.Value.X;
                _handleLine1.Y2 = node.ControlPoint1.Value.Y;

                if (_handle1Visual == null)
                {
                    _handle1Visual = new Ellipse
                    {
                        Width = 6,
                        Height = 6,
                        Fill = Brushes.LightBlue,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1
                    };
                    _inkCanvas.Children.Add(_handle1Visual);
                    _visualElements.Add(_handle1Visual);
                }
                
                Canvas.SetLeft(_handle1Visual, node.ControlPoint1.Value.X - 3);
                Canvas.SetTop(_handle1Visual, node.ControlPoint1.Value.Y - 3);
            }

            if (node.ControlPoint2.HasValue)
            {
                if (_handleLine2 == null)
                {
                    _handleLine2 = new Line
                    {
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1
                    };
                    _inkCanvas.Children.Add(_handleLine2);
                    _visualElements.Add(_handleLine2);
                }
                
                _handleLine2.X1 = node.Position.X;
                _handleLine2.Y1 = node.Position.Y;
                _handleLine2.X2 = node.ControlPoint2.Value.X;
                _handleLine2.Y2 = node.ControlPoint2.Value.Y;

                if (_handle2Visual == null)
                {
                    _handle2Visual = new Ellipse
                    {
                        Width = 6,
                        Height = 6,
                        Fill = Brushes.LightBlue,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1
                    };
                    _inkCanvas.Children.Add(_handle2Visual);
                    _visualElements.Add(_handle2Visual);
                }
                
                Canvas.SetLeft(_handle2Visual, node.ControlPoint2.Value.X - 3);
                Canvas.SetTop(_handle2Visual, node.ControlPoint2.Value.Y - 3);
            }
        }

        private void ClearHandleVisuals()
        {
            if (_handleLine1 != null)
            {
                _inkCanvas.Children.Remove(_handleLine1);
                _visualElements.Remove(_handleLine1);
                _handleLine1 = null;
            }
            if (_handleLine2 != null)
            {
                _inkCanvas.Children.Remove(_handleLine2);
                _visualElements.Remove(_handleLine2);
                _handleLine2 = null;
            }
            if (_handle1Visual != null)
            {
                _inkCanvas.Children.Remove(_handle1Visual);
                _visualElements.Remove(_handle1Visual);
                _handle1Visual = null;
            }
            if (_handle2Visual != null)
            {
                _inkCanvas.Children.Remove(_handle2Visual);
                _visualElements.Remove(_handle2Visual);
                _handle2Visual = null;
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
            ClearHandleVisuals();
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
            
            ClearHandleVisuals();
        }
    }
}
