using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyText.UndoActions
{
    public class UndoInsertNewline : IUndoAction
    {
        private readonly int _lineNumber;
        private readonly int _position;
        private readonly Document _document;

        public UndoInsertNewline(int lineNumber, int position, Document document)
        {
            _lineNumber = lineNumber;
            _position = position;
            _document = document;
        }

        public void Execute()
        {
            var theLine = _document.Output[_lineNumber];
            theLine.RemoveAt(theLine.Count-1); //remove the inserted newline

            var theNextLine = _document.Output[_lineNumber + 1];

            theLine.AddRange(theNextLine);

            _document.Output[_lineNumber] = theLine;

            _document.Output.RemoveAt(_lineNumber + 1);

            _document.CursorLineNumber = _lineNumber;
            _document.CursorPosition = _position;
        }
    }
}
