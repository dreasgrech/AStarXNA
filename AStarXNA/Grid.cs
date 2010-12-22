using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AStarXNA
{
    class Grid : GameObject, IEnumerable<GridCell>
    {
        private static readonly Color lineColor = Color.Black;

        public int CellSize { get; private set; }
        public bool DrawGridLines { get; set; }
        public GridCell Source { get; set; }
        public GridCell Destination { get; set; }
        public IEnumerable<GridCell> Path { get; set; }
        public int Width
        {
            get
            {
                return ScreenWidth/CellSize;
            }
        }

        public int Height
        {
            get
            {
                return ScreenHeight/CellSize;
            }
        }

        private Texture2D pixel;
        private GridCell[,] cells;

        public Grid(Game game, int cellSize)
            : base(game)
        {
            CellSize = cellSize;
            pixel = new Texture2D(game.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            pixel.SetData(new[] { lineColor });
            InitCells();
        }

        public override void Draw(GameTime gameTime)
        {
            DrawCells(gameTime);
            if (DrawGridLines)
            {
                DrawLines();
            }
        }

        public GridCell CellAtCoordinate(float x, float y)
        {
            var position = PositionAtCoordinate(x, y);
            return CellAtPosition(position.X, position.Y);
        }

        public GridCell CellAtPosition(int x, int y)
        {
            return IsPositionValid(new Point(x, y)) ? cells[x, y] : null;
        }

        public IEnumerable<GridCell> GetAdjacentCells(GridCell cell)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var adjacentCell = CellAtPosition(cell.Position.X + i, cell.Position.Y + j);
                    if (adjacentCell != null && adjacentCell != cell)
                    {
                        yield return adjacentCell;
                    }
                }
            }
        }

        public List<GridCell> GetValidAdjacentCells(GridCell cell)
        {
            var adjCells = GetAdjacentCells(cell).Where(c => c.State != GridCellState.Closed && c.Type != GridCellType.Unwalkable && CellIsWalkableTo(cell,c));
            return adjCells.ToList();
        }

        public bool CellIsWalkableTo(GridCell source, GridCell destination)
        {
            return !(CellAtPosition(source.Position.X, destination.Position.Y).Type == GridCellType.Unwalkable || CellAtPosition(destination.Position.X, source.Position.Y).Type == GridCellType.Unwalkable);
        }

        public void Reset()
        {
            foreach (var cell in this)
            {
                cell.Reset();
            }
        }

        public void Clear()
        {
            foreach (var cell in this)
            {
                cell.Clear();
            }
        }

        public void Resize(int cellSize)
        {
            CellSize = cellSize;
            InitCells();
            //Clear();
        }


        Point PositionAtCoordinate(float x, float y)
        {
            return new Point((int)Math.Ceiling(Convert.ToDouble(Width * x / ScreenWidth)) - 1, (int)Math.Ceiling(Convert.ToDouble(Height * y / ScreenHeight)) - 1);
        }

        bool IsPositionValid(Point position)
        {
            return position.X >= 0 && position.X < Width && position.Y >= 0 && position.Y < Height;
        }


        void newCell_TypeChanged(object sender, GridCellTypeChangedEventArgs e)
        {
            var cell = (GridCell)sender;
            if (e.NewType == GridCellType.Source)
            {
                if (Source != null && Source != cell)
                {
                    Source.Type = GridCellType.Walkable;
                }
                Source = cell;
            }
            else if (e.NewType == GridCellType.Destination)
            {
                if (Destination != null && Destination != cell)
                {
                    Destination.Type = GridCellType.Walkable;
                }
                Destination = cell;
            }
        }

        void DrawCells(GameTime gameTime)
        {
            foreach (var cell in cells)
            {
                cell.Draw(gameTime);
            }
        }

        void DrawLines()
        {
            for (int i = 1; i < Width; i++)
            {
                SpriteBatch.Draw(pixel, new Rectangle(i * CellSize, 0, 1, Game.Window.ClientBounds.Height), Color.White);
            }
            for (int i = 1; i < Height; i++)
            {
                SpriteBatch.Draw(pixel, new Rectangle(0, i * CellSize, Game.Window.ClientBounds.Width, 1), Color.White);
            }
        }

        void InitCells()
        {
            cells = new GridCell[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var newCell = new GridCell(Game, GridCellType.Walkable, new Point(i,j),  new Point(i * CellSize, j * CellSize), CellSize, CellSize);
                    cells[i, j] = newCell;
                    newCell.TypeChanged += newCell_TypeChanged;
                }
            }
        }
        public IEnumerator<GridCell> GetEnumerator()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    yield return cells[i, j];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}