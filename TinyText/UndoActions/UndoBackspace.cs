using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyText.UndoActions
{
    public class UndoBackspace : IUndoAction
    {
        private readonly int _lineNumber;
        private readonly int _position;
        private readonly OutputCharacters _deletedCharacter;
        private readonly Document _document;

        public UndoBackspace(int lineNumber, int position, OutputCharacters deletedCharacter, Document document)
        {
            _lineNumber = lineNumber;
            _position = position;
            _deletedCharacter = deletedCharacter;
            _document = document;
        }

        public void Execute()
        {
            var theLine = _document.TheOutput[_lineNumber];
            theLine.Insert(_position -1,_deletedCharacter);
            _document.TheOutput[_lineNumber] = theLine;

            _document.CursorLineNumber = _lineNumber;
            _document.CursorPosition = _position;
        }
    }
}
