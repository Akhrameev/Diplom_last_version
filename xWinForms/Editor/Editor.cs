using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace xWinForms
{
    class Editor
    {
        SpriteBatch spriteBatch;

        Grid grid = new Grid(new Vector2(800, 600), 5, new Color(0, 0, 0, 40));

        public Editor(IServiceProvider services, GraphicsDeviceManager graphics)
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            grid.Initialize(graphics.GraphicsDevice);
        }

        public void Dispose()
        {
            grid.Dispose();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw()
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            
            grid.Draw(spriteBatch);
            
            spriteBatch.End();
        }
    }
}
