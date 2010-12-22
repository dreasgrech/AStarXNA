using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AStarXNA.Heuristics
{
    class ManhattanDistance:IHeuristic
    {
        public string Name
        {
            get { return "Manhattan Distance"; }
        }
        public int GetEstimate(Point source, Point destination)
        {
            return Math.Abs(destination.X - source.X) + Math.Abs(destination.Y - source.Y);
        }
    }
}