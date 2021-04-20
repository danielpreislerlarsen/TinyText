using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyText.UndoActions
{
    public class UndoMove : IUndoAction
    {
        private readonly int _lineNumber;
        private readonly int _position;
        private readonly Document _document;

        public UndoMove(int lineNumber, int position, Document document)
        {
            _lineNumber = lineNumber;
            _position = position;
            _document = document;
        }

        public void Execute()
        {
            _document.CursorLineNumber = _lineNumber;
            _document.CursorPosition = _position;
        }
    }
}
