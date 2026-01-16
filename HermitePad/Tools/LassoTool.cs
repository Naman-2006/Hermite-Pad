using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Linq;

namespace HermitePad.Tools
{
    public class LassoTool
    {
        private readonly InkCanvas _inkCanvas;

        public LassoTool(InkCanvas inkCanvas)
        {
            _inkCanvas = inkCanvas;
        }

        public StrokeCollection GetSelectedStrokes()
        {
            return _inkCanvas.GetSelectedStrokes();
        }

        public void SelectStrokesByArea(Rect area)
        {
            var strokesToSelect = new StrokeCollection();
            
            foreach (Stroke stroke in _inkCanvas.Strokes)
            {
                if (area.IntersectsWith(stroke.GetBounds()))
                {
                    strokesToSelect.Add(stroke);
                }
            }
            
            _inkCanvas.Select(strokesToSelect);
        }

        public void ClearSelection()
        {
            _inkCanvas.Select(new StrokeCollection());
        }
    }
}
