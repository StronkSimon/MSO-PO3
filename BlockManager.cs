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

        public void CreateBlock(CommandType type, int? id = null, int initialValue = 1, int? parentId = null)
        {
            // If `id` is null, add a new command and get its ID; otherwise, use the provided ID
            int commandId = id ?? programController.AddCommand(type, initialValue, parentId);

            Panel block = new Panel
            {
                Width = type == CommandType.Repeat ? 170 : 150,
                Height = type == CommandType.Repeat ? 70 : 50,
                AllowDrop = true,
                BackColor = GetBlockColor(type),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = commandId // Store the command ID in the Tag
            };

            Button deleteButton = new Button
            {
                Text = "✖",
                Size = new Size(25, 25),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.None,
                Location = new Point(0, 0)
            };

            // Delete button event handler
            deleteButton.Click += (sender, e) =>
            {
                if (parentId.HasValue)
                {
                    // Remove from parent (Repeat command) subcommands
                    programController.DeleteSubCommand(parentId.Value, commandId);
                }
                else
                {
                    // Remove from top-level commands
                    programController.DeleteCommand(commandId);
                }

                // Remove from UI
                var parentPanel = block.Parent as FlowLayoutPanel;
                parentPanel?.Controls.Remove(block);
            };
            block.Controls.Add(deleteButton);

            Label commandLabel = new Label
            {
                Text = type.ToString(),
                Dock = DockStyle.Top,
                Width = 70,
                TextAlign = ContentAlignment.MiddleCenter
            };

            TextBox inputField = new TextBox
            {
                Width = 50,
                Text = initialValue.ToString(),
                Dock = DockStyle.Right
            };

            block.Controls.Add(commandLabel);
            block.Controls.Add(inputField);

            if (type == CommandType.Repeat)
            {
                // Subcommand panel for vertical stacking of subcommands
                FlowLayoutPanel subCommandPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoScroll = true,
                    AllowDrop = true,
                    WrapContents = false,
                    Dock = DockStyle.Fill,
                    Tag = commandId,
                    Padding = new Padding(0, 25, 0, 0)
                };

                // Adjust Repeat block height based on subcommand count
                subCommandPanel.ControlAdded += (sender, args) => AdjustRepeatBlockHeight(block, subCommandPanel);
                subCommandPanel.ControlRemoved += (sender, args) => AdjustRepeatBlockHeight(block, subCommandPanel);

                subCommandPanel.DragEnter += Block_DragEnter;
                subCommandPanel.DragDrop += (s, e) => SubCommandPanel_DragDrop(s, e, commandId); // Pass parent ID

                block.Controls.Add(subCommandPanel);
            }

            // Event handler for TextBox to update the command value in ProgramController
            inputField.TextChanged += (sender, args) =>
            {
                if (int.TryParse(inputField.Text, out int newValue))
                {
                    programController.UpdateCommandValue(commandId, newValue);
                }
                else
                {
                    //TODO
                }
            };

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
                CommandType.Move => Color.Green,
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

        private void SubCommandPanel_DragDrop(object sender, DragEventArgs e, int parentId)
        {
            if (e.Data.GetData(typeof(Panel)) is Panel draggedBlock && sender is FlowLayoutPanel subCommandPanel)
            {
                int subCommandId = (int)draggedBlock.Tag;

                subCommandPanel.Controls.Add(draggedBlock); // Add the block to the UI

                // Add to ProgramController as a nested command
                programController.AddSubCommand(parentId, subCommandId);
            }
        }

        // Helper method to adjust the height of the Repeat block based on subcommands
        private void AdjustRepeatBlockHeight(Panel repeatBlock, FlowLayoutPanel subCommandPanel)
        {
            int baseHeight = 60; // Base height for label, input, and padding
            int subCommandsHeight = subCommandPanel.Controls.Count * 60;
            repeatBlock.Height = baseHeight + subCommandsHeight;
        }
    }
}