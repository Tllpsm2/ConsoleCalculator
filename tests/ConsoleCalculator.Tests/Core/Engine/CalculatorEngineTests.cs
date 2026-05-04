using ConsoleCalculator.Core.Engine;
using ConsoleCalculator.Core.Engine.Operations;
using FluentAssertions;

namespace ConsoleCalculator.Tests.Core.Engine;

public class CalculatorEngineTests
{
    [Fact]
    public void Constructor_WhenCreatedWithInitialExpression_InitializesState()
    {
        // Arrange & Act
        var engine = new CalculatorEngine("12+3", new BasicService());

        // Assert
        engine.State.CurrentInput.Should().Be("12+3");
        engine.State.CurrentExpression.Should().Be(string.Empty);
    }

    [Theory]
    [InlineData("(1+7)*2/4", "4")]
    [InlineData("10+5*2", "20")]
    [InlineData("-10*5+2", "-48")]
    [InlineData("2 +  2", "4")]
    [InlineData("invalid +* -2^2", "Error")]
    [InlineData("1/0", "Error")]
    [InlineData("", "")]
    public void Evaluate_WhenGivenExpression_ReturnsExpectedResult(string input, string expectedResult)
    {
        // Arrange
        var engine = new CalculatorEngine(input, new BasicService());

        // Act
        engine.Evaluate();

        // Assert
        engine.State.CurrentInput.Should().Be(expectedResult);
    }

    [Fact]
    public void Evaluate_AfterSequentialProcessInput_ReturnsCorrectResult()
    {
        // Arrange
        var engine = new CalculatorEngine(string.Empty, new BasicService());

        // Act
        engine.ProcessInput("1");
        engine.ProcessInput("+");
        engine.ProcessInput("2");

        engine.Evaluate();

        // Assert
        engine.State.CurrentInput.Should().Be("3");
    }

    [Fact]
    public void Evaluate_WhenExpressionIsValid_PreservesOriginalExpression()
    {
        // Arrange
        var engine = new CalculatorEngine("2+3*4", new BasicService());

        // Act
        engine.Evaluate();

        // Assert
        engine.State.CurrentInput.Should().Be("14");
        engine.State.CurrentExpression.Should().Be("2+3*4");
    }

    [Fact]
    public void Evaluate_WhenResultIsRepeatingDecimal_TruncatesToEightDecimalPlaces()
    {
        // Arrange
        var engine = new CalculatorEngine("1/3", new BasicService());

        // Act
        engine.Evaluate();

        // Assert
        engine.State.CurrentInput.Should().Be("0.33333333");
    }

    [Fact]
    public void ProcessInput_WhenInputIsDigit_AppendsToExistingExpression()
    {
        // Arrange
        var engine = new CalculatorEngine("12", new BasicService());

        // Act
        engine.ProcessInput("3");

        // Assert
        engine.State.CurrentInput.Should().Be("123");
    }

    [Fact]
    public void DeleteLast_WhenCurrentInputHasMultipleCharacters_RemovesLastCharacter()
    {
        // Arrange
        var engine = new CalculatorEngine("123", new BasicService());

        // Act
        engine.DeleteLast();

        // Assert
        engine.State.CurrentInput.Should().Be("12");
    }

    [Fact]
    public void DeleteLast_WhenCurrentInputHasSingleCharacter_ResetsToEmpty()
    {
        // Arrange
        var engine = new CalculatorEngine("7", new BasicService());

        // Act
        engine.DeleteLast();

        // Assert
        engine.State.CurrentInput.Should().Be(string.Empty);
    }

    [Fact]
    public void AllClear_WhenCalled_ResetsState()
    {
        // Arrange
        var engine = new CalculatorEngine("1+1", new BasicService());

        // Act
        engine.AllClear();

        // Assert
        engine.State.CurrentInput.Should().Be(string.Empty);
    }

    [Fact]
    public void ProcessInput_WhenInErrorState_ReplacesInputWithNewValue()
    {
        // Arrange
        var engine = new CalculatorEngine("Error", new BasicService());

        // Act
        engine.ProcessInput("1");

        // Assert
        engine.State.CurrentInput.Should().Be("1");
    }

    [Fact]
    public void ProcessInput_AfterEvaluation_WhenOperatorFollowedByNumber_ContinuesChainedExpression()
    {
        // Arrange
        var engine = new CalculatorEngine("3-5", new BasicService());
        engine.Evaluate();

        // Act
        engine.ProcessInput("+");
        engine.ProcessInput("5");

        // Assert
        engine.State.CurrentInput.Should().Be("-2+5");
        engine.Evaluate();
        engine.State.CurrentInput.Should().Be("3");
    }
}