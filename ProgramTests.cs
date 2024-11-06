using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ProgrammingLearningApp.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void TestCharacterMovement()
        {
            Grid grid = new Grid(10, 10);
            Character character = new Character(grid);
            Command moveCommand = new Command(CommandType.Move, 5, character);
            moveCommand.Execute(character);

            Assert.That(character.X, Is.EqualTo(5));
            Assert.That(character.Y, Is.EqualTo(0));
        }

        [Test]
        public void TestTurnCharacter()
        {
            Grid grid = new Grid(10, 10);
            Character character = new Character(grid);
            Command turnCommand = new Command(CommandType.Turn, 1, character);
            turnCommand.Execute(character);

            Assert.That(character.Direction, Is.EqualTo(Direction.South));
        }

        [Test]
        public void TestRepeatCommand()
        {
            Grid grid = new Grid(10, 10);
            Character character = new Character(grid);
            Command moveCommand = new Command(CommandType.Move, 2, character);
            Command repeatCommand = new Command(CommandType.Repeat, 3, character);
            repeatCommand.SubCommands.Add(moveCommand);
            repeatCommand.Execute(character);

            Assert.That(character.X, Is.EqualTo(6));
        }
    }
}