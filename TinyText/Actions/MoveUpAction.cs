using TinyText.UndoActions;

namespace TinyText.Actions
{
    public class MoveUpAction : IAction
    {
        private readonly Document _document;

        public MoveUpAction(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            if (_document.CursorLineNumber == 0)
            {
                //We can't move above the top line
                _document.UndoActions.Add(new UndoWithNoEffect());
                return;
            }

            var undoAction = new UndoMove(_document.CursorLineNumber, _document.CursorPosition, _document);

            var lenghtOfPreviousLine = _document.Output[_document.CursorLineNumber - 1].Count;
            if (_document.CursorPosition > lenghtOfPreviousLine - 1)
            {
                _document.CursorLineNumber--;
                _document.CursorPosition = lenghtOfPreviousLine - 1;
                _document.UndoActions.Add(undoAction);
                return;
            }

            _document.CursorLineNumber--;
            //Note that the postion stays the same
            _document.UndoActions.Add(undoAction);
        }
    }
}
