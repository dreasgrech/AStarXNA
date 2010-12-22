using System;
using System.Collections.Generic;
using System.Linq;
using AStarXNA.ConsoleCommands;
using AStarXNA.Heuristics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using XNAGameConsole;
using XNAMouse;
using XNASingleStroke;

namespace AStarXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private List<GameObject> gameObjects;
        private Grid grid;
        private MouseCursor cursor;
        private AStar aStar;
        private GameConsole console;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameObjects = new List<GameObject>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            grid = new Grid(this, 20) {DrawGridLines = true};
            cursor = new MouseCursor(this, spriteBatch, Content.Load<Texture2D>("cursor"));
            cursor.LeftClick += (o, e) => HandleLeftButton(e.Position);
            cursor.LeftDrag += (o, e) => HandleLeftButton(e.Position);
            cursor.RightClick += (o, e) => HandleRightButton(e.Position);
            cursor.RightDrag += (o, e) => HandleRightButton(e.Position);
            aStar = new AStar(this, grid, new ManhattanDistance());
            aStar.PathFound += (o, e) => {
                console.WriteLine(String.Format("Heuristic: {0}; Path length: {1}; Total Explored length: {2}", aStar.Heuristic.Name, e.TotalPathLength, e.TotalExplored));
                                             foreach (var cell in e.Path)
                                             {
                                                 cell.Type = GridCellType.Path;
                                             }
            };
            Components.Add(cursor);
            gameObjects.Add(grid);
            gameObjects.Add(aStar);

            var singleStroke = new SingleKeyStroke(this);
            singleStroke.KeyDown += (o, e) =>
                                        {
                                            if (console.Opened)
                                            {
                                                return;
                                            }
                                            if (e.Key == Keys.Space)
                                            {
                                                grid.DrawGridLines = !grid.DrawGridLines;
                                            }
                                            if (e.Key == Keys.Enter)
                                            {
                                                grid.Reset();
                                                aStar.Start(); 
                                            }
                                        };
            Components.Add(singleStroke);
            console = new GameConsole(this, spriteBatch, new IConsoleCommand[]
                                                             {
                                                                new DisplayGridlinesCommand(grid),
                                                                new ResetGridCommand(grid),
                                                                new CellSizeCommand(grid),
                                                                new SetHeuristicCommand(aStar),
                                                                new FullScreenCommand(graphics)
                                                             }, new GameConsoleOptions{Prompt=">", FontColor = Color.GreenYellow});

            base.Initialize();
        }

        void HandleLeftButton(Point position)
        {
            var cell = grid.CellAtCoordinate(position.X, position.Y);
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.LeftControl))
            {
                SetCellType(cell, GridCellType.Source);
            }
            else if (state.IsKeyDown(Keys.LeftAlt))
            {
                SetCellType(cell, GridCellType.Destination);
            }
            else
            {
                SetCellType(cell, GridCellType.Unwalkable);
            }
        }

        void HandleRightButton(Point position)
        {
            var cell = grid.CellAtCoordinate(position.X, position.Y);
            SetCellType(cell, GridCellType.Walkable);
        }

        void SetCellType(GridCell cell, GridCellType type)
        {
            if (cell == null)
            {
                return;
            }
            cell.Type = type;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            gameObjects.ForEach(o => o.Update(gameTime));
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            gameObjects.ForEach(o => o.Draw(gameTime));
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
