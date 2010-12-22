using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGameConsole;

namespace AStarXNA.ConsoleCommands
{
    class ResetGridCommand:IConsoleCommand
    {
        private Grid grid;
        public ResetGridCommand(Grid grid)
        {
            this.grid = grid;
            
        }
        public string Execute(string[] arguments)
        {
            grid.Clear();
            return "Grid has been resetted";
        }

        public string Name
        {
            get { return "reset"; }
        }

        public string Description
        {
            get { return "Resets the grid"; }
        }
    }
}
