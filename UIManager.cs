using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ProgrammingLearningApp
{
    public partial class UIManager : Form
    {
        private ProgramController programController;
        private TextBox commandDisplay;
        private TextBox outputDisplay;

        public UIManager()
        {
            InitializeComponent();
            programController = new ProgramController();
        }

        private void InitializeComponent()
        {
            // Set up main form
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
            Button turnButton = new Button { Text = "Turn", BackColor = System.Drawing.Color.Orange, Width = 80 };
            Button moveButton = new Button { Text = "Move", BackColor = System.Drawing.Color.Orange, Width = 80 };
            Button repeatButton = new Button { Text = "Repeat", BackColor = System.Drawing.Color.Orange, Width = 80 };
            Button saveButton = new Button { Text = "Save", BackColor = System.Drawing.Color.Orange, Width = 80 };
            turnButton.Click += (s, e) => AddCommand(CommandType.Turn, 1);
            moveButton.Click += (s, e) => AddCommand(CommandType.Move, 3);
            repeatButton.Click += (s, e) => AddCommand(CommandType.Repeat, 2);
            saveButton.Click += (s, e) => SaveProgram();

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
            topButtonPanel.Controls.Add(saveButton);
            mainLayout.Controls.Add(topButtonPanel, 0, 0);
            mainLayout.SetColumnSpan(topButtonPanel, 3); // Span across all columns

            // Command Display (for list of commands)
            commandDisplay = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            mainLayout.Controls.Add(commandDisplay, 0, 1);

            // Visualization Panel (Grid for character movement)
            Panel gridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainLayout.Controls.Add(gridPanel, 2, 1);

            // Run and Metrics Buttons
            Button runButton = new Button { Text = "Run", BackColor = System.Drawing.Color.Green, Width = 100 };
            Button metricsButton = new Button { Text = "Metrics", BackColor = System.Drawing.Color.Blue, Width = 100 };
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

            // Output Display
            outputDisplay = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            mainLayout.Controls.Add(outputDisplay, 0, 3);
            mainLayout.SetColumnSpan(outputDisplay, 3); // Span across all columns
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                // Code to load from file can go here
            }
        }

        private void LoadSampleProgram(string level)
        {
            programController.LoadSampleProgram(level);
            UpdateCommandDisplay();
        }

        private void UpdateCommandDisplay()
        {
            commandDisplay.Clear();
            foreach (var commandText in programController.GetCommandDisplayList())
            {
                commandDisplay.AppendText(commandText + "\n");
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            string result = programController.RunProgram();
            outputDisplay.Clear();
            outputDisplay.AppendText(result);
        }

        private void MetricsButton_Click(object sender, EventArgs e)
        {
            outputDisplay.Clear();
            var metrics = programController.GetMetrics();
            outputDisplay.AppendText("Program Metrics: \n");
            foreach (var metric in metrics)
            {
                outputDisplay.AppendText($"{metric.Key}: {metric.Value}\n");
            }
        }

        private void AddCommand(CommandType type, int value)
        {
            programController.AddCommand(type, value);
            UpdateCommandDisplay();
        }
        private void SaveProgram()
        {
            programController.SaveProgram();
            UpdateCommandDisplay();
        }

        private void LoadProgramComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                string selectedOption = comboBox.SelectedItem.ToString();

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

        private void LoadProgramFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                // Load the program from the selected file (implementation will vary)
                // Example: programController.LoadProgramFromFile(filePath);
                UpdateCommandDisplay();
            }
        }
    }
}