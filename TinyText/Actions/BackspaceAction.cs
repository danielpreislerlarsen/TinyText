using System.Collections.Generic;
using TinyText.UndoActions;

namespace TinyText.Actions
{
    public class BackspaceAction : IAction
    {
        private readonly Document _document;

        public BackspaceAction(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            //hvis position og linje er 0 sker der intet.
            if (_document.CursorLineNumber == 0 && _document.CursorPosition == 0)
            {
                _document.UndoActions.Add(new UndoWithNoEffect());
                return;
            }

            //Hvis position er 0 og linjen >= 1 skal hele linjen flyttes op i forlængelse af den foregående linje
            if (_document.CursorLineNumber >= 1 && _document.CursorPosition == 0)
            {
                AppendLineToPrevious();
            }
            else
            {
                DeleteCharacter(); //'Normal' sletning hvor tegnet før cursoren bliver slettet
            }
        }

        private void AppendLineToPrevious()
        {
            var previousCursorLineLenght = _document.Output[_document.CursorLineNumber - 1].Count;

            _document.Output[_document.CursorLineNumber - 1].RemoveAt(previousCursorLineLenght - 1); // Removing the newline
            _document.Output[_document.CursorLineNumber - 1].AddRange(_document.Output[_document.CursorLineNumber]);
            _document.Output.RemoveAt(_document.CursorLineNumber);

            _document.CursorLineNumber--;
            _document.CursorPosition = previousCursorLineLenght - 1;

            var undoAction = new UndoBackspaceResultingInLineMove(_document.CursorLineNumber, previousCursorLineLenght - 1, _document);
            _document.UndoActions.Add(undoAction);
        }

        private void DeleteCharacter()
        {
            var cursorLine = _document.Output[_document.CursorLineNumber];
            var outputToBeDeleted = cursorLine[_document.CursorPosition - 1];
            var undoAction = new UndoBackspace(_document.CursorLineNumber, _document.CursorPosition, outputToBeDeleted, _document);

            List<OutputCharacters> newLine = new();

            var textPreCursorWithoutDeletedCharacter = cursorLine.GetRange(0, _document.CursorPosition - 1); // Note the chacacter is deleted by not being included in the range
            var textPostCursor = cursorLine.GetRange(_document.CursorPosition, cursorLine.Count - _document.CursorPosition);
            newLine.AddRange(textPreCursorWithoutDeletedCharacter);
            newLine.AddRange(textPostCursor);
            _document.Output[_document.CursorLineNumber] = newLine;
            _document.CursorPosition--;

            _document.UndoActions.Add(undoAction);
        }
    }
}
