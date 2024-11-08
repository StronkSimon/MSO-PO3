using System.Collections.Generic;
using Xunit;
using Moq;
using ProgrammingLearningApp;
using System.Threading.Tasks;

namespace ProgrammingLearningApp.Tests
{
    public class ProgramControllerTests
    {
        private readonly ProgramController _controller;

        public ProgramControllerTests()
        {
            // Setup controller instance before each test
            _controller = new ProgramController();
            _controller.MakeCharacter(new Grid(10, 10));  // Mocked grid of size 10x10 for character actions
        }

        [Fact]
        public void LoadSampleProgram_BasicLevel_ShouldLoadExpectedCommands()
        {
            // Act
            _controller.LoadSampleProgram("Basic");

            // Assert
            var commands = _controller.GetCommandDisplayList();
            Assert.Equal(2, commands.Count);
            Assert.Equal(CommandType.Move, commands[0].Type);
            Assert.Equal(5, commands[0].Value);
            Assert.Equal(CommandType.Turn, commands[1].Type);
            Assert.Equal(1, commands[1].Value);
        }

        [Fact]
        public void LoadSampleProgram_AdvancedLevel_ShouldLoadNestedCommands()
        {
            // Act
            _controller.LoadSampleProgram("Advanced");

            // Assert
            var commands = _controller.GetCommandDisplayList();
            Assert.Single(commands);
            Assert.Equal(CommandType.Repeat, commands[0].Type);
            Assert.Equal(3, commands[0].Value);
            Assert.Equal(2, commands[0].SubCommands.Count);
            Assert.Equal(CommandType.Move, commands[0].SubCommands[0].Type);
            Assert.Equal(2, commands[0].SubCommands[0].Value);
            Assert.Equal(CommandType.Turn, commands[0].SubCommands[1].Type);
            Assert.Equal(1, commands[0].SubCommands[1].Value);
        }

        [Fact]
        public void RunProgram_ShouldReturnCorrectFinalState()
        {
            // Arrange
            _controller.LoadSampleProgram("Basic");

            // Act
            string result = _controller.RunProgram();

            // Assert
            Assert.Contains("Final Position:", result);
            Assert.Contains("Facing:", result);
        }

        [Fact]
        public void AddCommand_ShouldAddNewCommandToProgram()
        {
            // Act
            int commandId = _controller.AddCommand(CommandType.Move, 5);

            // Assert
            var commands = _controller.GetCommandDisplayList();
            Assert.Single(commands);
            Assert.Equal(commandId, commands[0].Id);
            Assert.Equal(CommandType.Move, commands[0].Type);
            Assert.Equal(5, commands[0].Value);
        }

        [Fact]
        public void DeleteCommand_ShouldRemoveCommandFromProgram()
        {
            // Arrange
            int commandId = _controller.AddCommand(CommandType.Turn, 1);

            // Act
            _controller.DeleteCommand(commandId);

            // Assert
            var commands = _controller.GetCommandDisplayList();
            Assert.Empty(commands);
        }

        [Fact]
        public void UpdateCommandValue_ShouldModifyCommandValue()
        {
            // Arrange
            int commandId = _controller.AddCommand(CommandType.Move, 5);

            // Act
            _controller.UpdateCommandValue(commandId, 10);

            // Assert
            var commands = _controller.GetCommandDisplayList();
            Assert.Equal(10, commands[0].Value);
        }

        [Fact]
        public async Task RunProgramWithDelay_ShouldExecuteCommandsWithDelay()
        {
            // Arrange
            _controller.LoadSampleProgram("Basic");
            var delayMs = 100;
            int updatesCount = 0;
            void Update() => updatesCount++;

            // Act
            await _controller.RunProgramWithDelay(delayMs, Update);

            // Assert
            Assert.True(updatesCount > 0, "Update should have been called at least once per command execution.");
        }

        [Fact]
        public void GetMetrics_ShouldReturnExpectedMetrics()
        {
            // Arrange
            _controller.LoadSampleProgram("Expert");

            // Act
            var metrics = _controller.GetMetrics();

            // Assert
            Assert.Equal(3, metrics.Count);
            Assert.Equal(2, metrics["Total Commands"]);
            Assert.Equal(1, metrics["Repeat Commands"]);
            Assert.Equal(1, metrics["Max Nesting Level"]);
        }
    }
}