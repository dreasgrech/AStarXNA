using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AStarXNA.Heuristics
{
    class Dijkstra:IHeuristic
    {
        public string Name
        {
            get { return "Dijkstra's Algorithm"; }
        }

        public int GetEstimate(Point source, Point destination)
        {
            return 0;
        }
    }
}