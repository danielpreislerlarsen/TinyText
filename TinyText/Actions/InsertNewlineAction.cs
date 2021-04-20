using TinyText.UndoActions;

namespace TinyText.Actions
{
    public class InsertNewlineAction : IAction
    {
        private readonly Document _document;
        private readonly OutputCharacters _outputCharacter;

        public InsertNewlineAction(Document document, OutputCharacters outputCharacter)
        {
            _document = document;
            _outputCharacter = outputCharacter;
        }

        public void Execute()
        {
            var undoAction = new UndoInsertNewline(_document.CursorLineNumber, _document.CursorPosition, _document);

            var cursorLine = _document.Output[_document.CursorLineNumber];
            var textPreCursor = cursorLine.GetRange(0, _document.CursorPosition);
            var textPostCursor = cursorLine.GetRange(_document.CursorPosition, cursorLine.Count - textPreCursor.Count);

            var newLine = textPreCursor;
            newLine.Add(OutputCharacters.newline);

            _document.Output[_document.CursorLineNumber] = newLine;

            _document.Output.Insert(_document.CursorLineNumber + 1, textPostCursor);
            _document.CursorLineNumber++;
            _document.CursorPosition = 0;

            _document.UndoActions.Add(undoAction);
        }
    }
}
