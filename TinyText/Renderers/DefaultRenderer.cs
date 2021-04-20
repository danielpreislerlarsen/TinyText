using System;
using System.Collections.Generic;
using System.Text;

namespace TinyText.Renderers
{
    public class DefaultRenderer : IRenderer
    {
        public static string Newline = "\r\n";

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
    }
}
