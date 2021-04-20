using System.Linq;
using TinyText.UndoActions;

namespace TinyText.Actions
{
    public class MoveDownAction : IAction
    {
        private readonly Document _document;

        public MoveDownAction(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            if (_document.CursorLineNumber == _document.Output.Count - 1) // are we on the last line of the document?
            {
                //Can't move below the document. No move performed, no action to undo
                _document.UndoActions.Add(new UndoWithNoEffect());
                return;
            }

            var undoAction = new UndoMove(_document.CursorLineNumber, _document.CursorPosition, _document);

            var nextLine = _document.Output[_document.CursorLineNumber + 1];
            var lengthOfNextLine = nextLine.Count;
            if (_document.CursorPosition >= lengthOfNextLine)
            {
                _document.CursorLineNumber++;

                if(nextLine.Count > 0 && nextLine.Last() == OutputCharacters.newline)
                {
                    _document.CursorPosition = lengthOfNextLine -1;
                }
                else
                {
                    _document.CursorPosition = lengthOfNextLine;
                }

                _document.UndoActions.Add(undoAction);
                return;
            }

            _document.CursorLineNumber++;
            //Note that the position stays the same in this case
            _document.UndoActions.Add(undoAction);
        }
    }
}
