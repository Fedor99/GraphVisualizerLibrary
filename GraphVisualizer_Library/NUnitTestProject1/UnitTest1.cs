using NUnit.Framework;
using GraphVisualizer;
using CalculationSteps;
using System.Collections.Generic;
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
           // Assert.Pass();
        }

        [Test]
        public void Test()
        {
            Graph graph = new Graph(500, 500, 10, 
                new GraphPointArray[] 
                {

                new GraphPointArray(
                        new List<Position> 
                        {
                            new Position(50, 100),
                            new Position(200, 200)
                        },
                        Color.Red),

                new GraphPointArray(
                        new List<Position>
                        {
                            new Position(50, 150),
                            new Position(20, 250)
                        },
                        Color.Black)
                },
                Color.FromArgb(70, 70, 70),
                "no name"
            );
        }
    }
}