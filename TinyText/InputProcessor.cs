using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyText.Actions;

namespace TinyText
{
    public class InputProcessor
    {
        private Document _document;

        public InputProcessor()
        {
            _document = new Document();
        }

        public void Process(InputTypes input)
        {
            var action = MapInputToAction(input);
            action.Execute();
        }

        public List<List<OutputCharacters>> GetOutput => _document.Output;

        private IAction MapInputToAction(InputTypes input)
        {
            IAction action;

            switch (input)
            {
                case InputTypes.a:
                    action = new InsertCharacterAction(_document, OutputCharacters.a);
                    break;
                case InputTypes.b:
                    action = new InsertCharacterAction(_document, OutputCharacters.b);
                    break;
                case InputTypes.H:
                    action = new InsertCharacterAction(_document, OutputCharacters.H);
                    break;
                case InputTypes.E:
                    action = new InsertCharacterAction(_document, OutputCharacters.E);
                    break;
                case InputTypes.Y:
                    action = new InsertCharacterAction(_document, OutputCharacters.Y);
                    break;
                case InputTypes.space:
                    action = new InsertCharacterAction(_document, OutputCharacters.space);
                    break;
                case InputTypes.L:
                    action = new InsertCharacterAction(_document, OutputCharacters.L);
                    break;
                case InputTypes.O:
                    action = new InsertCharacterAction(_document, OutputCharacters.O);
                    break;
                case InputTypes.R:
                    action = new InsertCharacterAction(_document, OutputCharacters.R);
                    break;
                case InputTypes.D:
                    action = new InsertCharacterAction(_document, OutputCharacters.D);
                    break;
                case InputTypes.W:
                    action = new InsertCharacterAction(_document, OutputCharacters.W);
                    break;
                case InputTypes.newline:
                    action = new InsertNewlineAction(_document, OutputCharacters.newline);
                    break;
                case InputTypes.backspace:
                    action = new BackspaceAction(_document);
                    break;
                case InputTypes.left:
                    action = new MoveLeftAction(_document);
                    break;
                case InputTypes.right:
                    action = new MoveRightAction(_document);
                    break;
                case InputTypes.up:
                    action = new MoveUpAction(_document);
                    break;
                case InputTypes.down:
                    action = new MoveDownAction(_document);
                    break;
                case InputTypes.undo:
                    action = new UndoAction(_document);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }

            return action;
        }
    }
}
