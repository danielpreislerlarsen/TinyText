using TinyText.UndoActions;

namespace TinyText.Actions
{
    public class MoveLeftAction : IAction
    {
        private readonly Document _document;

        public MoveLeftAction(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            var undoAction = new UndoMove(_document.CursorLineNumber, _document.CursorPosition, _document);

            if (_document.CursorPosition >= 1)
                _document.CursorPosition--;
            else
            {
                if (_document.CursorLineNumber == 0)
                    return; // Can't go further back than 0,0

                var previousLine = _document.Output[_document.CursorLineNumber - 1];
                _document.CursorPosition = previousLine.Count - 1; // - 2 because we are skipping the newline also
                _document.CursorLineNumber--;
            }

            _document.UndoActions.Add(undoAction);
        }
    }
}
