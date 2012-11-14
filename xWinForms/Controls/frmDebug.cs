using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using xWinFormsLib;

namespace xWinForms
{
    class frmDebug : Form
    {
        Textbox txtOutput = new Textbox("txtOutput", new Vector2(10, 30), 380, 150, "");
        public string Output { get { return txtOutput.Text; } set { txtOutput.Text = value; } }

        public bool invertedOutput = true;

        public frmDebug()
            : base("frmDebug", "Debugging Window", new Vector2(400f, 195f), BorderStyle.Sizable)
        {
            txtOutput.Scrollbar = Textbox.Scrollbars.Both;
            txtOutput.HasFocus = true;

            this.OnResized = Form_OnResized;

            Controls.Add(txtOutput);
            
            Show();
        }

        public void Write(string text)
        {
            txtOutput.Text += text;
        }
        public void WriteLine(string text)
        {
            txtOutput.Text += "\n" + text + "\n"; 
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void Form_OnResized(object obj, EventArgs e)
        {
            txtOutput.Width = Width - 20;
            txtOutput.Height = Height - 55;

            //Need to reinitialize the control..
            txtOutput.Initialize(FormCollection.ContentManager, FormCollection.Graphics.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
