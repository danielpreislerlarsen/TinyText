using System;
using System.Collections.Generic;
using System.Text;
using TinyText.UndoActions;

namespace TinyText
{
    public class Document
    {
        internal readonly List<List<OutputCharacters>> TheOutput = new(){new()}; //TODO find better name
        internal int CursorLineNumber = 0;
        internal int CursorPosition = 0;
        private readonly List<IUndoAction> _undoActions = new();

        //TODO move the ProcessInputs out
        public string ProcessInputs(List<InputTypes> inputs)
        {
            foreach (var input in inputs)
            {
                ProcessInput(input);
            }

            StringBuilder sb = new();
            foreach (var documentLine in TheOutput)
            {
                foreach (var outputCharacter in documentLine)
                {
                    var character = MapToView(outputCharacter);
                    sb.Append(character);
                }
            }

            var document = sb.ToString();

            return document;
        }

        private string MapToView(OutputCharacters outputCharacter)
        {
            switch (outputCharacter)
            {
                case OutputCharacters.a:
                    return "a";
                    break;
                case OutputCharacters.b:
                    return "b";
                    break;
                case OutputCharacters.newline:
                    return "\r\n";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(OutputCharacters), outputCharacter, null);
                    break;
            }
        }

        //TODO Hvordan sikres at enhver handling får registreret sin undoaction?
        public void ProcessInput(InputTypes input)
        {
            switch (input)
            {
                case InputTypes.a:
                    InsertCharacter(OutputCharacters.a);
                    break;
                case InputTypes.b:
                    InsertCharacter(OutputCharacters.b);
                    break;
                case InputTypes.newline:
                    InsertNewline();
                    break;
                case InputTypes.backspace:
                    Backspace();
                    break;
                case InputTypes.left:
                    MoveCursorLeft();
                    break;
                case InputTypes.right:
                    MoveCursorRight();
                    break;
                case InputTypes.up:
                    MoveCursorUp();
                    break;
                case InputTypes.down:
                    MoveCursorDown();
                    break;
                case InputTypes.undo:
                    Undo();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }

        private void InsertCharacter(OutputCharacters character)
        {
            var undoAction = new UndoInsertCharacter(CursorLineNumber, CursorPosition, TheOutput);

            var cursorLine = TheOutput[CursorLineNumber];
            if (CursorPosition == cursorLine.Count)
            {
                cursorLine.Add(character);
            }
            else
            {
                var newLine = TheOutput[CursorLineNumber];
                newLine.Insert(CursorPosition, character);

                TheOutput[CursorLineNumber] = newLine;
            }

            CursorPosition++;

            _undoActions.Add(undoAction);
        }

        public void InsertNewline()
        {
            var undoAction = new UndoInsertNewline(CursorLineNumber, CursorPosition, this);

            var cursorLine = TheOutput[CursorLineNumber];
            var textPreCursor = cursorLine.GetRange(0, CursorPosition);
            var textPostCursor = cursorLine.GetRange(CursorPosition, cursorLine.Count - textPreCursor.Count);

            var newLine = textPreCursor;
            newLine.Add(OutputCharacters.newline);

            TheOutput[CursorLineNumber] = newLine;

            TheOutput.Insert(CursorLineNumber + 1, textPostCursor);
            CursorLineNumber++;
            CursorPosition = 0;

            _undoActions.Add(undoAction);
        }

        public void Backspace()
        {
            //hvis position og linje er 0 sker der intet.
            if (CursorLineNumber == 0 && CursorPosition == 0)
            {
                // The undo will in, this case, do nothing
                _undoActions.Add(new UndoWithNoEffect());
                return;
            }

            //Hvis position er 0 og linjen >= 1 skal hele linjen flyttes op i forlængelse af den foregående linje
            if (CursorLineNumber >= 1 && CursorPosition == 0)
            {
                var previousCursorLineLenght = TheOutput[CursorLineNumber - 1].Count;

                TheOutput[CursorLineNumber - 1].RemoveAt(previousCursorLineLenght -1); // Removing the newline
                TheOutput[CursorLineNumber - 1].AddRange(TheOutput[CursorLineNumber]);
                TheOutput.RemoveAt(CursorLineNumber);

                CursorLineNumber--;
                CursorPosition = TheOutput[CursorLineNumber].Count;

                var undoAction = new UndoBackspaceResultingInLineMove(CursorLineNumber, previousCursorLineLenght - 1, this);
                _undoActions.Add(undoAction);
            }
            else
            {
                //'Normal' case where the character before the cursor is deleted
                var cursorLine = TheOutput[CursorLineNumber];
                var outputToBeDeleted = cursorLine[CursorPosition -1];
                var undoAction = new UndoBackspace(CursorLineNumber, CursorPosition, outputToBeDeleted, this);

                List<OutputCharacters> newLine = new();

                var textPreCursorWithoutDeletedCharacter = cursorLine.GetRange(0, CursorPosition - 1); // Note the chacacter is deleted by not being included in the range
                var textPostCursor = cursorLine.GetRange(CursorPosition, cursorLine.Count - CursorPosition);
                newLine.AddRange(textPreCursorWithoutDeletedCharacter);
                newLine.AddRange(textPostCursor);
                TheOutput[CursorLineNumber] = newLine;
                CursorPosition--;

                _undoActions.Add(undoAction);
            }
        }

        public void MoveCursorLeft()
        {
            var undoAction = new UndoMove(CursorLineNumber, CursorPosition, this);

            if (CursorPosition >= 1)
                CursorPosition--;
            else
            {
                if (CursorLineNumber == 0)
                    return; // Can't go further back than 0,0

                var previousLine = TheOutput[CursorLineNumber - 1];
                CursorPosition = previousLine.Count - 1; // - 2 because we are skipping the newline also
                CursorLineNumber--;
            }

            _undoActions.Add(undoAction);
        }

        public void MoveCursorRight()
        {
            var lengthOfCurrentLine = TheOutput[CursorLineNumber].Count;
            if (CursorLineNumber == (TheOutput.Count - 1) && CursorPosition == lengthOfCurrentLine)
            {
                // moving beyond the end of the document is not allowed.
                _undoActions.Add(new UndoWithNoEffect());
                return;
            }

            if (CursorPosition == lengthOfCurrentLine - 1 && CursorLineNumber != (TheOutput.Count - 1))
            {
                var undoAction = new UndoMove(CursorLineNumber, CursorPosition, this);
                //We are at the end of a line, just before newline. The line is not on the last line of the document, so move to next line.
                CursorLineNumber++;
                CursorPosition = 0;
                _undoActions.Add(undoAction);
                return;
            }

            if (CursorPosition < lengthOfCurrentLine)
            {
                // We are somewhere inside a line. We can safely move forward.
                var undoAction = new UndoMove(CursorLineNumber, CursorPosition, this);
                CursorPosition++;
                _undoActions.Add(undoAction);
                return;
            }
        }

        public void MoveCursorUp()
        {
            if(CursorLineNumber == 0)
            {
                //We can't move above the top line
                _undoActions.Add(new UndoWithNoEffect());
                return;
            }

            var undoAction = new UndoMove(CursorLineNumber, CursorPosition, this);

            var lenghtOfPreviousLine = TheOutput[CursorLineNumber - 1].Count;
            if(CursorPosition > lenghtOfPreviousLine - 1)
            {
                CursorLineNumber--;
                CursorPosition = lenghtOfPreviousLine -1;
                _undoActions.Add(undoAction);
                return;
            }

            CursorLineNumber--;
            //Note that the postion stays the same
            _undoActions.Add(undoAction);
        }

        public void MoveCursorDown()
        {
            if(CursorLineNumber == TheOutput.Count -1) // are we on the last line of the document?
            {
                //Can't move below the document. No move performed, no action to undo
                _undoActions.Add(new UndoWithNoEffect());
                return;
            }

            var undoAction = new UndoMove(CursorLineNumber, CursorPosition, this);

            var lengthOfNextLine = TheOutput[CursorLineNumber + 1].Count;
            if(CursorPosition > lengthOfNextLine)
            {
                CursorLineNumber++;
                CursorPosition = lengthOfNextLine;
                _undoActions.Add(undoAction);
                return;
            }

            CursorLineNumber++;
            //Note that the position stays the same in this case
            _undoActions.Add(undoAction);
        }

        public void Undo()
        {
            if (_undoActions.Count == 0)
                return;

            var indexOfNewstUndoAction = _undoActions.Count - 1;
            var undoAction = _undoActions[indexOfNewstUndoAction];

            undoAction.Execute();

            _undoActions.RemoveAt(indexOfNewstUndoAction);
        }
    }
}
