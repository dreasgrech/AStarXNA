using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGameConsole;

namespace AStarXNA.ConsoleCommands
{
    class FullScreenCommand:IConsoleCommand
    {
        private GraphicsDeviceManager graphics;

        public FullScreenCommand(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public string Execute(string[] arguments)
        {
            switch (arguments[0])
            {
                case "on": graphics.IsFullScreen = true; break;
                case "off": graphics.IsFullScreen = false; break;
            }
            graphics.ApplyChanges();
            return "";
        }

        public string Name
        {
            get { return "fullscreen"; }
        }

        public string Description
        {
            get { return "Turns fullscreen on/off; Usage: [on] [off]"; }
        }
    }
}
