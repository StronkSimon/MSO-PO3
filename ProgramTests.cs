using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ProgrammingLearningApp.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void TestCharacterMovement()
        {
            Character character = new Character();
            Command moveCommand = new Command(CommandType.Move, 5, character);
            moveCommand.Execute(character);
            NUnit.Framework.Assert.That(character.X, Is.EqualTo(5));
            NUnit.Framework.Assert.That(character.Y, Is.EqualTo(0));
        }

        [Test]
        public void TestTurnCharacter()
        {
            Character character = new Character();
            Command turnCommand = new Command(CommandType.Turn, 1, character);
            turnCommand.Execute(character);
            NUnit.Framework.Assert.That(character.Direction, Is.EqualTo(Direction.South));
        }

        [Test]
        public void TestRepeatCommand()
        {
            Character character = new Character();
            Command moveCommand = new Command(CommandType.Move, 2, character);
            Command repeatCommand = new Command(CommandType.Repeat, 3, character);
            repeatCommand.SubCommands.Add(moveCommand);
            repeatCommand.Execute(character);
            NUnit.Framework.Assert.That(character.X, Is.EqualTo(6));
        }
    }
}
