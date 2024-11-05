using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProgrammingLearningApp
{
    public class BlockManager
    {
        private readonly FlowLayoutPanel blockPanel;
        private readonly ProgramController programController;

        public BlockManager(FlowLayoutPanel blockPanel, ProgramController programController)
        {
            this.blockPanel = blockPanel;
            this.programController = programController;
        }

        public void CreateBlock(CommandType type)
        {
            // Create a panel (block) for each command type
            Panel block = new Panel
            {
                Width = 150,
                Height = 50,
                AllowDrop = true,
                BackColor = GetBlockColor(type),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label commandLabel = new Label
            {
                Text = type.ToString(),
                Dock = DockStyle.Left,
                Width = 70,
                TextAlign = ContentAlignment.MiddleCenter
            };

            TextBox inputField = new TextBox
            {
                Width = 50,
                Text = "1", // Default value for the command parameter (e.g., move steps or repeat count)
                Dock = DockStyle.Right
            };

            // Add the command to the program and get its ID
            int commandId = programController.AddCommand(type, int.Parse(inputField.Text));
            inputField.Tag = commandId;

            inputField.TextChanged += (sender, args) =>
            {
                if (sender is TextBox textBox && int.TryParse(textBox.Text, out int newValue))
                {
                    int id = (int)textBox.Tag;
                    programController.UpdateCommandValue(id, newValue);
                }
                else
                {
                    //TODO
                }
            };

            block.Controls.Add(commandLabel);
            block.Controls.Add(inputField);
            block.MouseDown += Block_MouseDown;
            block.DragEnter += Block_DragEnter;
            block.DragDrop += Block_DragDrop;

            blockPanel.Controls.Add(block);
        }

        private Color GetBlockColor(CommandType type)
        {
            return type switch
            {
                CommandType.Turn => Color.Blue,
                CommandType.Move => Color.Red,
                CommandType.Repeat => Color.Yellow,
                _ => Color.LightBlue
            };
        }

        private void Block_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel block)
            {
                block.DoDragDrop(block, DragDropEffects.Move);
            }
        }

        private void Block_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void Block_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Panel)) is Panel block && sender is FlowLayoutPanel destination)
            {
                blockPanel.Controls.Remove(block);
                destination.Controls.Add(block);
            }
        }
    }
}