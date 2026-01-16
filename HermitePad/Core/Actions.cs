using System.Windows.Controls;
using System.Windows.Ink;

namespace HermitePad.Core
{
    public class AddStrokeAction : IAction
    {
        private readonly InkCanvas _canvas;
        private readonly Stroke _stroke;

        public AddStrokeAction(InkCanvas canvas, Stroke stroke)
        {
            _canvas = canvas;
            _stroke = stroke;
        }

        public void Undo()
        {
            _canvas.Strokes.Remove(_stroke);
        }

        public void Redo()
        {
            _canvas.Strokes.Add(_stroke);
        }
    }

    public class RemoveStrokeAction : IAction
    {
        private readonly InkCanvas _canvas;
        private readonly Stroke _stroke;

        public RemoveStrokeAction(InkCanvas canvas, Stroke stroke)
        {
            _canvas = canvas;
            _stroke = stroke;
        }

        public void Undo()
        {
            _canvas.Strokes.Add(_stroke);
        }

        public void Redo()
        {
            _canvas.Strokes.Remove(_stroke);
        }
    }

    public class ClearCanvasAction : IAction
    {
        private readonly InkCanvas _canvas;
        private readonly StrokeCollection _oldStrokes;

        public ClearCanvasAction(InkCanvas canvas, StrokeCollection oldStrokes)
        {
            _canvas = canvas;
            _oldStrokes = oldStrokes;
        }

        public void Undo()
        {
            _canvas.Strokes.Add(_oldStrokes);
        }

        public void Redo()
        {
            _canvas.Strokes.Clear();
        }
    }

    public class ConvertToMathAction : IAction
    {
        private readonly InkCanvas _canvas;
        private readonly StrokeCollection _strokes;
        private readonly MathObject _mathObject;

        public ConvertToMathAction(InkCanvas canvas, StrokeCollection strokes, MathObject mathObject)
        {
            _canvas = canvas;
            _strokes = strokes;
            _mathObject = mathObject;
        }

        public void Undo()
        {
            _canvas.Strokes.Add(_strokes);
            // Remove math object from canvas manager (would need reference)
        }

        public void Redo()
        {
            _canvas.Strokes.Remove(_strokes);
            // Add math object back to canvas manager
        }
    }
}
