using System;
using System.Collections.Generic;
using TinyText.UndoActions;

namespace TinyText
{
    public class Document
    {
        public readonly List<List<OutputCharacters>> Output = new(){new()};
        internal int CursorLineNumber = 0;
        internal int CursorPosition = 0;
        public readonly List<IUndoAction> UndoActions = new();
    }
}
