using System;
using System.Collections.Generic;
using System.Text;
using TinyText.Renderers;

namespace TinyText
{
    class Program
    {
        static void Main(string[] args)
        {
            List<InputTypes> inputs = new()
            {
                InputTypes.a,
                InputTypes.a,
                InputTypes.a,
                InputTypes.newline,
                InputTypes.b,
                InputTypes.b,
                InputTypes.b,
                InputTypes.left,
                InputTypes.left,
                InputTypes.backspace
            };

            var renderer = new DefaultRenderer();

            var outputForRendering = ProcessInputs(inputs);

            var documentAsString = renderer.RenderDocument(outputForRendering);

            Console.WriteLine(documentAsString);

            Console.ReadKey();
        }

        public static List<List<OutputCharacters>> ProcessInputs(List<InputTypes> inputs)
        {
            var document = new Document();

            foreach (var input in inputs)
            {
                document.ProcessInput(input);
            }

            return document.TheOutput;
        }
    }
}

