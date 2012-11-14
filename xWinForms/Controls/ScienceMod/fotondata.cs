using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace xWinForms
{

    public class fotondata
    {

        int n = 1;
        double maxtime = 0;
        public double PartTime = 0;//fignj kotoraj otvechaet za delenij 10 cek na chasti bezrazmernogo vremjni

        public class Foton
        {

            
            public Vector3 MoveTo;
            public Vector3 Coners; 
            public double maxtime;
            public int schetchik = 0;
            public bool Visible = false;
            public bool Stop = true;
            public int status=2;
            public List<Vector4> Points = new List<Vector4>();

        }

        public List<Foton> FotonList = new List<Foton>();

        
        public void AddFatonList(double time)
        {
            Foton temp = new Foton();
            FotonList.Add(temp);

            //FotonList[FotonList.Count - 1].Visible = true;
            FotonList[FotonList.Count - 1].maxtime = time;

            //FotonList[FotonList.Count - 1].Points.Add(Vector4.Zero);
            SeachMaxTime();
            PartTime = maxtime / 10;


        }
        
        public void SeachMaxTime()
        {
            for (int i = 0; i < FotonList.Count; i++)
                if (maxtime < FotonList[i].maxtime)
                    maxtime = FotonList[i].maxtime;
        }

       
#region Bred
    //    public void FotonMaker()
    //    {
    //        for (int i = 0; i < n; i++)
    //        {
    //            AddFatonList();
    //            FotonRandom();
    //            MoveFoton(i);
    //        }
    //    }
    //    public void AddPoints(Vector3 Position, int i)
    //    {
    //        #region Bred...
    //        ////Foton temp = new Foton();
    //        //
    //        //GameTime gtime;
    //        //VertexPositionColor[] vert = new VertexPositionColor[1];
    //        //for (int j = 0; j < FotonList[0].k; j++)
    //        //    vert[j] = FotonList[i].vert[j];
    //        //vert[FotonList[0].k] = new VertexPositionColor();
    //        //FotonList[i].vert = new VertexPositionColor[FotonList[0].k];

    //        //for (int j = 0; j < FotonList[0].k - 1; j++)
    //        //   FotonList[i].vert[j]= vert[j];
    //        //FotonList[i].vert.Add(Position, Color.White);

    //        //
    //        //vert[0] = new VertexPositionColor(Position, Color.White);
    //        //FotonList[i].vert.Add(vert);
    //        #endregion
    //        FotonList[i].Points.Add(Position);
    //        //FotonList[i].mTrajectory.Add();
    //    }
    //    public void FotonRandom()
    //    {
    //        Random rnd = new Random();
    //        for (int i = 0; i < FotonList.Count; i++)
        //            FotonList[i].MoveTo = new Vector3((float)(rnd.NextDouble() - rnd.NextDouble()) * 1, (float)(rnd.NextDouble() - rnd.NextDouble()) * 1, (float)(rnd.NextDouble() - rnd.NextDouble()) * 1);

    //    }
    //    public void MoveFoton(int number)
    //    {
    //        if (FotonList[number].Stop == false)
    //        {
    //            FotonList[number].Position += FotonList[number].MoveTo * 50;
    //            AddPoints(FotonList[number].Position, number);
    //        }
    //    }

    //    public void Update(GameTime gametime)
    //    {
    //        if (FotonList[0].Visible == true)

    //            for (int i = 0; i < FotonList.Count; i++)
    //                MoveFoton(i);
    //    }
    //    public void MakeTrajectory()
    //    {
    //        //vert = new VertexPositionColor[];
    //    }    
#endregion
    }
}