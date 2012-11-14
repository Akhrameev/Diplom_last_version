using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using xWinFormsLib;

namespace xWinForms
{
    class NewControl : Control
    {
        public NewControl(string name, Vector2 position)
            : base(name, position)
        {
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            // TODO: load your content here

            base.Initialize(content, graphics);
        }

        public override void Dispose()
        {
            // TODO: dispose of your content here

            base.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // TODO: Add your update logic here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here
        }
    }
}
