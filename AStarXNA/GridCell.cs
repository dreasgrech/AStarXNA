using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AStarXNA
{
    internal enum GridCellType
    {
        Walkable,
        Unwalkable,
        Source,
        Destination,
        Path
    }
     
    enum GridCellState
    {
        Open,
        Closed,
        NotVisited
    }

    class GridCell:GameObject
    {
        public GridCellType Type
        {
            get
            {
                return type;
            }
            set
            {
                var oldType = type;
                type = value;
                TypeChanged(this, new GridCellTypeChangedEventArgs(oldType,value));
            }
        }
        public Point Position { get; private set; }
        public Point ScreenCoordinates { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public GridCellState State { get; set; }
        public GridCell Parent { get; set; }
        public int G { get; set; }
        public double H { get; set; }
        public double F
        {
            get
            {
                return G + H;
            }
        }

        private Texture2D pixel;
        private GridCellType type;

        public event EventHandler<GridCellTypeChangedEventArgs> TypeChanged = delegate { };

        public GridCell(Game game, GridCellType type, Point position, Point screenCoordinates, int width, int height) : base(game)
        {
            Type = type;
            Position = position;
            ScreenCoordinates = screenCoordinates;
            Width = width;
            Height = height;
            pixel = new Texture2D(game.GraphicsDevice,1,1,1,TextureUsage.None,SurfaceFormat.Color);
            pixel.SetData(new[] {Color.White});
            State = GridCellState.NotVisited;
        }

        public override void Draw(GameTime gameTime)
        {
            var color = Type == GridCellType.Walkable && State == GridCellState.Closed ? Color.LightSkyBlue : GetColor(Type);
            SpriteBatch.Draw(pixel, new Rectangle(ScreenCoordinates.X,ScreenCoordinates.Y,Width,Height), color);
        }

        public void Reset ()
        {
            if (Type == GridCellType.Path)
            {
                type = GridCellType.Walkable;
            }
            State = GridCellState.NotVisited;
            Parent = null;
            H = G = 0;
        }

        public void Clear()
        {
            Reset();
            Type = GridCellType.Walkable;
        }

        Color GetColor(GridCellType type)
        {
            switch (type)
            {
                case GridCellType.Walkable: return Color.Gray;
                case GridCellType.Unwalkable: return Color.Black;
                case GridCellType.Source: return Color.Green;
                case GridCellType.Destination: return Color.Red;
                case GridCellType.Path: return Color.Yellow;
                default: return Color.CornflowerBlue;
            }
        }

        public bool IsOrthagonalWith(GridCell otherCell)
        {
            return Position.X == otherCell.Position.X || Position.Y == otherCell.Position.Y;
        }

        public override string ToString()
        {
            return String.Format("Position: {0}", Position);
        }

        public IEnumerable<GridCell> GetPath()
        {
            var path = new List<GridCell>();
            var currCell = this;
            while ((currCell = currCell.Parent) != null)
            {
                path.Add(currCell);
            }
            path.Reverse();
            path.RemoveAt(0); //remove the grid source
            return path;
        }
    }
}
