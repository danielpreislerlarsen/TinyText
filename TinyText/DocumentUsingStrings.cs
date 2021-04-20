using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyText
{
    public class DocumentUsingStrings
    {
        public const string Newline = "\r\n";
        private List<string> _document = new() { "" };
        private int _cursorLineNumber = 0;
        private int _cursorPosition = 0;

        public string ProcessInputs(List<InputTypes> inputs)
        {
            foreach (var input in inputs)
            {
                ProcessInput(input);
            }

            StringBuilder sb = new();
            foreach (var documentLine in _document)
            {
                sb.Append(documentLine);
            }

            var document = sb.ToString();

            return document;
        }


        public void ProcessInput(InputTypes input)
        {
            switch (input)
            {
                case InputTypes.a:
                    _document[_cursorLineNumber] = InsertCharacter('a');
                    _cursorPosition++;
                    break;

                case InputTypes.b:
                    _document[_cursorLineNumber] = InsertCharacter('b');
                    _cursorPosition++;
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }

        public void MoveCursorLeft()
        {
            if (_cursorPosition >= 1)
                _cursorPosition--;
            else
            {
                if (_cursorLineNumber == 0) return; // Can't further back than 0,0

                var previousLine = _document[_cursorLineNumber - 1];
                _cursorPosition = previousLine.Length - Newline.Length;
                _cursorLineNumber--;
            }
        }

        public void MoveCursorRight()
        {
            var lengthOfCurrentLine = _document[_cursorLineNumber].Length;
            if(_cursorLineNumber == (_document.Count - 1) && _cursorPosition == lengthOfCurrentLine) return; // moving beyond the end of the document is not allowed.

            if (_cursorPosition == lengthOfCurrentLine - Newline.Length && _cursorLineNumber != (_document.Count - 1))
            {
                //We are at the end of a line, just before newline. The line is not on the last line of the document, so move to next line.
                _cursorLineNumber++;
                _cursorPosition = 0;
                return;
            }

            if (_cursorPosition < lengthOfCurrentLine)
            {
                // We are somewhere inside a line. We can safely move forward.
                _cursorPosition++;
                return;
            }
        }

        public string InsertCharacter(char character)
        {
            var cursorLine = _document[_cursorLineNumber];
            if (_cursorPosition == cursorLine.Length)
            {
                return cursorLine + character;
            }
            else
            {
                var textPreCursor = cursorLine.Substring(0, _cursorPosition);
                var textPostCursor = cursorLine.Substring(_cursorPosition);

                return textPreCursor + character + textPostCursor;
            }
        }

        public void InsertNewline()
        {
            var cursorLine = _document[_cursorLineNumber];
            var textPreCursor = cursorLine.Substring(0, _cursorPosition);
            var textPostCursor = cursorLine.Substring(_cursorPosition);
            _document[_cursorLineNumber] = textPreCursor + Newline;

            _document.Insert(_cursorLineNumber + 1, textPostCursor);
            _cursorLineNumber++;
            _cursorPosition = 0;
        }

        public void Backspace()
        {
            //hvis position og linje er 0 sker der intet.
            if (_cursorLineNumber == 0 && _cursorPosition == 0) return;

            //Hvis position er 0 og linjen >= 1 skal hele linjen flyttes op bagerst på linjen før
            if (_cursorLineNumber >= 1 && _cursorPosition == 0)
            {
                var previousCursorLineLenght = _document[_cursorLineNumber - 1].Length;
                var previousCursorLineWithoutNewline = _document[_cursorLineNumber - 1].Substring(0, previousCursorLineLenght - Newline.Length);
                _document[_cursorLineNumber - 1] = previousCursorLineWithoutNewline + _document[_cursorLineNumber];
                _cursorLineNumber--;
                _cursorPosition = _document[_cursorLineNumber].Length;

                return;
            }

            //Slet tegnet før cursoren
            var cursorLine = _document[_cursorLineNumber];
            var textPreCursorWithoutDeletedCharacter = cursorLine.Substring(0, _cursorPosition - 1); // Note the chacacter is deleted by not being included in the substring
            var textPostCursor = cursorLine.Substring(_cursorPosition);
            _document[_cursorLineNumber] = textPreCursorWithoutDeletedCharacter + textPostCursor;
            _cursorPosition--;
        }
    }
}
