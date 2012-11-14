

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace xWinForms
{
    public class ChaseCamera
    {
        
        
        
     
        public Vector3 ChasePosition
        {
            get { return chasePosition; }
            set { chasePosition = value; }
        }
        private Vector3 chasePosition;

        public Vector3 ChaseDirection
        {
            get { return chaseDirection; }
            set { chaseDirection = value; }
        }
        private Vector3 chaseDirection;

        public Vector3 Up
        {
            get { return up; }
            set { up = value; }
        }
        private Vector3 up = Vector3.Up;
        public Vector3 DesiredPositionOffset
        {
            get { return desiredPositionOffset; }
            set { desiredPositionOffset = value; }
        }
        private Vector3 desiredPositionOffset = new Vector3(0, 2.0f, 2.0f);
        public Vector3 DesiredPosition
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return desiredPosition;
            }
        }
        private Vector3 desiredPosition;
        public Vector3 LookAtOffset
        {
            get { return lookAtOffset; }
            set { lookAtOffset = value; }
        }
        private Vector3 lookAtOffset = new Vector3(0, 2.8f, 0);
        public Vector3 LookAt
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return lookAt;
            }
        }
        private Vector3 lookAt;

        
        public float Stiffness
        {
            get { return stiffness; }
            set { stiffness = value; }
        }
        private float stiffness = 1800.0f;
        public float Damping
        {
            get { return damping; }
            set { damping = value; }
        }
        private float damping = 600.0f;
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        private float mass = 50.0f;
        public Vector3 Position
        {
            get { return position; }
        }
        private Vector3 position;
        public Vector3 Velocity
        {
            get { return velocity; }
        }
        private Vector3 velocity;
       


        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }
        private float aspectRatio = 4.0f / 3.0f;
        public float FieldOfView
        {
            get { return fieldOfView; }
            set { fieldOfView = value; }
        }
        private float fieldOfView = MathHelper.ToRadians(45.0f);
        public float NearPlaneDistance
        {
            get { return nearPlaneDistance; }
            set { nearPlaneDistance = value; }
        }
        private float nearPlaneDistance = 1.0f;
        public float FarPlaneDistance
        {
            get { return farPlaneDistance; }
            set { farPlaneDistance = value; }
        }
        private float farPlaneDistance = 10000.0f;


        public Matrix View
        {
            get { return view; }
        }
        private Matrix view;
        public Matrix World
        {
            get { return world; }
        }
        private Matrix world;
        public Matrix Projection
        {
            get { return projection; }
        }
        private Matrix projection;



        
        private void UpdateWorldPositions()
        {
            // Construct a matrix to transform from object space to worldspace
            Matrix transform = Matrix.Identity;
            transform.Forward = ChaseDirection;
            transform.Up = Up;
            transform.Right = Vector3.Cross(Up, ChaseDirection);

            // Calculate desired camera properties in world space
            desiredPosition = ChasePosition + Vector3.TransformNormal(DesiredPositionOffset, transform);
            lookAt = ChasePosition + Vector3.TransformNormal(LookAtOffset, transform);
        }
        private void UpdateMatrices()
        {
            view = Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView,AspectRatio, NearPlaneDistance, FarPlaneDistance);
        }
        public void Reset()
        {
            UpdateWorldPositions();

            // Stop motion
            velocity = Vector3.Zero;

            // Force desired position
            position = desiredPosition;

            UpdateMatrices();
        }
        public void Update(GameTime gameTime,MouseState mMouseState)
        {
            bool firsttime = true;
            
            float mX = 0, mY = 0;
            Vector3 tVec;
           
            int k = 0;
            float zoom = 1f, tzoom = 1f;
            #region Right button

            if (mMouseState.RightButton == ButtonState.Pressed)
            {
                if (firsttime == true)
                {
                    mX = Mouse.GetState().X;
                    mY = Mouse.GetState().Y;
                    firsttime = false;
                }
                Vector3 tmp = ChasePosition - LookAt;
                

                tVec = Vector3.Normalize(new Vector3(tmp.X, 0, tmp.Z));
                tVec = Vector3.Transform(Vector3.Up, Quaternion.CreateFromAxisAngle(tVec, MathHelper.Pi / 2 + (float)(Math.Atan2(Mouse.GetState().X - mX, Mouse.GetState().Y - mY))));
                //InitCamera();

                position = Vector3.Transform(position - lookAt, Quaternion.CreateFromAxisAngle(tVec, (float)Math.Sqrt((mMouseState.X - mX) * (mMouseState.X - mX) + (mMouseState.Y - mY) * (mMouseState.Y - mY)) * 0.01f));// + lookAt;


                mX = Mouse.GetState().X;
                mY = Mouse.GetState().Y;

            }
            if (mMouseState.RightButton == ButtonState.Released)
            {
                firsttime = true;
            }
            #endregion
            #region Scroll Wheel
            
            zoom = mMouseState.ScrollWheelValue / 10 + 1;

            if ((tzoom != zoom) && (k != 5) && (k != -5))
            {
                position -= (position - lookAt) / (zoom - tzoom)*2;
                tzoom = zoom;
                if (tzoom > zoom)
                    k++;
                if (tzoom < zoom)
                    k--;
                
            }

              #endregion

            //mouse.Update(game.GraphicsDevice, View, World, Projection);
            UpdateWorldPositions();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate spring force
            Vector3 stretch = position - desiredPosition;
            Vector3 force = -stiffness * stretch - damping * velocity;

            // Apply acceleration
            Vector3 acceleration = force / mass;
            velocity += acceleration * elapsed;

            // Apply velocity
            position += velocity * elapsed;

            UpdateMatrices();
        }
        public void InitCamera()
        {
            float nearPlane = 1f;
            float farPlane = 1000f;
            this.view = Matrix.CreateLookAt(position, lookAt, Vector3.Up);
            this.projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 3, aspectRatio, nearPlane, farPlane);
            this.world = Matrix.Identity;
        }

       
    }
}
