using ConsoleCalculator.Core.Engine.Operations;
using FluentAssertions;

namespace ConsoleCalculator.Tests.Core.Engine;

public class BasicServiceTests
{
    [Theory]
    [InlineData("2×3+8÷4", 8)]
    [InlineData("(1,5×2)+(4÷2)", 5)]
    [InlineData("10-3×2", 4)]
    public void EvaluateComplex_WhenExpressionContainsLocalizedOperators_ReturnsExpectedResult(string expression,
        double expectedResult)
    {
        // Act
        var result = new BasicService().EvaluateComplex(expression);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void EvaluateComplex_WhenExpressionIsInvalid_ThrowsInvalidOperationException()
    {
        // Arrange
        Action act = () => new BasicService().EvaluateComplex("invalid +* -2^2");

        // Act & Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Invalid expression*");
    }
}