using System;
using System.Collections.Generic;
using System.Xml;
using TinyText;
using TinyText.Renderers;
using Xunit;

namespace UnitTests
{
    public class Tests
    {
        private static readonly string Newline = Environment.NewLine;

        [Theory]
        [MemberData(nameof(InputsAndExpectedOutputs))]
        public void Given_inputs_the_expected_output_is_returned(List<InputTypes> inputs, string expectedOutput)
        {
            //Arrange
            var inputProcessor = new InputProcessor();

            //Act
            foreach (var input in inputs)
            {
                inputProcessor.Process(input);
            }

            var renderer = new DefaultRenderer();
            var actualOutput = renderer.RenderDocument(inputProcessor.GetOutput);

            //Assert
            Assert.Equal(expectedOutput, actualOutput);
        }

        public static IEnumerable<object[]> InputsAndExpectedOutputs()
        {
            // Each of the lines below is a testscenario following the format: intputs, expectedOutput

            yield return new object[] { new List<InputTypes> { }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a }, "aa" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b }, "ab" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.a }, "aba" };

            yield return new object[] { new List<InputTypes> {InputTypes.backspace}, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.backspace, InputTypes.backspace }, "" };
            yield return new object[] { new List<InputTypes> {InputTypes.a, InputTypes.backspace}, "" };
            yield return new object[] { new List<InputTypes> {InputTypes.a, InputTypes.b, InputTypes.backspace}, "a" };

            yield return new object[] { new List<InputTypes> {InputTypes.newline}, $"{Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.newline }, $"{Newline}{Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline }, $"a{Newline}" };
            yield return new object[] { new List<InputTypes> {InputTypes.a, InputTypes.newline, InputTypes.b}, $"a{Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.newline, InputTypes.b, InputTypes.a }, $"ab{Newline}ba" };
            yield return new object[] { new List<InputTypes> { InputTypes.b, InputTypes.b, InputTypes.b, InputTypes.left, InputTypes.left, InputTypes.newline }, $"b{Newline}bb" };
            yield return new object[] { new List<InputTypes> {
                InputTypes.a, InputTypes.a, InputTypes.a, InputTypes.newline,
                InputTypes.b, InputTypes.b, InputTypes.b, InputTypes.newline,
                InputTypes.left, InputTypes.left, InputTypes.left, InputTypes.left, InputTypes.newline
            }, $"aaa{Newline}{Newline}bbb{Newline}" }; // Test of insert newline in the middle of document

            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.backspace, InputTypes.b }, "ab" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.a, InputTypes.backspace, InputTypes.backspace, InputTypes.b }, "ab" };

            yield return new object[] { new List<InputTypes> {InputTypes.left}, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.left, InputTypes.left }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.left, InputTypes.b }, "ba" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.left, InputTypes.b }, "aba" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.left, InputTypes.backspace, InputTypes.b }, "ba" };

            yield return new object[]
            {
                new List<InputTypes>
                {
                    InputTypes.a, InputTypes.a, InputTypes.newline,
                    InputTypes.a, InputTypes.left, InputTypes.left, InputTypes.b
                },
                $"aab{Newline}a"
            };

            yield return new object[] { new List<InputTypes> { InputTypes.right }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.right,InputTypes.right }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.right }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.right }, "aa" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.b, InputTypes.left, InputTypes.left, InputTypes.right, InputTypes.b }, "aabb" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.newline, InputTypes.right }, $"aa{Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.right }, $"{Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.newline, InputTypes.right }, $"{Newline}{Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.newline, InputTypes.right, InputTypes.b }, $"{Newline}{Newline}b" };
            yield return new object[] { new List<InputTypes>
                {
                    InputTypes.a, InputTypes.a, InputTypes.newline,
                    InputTypes.a, InputTypes.left, InputTypes.left, InputTypes.right, InputTypes.b
                },
                $"aa{Newline}ba" };

            yield return new object[] { new List<InputTypes> { InputTypes.up }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.up, InputTypes.up }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.up }, "ab" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.up, InputTypes.a }, "aba" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.newline, InputTypes.up }, $"ab{Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.newline, InputTypes.up, InputTypes.b }, $"bab{Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.up, InputTypes.b }, $"aba{Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.b, InputTypes.up, InputTypes.b }, $"ab{Newline}bb" };

            yield return new object[] { new List<InputTypes> { InputTypes.down }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.down, InputTypes.down }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.down }, "ab" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.down, InputTypes.a }, "aba" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.up, InputTypes.down, InputTypes.b }, $"a{Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.b, InputTypes.newline, InputTypes.up, InputTypes.right, InputTypes.right, InputTypes.right, InputTypes.down, InputTypes.a }, $"abb{Newline}a" };

            yield return new object[] { new List<InputTypes> { InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.undo, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.undo, InputTypes.b }, "b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.undo, InputTypes.undo }, "" };

            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.b, InputTypes.undo }, $"a{Newline}b" };

            yield return new object[] { new List<InputTypes> { InputTypes.backspace, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.backspace, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.backspace, InputTypes.undo }, "ab" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.backspace, InputTypes.undo }, $"a{Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.newline, InputTypes.backspace, InputTypes.undo }, $"ab{Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.left, InputTypes.backspace, InputTypes.undo }, $"a{Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.left, InputTypes.backspace, InputTypes.backspace }, "b" };

            yield return new object[] { new List<InputTypes> { InputTypes.left, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.left, InputTypes.left, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.left, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.left, InputTypes.undo }, "aa" };
            yield return new object[] { new List<InputTypes> { InputTypes.b, InputTypes.b, InputTypes.left, InputTypes.left, InputTypes.undo, InputTypes.a }, $"bab" };

            yield return new object[] { new List<InputTypes> { InputTypes.right, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.right, InputTypes.right, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.right, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.right, InputTypes.undo }, "aa" };
            yield return new object[] { new List<InputTypes> { InputTypes.right, InputTypes.a, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.right, InputTypes.right, InputTypes.a, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.b, InputTypes.b, InputTypes.left, InputTypes.left, InputTypes.right, InputTypes.undo, InputTypes.a }, $"abb" };

            yield return new object[] { new List<InputTypes> { InputTypes.up, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.up, InputTypes.up, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.up, InputTypes.up, InputTypes.undo, InputTypes.undo, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.up, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.up, InputTypes.undo }, "aa" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.newline, InputTypes.up, InputTypes.undo, InputTypes.b }, $"aa{Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.b, InputTypes.up, InputTypes.undo, InputTypes.b }, $"a{Newline}bbb" };

            yield return new object[] { new List<InputTypes> { InputTypes.down, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.down, InputTypes.down, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.down, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.down, InputTypes.undo, InputTypes.b }, $"a{Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.up, InputTypes.down, InputTypes.undo, InputTypes.b }, $"ba{Newline}" };

            yield return new object[] { new List<InputTypes> { InputTypes.O, InputTypes.newline, InputTypes.newline, InputTypes.up, InputTypes.left, InputTypes.down,
                    InputTypes.newline, InputTypes.down, InputTypes.backspace,}, $"O{Newline}{Newline}"};

            yield return new object[]
            {
                new List<InputTypes>
                {
                    InputTypes.H, InputTypes.E, InputTypes.Y, InputTypes.space, InputTypes.L, InputTypes.O, InputTypes.R, InputTypes.D,
                    InputTypes.left, InputTypes.left, InputTypes.left, InputTypes.left, InputTypes.backspace, InputTypes.newline, InputTypes.backspace, InputTypes.backspace,
                    InputTypes.newline, InputTypes.undo, InputTypes.right, InputTypes.newline, InputTypes.W, InputTypes.right, InputTypes.right, InputTypes.L, InputTypes.up,
                    InputTypes.L, InputTypes.O }, $"HELLO{Newline}WORLD"
            };
        }


        [Fact]
        public void HandleRandomlyGeneratedInputs()
        {
                var inputProcessor = new InputProcessor();
                var inputs = GenerateRandomInputs(numberOfInputs: 1000000);

                //Act
                foreach (var input in inputs)
                {
                    inputProcessor.Process(input);
                }

                var renderer = new DefaultRenderer();
                renderer.RenderDocument(inputProcessor.GetOutput);
        }

        private IEnumerable<InputTypes> GenerateRandomInputs(int numberOfInputs)
        {
            var rand = new Random();
            var inputs = new List<InputTypes>();
            for (int i = 0; i < numberOfInputs; i++)
            {
                inputs.Add((InputTypes)rand.Next(18));
            }

            return inputs;
        }
    }
}
