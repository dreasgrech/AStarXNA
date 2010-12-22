using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AStarXNA.Heuristics;
using Microsoft.Xna.Framework;

namespace AStarXNA
{
    class PathFoundEventArgs : EventArgs
    {
        public IEnumerable<GridCell> Path { get; set; }
        public int TotalPathLength { get; set; }
        public int TotalExplored { get; set; }
        public PathFoundEventArgs(IEnumerable<GridCell> path, int pathLength, int totalExplored)
        {
            Path = path;
            TotalPathLength = pathLength;
            TotalExplored = totalExplored;
        }
    }
    class AStar : GameObject
    {
        public bool IsActive { get; set; }
        public event EventHandler<PathFoundEventArgs> PathFound = delegate { };
        public IHeuristic Heuristic{ get; set;}

        private Grid grid;
        private BinaryHeap<double, GridCell> openList;
        private GridCell currentCell;
        private int totalVisited;

        public AStar(Game game, Grid grid, IHeuristic heuristic)
            : base(game)
        {
            this.grid = grid;
            Heuristic = heuristic;
            IsActive = false;
        }

        public void Start()
        {
            if (grid.Source == null || grid.Destination == null)
            {
                return;
            }
            totalVisited = 0;
            IsActive = true;
            openList = new BinaryHeap<double, GridCell>();
            currentCell = grid.Source;
            currentCell.State = GridCellState.Closed;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }
            foreach (var currentAdjacentCell in grid.GetValidAdjacentCells(currentCell))
            {
                if (currentAdjacentCell.State != GridCellState.Open)
                {
                    currentAdjacentCell.State = GridCellState.Open;
                    currentAdjacentCell.Parent = currentCell;
                    currentAdjacentCell.H = Heuristic.GetEstimate(currentAdjacentCell.Position, grid.Destination.Position) * 10;
                    currentAdjacentCell.H += GetTieBreaker(currentAdjacentCell);
                    //currentAdjacentCell.H *= 1 + 1/1000f;
                    currentAdjacentCell.G = currentCell.G + (currentCell.IsOrthagonalWith(currentAdjacentCell) ? 10 : 14);
                    openList.Insert(currentAdjacentCell.F, currentAdjacentCell);
                }
            }
            if (openList.Count == 0) //A path cannot be found
            {
                IsActive = false;
                return;
            }
            totalVisited++;
            currentCell = openList.RemoveMin();
            currentCell.State = GridCellState.Closed;
            if (grid.Destination.State == GridCellState.Closed) //A path has been found
            {
                var path = currentCell.GetPath();
                IsActive = false;
                PathFound(this, new PathFoundEventArgs(path, path.Count(),totalVisited -1));
            }

            base.Update(gameTime);
        }

        double GetTieBreaker(GridCell cell)
        {
            /*
             * http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html#S12
             */
            int dx1 = cell.Position.X - grid.Destination.Position.X,
                dy1 = cell.Position.Y - grid.Destination.Position.Y,
                dx2 = grid.Source.Position.X - grid.Destination.Position.X,
                dy2 = grid.Source.Position.Y - grid.Destination.Position.Y,
                cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
            return cross * 0.01;
        }
    }
}
