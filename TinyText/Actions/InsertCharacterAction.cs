using TinyText.UndoActions;

namespace TinyText.Actions
{
    internal class InsertCharacterAction : IAction
    {
        private readonly Document _document;
        private readonly OutputCharacters _outputCharacter;

        public InsertCharacterAction(Document document, OutputCharacters outputCharacter)
        {
            _document = document;
            _outputCharacter = outputCharacter;
        }

        public void Execute()
        {
            var undoAction = new UndoInsertCharacter(_document.CursorLineNumber, _document.CursorPosition, _document);

            var cursorLine = _document.Output[_document.CursorLineNumber];
            if (_document.CursorPosition == cursorLine.Count)
            {
                cursorLine.Add(_outputCharacter);
            }
            else
            {
                var newLine = _document.Output[_document.CursorLineNumber];
                newLine.Insert(_document.CursorPosition, _outputCharacter);

                _document.Output[_document.CursorLineNumber] = newLine;
            }

            _document.CursorPosition++;

            _document.UndoActions.Add(undoAction);
        }
    }
}
