using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationSteps
{
    public abstract class AbstractCalculationStep<T, P> : ICalculationStep<T, P>
    {
        protected AbstractCalculationStep(T previousStep, List<P> points)
        {
            this.previousStep = previousStep;
            this.points = points;
        }

        public List<P> points = new List<P>();
        public T previousStep { get; set; }
        public T nextStep { get; set; }

        public P graphPoint { get; set; }

        // Returns next CalculationStep
        public T Calculate()
        {
            T _nextStep = PerformCalculations();
            points.Add(graphPoint);
            SetNextStep(_nextStep);
            return _nextStep;
        }

        // Returns next CalculationStep
        protected abstract T PerformCalculations();

        public void SetNextStep(T nextStep)
        {
            this.nextStep = nextStep;
        }
    }
}