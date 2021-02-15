using System;
using System.Collections.Generic;
using System.Text;
using CalculationSteps;
using Newtonsoft.Json;

namespace GraphVisualizerExample
{
    public class GameOfLifeRules
    {
        // definition of neighbours relative to the living sell
        //[JsonProperty("neighbours needed to kill a cell")]
        //public PositionInt[] neighbours_ToDie { get; set; }

        //[JsonProperty("neighbours needed to survive")]
        //public PositionInt[] neighbours_ToSurvive { get; set; }

        //[JsonProperty("neighbours needed to create a new cell")]
        //public PositionInt[] neighbours_ToBecomeAlive { get; set; }

        [JsonProperty("Initial live cells (count starts from left bottom corner)")]
        public PositionInt[] initialLiveCells { get; set; }

        [JsonProperty("number of neighbours needed to kill a cell")]
        public int[] neighbours_ToDie { get; set; }

        [JsonProperty("number of neighbours needed to survive")]
        public int[] neighbours_ToSurvive { get; set; }

        [JsonProperty("number of neighbours needed to create a new cell")]
        public int[] neighbours_ToCreate { get; set; }


        [JsonProperty("number of iterations")]
        public int numberOfIterations { get; set; }

        [JsonProperty("delay")]
        public int delay { get; set; }

        public PositionInt plateSize { get; set; }
    }
}