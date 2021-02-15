using System;
using System.Collections.Generic;
using System.Text;
using CalculationSteps;

namespace NUnitTestProject1
{
    public class TestCalculationStep_New : AbstractCalculationStep<TestCalculationStep_New, Position>
    {
        public TestCalculationStep_New(TestCalculationStep_New previousStep, List<Position> points) : base(previousStep, points)
        { }

        override protected TestCalculationStep_New PerformCalculations()
        {
            TestCalculationStep_New next = new TestCalculationStep_New(this, points);
            this.graphPoint = new Position(previousStep.graphPoint.GetX() + 1, previousStep.graphPoint.GetY() - 1);
            return next;
        }
    }
}