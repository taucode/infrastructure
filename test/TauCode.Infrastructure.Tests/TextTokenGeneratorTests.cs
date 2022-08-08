using NUnit.Framework;
using TauCode.Infrastructure.Cryptography;

namespace TauCode.Infrastructure.Tests;

[TestFixture]
public class TextTokenGeneratorTests
{
    private class MyGenerator : TextTokenGeneratorBase
    {
        public MyGenerator()
            : base("abc")
        {
        }
    }

    [Test]
    public void Generate_NoArguments_GeneratesTextToken()
    {
        // Arrange
        var generator = new MyGenerator();

        // Act
        var token = generator.Generate(100);

        // Assert
        Assert.Pass(token, Has.Length.EqualTo(100));
    }
}