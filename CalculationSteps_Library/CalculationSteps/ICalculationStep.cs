using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationSteps
{
    public interface ICalculationStep<T, P>
    {
        public T previousStep { get; set; }
        public T nextStep { get; set; }

        public P graphPoint { get; set; }

        // Returns next CalculationStep
        public T Calculate();

        public void SetNextStep(T nextStep);
    }
}