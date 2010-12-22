using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AStarXNA
{
    abstract class GameObject
    {
        protected Game Game { get; set; }
        protected SpriteBatch SpriteBatch { get; set; }
        protected int ScreenWidth
        {
            get
            {
                return Game.Window.ClientBounds.Width;
            }
        }
        protected int ScreenHeight
        {
            get
            {
                return Game.Window.ClientBounds.Height;
            }
        }

        protected GameObject(Game game)
        {
            Game = game;
            SpriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));
        }

        public virtual void Draw(GameTime gameTime){}
        public virtual void Update(GameTime gameTime){}
    }
}
