using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyText.UndoActions
{
    public class UndoBackspaceResultingInLineMove : IUndoAction
    {
        private readonly int _cursorLineNumber;
        private readonly int _cursorPosition;
        private readonly Document _document;

        public UndoBackspaceResultingInLineMove(int cursorLineNumber, int cursorPosition, Document document)
        {
            _cursorLineNumber = cursorLineNumber;
            _cursorPosition = cursorPosition;
            _document = document;
        }

        public void Execute()
        {
            var curentLine = _document.TheOutput[_cursorLineNumber];
            var newLine = curentLine.GetRange(0, _cursorPosition);
            newLine.Add(OutputCharacters.newline);

            var nextLine = curentLine.GetRange(_cursorPosition, curentLine.Count - _cursorPosition);

            _document.TheOutput[_cursorLineNumber] = newLine;
            _document.TheOutput.Insert(_cursorLineNumber + 1, nextLine);

            _document.CursorLineNumber = _cursorLineNumber + 1;
            _document.CursorPosition = 0;
        }
    }
}
