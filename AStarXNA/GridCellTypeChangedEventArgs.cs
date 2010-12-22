using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarXNA
{
    class GridCellTypeChangedEventArgs:EventArgs
    {
        public GridCellType OldType { get; set; }
        public GridCellType NewType { get; set; }

        public GridCellTypeChangedEventArgs(GridCellType oldType, GridCellType newType)
        {
            OldType = oldType;
            NewType = newType;
        }
    }
}
