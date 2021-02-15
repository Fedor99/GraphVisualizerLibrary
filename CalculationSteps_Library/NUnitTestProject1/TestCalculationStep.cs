using System;
using System.Collections.Generic;
using System.Text;
using CalculationSteps;

namespace NUnitTestProject1
{
    public class TestCalculationStep : ICalculationStep<TestCalculationStep, Position>
    {
        public TestCalculationStep previousStep { get; set; }
        public TestCalculationStep nextStep { get; set; }

        public Position graphPoint { get; set; }

        public TestCalculationStep(TestCalculationStep previousStep)
        {
            this.previousStep = previousStep;
        }

        public TestCalculationStep Calculate()
        {
            TestCalculationStep next = new TestCalculationStep(this);
            if(graphPoint == null)
                graphPoint = previousStep.graphPoint;
            graphPoint = new Position(graphPoint.GetX() + 1, graphPoint.GetY() - 1);
            this.nextStep = next;

            return next;
        }

        public void SetNextStep(TestCalculationStep nextStep)
        {
            this.nextStep = nextStep;
        }
    }
}