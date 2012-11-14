using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace xWinForms

{

    public class CameraMove : Camera
    {
        public CameraMove(Vector3 position, Vector3 lookat)
            : base(position, lookat)
        {
            VectorUp = Vector3.Up;
        }
        bool firsttime = true;
        bool firsttime1 = true;
        
        public float mX, mY;
        Vector3 rotateVector = Vector3.Zero;
        public Vector3 tVec, tVec1;
        float zoom = 1f, tzoom = 1f;
        public void Update(MouseState mMouseState)
        {
            
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
            float a=0;
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
                if (tVec1.Y == 0)
                {
                    
                  
                    tVec1.Y = 1-tVec1.Y;
                    
                }
                Position = Vector3.Transform(Position-Lookat, Quaternion.CreateFromAxisAngle(tVec1, (float)Math.Sqrt((mMouseState.X - mX) * (mMouseState.X - mX) + (mMouseState.Y - mY) * (mMouseState.Y - mY)) * 0.01f))+Lookat;
                 // if(Position.Z==0)
                
                
                mX = Mouse.GetState().X;
                mY = Mouse.GetState().Y;
            
            }
            if (mMouseState.RightButton == ButtonState.Released)
            {
                firsttime1 = true;
            }
            #endregion                       
            #region Зум

            zoom = mMouseState.ScrollWheelValue / 10 + 1;

            if (tzoom != zoom)
            {
                Position -= (Position - Lookat) / (zoom - tzoom);
                tzoom = zoom;
            }

            #endregion

        }
    }
}
