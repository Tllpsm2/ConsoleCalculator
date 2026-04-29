# Console Calculator Pro

This is an interactive console-based calculator application built as part of [The C# Academy](https://www.thecsharpacademy.com/project/11/calculator) curriculum. The primary goal of this project is to practice and level up my skills in building C# console applications, but with a modern, highly interactive twist.

## Features

- **Interactive Console Interface:** This isn't your standard text-prompt calculator. By utilizing the `Terminal.Gui` library, the console features a fully interactive, mouse-friendly interface that looks and feels like a real desktop calculator right inside your terminal.
- **Advanced Math Evaluation:** Powered by `NCalc`, the calculator goes beyond simple step-by-step math. It natively supports complex expressions, parentheses, and operator precedence (e.g., `(10 + 5) * 2 / 4`).
- **Clean Architecture & Reliable Logic:** The project separates the UI from the business logic (Service-Driven approach). The core calculator engine is covered by automated tests using **xUnit** and **FluentAssertions** to ensure that all math operations return accurate results and edge cases (like division by zero) are handled gracefully.
- **Upcoming Feature: AI Voice Integration:** As part of the C# Academy's _AI Challenge_, this project is being prepared to support voice commands! In future updates, we'll be integrating **Azure's Language Services** so users can speak their calculations instead of typing them.

## Technologies Used

- **C# 12 / .NET 8:** The core framework.
- **[Terminal.Gui](https://github.com/gui-cs/Terminal.Gui):** For building the rich, event-driven console UI.
- **[NCalcSync](https://github.com/ncalc/ncalc):** For parsing and evaluating complex mathematical expressions.
- **xUnit & [FluentAssertions](https://fluentassertions.com/):** For creating readable and robust unit tests.
- **Azure Cognitive Services (Planned):** To tackle the AI speech-to-text challenge.

## How to Run

1. Clone this repository to your local machine.
2. Open a terminal and navigate to the root folder of the project.
3. Restore the packages by running:
   ```bash
   dotnet restore
   ```
4. Start the application:
   ```bash
   dotnet run --project ConsoleCalculator
   ```
5. To run the test suite:
   ```bash
   dotnet test
   ```
