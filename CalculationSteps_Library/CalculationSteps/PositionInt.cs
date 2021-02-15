using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationSteps
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PositionInt
    {
        public PositionInt(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public PositionInt()
        {
            this.x = 0;
            this.y = 0;
        }


        [JsonProperty]
        public int x { get; set; }
        [JsonProperty]
        public int y { get; set; }

        public int GetX() { return x; }
        public int GetY() { return y; }

        public Position ToPosition()
        {
            return new Position(x, y);
        }

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
            return $"{x}{y}".GetHashCode();
        }
    }
}
