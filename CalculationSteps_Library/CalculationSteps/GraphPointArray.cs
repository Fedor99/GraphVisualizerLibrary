using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CalculationSteps
{
    public class GraphPointArray
    {
        public GraphPointArray(List<Position> points, Color color)
        {
            this.points = points;
            this.color = color;
        }

        private List<Position> points;
        private Color color;

        public List<Position> GetPoints() { return points; }
        public Color GetColor() { return color; }
    }
}
