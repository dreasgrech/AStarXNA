using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGameConsole;

namespace AStarXNA.ConsoleCommands
{
    class CellSizeCommand:IConsoleCommand
    {
        private Grid grid;
        public CellSizeCommand(Grid grid)
        {
            this.grid = grid;
        }
        public string Execute(string[] arguments)
        {
            grid.Resize(Convert.ToInt32(arguments[0]));
            return "Cells are now size " + grid.CellSize;
        }

        public string Name
        {
            get { return "cellsize"; }
        }

        public string Description
        {
            get { return "Resize the cells"; }
        }
    }
}
