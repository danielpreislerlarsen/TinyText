using System;
using System.Collections.Generic;
using System.Text;

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

            var document = new DocumentUsingStrings();

            var processedInputs = document.ProcessInputs(inputs);
            Console.WriteLine(processedInputs);

            Console.ReadKey();
        }
    }
}

