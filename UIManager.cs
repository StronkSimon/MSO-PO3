using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace ProgrammingLearningApp
{
    public partial class UIManager : Form
    {
        private readonly ProgramController programController;
        private readonly BlockManager blockManager;
        private FlowLayoutPanel blockPanel;
        private Panel gridPanel;
        private Grid grid;

        public UIManager()
        {
            InitializeComponent();
            grid = new Grid(10, 10);
            programController = new ProgramController();
            programController.grid = this.grid;
            programController.MakeCharacter(this.grid);
            blockManager = new BlockManager(blockPanel, programController);
        }

        private void InitializeComponent()
        {
            // Set up the main form
            this.Text = "Learn To Program!";
            this.Width = 1034;
            this.Height = 867;

            // Create main layout table
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Left column for command display
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Right column for grid panel
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Top row for load and command buttons
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 70F)); // Middle row for command display and grid
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Run and Metrics buttons row
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F)); // Bottom row for output display
            this.Controls.Add(mainLayout);

            // Load Program Dropdown using ComboBox
            ComboBox loadProgramComboBox = new ComboBox
            {
                Text = "Load Program",
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList // Prevents text entry
            };
            loadProgramComboBox.Items.Add("Basic");
            loadProgramComboBox.Items.Add("Advanced");
            loadProgramComboBox.Items.Add("Expert");
            loadProgramComboBox.Items.Add("From file...");
            loadProgramComboBox.SelectedIndexChanged += LoadProgramComboBox_SelectedIndexChanged;
            loadProgramComboBox.SelectedIndex = -1; // No item selected initially

            // Load Exercise Dropdown using CombobBox
            ComboBox loadExerciseComboBox = new ComboBox
            {
                Text = "Load Exercise",
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList // Prevents text entry
            };
            loadExerciseComboBox.Items.Add("Pathfinding");
            loadExerciseComboBox.Items.Add("Clear");
            loadExerciseComboBox.SelectedIndexChanged += LoadExerciseComboBox_SelectedIndexChanged;
            loadExerciseComboBox.SelectedIndex = -1;

            // Command Buttons (Turn, Move, Repeat)
            Button turnButton = new Button { Text = "Turn", BackColor = System.Drawing.Color.Orange, Width = 80 };
            Button moveButton = new Button { Text = "Move", BackColor = System.Drawing.Color.Orange, Width = 80 };
            Button repeatButton = new Button { Text = "Repeat", BackColor = System.Drawing.Color.Orange, Width = 80 };
            Button saveButton = new Button { Text = "Save", BackColor = System.Drawing.Color.Orange, Width = 80 };
            saveButton.Click += (s, e) => SaveProgram();
            turnButton.Click += (s, e) => AddBlockToPanel(CommandType.Turn);
            moveButton.Click += (s, e) => AddBlockToPanel(CommandType.Move);
            repeatButton.Click += (s, e) => AddBlockToPanel(CommandType.Repeat);

            // Top button panel
            FlowLayoutPanel topButtonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            topButtonPanel.Controls.Add(loadProgramComboBox);
            topButtonPanel.Controls.Add(loadExerciseComboBox);
            topButtonPanel.Controls.Add(turnButton);
            topButtonPanel.Controls.Add(moveButton);
            topButtonPanel.Controls.Add(repeatButton);
            topButtonPanel.Controls.Add(saveButton);
            mainLayout.Controls.Add(topButtonPanel, 0, 0);
            mainLayout.SetColumnSpan(topButtonPanel, 2); // Span across all columns

            // Block-based Command Panel (to list the blocks)
            blockPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainLayout.Controls.Add(blockPanel, 0, 1);

            // Visualization Panel (Grid for character movement)
            gridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            gridPanel.Paint += GridPanel_Paint;
            mainLayout.Controls.Add(gridPanel, 2, 1);

            // Run and Metrics Buttons
            Button runButton = new Button { Text = "Run", BackColor = Color.Red, Width = 100 };
            Button metricsButton = new Button { Text = "Metrics", BackColor = Color.Blue, Width = 100 };
            runButton.Click += RunButton_Click;
            metricsButton.Click += MetricsButton_Click;

            // Run and Metrics button panel
            FlowLayoutPanel actionButtonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            actionButtonPanel.Controls.Add(runButton);
            actionButtonPanel.Controls.Add(metricsButton);
            mainLayout.Controls.Add(actionButtonPanel, 0, 2);
            mainLayout.SetColumnSpan(actionButtonPanel, 3); // Span across all columns
        }

        private void LoadProgramComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is string selectedOption)
            {
                switch (selectedOption)
                {
                    case "Basic":
                        LoadSampleProgram("Basic");
                        break;
                    case "Advanced":
                        LoadSampleProgram("Advanced");
                        break;
                    case "Expert":
                        LoadSampleProgram("Expert");
                        break;
                    case "From file...":
                        LoadProgramFromFile();
                        break;
                }
            }
        }

        private void LoadExerciseComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is string selectedOption)
            {
                switch (selectedOption)
                {
                    case "Pathfinding":
                        LoadExerciseFromFile();
                        break;
                    case "Clear":
                        ClearExercise();
                        break;
                }
            }
        }

        private void GridPanel_Paint(object sender, PaintEventArgs e)
        {
            // Get the latest character position
            int characterX = programController.character.X;
            int characterY = programController.character.Y;
            var trail = programController.character.Trail;

            // Draw the grid and character position using the Grid class
            grid.Draw(e.Graphics, gridPanel.Width, gridPanel.Height, characterX, characterY, trail);
        }

        private async void RunButton_Click(object sender, EventArgs e)
        {
            int delay = 300;
            string result = programController.RunProgram();
            await programController.RunProgramWithDelay(delay, () => gridPanel.Invalidate());
            MessageBox.Show(result, "Execution Result");
        }

        private void MetricsButton_Click(object sender, EventArgs e)
        {
            var metrics = programController.GetMetrics();
            string metricsDisplay = "Program Metrics: \n";
            foreach (var metric in metrics)
            {
                metricsDisplay += $"{metric.Key}: {metric.Value}\n";
            }
            MessageBox.Show(metricsDisplay, "Metrics");
        }
        private void SaveProgram()
        {
            programController.SaveProgram();
        }

        private void LoadSampleProgram(string level)
        {
            programController.LoadSampleProgram(level);
            blockPanel.Controls.Clear();

            foreach (var command in programController.GetCommandDisplayList())
            {
                // Call a new helper method to recursively add commands and subcommands
                AddCommandToUI(command, parentPanel: null);
            }
        }

        // New helper method to add commands and their subcommands recursively
        private void AddCommandToUI(Command command, FlowLayoutPanel parentPanel)
        {
            var block = blockManager.CreateBlock(command.Type, command.Id, command.Value);

            // If the parentPanel is provided, add the block to it; otherwise, add it to the top-level blockPanel
            if (parentPanel != null)
            {
                parentPanel.Controls.Add(block);
            }
            else
            {
                blockPanel.Controls.Add(block);
            }

            // Recursively add subcommands if the command is of type Repeat
            if (command.Type == CommandType.Repeat)
            {
                var subCommandPanel = block.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();
                if (subCommandPanel != null)
                {
                    foreach (var subCommand in command.SubCommands)
                    {
                        AddCommandToUI(subCommand, subCommandPanel);
                    }
                }
            }
        }

        private void AddBlockToPanel(CommandType commandType)
        {
            // Create a new block and add it to the main block panel
            var block = blockManager.CreateBlock(commandType);
            blockPanel.Controls.Add(block);
        }

        private void LoadProgramFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                programController.LoadProgramFromFile(filePath);

                blockPanel.Controls.Clear();

                // Update the block panel to display the loaded program commands
                foreach (var command in programController.GetCommandDisplayList())
                {
                    AddCommandToUI(command, parentPanel: null);
                }
            }
        }

        private void LoadExerciseFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PathFindingExercise pathFindingExercise = new PathFindingExercise();
                string filePath = openFileDialog.FileName;
                pathFindingExercise.loadExercise(filePath);
                grid.InitializeExercise(pathFindingExercise.exerciseCharList);
                gridPanel.Invalidate();
            }
        }

        private void ClearExercise()
        {
            grid.ClearExercise();
            gridPanel.Invalidate();
        }
    }
}