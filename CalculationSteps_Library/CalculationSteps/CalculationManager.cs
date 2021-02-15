using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationSteps
{
    public class CalculationManager<T, P>
        where T: AbstractCalculationStep<T, P>
    {
        public CalculationManager() { }

        public List<P> Calculate(int numberOfIterations, T stepZero, T stepOne)
        {
            List<P> allPoints = new List<P>();
            stepZero.points = allPoints;
            stepOne.previousStep = stepZero;
            stepOne.points = allPoints;

            {
                T thisStep = stepOne;
                for (int i = 0; i < numberOfIterations; i++)
                {
                    thisStep = thisStep.Calculate();
                }
            }

            return allPoints;
        }
    }
}