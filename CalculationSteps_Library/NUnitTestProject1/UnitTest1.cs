using CalculationSteps;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
            List<Position> allPoints = new List<Position>();
            TestCalculationStep_New stepZero = new TestCalculationStep_New(null, allPoints);
            stepZero.graphPoint = new Position(0, 0);
            TestCalculationStep_New firstStep = new TestCalculationStep_New(stepZero, allPoints);

            int r = 100;

            {
                TestCalculationStep_New thisStep = firstStep;
                for (int i = 0; i < r; i++)
                {
                    thisStep = thisStep.Calculate();
                }
            }

            //{
            //    TestCalculationStep_New thisStep = firstStep;
            //    while (thisStep.nextStep != null)
            //    {
            //        Console.WriteLine("X = " + thisStep.graphPoint.GetX());
            //        Console.WriteLine("Y = " + thisStep.graphPoint.GetY());
            //        thisStep = thisStep.nextStep;
            //    }
            //}

            {
                foreach (Position p in allPoints)
                {
                    Console.WriteLine("X = " + p.GetX());
                    Console.WriteLine("Y = " + p.GetY());
                }
            }
        }
    }
}