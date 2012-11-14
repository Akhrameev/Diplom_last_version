using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace xWinForms
{
    class FotonTrajectror
    {
        public class FotonDataTrajectory
        {
            public double Radius;
            public double teta;
            public double fi;
            
            public Vector3 Position;
            
        }
        public bool stop = false;
        public List<FotonDataTrajectory> FotonDataTr = new List<FotonDataTrajectory>();

        public void AddFatonDataList(double radius, double teta, double fi)
        {
            FotonDataTrajectory temp = new FotonDataTrajectory();
            FotonDataTr.Add(temp);
            FotonDataTr[FotonDataTr.Count - 1].Radius = radius;
            FotonDataTr[FotonDataTr.Count - 1].teta = teta;
            FotonDataTr[FotonDataTr.Count - 1].fi = fi;
            FotonDataTr[FotonDataTr.Count - 1].Position = new Vector3((float)(radius * Math.Sin((Math.PI / 180) * teta) * Math.Cos((Math.PI / 180) * fi)), (float)(radius * Math.Sin((Math.PI / 180) * teta) * Math.Sin((Math.PI / 180) * fi)), (float)(radius * Math.Cos((Math.PI / 180) * teta))); 
       //     float c = (float)Math.Sin((Math.PI/180) * 30);
       //     int a = 10;
       ////
        }
        public void change(double radius1, double teta1, double fi1,int number)
        {
            float fi = (float)fi1;
            float teta = (float)teta1;
            float radius = (float)radius1;
           
        }
        public void CleanTrajectory()
        {

            //List<FotonDataTrajectory> FotonDataTr = new List<FotonDataTrajectory>();
            FotonDataTr.Clear();
        }
    }
    
}