using TinyText.UndoActions;

namespace TinyText.Actions
{
    public class MoveRightAction : IAction
    {
        private readonly Document _document;

        public MoveRightAction(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            var lengthOfCurrentLine = _document.Output[_document.CursorLineNumber].Count;
            if (_document.CursorLineNumber == (_document.Output.Count - 1) && _document.CursorPosition == lengthOfCurrentLine)
            {
                // moving beyond the end of the document is not allowed.
                _document.UndoActions.Add(new UndoWithNoEffect());
                return;
            }

            if (_document.CursorPosition == lengthOfCurrentLine - 1 && _document.CursorLineNumber != (_document.Output.Count - 1))
            {
                var undoAction = new UndoMove(_document.CursorLineNumber, _document.CursorPosition, _document);
                //We are at the end of a line, just before newline. The line is not on the last line of the document, so move to next line.
                _document.CursorLineNumber++;
                _document.CursorPosition = 0;
                _document.UndoActions.Add(undoAction);
                return;
            }

            if (_document.CursorPosition < lengthOfCurrentLine)
            {
                // We are somewhere inside a line. We can safely move forward.
                var undoAction = new UndoMove(_document.CursorLineNumber, _document.CursorPosition, _document);
                _document.CursorPosition++;
                _document.UndoActions.Add(undoAction);
                return;
            }
        }
    }
}
