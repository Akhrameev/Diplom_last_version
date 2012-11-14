using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace xWinForms
{

    public class MyCamera : Camera
    {
        public MyCamera(Vector3 position, Vector3 lookat)
            : base(position, lookat)
        {
            VectorUp = Vector3.Up;
        }
        bool firsttime = true;
        bool firsttime1 = true;
        float angle = 0f;
        bool flagL = true, flagR = true;
        public float mX, mY;
        Vector3 rotateVector = Vector3.Zero;
        public Vector3 tVec, tVec1;
        float zoom = 1f, tzoom = 1f;
        public void Update(MouseState mMouseState)
        {
            #region Бред
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                angle -= 0.05f;
                //    mVectorUp = Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), angle));

                //  View = Matrix.CreateLookAt(Position, Lookat, mVectorUp);
                Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 3, AspectRatio, NearPlane, FarPlane);
                World = Matrix.Identity;
                flagR = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                angle += 0.05f;
                //  mVectorUp = Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), angle));
                // View = Matrix.CreateLookAt(Position, Lookat, mVectorUp);
                Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 3, AspectRatio, NearPlane, FarPlane);
                World = Matrix.Identity;
                flagL = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Left))
            {
                if (flagL == true)
                {
                    //      VectorUp = mVectorUp;
                    angle = 0f;
                    flagL = false;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                if (flagR == true)
                {
                    //         VectorUp = mVectorUp;
                    angle = 0f;
                    flagR = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
            {
                Position += Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), (float)Math.PI / 2));
                Lookat += Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), (float)Math.PI / 2));

            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
            {
                Position += Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), -(float)Math.PI / 2));
                Lookat += Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), -(float)Math.PI / 2));

            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Position += VectorUp;
                Lookat += VectorUp;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                // лол 
                // но это движение вперед 

                //                 Position -= Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), (float)Math.PI));
                //               Lookat -= Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), (float)Math.PI));
                //

                Position += Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), MathHelper.TwoPi / 2));
                Lookat += Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(Vector3.Normalize(Position - Lookat), MathHelper.TwoPi / 2));

                // Position -= VectorUp;
                // Lookat -= VectorUp;

            }

            #endregion

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                Position = new Vector3(30, 30, 0);
                Lookat = Vector3.Zero;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                Position = new Vector3(50, 0, 0);
                Lookat = Vector3.Zero;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
            {
                Position = new Vector3(0, 0, 50);
                Lookat = Vector3.Zero;
            }

            #region Средняя кнопка
            if (mMouseState.MiddleButton == ButtonState.Pressed)
            {
                if (firsttime == true)
                {
                    mX = Mouse.GetState().X;
                    mY = Mouse.GetState().Y;
                    firsttime = false;
                }

                tVec = Vector3.Normalize(new Vector3((Position - Lookat).X, 0, (Position - Lookat).Z));

                rotateVector = Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(tVec, (float)(Math.Atan2(mMouseState.X - mX, mMouseState.Y - mY))));
                rotateVector *= (float)Math.Sqrt((mMouseState.X - mX) * (mMouseState.X - mX) + (mMouseState.Y - mY) * (mMouseState.Y - mY)) * 0.5f;


                Position += rotateVector;
                Lookat += rotateVector;

                mX = Mouse.GetState().X;
                mY = Mouse.GetState().Y;

            }
            if (mMouseState.MiddleButton == ButtonState.Released)
            {
                firsttime = true;
            }
#endregion
         
            #region Правая кнопка
            if (mMouseState.RightButton == ButtonState.Pressed)
            {
                if (firsttime1 == true)
                {
                    mX = Mouse.GetState().X;
                    mY = Mouse.GetState().Y;
                    firsttime1 = false;
                }
                Vector3 tmp = Position - Lookat;


                tVec1 = Vector3.Normalize(new Vector3(tmp.X, 0, tmp.Z));
                tVec1 = Vector3.Transform(VectorUp, Quaternion.CreateFromAxisAngle(tVec1, MathHelper.Pi / 2 + (float)(Math.Atan2(Mouse.GetState().X - mX, Mouse.GetState().Y - mY))));

                  Position = Vector3.Transform(Position-Lookat, Quaternion.CreateFromAxisAngle(tVec1, (float)Math.Sqrt((mMouseState.X - mX) * (mMouseState.X - mX) + (mMouseState.Y - mY) * (mMouseState.Y - mY)) * 0.01f))+Lookat;

                
                
                mX = Mouse.GetState().X;
                mY = Mouse.GetState().Y;
            
            }
            if (mMouseState.RightButton == ButtonState.Released)
            {
                firsttime1 = true;
            }
#endregion
           
            
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad7))
            {
                Position = Vector3.Transform(Position, Quaternion.CreateFromAxisAngle(Vector3.Up, 0.01f));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
            {
                Position = Vector3.Transform(Position, Quaternion.CreateFromAxisAngle(Vector3.Up, -0.01f));
            }
            #region Зум

            zoom = mMouseState.ScrollWheelValue / 10 + 1;

            if (tzoom != zoom)
            {
                Position -= (Position - Lookat) / (zoom - tzoom);
                tzoom = zoom;
            }

            #endregion

            if (mMouseState.LeftButton == ButtonState.Pressed)
            {
         
            }

        }
    }
}
