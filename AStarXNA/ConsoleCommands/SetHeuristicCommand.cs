using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AStarXNA.Heuristics;
using XNAGameConsole;

namespace AStarXNA.ConsoleCommands
{
    class SetHeuristicCommand:IConsoleCommand
    {
        private AStar aStar;
        public SetHeuristicCommand(AStar aStar)
        {
            this.aStar = aStar;
        }
        public string Execute(string[] arguments)
        {
            var isValid = true;
            switch (arguments[0])
            {
                case "diagonal": aStar.Heuristic = new DiagonalDistance(); break;
                case "euclidean": aStar.Heuristic = new EuclideanDistance();break;
                case "manhattan": aStar.Heuristic = new ManhattanDistance();break;
                case "dijkstra": aStar.Heuristic = new Dijkstra();break;
                default: isValid = false; break;
            }
            return isValid ? "Heuristic is now set to " + arguments[0] : "Unknown Heuristic";
        }

        public string Name
        {
            get { return "heuristic"; }
        }

        public string Description
        {
            get { return "Sets the heuristic of the algorithm; Usage: [diagonal] [euclidean] [manhattan] [dijkstra]"; }
        }
    }
}
