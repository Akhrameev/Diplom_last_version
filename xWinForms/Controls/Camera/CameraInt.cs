using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace xWinForms
{


    public class Camera
    {
        private Matrix world;
        private Matrix view;
        private Matrix projection;
        protected Vector3 position;
        private Vector3 lookat;
        private float aspectRatio;
        private float nearPlane;
        private float farPlane;
        private Vector3 vectorup;
        #region Propeties      
        public  Vector3 VectorUp
        {
            get { return vectorup; }
            set {
                vectorup = value;
                InitCamera();
            }
        }        
        public float FarPlane
        {
            get { return farPlane; }
            set
            {
                farPlane = value;
                InitCamera();
            }
        }
        public float NearPlane
        {
            get { return nearPlane; }
            set
            {
                nearPlane = value;
                InitCamera();
            }
        }
        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                aspectRatio = value;
                InitCamera();
            }
        }
        public  Vector3 Lookat
        {
            get { return lookat; }
            set
            {
                lookat = value;
                InitCamera();
            }
        }
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                InitCamera();
            }
        }
        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }
        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }
        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }
        public Matrix WoldViewProj
        {
            get { return (world * view * projection); }
        }
        #endregion
        #region Public Methods
        public Camera(Vector3 position)
        {
            this.position = position;
            this.lookat = new Vector3 (0,0,0);
            aspectRatio = 640f / 480f;
            nearPlane = 1f;
            farPlane = 1000f;
            vectorup = Vector3.Up;
            InitCamera();
            
        }
        public Camera(Vector3 position, Vector3 lookat)
        {
            this.position = position;
            this.lookat = lookat;
            aspectRatio = 640f / 480f;
            nearPlane = 1f;
            farPlane = 1000f;
            InitCamera();
        }
        #endregion
        #region Public Method
        private void InitCamera()
        {
            this.view = Matrix.CreateLookAt(position, lookat, vectorup);
            this.projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 3, aspectRatio, nearPlane, farPlane);
            this.world = Matrix.Identity;
        }
        #endregion
    }
}
