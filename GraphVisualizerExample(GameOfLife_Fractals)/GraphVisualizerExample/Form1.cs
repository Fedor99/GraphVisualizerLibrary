using CalculationSteps;
using GraphVisualizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphVisualizerExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            flowPanel = new FlowLayoutPanel();
            flowPanel.AutoSize = true;
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            this.Controls.Add(flowPanel);

            flowPanel_Vertical = new FlowLayoutPanel();
            flowPanel_Vertical.AutoSize = true;
            flowPanel_Vertical.FlowDirection = FlowDirection.TopDown;

            textBox = new TextBox();
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.AcceptsReturn = true;
            textBox.AcceptsTab = true;
            textBox.WordWrap = true;
            textBox.AutoSize = true;

            flowPanel_Vertical.Controls.Add(textBox);


            runButton = new Button();
            runButton.Text = "Run";
            runButton.Click += new System.EventHandler(RunButton_Click);

            defaultButton = new Button();
            defaultButton.Text = "Default rules";
            defaultButton.Click += new System.EventHandler(DefaultButton_Click);

            flowPanel_Vertical.Controls.Add(runButton);
            flowPanel_Vertical.Controls.Add(defaultButton);

            picture = new PictureBox
            {
                Name = "pictureBox",
                Size = new Size(500, 500),
                Location = new Point(0, 0),
            };
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            this.flowPanel.Controls.Add(picture);

            this.flowPanel.Controls.Add(flowPanel_Vertical);



            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
        }

        private FlowLayoutPanel flowPanel;
        private FlowLayoutPanel flowPanel_Vertical;
        private TextBox textBox;
        private Button runButton;
        private Button defaultButton; // shows the default JSON text
        public PictureBox picture;
        public volatile Graph graph;

        private GameOfLifeRules defaultRules = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            //OnStart();
        }

        private void Form1_Shown(Object sender, EventArgs e)
        {
            PerformCalculations();
        }

        protected void RunButton_Click(object sender, EventArgs e)
        {
            GameOfLifeRules gameRules = Newtonsoft.Json.JsonConvert.DeserializeObject<GameOfLifeRules>(textBox.Text);
            PerformCalculations(gameRules);
        }
        protected void DefaultButton_Click(object sender, EventArgs e)
        {
            textBox.Text = JsonConvert.SerializeObject(defaultRules, Formatting.Indented);
        }

        public async void PerformCalculations(GameOfLifeRules gameRules = null)
        {
            runButton.Enabled = false;

            GameOfLifeRules rules = new GameOfLifeRules();
            if (gameRules == null)
            {
                rules.plateSize = new PositionInt(60, 60);
                rules.initialLiveCells = new PositionInt[]
                {
                    new PositionInt(1, 0),
                    new PositionInt(2, 0),

                    new PositionInt(0, 1),
                    new PositionInt(0, 2),
                    new PositionInt(0, 3),

                    new PositionInt(1, 2),

                    new PositionInt(1, 4),
                    new PositionInt(2, 4),

                    new PositionInt(3, 1),
                    new PositionInt(3, 3),

                    new PositionInt(4, 2),
                    new PositionInt(5, 2),
                    new PositionInt(6, 2),
                    new PositionInt(7, 2),
                    new PositionInt(8, 2),

                    new PositionInt(7, 1),
                    new PositionInt(7, 3)
                };

                for (int i = 0; i < rules.initialLiveCells.Length; i++)
                {
                    rules.initialLiveCells[i] = 
                        new PositionInt(
                            rules.initialLiveCells[i].GetX() + (rules.plateSize.GetX() / 2), 
                            rules.initialLiveCells[i].GetY() + (rules.plateSize.GetY() / 2));
                }

                //rules.initialLiveCells = new PositionInt[] { new PositionInt(0, 0), new PositionInt(1, 0), new PositionInt(2, 0) };

                rules.neighbours_ToSurvive = new int[] { 2, 3 };
                rules.neighbours_ToDie = new int[] { 0, 1, 4, 5, 6, 7, 8 };
                rules.neighbours_ToCreate = new int[] { 3 };
                rules.numberOfIterations = 50;
                rules.delay = 70;

                defaultRules = rules;
            }
            else
                rules = gameRules;

            textBox.Height = picture.Size.Height;
            textBox.Width = picture.Size.Width;
            textBox.Text = JsonConvert.SerializeObject(rules, Formatting.Indented);

            GameOfLifePlate plate = new GameOfLifePlate(rules);


            for (int i = 0; i < rules.numberOfIterations; i++)
            {
                List<GraphPointArray> cells = new List<GraphPointArray>();
                int x = 0;
                foreach (GameOfLifePlate.Cell[] cellRow in plate.GetCells())
                {
                    int y = 0;
                    foreach (GameOfLifePlate.Cell thisCell in cellRow)
                    {
                        Color cellColor = Color.Black;
                        if (thisCell.isAlive)
                            cellColor = Color.White;

                        cells.Add(new GraphPointArray(new List<Position> { new Position(x, y) }, cellColor));

                        y++;
                    }
                    x++;
                }


                if (graph == null)
                {
                        graph = new Graph(
                        500, 500,
                        4,
                        cells.ToArray(),
                        Color.FromArgb(70, 70, 70),
                        $"",
                        new Size(1, 1),
                        false
                        );
                        graph.StartForm();
                }

                picture.Image = (Image)graph.PaintGraph();

                graph.SetGraphPointArray(cells.ToArray());

                plate.NextIteration();

                picture.Invalidate();
                picture.Update();
                picture.Refresh();

                await Task.Delay(rules.delay);
            }

            runButton.Enabled = true;
        }
    }
}