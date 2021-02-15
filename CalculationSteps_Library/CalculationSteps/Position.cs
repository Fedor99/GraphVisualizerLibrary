using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationSteps
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Position
    {
        public Position(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Position()
        {
            this.x = 0;
            this.y = 0;
        }


        [JsonProperty]
        public double x { get; set; }
        [JsonProperty]
        public double y { get; set; }

        public double GetX() { return x; }
        public double GetY() { return y; }

        override public string ToString()
        {
            return $"Position({x}, {y})";
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Position p = (Position)obj;
                return (x == p.x) && (y == p.y);
            }
        }

        public override int GetHashCode()
        {
            //if (name == null) return 0;
            return $"{x}{y}".GetHashCode();
        }
    }
}