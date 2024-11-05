using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ProgrammingLearningApp
{
    public partial class UIManager : Form
    {
        private readonly ProgramController programController;
        private readonly BlockManager blockManager;
        private FlowLayoutPanel blockPanel;
        private Panel gridPanel;

        public UIManager()
        {
            InitializeComponent();
            programController = new ProgramController();

            // Initialize BlockManager with blockPanel and programController
            blockManager = new BlockManager(blockPanel, programController);
        }

        private void InitializeComponent()
        {
            // Set up the main form
            this.Text = "Learn To Program!";
            this.Width = 800;
            this.Height = 600;

            // Create main layout table
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 4
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // Left column for command display
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F)); // Center column for buttons
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // Right column for grid panel
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Top row for load and command buttons
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 70F)); // Middle row for command display and grid
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Run and Metrics buttons row
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F)); // Bottom row for output display
            this.Controls.Add(mainLayout);

            // Load Program Dropdown using ComboBox
            ComboBox loadProgramComboBox = new ComboBox
            {
                Text = "Load",
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList // Prevents text entry
            };
            loadProgramComboBox.Items.Add("Basic");
            loadProgramComboBox.Items.Add("Advanced");
            loadProgramComboBox.Items.Add("Expert");
            loadProgramComboBox.Items.Add("From file...");
            loadProgramComboBox.SelectedIndexChanged += LoadProgramComboBox_SelectedIndexChanged;
            loadProgramComboBox.SelectedIndex = -1; // No item selected initially

            // Command Buttons (Turn, Move, Repeat)
            Button turnButton = new Button { Text = "Turn", BackColor = Color.Blue, Width = 80 };
            Button moveButton = new Button { Text = "Move", BackColor = Color.Green, Width = 80 };
            Button repeatButton = new Button { Text = "Repeat", BackColor = Color.Yellow, Width = 80 };
            turnButton.Click += (s, e) => blockManager.CreateBlock(CommandType.Turn);
            moveButton.Click += (s, e) => blockManager.CreateBlock(CommandType.Move);
            repeatButton.Click += (s, e) => blockManager.CreateBlock(CommandType.Repeat);

            // Top button panel
            FlowLayoutPanel topButtonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            topButtonPanel.Controls.Add(loadProgramComboBox);
            topButtonPanel.Controls.Add(turnButton);
            topButtonPanel.Controls.Add(moveButton);
            topButtonPanel.Controls.Add(repeatButton);
            mainLayout.Controls.Add(topButtonPanel, 0, 0);
            mainLayout.SetColumnSpan(topButtonPanel, 3); // Span across all columns

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

        private void RunButton_Click(object sender, EventArgs e)
        {
            string result = programController.RunProgram();
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

        private void LoadSampleProgram(string level)
        {
            programController.LoadSampleProgram(level);
            blockPanel.Controls.Clear();

            foreach (var command in programController.GetCommandDisplayList())
            {
                if (command.Type == CommandType.Repeat)
                {
                    blockManager.CreateBlock(command.Type, command.Id, command.Value);

                    foreach (var subCommand in command.SubCommands)
                    {
                        blockManager.CreateBlock(subCommand.Type, subCommand.Id, subCommand.Value);
                    }
                }
                else
                {
                    blockManager.CreateBlock(command.Type, command.Id, command.Value);
                }
            }
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
                // programController.LoadProgramFromFile(filePath);

                blockPanel.Controls.Clear();

                // Update the block panel to display the loaded program commands
                foreach (var command in programController.GetCommandDisplayList())
                {
                    blockManager.CreateBlock(command.Type, command.Id, command.Value);
                }
            }
        }
    }
}