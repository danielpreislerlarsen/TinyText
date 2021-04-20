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
        private readonly IRenderer _renderer = new DefaultRenderer();

        [Theory]
        [MemberData(nameof(InputsAndExpectedOutputs))]
        public void Given_inputs_the_expected_output_is_returned(List<InputTypes> inputs, string expectedOutput)
        {
            //Arrange
            var document = new Document();

            //Act
            foreach (var input in inputs)
            {
                document.ProcessInput(input);
            }

            var outputForRendering = document.TheOutput;
            var actualOutput = _renderer.RenderDocument(outputForRendering);

            //Assert
            Assert.Equal(expectedOutput, actualOutput);
        }

        public static IEnumerable<object[]> SingleInputsAndExpectedOutputs()
        {
            yield return new object[] { new List<InputTypes> { }, "" };
        }

        public static IEnumerable<object[]> InputsAndExpectedOutputs()
        {// Each of the lines below is a testscenario following the format: intputs, expectedOutput

            yield return new object[] { new List<InputTypes> { }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a }, "aa" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b }, "ab" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.a }, "aba" };

            yield return new object[] { new List<InputTypes> {InputTypes.backspace}, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.backspace, InputTypes.backspace }, "" };
            yield return new object[] { new List<InputTypes> {InputTypes.a, InputTypes.backspace}, "" };
            yield return new object[] { new List<InputTypes> {InputTypes.a, InputTypes.b, InputTypes.backspace}, "a" };

            yield return new object[] { new List<InputTypes> {InputTypes.newline}, $"{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.newline }, $"{DefaultRenderer.Newline}{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline }, $"a{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> {InputTypes.a, InputTypes.newline, InputTypes.b}, $"a{DefaultRenderer.Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.newline, InputTypes.b, InputTypes.a }, $"ab{DefaultRenderer.Newline}ba" };
            yield return new object[] { new List<InputTypes> { InputTypes.b, InputTypes.b, InputTypes.b, InputTypes.left, InputTypes.left, InputTypes.newline }, $"b{DefaultRenderer.Newline}bb" };
            yield return new object[] { new List<InputTypes> {
                InputTypes.a, InputTypes.a, InputTypes.a, InputTypes.newline,
                InputTypes.b, InputTypes.b, InputTypes.b, InputTypes.newline,
                InputTypes.left, InputTypes.left, InputTypes.left, InputTypes.left, InputTypes.newline
            }, $"aaa{DefaultRenderer.Newline}{DefaultRenderer.Newline}bbb{DefaultRenderer.Newline}" }; // Test of insert newline in the middle of document

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
                $"aab{DefaultRenderer.Newline}a"
            };

            yield return new object[] { new List<InputTypes> { InputTypes.right }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.right,InputTypes.right }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.right }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.right }, "aa" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.b, InputTypes.left, InputTypes.left, InputTypes.right, InputTypes.b }, "aabb" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.newline, InputTypes.right }, $"aa{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.right }, $"{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.newline, InputTypes.right }, $"{DefaultRenderer.Newline}{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.newline, InputTypes.right, InputTypes.b }, $"{DefaultRenderer.Newline}{DefaultRenderer.Newline}b" };
            yield return new object[] { new List<InputTypes>
                {
                    InputTypes.a, InputTypes.a, InputTypes.newline,
                    InputTypes.a, InputTypes.left, InputTypes.left, InputTypes.right, InputTypes.b
                },
                $"aa{DefaultRenderer.Newline}ba" };

            yield return new object[] { new List<InputTypes> { InputTypes.up }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.up, InputTypes.up }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.up }, "ab" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.up, InputTypes.a }, "aba" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.newline, InputTypes.up }, $"ab{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.newline, InputTypes.up, InputTypes.b }, $"bab{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.up, InputTypes.b }, $"aba{DefaultRenderer.Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.b, InputTypes.up, InputTypes.b }, $"ab{DefaultRenderer.Newline}bb" };

            yield return new object[] { new List<InputTypes> { InputTypes.down }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.down, InputTypes.down }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.down }, "ab" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.down, InputTypes.a }, "aba" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.up, InputTypes.down, InputTypes.b }, $"a{DefaultRenderer.Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.b, InputTypes.newline, InputTypes.up, InputTypes.right, InputTypes.right, InputTypes.right, InputTypes.down, InputTypes.a }, $"abb{DefaultRenderer.Newline}a" };

            yield return new object[] { new List<InputTypes> { InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.undo, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.undo, InputTypes.undo }, "" };

            yield return new object[] { new List<InputTypes> { InputTypes.newline, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.b, InputTypes.undo }, $"a{DefaultRenderer.Newline}b" };

            yield return new object[] { new List<InputTypes> { InputTypes.backspace, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.backspace, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.backspace, InputTypes.undo }, "ab" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.backspace, InputTypes.undo }, $"a{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.b, InputTypes.newline, InputTypes.backspace, InputTypes.undo }, $"ab{DefaultRenderer.Newline}" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.left, InputTypes.backspace, InputTypes.undo }, $"a{DefaultRenderer.Newline}b" };

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
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.a, InputTypes.newline, InputTypes.up, InputTypes.undo, InputTypes.b }, $"aa{DefaultRenderer.Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.b, InputTypes.b, InputTypes.up, InputTypes.undo, InputTypes.b }, $"a{DefaultRenderer.Newline}bbb" };

            yield return new object[] { new List<InputTypes> { InputTypes.down, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.down, InputTypes.down, InputTypes.undo }, "" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.down, InputTypes.undo }, "a" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.down, InputTypes.undo, InputTypes.b }, $"a{DefaultRenderer.Newline}b" };
            yield return new object[] { new List<InputTypes> { InputTypes.a, InputTypes.newline, InputTypes.up, InputTypes.down, InputTypes.undo, InputTypes.b }, $"ba{DefaultRenderer.Newline}" };
        }
    }
}
