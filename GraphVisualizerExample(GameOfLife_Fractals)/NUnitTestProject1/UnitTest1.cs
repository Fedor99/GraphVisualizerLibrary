using NUnit.Framework;
using GraphVisualizerExample;
using CalculationSteps;
using GraphVisualizer;
using System;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading;
using System.Drawing;

namespace NUnitTestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            GameOfLifeRules gofr = new GameOfLifeRules();
            //gofr.neighbours_ToBecomeAlive = new PositionInt[] { new PositionInt(1, 1), new PositionInt(0, 1) };
            gofr.initialLiveCells = new PositionInt[] {
                new PositionInt(1, 1),
                new PositionInt(0, 0)
            };
            gofr.neighbours_ToSurvive = new int[] { 2, 3 };
            gofr.neighbours_ToDie = new int[] { 0, 1, 4, 5, 6, 7, 8 };
            gofr.neighbours_ToCreate = new int[]{ 3 };
            gofr.plateSize = new PositionInt(20, 20);
            //Console.WriteLine(JsonSerializer.Serialize(gofr));
            Console.WriteLine(JsonConvert.SerializeObject(gofr, Formatting.Indented));
        }

        [Test]
        public void Test2()
        {
            GameOfLifePlate.Cell[][] cells = new GameOfLifePlate.Cell[10][];
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new GameOfLifePlate.Cell[10];
            }

            for (int i = -1; i < 15; i++)
            {
                PositionInt p = new PositionInt(i, 0);
                Console.WriteLine("Input : " + p);

                GameOfLifePlate.Cell.GetByPosition(ref p, cells);

                Console.WriteLine("Output : " + p);
            }

            Console.WriteLine("\n\n");

            for (int i = -1; i < 15; i++)
            {
                PositionInt p = new PositionInt(0, i);
                Console.WriteLine("Input : " + p);

                GameOfLifePlate.Cell.GetByPosition(ref p, cells);

                Console.WriteLine("Output : " + p);
            }
        }

        [Test]
        public void Test3_Graphics()
        {
            GraphPointArray[] cells = new GraphPointArray[0];

            GameOfLifeRules rules = new GameOfLifeRules();
            rules.initialLiveCells = new PositionInt[] { new PositionInt(5, 5) };
            rules.neighbours_ToSurvive = new int[] { 2, 3 };
            rules.neighbours_ToDie = new int[] { 0, 1, 4, 5, 6, 7, 8 };
            rules.neighbours_ToCreate = new int[] { 3 };
            rules.plateSize = new PositionInt(20, 20);
            GameOfLifePlate plate = new GameOfLifePlate(rules);

            new Thread(() =>
            {
                Graph graph = new Graph(
                    500, 500,
                    1,
                    cells,
                    Color.FromArgb(70, 70, 70),
                    $"",
                    new Size(1, 1),
                    false
                );
            }).Start();
        }
    }
}