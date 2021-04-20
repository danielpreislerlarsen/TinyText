namespace TinyText.Actions
{
    public class UndoAction : IAction
    {
        private readonly Document _document;

        public UndoAction(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            if (_document.UndoActions.Count == 0)
                return;

            var indexOfNewstUndoAction = _document.UndoActions.Count - 1;
            var undoAction = _document.UndoActions[indexOfNewstUndoAction];

            undoAction.Execute();

            _document.UndoActions.RemoveAt(indexOfNewstUndoAction);
        }
    }
}
