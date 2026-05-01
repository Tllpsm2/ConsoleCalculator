using ConsoleCalculator.Core.Engine;
using FluentAssertions;

namespace ConsoleCalculator.Tests.Core.Engine;

public class CalculatorEngineTests
{
    [Theory]
    [InlineData("(1+7)*2/4", "4")]
    [InlineData("10+5*2", "20")]
    [InlineData("-10*5+2", "-48")]
    [InlineData("2 +  2", "4")]
    [InlineData("invalid +* -2^2", "Error")]
    [InlineData("1/0", "Error")]
    [InlineData("0/0", "Error")]
    [InlineData("", "")]
    public void Evaluate_MultipleScenarios_ReturnsExpectedResult(string input, string expectedResult)
    {
        // Arrange
        var engine = new CalculatorEngine(input);

        // Act
        engine.Evaluate();

        // Assert
        engine.State.CurrentInput.Should().Be(expectedResult);
    }

    [Fact]
    public void Evaluate_StepByStepInput_ReturnsCorrectResult()
    {
        // Arrange
        var engine = new CalculatorEngine();

        // Act
        engine.ProcessInput("1");
        engine.ProcessInput("+");
        engine.ProcessInput("2");

        engine.Evaluate();

        // Assert
        engine.State.CurrentInput.Should().Be("3");
    }

    [Fact]
    public void AllClear_WhenCalled_ResetsState()
    {
        // Arrange
        var engine = new CalculatorEngine("1+1");

        // Act
        engine.AllClear();

        // Assert
        engine.State.CurrentInput.Should().Be(" ");
    }

    [Fact]
    public void ProcessInput_ErrorState_ResetsExpression()
    {
        // Arrange
        var engine = new CalculatorEngine("Error");

        // Act
        engine.ProcessInput("1");

        // Assert
        engine.State.CurrentInput.Should().Be("1");
    }
}