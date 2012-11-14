using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace xWinForms
{
    class Grid
    {
        Vector2 size;
        int spacing;
        Texture2D pixel;
        Vector2 pixelPos;
        Color color;

        public Grid(Vector2 size, int spacing, Color color)
        {
            this.size = size;
            this.spacing = spacing;
            this.color = color;
        }

        public void Initialize(GraphicsDevice graphics)
        {
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        public void Dispose()
        {
            pixel.Dispose();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < size.Y; y += spacing)
            {
                for (int x = 0; x < size.X; x += spacing)
                {
                    pixelPos.X = x;
                    pixelPos.Y = y;
                    spriteBatch.Draw(pixel, pixelPos, color);
                }

            }
        }
    }
}
