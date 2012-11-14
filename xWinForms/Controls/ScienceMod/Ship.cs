using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;


namespace xWinForms
{
    class Ship
    {
        
        
        private const float MinimumAltitude = 350.0f;
        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 Up;
        private Vector3 right;
        public Vector3 Right
        {
            get { return right; }
        }
        public bool StopFlag=false;
        private const float RotationRate = 1.5f;
        private const float Mass = 1.0f;
        private const float ThrustForce = 24000.0f;
        private const float DragFactor = 0.97f;
        public Vector3 Velocity;
        public Matrix World
        {
            get { return world; }
        }
        private Matrix world;
        public Vector2 rotation;
     
        public Ship()
        {
            Reset();
        }
        public void Reset()
        {
            Position = new Vector3(300, 0, 0);
            Direction = Vector3.Forward;
            Up = Vector3.Up;
            right = Vector3.Right;
            Velocity = Vector3.Zero;
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            Vector2 rotationAmount = -gamePadState.ThumbSticks.Left;
            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Determine rotation amount from input
            if (StopFlag == false)
            {
                if (keyboardState.IsKeyDown(Keys.Left))
                    rotationAmount.X = 1.0f;
                if (keyboardState.IsKeyDown(Keys.Right))
                    rotationAmount.X = -1.0f;
                if (keyboardState.IsKeyDown(Keys.Up))
                    rotationAmount.Y = -1.0f;
                if (keyboardState.IsKeyDown(Keys.Down))
                    rotationAmount.Y = 1.0f;
            }
            // Scale rotation amount to radians per second
            rotationAmount = rotationAmount * RotationRate * elapsed;

            // Correct the X axis steering when the ship is upside down
            //if (Up.Y < 0)
                //rotationAmount.X = -rotationAmount.X;


            // Create rotation matrix from rotation amount
            Matrix rotationMatrix = Matrix.CreateFromAxisAngle(Right, rotationAmount.Y) * Matrix.CreateRotationZ(rotationAmount.X); //Matrix.CreateRotationY(rotationAmount.X);

            // Rotate orientation vectors
            Direction = Vector3.TransformNormal(Direction, rotationMatrix);
            Up = Vector3.TransformNormal(Up, rotationMatrix);

            // Re-normalize orientation vectors
            // Without this, the matrix transformations may introduce small rounding
            // errors which add up over time and could destabilize the ship.
            Direction.Normalize();
            Up.Normalize();

            // Re-calculate Right
            right = Vector3.Cross(Direction, Up);

            // The same instability may cause the 3 orientation vectors may
            // also diverge. Either the Up or Direction vector needs to be
            // re-computed with a cross product to ensure orthagonality
            Up = Vector3.Cross(Right, Direction);

            
            // Determine thrust amount from input
            float thrustAmount = gamePadState.Triggers.Right;
            if ((keyboardState.IsKeyDown(Keys.Space)) && (StopFlag == false))
                thrustAmount = 1.0f;
            if ((keyboardState.IsKeyDown(Keys.B)) && (StopFlag == false))
                thrustAmount = -1.0f;

            //Position.Z = Math.Max(Position.Z, 400);
            // Calculate force from thrust amount
            Vector3 force = Direction * thrustAmount * ThrustForce;


            // Apply acceleration
            Vector3 acceleration = force / Mass;
            Velocity += acceleration * elapsed;

            // Apply psuedo drag
            Velocity *= DragFactor;

            // Apply velocity
            Position += Velocity * elapsed;

           
            // Reconstruct the ship's world matrix
            world = Matrix.Identity;
            world.Forward = Direction;
            world.Up = Up;
            world.Right = right;
            world.Translation = Position;
            rotation = rotationAmount;
           // foton.Update(gameTime);
            
        }
    }
}
