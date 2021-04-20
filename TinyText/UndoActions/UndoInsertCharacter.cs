using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyText.UndoActions
{
    public class UndoInsertCharacter : IUndoAction
    {
        private readonly int _lineNumber;
        private readonly int _position;
        private readonly Document _document;

        public UndoInsertCharacter(int lineNumber, int position, Document document)
        {
            _lineNumber = lineNumber;
            _position = position;
            _document = document;
        }

        public void Execute()
        {
            var theLine = _document.Output[_lineNumber];
            theLine.RemoveAt(_position);
            _document.Output[_lineNumber] = theLine;
            _document.CursorLineNumber = _lineNumber;
            _document.CursorPosition = _position;
        }
    }
}
