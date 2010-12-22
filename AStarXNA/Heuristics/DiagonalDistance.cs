using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AStarXNA.Heuristics
{
    class DiagonalDistance:IHeuristic
    {
        public string Name
        {
            get { return "Diagonal Distance"; }
        }

        public int GetEstimate(Point source, Point destination)
        {
            return Math.Max(Math.Abs(destination.X - source.X), Math.Abs(destination.Y - source.Y));
        }
    }
}