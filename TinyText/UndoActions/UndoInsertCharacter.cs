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
        private readonly List<List<OutputCharacters>> _theDocument;

        public UndoInsertCharacter(int lineNumber, int position, List<List<OutputCharacters>> theDocument)
        {
            _lineNumber = lineNumber;
            _position = position;
            _theDocument = theDocument;
        }

        public void Execute()
        {
            var theLine = _theDocument[_lineNumber];
            theLine.RemoveAt(_position);
            _theDocument[_lineNumber] = theLine;
        }
    }
}
