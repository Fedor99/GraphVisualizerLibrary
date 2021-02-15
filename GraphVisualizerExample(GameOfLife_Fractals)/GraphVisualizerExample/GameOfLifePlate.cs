using System;
using System.Collections.Generic;
using System.Text;
using CalculationSteps;

namespace GraphVisualizerExample
{
    public class GameOfLifePlate
    {
        public GameOfLifePlate(GameOfLifeRules rules)
        {
            cells = new Cell[rules.plateSize.x][];
            for (int x = 0; x < cells.Length; x++)
            {
                cells[x] = new Cell[rules.plateSize.y];
                for (int y = 0; y < cells[x].Length; y++)
                {
                    cells[x][y] = new Cell();
                }
            }

            this.rules = rules;

            foreach (PositionInt liveCellPosition in rules.initialLiveCells)
            {
                PositionInt position = liveCellPosition;
                Cell cell = Cell.GetByPosition(ref position, cells);
                cell.isAlive = true;
            }
        }

        private GameOfLifeRules rules;
        private Cell[][] cells;

        public void SetRules(GameOfLifeRules rules)
        {
            this.rules = rules;
        }

        public Cell[][] GetCells()
        {
            return cells;
        }

        public void NextIteration()
        {
            Cell[][] result = new Cell[cells.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Cell[cells[0].Length];
            }

            for (int x = 0; x < cells.Length; x++)
            {
                for (int y = 0; y < cells[0].Length; y++)
                {
                    PositionInt position = new PositionInt(x, y);
                    result[x][y] = UpdateCell(position, cells, rules);
                }
            }

            cells = result;
        }


        private Cell UpdateCell(PositionInt position, Cell[][] cells, GameOfLifeRules rules)
        {
            Cell result = new Cell();
            result.isAlive = false;

            int numberOfAliveNeighbours = 0;

            // cell position relative to the current cell,
            // find living neighbours
            for(int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (!((x == 0) && (y == 0))) // if not at the center
                    {
                        // position will then be normalized in Cell.GetByPosition() to create an 'endless' plate
                        PositionInt neighbourPosition = new PositionInt(position.GetX() + x, position.GetY() + y);
                        Cell neighbourCell = Cell.GetByPosition(ref neighbourPosition, cells);
                        if (neighbourCell.isAlive)
                            numberOfAliveNeighbours++;
                    }
                }
            }

            if (CompareWithRule(numberOfAliveNeighbours, rules.neighbours_ToCreate))
                result.isAlive = true;

            if (CompareWithRule(numberOfAliveNeighbours, rules.neighbours_ToSurvive))
                result.isAlive = true;
            else
                if (CompareWithRule(numberOfAliveNeighbours, rules.neighbours_ToDie))
                    result.isAlive = false;

            return result;
        }


        private bool CompareWithRule(int numberOfAliveNeighbours, int[] rule)
        {
            for (int i = 0; i < rule.Length; i++)
            {
                if (numberOfAliveNeighbours == rule[i])
                    return true;
            }
            return false;
        }


        public class Cell
        {
            public Cell()
            { 
            
            }

            public bool isAlive;

            public static Cell GetByPosition(ref PositionInt position, Cell[][] cells)
            {
                Cell result = null;

                // normalize position to create an illusion of an endless, borderless plate
                if (position.GetX() >= cells.Length)
                    position = new PositionInt(position.GetX() - cells.Length, position.GetY());
                if (position.GetY() >= cells[0].Length)
                    position = new PositionInt(position.GetX(), position.GetY() - cells[0].Length);
                if (position.GetX() < 0)
                    position = new PositionInt(cells.Length + position.GetX(), position.GetY());
                if (position.GetY() < 0)
                    position = new PositionInt(position.GetX(), cells[0].Length + position.GetY());


                try
                {
                    result = cells[position.GetX()][position.GetY()];
                }
                catch (IndexOutOfRangeException e)
                {
                    result = Cell.GetByPosition(ref position, cells);
                }
                return result;
            }
        }
    }
}