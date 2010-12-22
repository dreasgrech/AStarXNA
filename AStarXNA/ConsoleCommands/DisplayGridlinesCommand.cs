using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGameConsole;

namespace AStarXNA.ConsoleCommands
{
    class DisplayGridlinesCommand:IConsoleCommand
    {
        private Grid grid;
        public DisplayGridlinesCommand(Grid grid)
        {
            this.grid = grid;
        }
        public string Execute(string[] arguments)
        {
            var arg = arguments[0];
            grid.DrawGridLines = arg == "on" ? true : false;
            return String.Format("Grid lines are now {0}", grid.DrawGridLines ? "on" : "off");
        }

        public string Name
        {
            get { return "gridlines"; }
        }

        public string Description
        {
            get { return "Shows or hides gridlines; Usage: [on] [off] "; }
        }
    }
}
