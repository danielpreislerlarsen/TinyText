using System;
using System.Collections.Generic;
using System.Text;

namespace TinyText.Renderers
{
    public class DefaultRenderer : IRenderer
    {
        public string RenderDocument(List<List<OutputCharacters>> output)
        {
            StringBuilder sb = new();
            foreach (var documentLine in output)
            {
                foreach (var outputCharacter in documentLine)
                {
                    var character = MapToView(outputCharacter);
                    sb.Append(character);
                }
            }

            var documentAsString = sb.ToString();

            return documentAsString;
        }

        private string MapToView(OutputCharacters outputCharacter)
        {
            switch (outputCharacter)
            {
                case OutputCharacters.a:
                    return "a";
                case OutputCharacters.b:
                    return "b";
                case OutputCharacters.newline:
                    return Environment.NewLine;
                case OutputCharacters.H:
                    return "H";
                case OutputCharacters.E:
                    return "E";
                case OutputCharacters.Y:
                    return "Y";
                case OutputCharacters.space:
                    return " ";
                case OutputCharacters.L:
                    return "L";
                case OutputCharacters.O:
                    return "O";
                case OutputCharacters.R:
                    return "R";
                case OutputCharacters.D:
                    return "D";
                case OutputCharacters.W:
                    return "W";
                default:
                    throw new ArgumentOutOfRangeException(nameof(OutputCharacters), outputCharacter, null);
                    break;
            }
        }
    }
}
