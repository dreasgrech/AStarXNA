using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AStarXNA.Heuristics
{
    class EuclideanDistance:IHeuristic
    {
        public string Name
        {
            get { return "Euclidean Distance"; }
        }

        public int GetEstimate(Point source, Point destination)
        {
            return (int)Math.Sqrt(Math.Pow(source.X - destination.X,2) + Math.Pow(source.Y - destination.Y,2));
        }
    }
}