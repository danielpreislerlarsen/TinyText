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
                InputTypes.H,
                InputTypes.E,
                InputTypes.Y,
                InputTypes.space,
                InputTypes.L,
                InputTypes.O,
                InputTypes.R,
                InputTypes.D,
                InputTypes.left,
                InputTypes.left,
                InputTypes.left,
                InputTypes.left,
                InputTypes.backspace,
                InputTypes.newline,
                InputTypes.backspace,
                InputTypes.backspace,
                InputTypes.newline,
                InputTypes.undo,
                InputTypes.right,
                InputTypes.newline,
                InputTypes.W,
                InputTypes.right,
                InputTypes.right,
                InputTypes.L,
                InputTypes.up,
                InputTypes.L,
                InputTypes.O
            };

            var renderer = new DefaultRenderer();

            var outputForRendering = ProcessInputs(inputs);

            var documentAsString = renderer.RenderDocument(outputForRendering);

            Console.WriteLine(documentAsString);

            Console.ReadKey();
        }

        public static List<List<OutputCharacters>> ProcessInputs(List<InputTypes> inputs)
        {
            var renderer = new DefaultRenderer();

            var document = new Document();

            foreach (var input in inputs)
            {
                document.ProcessInput(input);
                Console.WriteLine(renderer.RenderDocument(document.TheOutput) + " --- " + input.ToString());
            }

            return document.TheOutput;
        }
    }
}

