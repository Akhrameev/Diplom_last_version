using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using WinForms = System.Windows.Forms;

using xWinFormsLib;


namespace xWinForms
{
   
  
    

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Переменные

        VertexDeclaration vertexDeclaration;
        VertexPositionColor[] pointList;
        VertexBuffer vertexBuffer;
       
       
        int points = 6 + 80;
        short[] lineListIndices;




        CameraMove Camera;
        Ship ship = new Ship();

        bool iscameraonship = false;
        int iscameraonshipcount = 0;
        BasicEffect basicEffect;
       
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        #endregion

        StorageDevice sDev;
        IAsyncResult res;

        ChaseCamera camera;
        FotonTrajectror fotonData = new FotonTrajectror();
        fotondata foton = new fotondata();
        BlackHole bhole = new BlackHole();        
        
        //MyCamera mMyCamera;
       
        KeyboardState lastKeyboardState = new KeyboardState();
        KeyboardState currentKeyboardState = new KeyboardState();


        Model shipModel;
        Model fotonModel;
        Model lineModel;
        Model blackholeModel;
        Model fotonTrajectRed;
        Model fotonTrajectGreen;
        Model fotonTrajectYellow;
        Model bh;


        

        static int row = 0;
        static int col = 5;
        string[,] str_array = new string[row, col];
        public float[] spec = new float[200];

        bool flag_save = true;
        Texture2D SpecLine;
        Texture2D ChoseMenuBackGround;
        bool ScienceMod = false;
        bool GameMod = false;
        bool SpecMenu = false;
        bool LibraryMod = false;
        bool readfromfile = false;
        bool EndDraw = true;
        bool d = false;
        Rectangle recChoseMenuBackGround = new Rectangle(0, 0, 1280, 1024);
        int length = 0;
        int spector = 0;
        int l = 0;
        int countnumber = 0;
        int k=0;
        string text = "";
        FormCollection formCollection;
    
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;
            //IsMouseVisible = true;

            camera = new ChaseCamera();

            // Set the camera offsets
            camera.DesiredPositionOffset = new Vector3(0.0f, 3 * 2000.0f, 3 * 3500.0f);
            camera.LookAtOffset = new Vector3(0.0f, 0.0f, 0.0f);

            // Set camera perspective
            camera.NearPlaneDistance = 1.0f;
            camera.FarPlaneDistance = 1000000000.0f;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera.AspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / graphics.GraphicsDevice.Viewport.Height;
            SaveSpecDat();
            
            //foton.FotonMaker();
            #region Camera in Sciense Mod
            Camera = new CameraMove(new Vector3(30f, 30f, 30f), Vector3.Zero);
            Camera.NearPlane = 0.1f;
            Camera.FarPlane = 10000f;
            #endregion
            #region Линии.

            InitializeEffect();
            InitializeLineList();
            InitializePointList();

            #endregion
            base.Initialize();
        }
      
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here            

            spriteBatch = new SpriteBatch(GraphicsDevice);
            ChoseMenuBackGround = Content.Load<Texture2D>("Textures\\Menu\\type2");
            SpecLine = Content.Load<Texture2D>("Textures\\point");
            //ChoseMenuBackGround = Content.Load<Texture2D>("Textures\\Menu\\type0");
            blackholeModel = Content.Load<Model>("Model\\153");
            fotonModel = Content.Load<Model>("Model\\shar");
            shipModel = Content.Load<Model>("Model\\modelShip2");
            
            fotonTrajectRed = Content.Load<Model>("Model\\trajectory_Red");
            //fotonTrajectRed = Content.Load<Model>("Model\\shar");
            fotonTrajectGreen = Content.Load<Model>("Model\\trajectory_Green");
            fotonTrajectYellow = Content.Load<Model>("Model\\trajectory_Yellow");
            bh = Content.Load<Model>("Model\\svinia");
            lineModel = Content.Load<Model>("Model\\128");

            formCollection = new FormCollection(this.Window, Services, ref graphics);


            #region Main Menu

            //Create a new form
            formCollection.Add(new Form("Main menu", "Main menu", new Vector2(820, 620), new Vector2(200, 30), Form.BorderStyle.Sizable));
            formCollection["Main menu"].Style = Form.BorderStyle.Sizable;

            #region Picture Box
            //formCollection["form1"].Controls.Add(new PictureBox("picturebox1", new Vector2(270, 50), @"content\textures\label1.png", 1));
            #endregion
            #region Add a Button
            formCollection["Main menu"].Controls.Add(new Button("button1", new Vector2(200, 250), "content\\textures\\controls\\button\\Amelia_ScienceMode.jpg", 0.25f, Color.White));
            formCollection["Main menu"]["button1"].OnPress += Button1_OnPress;

            formCollection["Main menu"].Controls.Add(new Button("button2", new Vector2(200, 320), "content\\textures\\controls\\button\\Amelia_SpectrumMode_dark.jpg", 0.25f, Color.White));
            formCollection["Main menu"]["button2"].OnPress += Button2_OnPress;

            formCollection["Main menu"].Controls.Add(new Button("button3", new Vector2(200, 390), "content\\textures\\controls\\button\\Amelia_GameMode_dark.jpg", 0.25f, Color.White));
            //formCollection["Main menu"]["button3"].OnPress += 

            formCollection["Main menu"].Controls.Add(new Button("button4", new Vector2(200, 460), "content\\textures\\controls\\button\\Amelia_Help_dark.jpg", 0.25f, Color.White));
            //formCollection["Main menu"]["button4"].OnPress += Button4_OnPress;

            formCollection["Main menu"].Controls.Add(new Button("button5", new Vector2(200, 530), "content\\textures\\controls\\button\\Amelia_Exit.jpg", 0.25f, Color.White));
            formCollection["Main menu"]["button5"].OnPress = Button5_OnPress;
            #endregion

            //Show Main Form
            formCollection["Main menu"].Show();

            #endregion
            //white corner bug for some reason if not focused.. NEED TO FIX
            formCollection["Main menu"].Focus();
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            //Dispose of the form collection
            formCollection.Dispose();
        }

        #region Form Creater
        protected void Create_Help()
        {
            formCollection.Add(new Form("Help", "Help", new Vector2(600, 400), new Vector2(180, 150), Form.BorderStyle.Sizable));
            formCollection["Help"].Style = Form.BorderStyle.Sizable;
            formCollection["Help"].FontName = "kootenay9";


        }
        protected void Create_Sciencemenu()
        {
            formCollection.Add(new Form("Science menu", "Science menu", new Vector2(600, 400), new Vector2(180, 150), Form.BorderStyle.Sizable));
            //formCollection["Science menu"].Visible = true;
            formCollection["Science menu"].Style = Form.BorderStyle.Sizable;
            formCollection["Science menu"].FontName = "pescadero9b_1";

            formCollection["Science menu"].Controls.Add(new Button("button1", new Vector2(310, 230), "content\\textures\\controls\\button\\Amelia_ScienceMode.jpg", 0.15f, Color.White));
            formCollection["Science menu"]["button1"].OnPress += Button1_sciecemenu_OnPress;

            formCollection["Science menu"].Controls.Add(new Button("button2", new Vector2(310, 270), "content\\textures\\controls\\button\\Amelia_Library_Dark.jpg", 0.15f, Color.White));
            formCollection["Science menu"]["button2"].OnPress += Button2_sciecemenu_OnPress;

            formCollection["Science menu"].Controls.Add(new Button("button3", new Vector2(310, 310), "content\\textures\\controls\\button\\Amelia_RandomGenerating.jpg", 0.15f, Color.White));
            formCollection["Science menu"]["button3"].OnPress += Button3_sciecemenu_OnPress;

            formCollection["Science menu"].Controls.Add(new RadiuButtonGroup("radiobuttongroup1", new RadioButton[] { 
                new RadioButton("group1check1", new Vector2(310, 150), "Kerr black hole", true),
                new RadioButton("group1check2", new Vector2(310, 180), "Schachilda black hole", false)}));
            
            //((RadiuButtonGroup)formCollection["Science menu"]["radiobuttongroup1"]).OnChangeSelection = ;
            formCollection["Science menu"].Controls.Add(new Textbox("textbox1", new Vector2(30, 160), 150, 10, ""));
            formCollection["Science menu"].Controls.Add(new Textbox("textbox2", new Vector2(30, 210), 150, 10, ""));
            formCollection["Science menu"].Controls.Add(new Textbox("textbox3", new Vector2(30, 260), 150, 10, ""));
            formCollection["Science menu"].Controls.Add(new Textbox("textbox4", new Vector2(30, 310), 150, 10, ""));
            
            formCollection["Science menu"].Controls.Add(new Label("label1", new Vector2(30, 135), "Input Radius:", Color.TransparentBlack, Color.White, 160, Label.Align.Left));
            formCollection["Science menu"].Controls.Add(new Label("label2", new Vector2(30, 185), "Input Theta:", Color.TransparentBlack, Color.White, 160, Label.Align.Left));
            formCollection["Science menu"].Controls.Add(new Label("label3", new Vector2(30, 235), "Input Phi:", Color.TransparentBlack, Color.White, 160, Label.Align.Left));
            formCollection["Science menu"].Controls.Add(new Label("label4", new Vector2(30, 285), "Input Count:", Color.TransparentBlack, Color.White, 160, Label.Align.Left));

            
            //((Textbox)formCollection["Science menu"]["textbox1"]).Scrollbar = Textbox.Scrollbars.Both;
        }
        protected void Create_ScienceMod()
        {
            //formCollection.Add(new Form("Science Mod", "Science Mod", new Vector2(1260, 924), new Vector2(10, 50), Form.BorderStyle.Sizable));
            
            ScienceMod = true;
            for (int i = 0; i < foton.FotonList.Count - 1; i++)
                foton.FotonList[i].Visible = true;
            //spriteBatch.Begin();
           //spriteBatch.End();
        }
        protected void Create_LibraryMenu()
        {

            formCollection.Add(new Form("LibraryMenu", "LibraryMenu", new Vector2(300, 150), new Vector2(60, 40), Form.BorderStyle.Sizable));
            formCollection["LibraryMenu"].Style = Form.BorderStyle.Sizable;

            formCollection["LibraryMenu"].FontName = "kootenay9_1";
            formCollection["LibraryMenu"].Controls.Add(new Label("label1", new Vector2(60, 20), "Input number:", Color.TransparentBlack, Color.White, 170, Label.Align.Left));
            formCollection["LibraryMenu"].Controls.Add(new Textbox("textbox1", new Vector2(60, 55), 150, 10, ""));

            formCollection["LibraryMenu"].Controls.Add(new Button("button1", new Vector2(30, 90), "content\\textures\\controls\\button\\StartScienceMode.jpg", 0.15f, Color.White));
            formCollection["LibraryMenu"]["button1"].OnPress += Button1_librarymenu_OnPress;
            
            
            //;
           


        }
        protected void Create_FotonListMenu()
        {
            int count = 0;
             
            formCollection.Add(new Form("FotonListMenu", "Foton List", new Vector2(1280, 250), new Vector2(0, 750), Form.BorderStyle.Sizable));
            formCollection["FotonListMenu"].Style = Form.BorderStyle.Sizable;
            formCollection["FotonListMenu"].FontName = "pescadero9b_1";
            for (int i = 0; i < foton.FotonList.Count; i++)
                if (foton.FotonList[i].Visible == true)
                    count++;
            str_array = new string[count,col];
            for (int i = 0; i < count; i++)
                //for (int j = 0; j < col; j++)
                if (foton.FotonList[i].Visible == true)
                {
                    str_array[i, 0] = Convert.ToString(i);
                    str_array[i, 1] = Convert.ToString(foton.FotonList[i].Coners.X);
                    str_array[i, 2] = Convert.ToString(foton.FotonList[i].Coners.Y);
                    str_array[i, 3] = Convert.ToString(foton.FotonList[i].Coners.Z); 
                    str_array[i, 4] = Convert.ToString(foton.FotonList[i].status);
                }
            //        formCollection["FotonListMenu"].Controls.Add(new Listview("listview1", new Vector2(285, 20), new Vector2(600, 350),
            //           str_array));

            //((Listview)formCollection["FotonListMenu"]["listview1"]).HeaderStyle = Listview.ListviewHeaderStyle.Clickable;
            ////((Listview)formCollection["form2"]["listview1"]).HeaderStyle = Listview.ListviewHeaderStyle.None;
            
            ////Create ListView Column Headers (required to draw items)
            //((Listview)formCollection["FotonListMenu"]["listview1"]).ColumnHeader.Add(new Listview.Header("Num", 100));
            //((Listview)formCollection["FotonListMenu"]["listview1"]).ColumnHeader.Add(new Listview.Header("Radius", 100));
            //((Listview)formCollection["FotonListMenu"]["listview1"]).ColumnHeader.Add(new Listview.Header("Phi", 100));
            //((Listview)formCollection["FotonListMenu"]["listview1"]).ColumnHeader.Add(new Listview.Header("Theta", 100));
            //((Listview)formCollection["FotonListMenu"]["listview1"]).ColumnHeader.Add(new Listview.Header("Status", 200));

            ////((Listview)formCollection["form2"]["listview1"]).ColumnHeader[0].OnPress = Listview_ColumnHeader0_OnPress;
            ////((Listview)formCollection["form2"]["listview1"]).ColumnHeader[0].OnRelease = Listview_ColumnHeader0_OnRelease;

            //((Listview)formCollection["FotonListMenu"]["listview1"]).FullRowSelect = true;
            //((Listview)formCollection["FotonListMenu"]["listview1"]).HoverSelection = true;

            formCollection["FotonListMenu"].Controls.Add(new Label("label1", new Vector2(60, 30), "Show only:", Color.TransparentBlack, Color.White, 170, Label.Align.Left));
            formCollection["FotonListMenu"].Controls.Add(new Textbox("textbox1", new Vector2(60, 55), 150, 10, ""));
            formCollection["FotonListMenu"].Controls.Add(new Label("label2", new Vector2(320, 30), "Save:", Color.TransparentBlack, Color.White, 170, Label.Align.Left));
            formCollection["FotonListMenu"].Controls.Add(new Textbox("textbox2", new Vector2(320, 55), 150, 10, ""));

            formCollection["FotonListMenu"].Controls.Add(new Button("button1", new Vector2(30, 100), "content\\textures\\controls\\button\\Amelia_ShowAll.jpg", 0.15f, Color.White));
            formCollection["FotonListMenu"]["button1"].OnPress += Button1_FotonListMenu_OnPress;
            formCollection["FotonListMenu"].Controls.Add(new Button("button2", new Vector2(30, 140), "content\\textures\\controls\\button\\Amelia_ShowOnlyIn.jpg", 0.15f, Color.White));
            formCollection["FotonListMenu"]["button2"].OnPress += Button2_FotonListMenu_OnPress;
            formCollection["FotonListMenu"].Controls.Add(new Button("button3", new Vector2(30, 180), "content\\textures\\controls\\button\\Amelia_ShowOnlyOut.jpg", 0.15f, Color.White));
            formCollection["FotonListMenu"]["button3"].OnPress += Button3_FotonListMenu_OnPress;
            formCollection["FotonListMenu"].Controls.Add(new Button("button4", new Vector2(300, 100), "content\\textures\\controls\\button\\Amelia_ShowOnlyReturned.jpg", 0.15f, Color.White));
            formCollection["FotonListMenu"]["button4"].OnPress += Button4_FotonListMenu_OnPress;
            formCollection["FotonListMenu"].Controls.Add(new Button("button5", new Vector2(300, 140), "content\\textures\\controls\\button\\Amelia_ShowOnlyReturned.jpg", 0.15f, Color.White));
            formCollection["FotonListMenu"]["button5"].OnPress += Button5_FotonListMenu_OnPress;


        }
        #endregion
        #region Form Controls Events
        private void Button1_OnPress(object obj, EventArgs e)
        {
            Create_Sciencemenu();
            formCollection["Science menu"].Show();
            
        }
        private void Button2_OnPress(object obj, EventArgs e)
        {


            
            LoadToFile(null);            
            GraphikSpec();
            SpecMenu = true;
            Create_ScienceMod();
        }
        private void Button4_OnPress(object obj, EventArgs e)
        {
            Create_Help();
            formCollection["Help"].Show();

        }
        private void Button5_OnPress(object obj, EventArgs e)
        {
            Exit();
        }

       
        private void Button1_sciecemenu_OnPress(object obj, EventArgs e)
        {
            
            res = Guide.BeginShowStorageDeviceSelector(PlayerIndex.One, null, null);
            sDev = Guide.EndShowStorageDeviceSelector(res);
            int n;
            int k;
            string fi = null;
            string teta = null;
            string radius = null;

            n = Convert.ToInt16(formCollection["Science menu"]["textbox1"].Text.Length - (-1 + Math.Sqrt(1 + 8 * formCollection["Science menu"]["textbox1"].Text.Length)) / 2);
            for (int i = n; i < formCollection["Science menu"]["textbox1"].Text.Length; i++)
                fi += formCollection["Science menu"]["textbox1"].Text[i];
            formCollection["Science menu"]["textbox1"].Text = fi;
            //formCollection["Science menu"]["textbox1"].Text.Length = fi.Length;
            
            n = Convert.ToInt16(formCollection["Science menu"]["textbox2"].Text.Length - (-1 + Math.Sqrt(1 + 8 * formCollection["Science menu"]["textbox2"].Text.Length)) / 2);
            for (int i = n; i < formCollection["Science menu"]["textbox2"].Text.Length; i++)
                teta += formCollection["Science menu"]["textbox2"].Text[i];
            formCollection["Science menu"]["textbox2"].Text = teta;
            //formCollection["Science menu"]["textbox2"].Text.Length = teta.Length;

            n = Convert.ToInt16(formCollection["Science menu"]["textbox3"].Text.Length - (-1 + Math.Sqrt(1 + 8 * formCollection["Science menu"]["textbox3"].Text.Length)) / 2);
            for (int i = n; i < formCollection["Science menu"]["textbox3"].Text.Length; i++)
                radius += formCollection["Science menu"]["textbox3"].Text[i];
            formCollection["Science menu"]["textbox3"].Text = radius;
            //formCollection["Science menu"]["textbox3"].Text.Length = radius.Length;

            flag_save = true;


            //SaveFile(sDev, fi, teta, alfa);
            
            SaveToFile(fi, teta, radius);
            LoadToFile(null);
            //formCollection["Science Mod"].Show();
            Create_ScienceMod();

        }
        private void Button3_sciecemenu_OnPress(object obj, EventArgs e)
        {

           
            res = Guide.BeginShowStorageDeviceSelector(PlayerIndex.One, null, null);
            sDev = Guide.EndShowStorageDeviceSelector(res);
            int n;
            int k;
            string fi = null;
           

            n = Convert.ToInt16(formCollection["Science menu"]["textbox4"].Text.Length - (-1 + Math.Sqrt(1 + 8 * formCollection["Science menu"]["textbox4"].Text.Length)) / 2);
            for (int i = n; i < formCollection["Science menu"]["textbox4"].Text.Length; i++)
                fi += formCollection["Science menu"]["textbox4"].Text[i];
            formCollection["Science menu"]["textbox4"].Text = fi;
            //formCollection["Science menu"]["textbox4"].Text.Length = fi.Length;
            
            RandomixePsram(Convert.ToInt16(fi));


            Create_ScienceMod();
            Create_FotonListMenu();
            formCollection["FotonListMenu"].Show();
            //SaveFile(sDev, fi, teta, alfa);
            
            //formCollection["Science Mod"].Show();


        }
        private void Button2_sciecemenu_OnPress(object obj, EventArgs e)
        {
            //
            //Сохраняем найденное устройство - позже мы используем его 
            //в процедурах работы с файлами
            //sDev = Guide.EndShowStorageDeviceSelector(res);
            //ScienceMod = true;
           
            Create_LibraryMenu();
            formCollection["LibraryMenu"].Show();
            text = formCollection["LibraryMenu"]["textbox1"].Text;
            LibraryMod = true;
           
            //LoadFile(sDev);


        }

        private void Button1_librarymenu_OnPress(object obj, EventArgs e)
        {
            res = Guide.BeginShowStorageDeviceSelector(PlayerIndex.One, null, null);
            sDev = Guide.EndShowStorageDeviceSelector(res);

            String Text = formCollection["LibraryMenu"]["textbox1"].Text;
            fotonData.FotonDataTr.Clear();
            LoadFile(sDev,Text);
           

        }

        private void Button1_FotonListMenu_OnPress(object obj, EventArgs e)
        {
            for (int i = 0; i < foton.FotonList.Count; i++)
            {
                foton.FotonList[i].Stop = false;
                foton.FotonList[i].schetchik = 0;
                foton.FotonList[i].Visible = true;
            }
        }
        private void Button2_FotonListMenu_OnPress(object obj, EventArgs e)
        {
            for (int i = 0; i < foton.FotonList.Count; i++)
            {
                if (foton.FotonList[i].status == 1)
                {
                    foton.FotonList[i].Stop = false;
                    foton.FotonList[i].schetchik = 0;
                    foton.FotonList[i].Visible = true;
                }
                else
                    foton.FotonList[i].Visible = false;
            }
        }
        private void Button3_FotonListMenu_OnPress(object obj, EventArgs e)
        {
            for (int i = 0; i < foton.FotonList.Count; i++)
            {
                if (foton.FotonList[i].status == 0)
                {
                    foton.FotonList[i].Stop = false;
                    foton.FotonList[i].schetchik = 0;
                    foton.FotonList[i].Visible = true;
                }
                else
                    foton.FotonList[i].Visible = false;
            }
        }
        private void Button4_FotonListMenu_OnPress(object obj, EventArgs e)
        {
            for (int i = 0; i < foton.FotonList.Count; i++)
            {
                if (foton.FotonList[i].status == 2)
                {
                    foton.FotonList[i].Stop = false;
                    foton.FotonList[i].schetchik = 0;
                    foton.FotonList[i].Visible = true;
                }
                else
                    foton.FotonList[i].Visible = false;
            }
        }
        private void Button5_FotonListMenu_OnPress(object obj, EventArgs e)
        {
            int n;            
            string fi = null;

            n = Convert.ToInt16(formCollection["FotonListMenu"]["textbox1"].Text.Length - (-1 + Math.Sqrt(1 + 8 * formCollection["FotonListMenu"]["textbox1"].Text.Length)) / 2);
            for (int i = n; i < formCollection["FotonListMenu"]["textbox1"].Text.Length; i++)
                fi += formCollection["FotonListMenu"]["textbox1"].Text[i];
            formCollection["FotonListMenu"]["textbox1"].Text = null;
            //formCollection["FotonListMenu"]["textbox1"].Text.Length


            foton.FotonList[Convert.ToInt16(fi)].Stop = false;
            foton.FotonList[Convert.ToInt16(fi)].schetchik = 0;
            
        }

        #endregion
        protected override void Update(GameTime gameTime)
        {
            bool g1 = false;
            Vector3 shipPosition = Vector3.Zero;
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            MouseState mMouseState = Mouse.GetState();
            int g = 0;
            int count = 0;
            for (int i = 0; i < foton.FotonList.Count; i++)
                if (foton.FotonList[i].Visible == true)
                    count++;
            str_array = new string[count, col];
            for (int i = 0; i < count; i++)
                //for (int j = 0; j < col; j++)
                if (foton.FotonList[i].Visible == true)
                {
                    str_array[i, 0] = Convert.ToString(i);
                    str_array[i, 1] = Convert.ToString(foton.FotonList[i].Coners.X);
                    str_array[i, 2] = Convert.ToString(foton.FotonList[i].Coners.Y);
                    str_array[i, 3] = Convert.ToString(foton.FotonList[i].Coners.Z);
                    str_array[i, 4] = Convert.ToString(foton.FotonList[i].status);
                }
            // Allows the game to exit
           if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
           
            formCollection.Update(gameTime);
            #region Game Mod
            if (GameMod == true)
            {
                #region Camera in Game Mod
                //UpdateCameraChaseTarget();
                //camera.Update(gameTime, Mouse.GetState());
                #endregion
            }
            #endregion
            #region Science Mod
            if (ScienceMod == true)
            {               
                #region Refresh camera
                basicEffect.World = Camera.World;
                basicEffect.View = Camera.View;
                basicEffect.Projection = Camera.Projection;

                ship.Update(gameTime);
                Camera.Update(mMouseState);
                #endregion
                #region Button C
                if (currentKeyboardState.IsKeyDown(Keys.C))
                {
                    iscameraonship = true;
                    g1 = true;
                    iscameraonshipcount++;
                }
                #endregion
                #region Button V
                if (currentKeyboardState.IsKeyDown(Keys.V))
                {
                    iscameraonship = false;
                    Camera.Position = new Vector3(100f, 1000f, 0f);
                    Camera.Lookat = Vector3.Zero;
                }
                #endregion
                #region Button T
                if (currentKeyboardState.IsKeyDown(Keys.T))
                {

                    for (int i = 0; i < foton.FotonList.Count; i++)
                   
                        foton.FotonList[i].Stop = true;
                       
                    
                    
                }
                #endregion
                #region Button S
                if (currentKeyboardState.IsKeyDown(Keys.S))
                {
                    for (int i = 0; i < foton.FotonList.Count; i++)
                    {
                        foton.FotonList[i].Stop = false;
                        foton.FotonList[i].schetchik = 0;
                        foton.FotonList[i].Visible = true;
                    }
                }
                #endregion
                #region Button R
                if (currentKeyboardState.IsKeyDown(Keys.R))
                {
                   
                }
                #endregion
                #region Camera in Game Mod
                //UpdateCameraChaseTarget();
                //camera.Update(gameTime, Mouse.GetState());
                #endregion
                #region Create model
               // bool flag = false;
               // if (Mouse.GetState().LeftButton == ButtonState.Pressed)
               // {

               //     if (flag == false)
               //     {
               //         for (int i = 0; i < foton.FotonList.Count; i++)
               //         {
               //             foton.FotonList[i].Visible = true;
               //             foton.FotonList[i].Position = mMyCamera.Lookat; ;

               //         }
               //         foton.FotonRandom();
               //      }
               //     flag = true;
               // }
               // if (Mouse.GetState().LeftButton == ButtonState.Released)
               // {
               //     flag = false;
               // }
               #endregion
                if (SpecMenu == true)
                {
                    spector++;
                    if (spector == 50)
                    {
                        GraphikSpec();
                        spector = 0;
                    }

                }
                if (iscameraonship == true)
                {
                    if (g1 == true)
                    {
                        Camera.Position = ship.Position + new Vector3(100f, 100f, 100f);
                        g1 = false;
                    }
                    Camera.Lookat = ship.Position;

                    Camera.Position += ship.Position - shipPosition;
                    shipPosition = ship.Position;
                }
               
                ship.Update(gameTime);                
                DrawObjects(gameTime);
                //int k = 0;
                //if (readfromfile ==true)
                //{
                //    foton.FotonList[0].Points = fotonData.FotonDataTr[k].Position;


                //    if ((d == true) && (k < length - 1))
                //    {
                //        k++;

                //    }
                //    if (k == length - 1)
                //        k = 0;
                //}
               
            }
            #endregion
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            Color DarkDarkBlue = new Color();
            GraphicsDevice device = graphics.GraphicsDevice;
            device.RenderState.PointSize=3;
            DarkDarkBlue.R=28;
            DarkDarkBlue.G=28;
            DarkDarkBlue.B=65;
            //Render the form collection (required before drawing)
            formCollection.Render();
           
            //spriteBatch.
                //spriteBatch.Draw(ChoseMenuBackGround, recChoseMenuBackGround, Color.White);
            
            GraphicsDevice.RenderState.AlphaTestEnable = false;
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            if (ScienceMod == true)
            {
                
                //GraphicsDevice.DrawPrimitives(PrimitiveType.LineStrip,

                
                //device.DrawUserPrimitives(PrimitiveType.LineList,
                device.Clear(DarkDarkBlue);                
                
                #region Axes & dot

                basicEffect.Begin();
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    basicEffect.VertexColorEnabled = true;

                    basicEffect.Alpha = 0.5f;
                    pass.Begin();
                    DrawLineListAxes();
                    pass.End();

                    basicEffect.Alpha = 1f;
                    pass.Begin();
                    DrawLineListOXZ();
                    pass.End();
                }
                basicEffect.End();




                #endregion
                DrawModel(blackholeModel, Matrix.CreateRotationY(0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(bhole.Position));
                DrawModel(bh, Matrix.CreateRotationY(0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(Vector3.Zero));
                DrawModel(shipModel, ship.World);
                
                for (int i = 0; i < foton.FotonList.Count; i++)
                {
                    if ((foton.FotonList[i].Stop == false)&&(foton.FotonList[i].Visible==true))
                    {
                        
                        for (int g = 0; g < foton.FotonList[i].Points.Count; g++)
                           // DrawModel(fotonTrajectRed, Matrix.CreateRotationY(ship.rotation.Y) * Matrix.CreateRotationX(ship.rotation.X) * Matrix.CreateTranslation(new Vector3(foton.FotonList[i].Points[g].X, foton.FotonList[i].Points[g].Y, foton.FotonList[i].Points[g].Z) * new Vector3(50, 50, 50)));

                        {
                            if (foton.FotonList[i].status == 0)
                                DrawModel(fotonTrajectRed, Matrix.CreateRotationY(0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(foton.FotonList[i].Points[g].X, foton.FotonList[i].Points[g].Y, foton.FotonList[i].Points[g].Z) * new Vector3(50, 50, 50)));
                            if (foton.FotonList[i].status == 1)
                                DrawModel(fotonTrajectGreen, Matrix.CreateRotationY(0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(foton.FotonList[i].Points[g].X, foton.FotonList[i].Points[g].Y, foton.FotonList[i].Points[g].Z) * new Vector3(50, 50, 50)));
                            if (foton.FotonList[i].status == 2)
                                DrawModel(fotonTrajectYellow, Matrix.CreateRotationY(0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(foton.FotonList[i].Points[g].X, foton.FotonList[i].Points[g].Y, foton.FotonList[i].Points[g].Z) * new Vector3(50, 50, 50)));
                        }
                        if (foton.FotonList[i].Points.Count != 1)
                        {
                            DrawModel(fotonModel, Matrix.CreateRotationY(0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(new Vector3(foton.FotonList[i].Points[foton.FotonList[i].schetchik].X, foton.FotonList[i].Points[foton.FotonList[i].schetchik].Y, foton.FotonList[i].Points[foton.FotonList[i].schetchik].Z) * new Vector3(50, 50, 50)));
                            foton.FotonList[i].schetchik++;
                            if (foton.FotonList[i].schetchik == foton.FotonList[i].Points.Count - 1)
                              foton.FotonList[i].Stop = true;
                            
                        }
                        //foton.FotonList[i].schetchik = 0;
                    }
                }
        
                //l++;
                //
                //    l = 0;

                #region Траектория
                //if (readfromfile == true) 
                //{
                    
                //}
                #endregion
                spriteBatch.Begin();
                if (ScienceMod == false)
                    spriteBatch.Draw(ChoseMenuBackGround, recChoseMenuBackGround, Color.White);
                if (SpecMenu == true)
                {
                    for (int i = 2; i < 400; i += 2)
                    {
                        DrawLine(spriteBatch, SpecLine, Color.Green, new Vector2(i, spec[i / 2]), new Vector2(i, 200));
                        DrawLine(spriteBatch, SpecLine, Color.Green, new Vector2(i + 1, spec[i / 2]), new Vector2(i + 1, 200));

                    }

                }
                spriteBatch.End();

            }

            //Draw the form collection
            formCollection.Draw();
            
           
            base.Draw(gameTime);
        }
        protected void DrawModel(Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = transforms[mesh.ParentBone.Index] * world;

                    // Use the matrices provided by the chase camera
                    effect.View = Camera.View;

                    effect.Projection = Camera.Projection;
                }
                mesh.Draw();
            }
        }
        protected void DrawObjects(GameTime gameTime)
        {
            int i;

            Components.Clear();


            DrawModel(blackholeModel, Matrix.CreateRotationY(ship.rotation.Y) * Matrix.CreateRotationX(ship.rotation.X) * Matrix.CreateTranslation(bhole.Position));
            
            for (i = 0; i < foton.FotonList.Count; i++)
                if ((foton.FotonList[i].Visible == true) && (foton.FotonList[i].Stop == false))
                    DrawModel(fotonModel, Matrix.CreateRotationY(ship.rotation.Y) * Matrix.CreateRotationX(ship.rotation.X) * Matrix.CreateTranslation(new Vector3(foton.FotonList[i].Points[l].X, foton.FotonList[i].Points[l].Y, foton.FotonList[i].Points[l].Z)));


          
            
                      
            DrawModel(shipModel, ship.World);
           
            
        }
        protected void Recount()
        {
            System.Diagnostics.Process Proc = new System.Diagnostics.Process();
            Proc.StartInfo.FileName = "game.exe";
            Proc.Start();

        }
        #region Bred s load and save
        protected void SaveFile(StorageDevice device, string fi, string teta, string alfa)
        {         
         //Массив байтов для записи в файл
            byte[] fia = new byte[fi.Length];
            byte[] tetaa = new byte[teta.Length];
            byte[] alfaa = new byte[alfa.Length];
            byte[] a = new byte[1];
            

            a[0] = Convert.ToByte(0);
           


            for (byte i = 0; i < fi.Length; i++)
            {
                fia[i] = Convert.ToByte(fi[i]);
            }
            for (byte i = 0; i < teta.Length; i++)
            {
                tetaa[i] = Convert.ToByte(teta[i]);
            }
            for (byte i = 0; i < alfa.Length; i++)
            {
                alfaa[i] = Convert.ToByte(alfa[i]);
            }
           

            //Открываем контейнер для хранения файлов
            //В случае с Windows-играми это - папка с соответствующим
            //именем в папке Мои документы текущего пользователя
            StorageContainer container = device.OpenContainer("Diplom");
            //Соединяем имя файла и имя контейнера
            string filename = Path.Combine(container.Path, "corner" + countnumber + ".txt");
            //Создаем новый файл
            countnumber++;
            if (!File.Exists(filename))
            {

                FileStream file = File.Create(filename);
                //Записываем в файл массив байтов, сгенерированный выше
                file.Write(fia, 0, fi.Length);
                file.Write(a, 0, 1);
                file.Write(tetaa, 0, teta.Length);
                file.Write(a, 0, 1);
                file.Write(alfaa, 0, alfa.Length);

                //закрываем файл
                file.Close();
                this.Window.Title = "Файл создан";
            }
            else
            {
                this.Window.Title = "Файл уже создан";
            }
            
            //Уничтожаем контейнер - только после этого файл будет 
            //сохранен
            container.Dispose();           
       

        }
        protected void LoadFile(StorageDevice device, string name)
        {
            StorageContainer container = device.OpenContainer("Diplom");
            long n = 0, m = 0;
            if (name.Length == 3)
            {
                m = Convert.ToInt64(name);
                n = Convert.ToInt64(name) / 100;
                name = Convert.ToString(m - n * 100);
            }
            if (name.Length == 6)
            {
                m = Convert.ToInt64(name);
                n = Convert.ToInt64(name) / 1000;
                name = Convert.ToString(m - n * 1000);
            }
            if (name.Length == 10)
            {
                m = Convert.ToInt64(name);
                n = Convert.ToInt64(name) / 10000;
                name = Convert.ToString(m - n * 10000);
            }

            string filename = Path.Combine(container.Path, "Trajectory" + name + ".dat");
            int k = -1;
            if (File.Exists(filename))
            {

                FileStream file = File.Open(filename, FileMode.Open);
                double[] Trajectory = new double[file.Length];
                byte[] bytes = new byte[4];
                //Выведем данные, считанные из файла, в заголовок игрового окна
                //this.Window.Title = "Содержимое файла: ";
                string s;
                int d = 14;
                double s1 = 0;
                double s2 = 0;
                double s3 = 0;
                length = (int)(file.Length / (14 * 5));
                int b;
                char c;
                for (int i = 1; i < file.Length + 1; i += d)
                {
                    s = "";

                    for (int j = 0; j < d; j++)
                    {
                        b = file.ReadByte();
                        c = Convert.ToChar(b);
                        //if (k == 1) s1 += c;
                        //if (c == 69)k=1;
                        if (c != 32) s += c;


                    }
                    this.Window.Title += s + " ";
                    string sd = s.Replace(".", ",");
                    sd = sd.Replace("D", "e");
                    k++;
                    if (k == 1)
                    {
                        s1 = Convert.ToDouble(sd);
                        d = 14;

                    }
                    if (k == 2)
                    {
                        s2 = Convert.ToDouble(sd);

                    }
                    if (k == 3)
                    {
                        s3 = Convert.ToDouble(sd);
                        d = 14;
                    }
                    if (k == 4)
                    {
                        d = 14;
                        fotonData.AddFatonDataList(s1, s2, s3);
                        k = -1;
                    }





                    //this.Window.Title += Convert.ToString(f) + "  ";


                }
                file.Close();
                ScienceMod = true;
                readfromfile = true;
            }
            else
            {
                this.Window.Title = "Отсутствует файл для чтения";
                readfromfile = false;
            }
            formCollection["LibraryMenu"]["textbox1"].Text = "";
            container.Dispose();

        }
        #endregion
         public void RandomixePsram(int kolvo)
        {
            Random rnd = new Random();
            float radius = rnd.Next(1, 500) / 100;
             
            for (int i = 0; i < kolvo; i++)
            {

                SaveToFile(Convert.ToString(radius), Convert.ToString(rnd.Next(0, 90)), Convert.ToString(rnd.Next(0, 180)));
                LoadToFile(null);                
            }

        }
         void SaveToFile(string phi, string teta, string radius)
        {
            string text = phi + " " + teta + " " + radius;
            StreamWriter writer = new StreamWriter("par.dat");
            writer.WriteLine(text);
            writer.Close();
            // if(formCollection["Science menu"]["radiobuttongroup1"].OnPress)
            System.Diagnostics.Process Proc = new System.Diagnostics.Process();
            Proc.StartInfo.FileName = "GRMotion2.exe";
            Proc.StartInfo.CreateNoWindow = true;
            Proc.StartInfo.UseShellExecute = false;
            Proc.StartInfo.RedirectStandardOutput = true;
            Proc.StartInfo.Arguments = "> null";
            Proc.Start();
            if (flag_save == true)
            {
                System.Threading.Thread.Sleep(750);
                flag_save = false;
            }
             else
                System.Threading.Thread.Sleep(300);
            
        }
         void SaveSpecDat()
        {
            string text ="   0";
            StreamWriter writer = new StreamWriter("spec.dat");
            for (int i = 0; i < 200; i++)
                writer.WriteLine(text);
            writer.Close();


            writer = new StreamWriter("Dist.dat");
           
                writer.WriteLine("3 87");
            writer.Close();

        }
         void LoadToFile(string t)
         {
             StreamReader reader = new StreamReader("Trajectory" + t + ".dat");
             string line = null;
             string radius = null;
             string teta = null;
             string phi = null;
             string time = null;
             if(SpecMenu==true)
                foton.FotonList.Clear();

             foton.AddFatonList(Convert.ToDouble(time));
             while ((line = reader.ReadLine()) != null)
             {
                 for (int i = 0; i < 56; i++)
                 {

                     if (i < 14)
                         radius += line[i];
                     if ((i > 14) && (i < 28))
                         teta += line[i];
                     if ((i > 28) && (i < 42))
                         phi += line[i];
                     if (i > 42)
                         time += line[i];

                 }
                 //for (int i = 0; i < 56; i+=14)
                 //{ 

                 //}

                 radius = radius.Replace(" ", string.Empty);
                 teta = teta.Replace(" ", string.Empty);
                 phi = phi.Replace(" ", string.Empty);
                 time = time.Replace(" ", string.Empty);

                 radius = radius.Replace(".", ",");
                 teta = teta.Replace(".", ",");
                 phi = phi.Replace(".", ",");
                 time = time.Replace(".", ",");


                 //foton.FotonList[foton.FotonList.Count - 1].Points[foton.FotonList[foton.FotonList.Count - 1].Points.Count - 1] = new Vector4((float)(Convert.ToDouble(radius) * Math.Sin((Math.PI / 180) * Convert.ToDouble(teta)) * Math.Cos((Math.PI / 180) * Convert.ToDouble(phi))), (float)(Convert.ToDouble(radius) * Math.Sin((Math.PI / 180) * Convert.ToDouble(teta)) * Math.Sin((Math.PI / 180) * Convert.ToDouble(phi))), (float)(Convert.ToDouble(radius) * Math.Cos((Math.PI / 180) * Convert.ToDouble(teta))), (float)Convert.ToDouble(time));
                 //if (firsttime == true)
                 //{
                 //    foton.FotonList[foton.FotonList.Count - 1].Coners = new Vector3((float)Convert.ToDouble(radius), (float)Convert.ToDouble(phi), (float)Convert.ToDouble(teta));
                 //    firsttime = false;
                 //}
                 foton.FotonList[foton.FotonList.Count - 1].Points.Add(new Vector4((float)(Convert.ToDouble(radius) * Math.Sin((Math.PI / 180) * Convert.ToDouble(teta)) * Math.Cos((Math.PI / 180) * Convert.ToDouble(phi))), (float)(Convert.ToDouble(radius) * Math.Sin((Math.PI / 180) * Convert.ToDouble(teta)) * Math.Sin((Math.PI / 180) * Convert.ToDouble(phi))), (float)(Convert.ToDouble(radius) * Math.Cos((Math.PI / 180) * Convert.ToDouble(teta))), (float)Convert.ToDouble(time)));
                
                     
                 //new Vector4((float)Convert.ToDouble(radius), (float)Convert.ToDouble(teta), (float)Convert.ToDouble(phi), (float)Convert.ToDouble(time));

                 double k = Convert.ToDouble(phi);
                 line = null;
                 radius = null;
                 teta = null;
                 phi = null;
                 time = null;

             }
             reader.Close();
             if (SpecMenu == false)
             {
                 reader = new StreamReader("total.dat");
                 line = null;
                 string status = null;
             
             
                 while ((line = reader.ReadLine()) != null)
             {


                 for (int i = 0; i < 2; i++)
                     status += line[i];
                 foton.FotonList[foton.FotonList.Count - 1].status = Convert.ToInt16(status);
                 status = null;
             }

             readfromfile = true;
             reader.Close();
             }
             else
                foton.FotonList[foton.FotonList.Count - 1].status=0;
         }

         void GraphikSpec()
         {
             Random rnd = new Random();
             string text = Convert.ToString(rnd.Next(1, 1000000000));
             StreamWriter writer = new StreamWriter("RandNum.dat");
             writer.WriteLine(text);
             writer.Close();

             

             System.Diagnostics.Process Proc = new System.Diagnostics.Process();
             Proc.StartInfo.FileName = "GRMotion3.exe";
             Proc.StartInfo.CreateNoWindow = true;
             Proc.StartInfo.UseShellExecute = false;
             Proc.StartInfo.RedirectStandardOutput = true;
             Proc.StartInfo.Arguments = "> null";
             Proc.Start();
            
             System.Threading.Thread.Sleep(100);

             LoadSpektr();
            
                if (foton.FotonList[foton.FotonList.Count-1].schetchik == 0)
                    LoadToFile(null);
            
             #region Spekt %
             float a = 0;
             int i;
             for (i = 0; i < spec.Length; i++)
                 if (a < spec[i])
                     a = spec[i];
             for (i = 0; i < spec.Length; i++)
                 spec[i] = (float)(200 - spec[i] / a * 100 * (1.24));
             #endregion
         }
         void LoadSpektr()
         {
             StreamReader reader = new StreamReader("spec.dat");
             string line = null;
             string spec1 = null;
             int j = 0;
           
             while ((line = reader.ReadLine()) != null)
             {
                 //for (int i = 0; i < line.; i++)
                 //    spec1 += line[i];
                 spec[j] = (float)Convert.ToDouble(line);
                 
                 //spec1 = null;
                 j++;
             }
             reader.Close();
             
         }

         void DrawLine(SpriteBatch batch, Texture2D blank,Color color, Vector2 point1, Vector2 point2)
         {
             float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
             float length = (point2 - point1).Length();

             batch.Draw(blank, point1, null, color,angle, Vector2.Zero, new Vector2(length/2, 1), SpriteEffects.None, 0);
         } 
        #region Линии и точки
        private void DrawLineListAxes()
        {
            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.LineList,
                 pointList,
                0,  // vertex buffer offset to add to each element of the index buffer
                points,  // number of vertices in pointList
                lineListIndices,  // the index buffer
                6,  // first index element to read
                40   // number of primitives to draw
            );
        }
        private void DrawLineListOXZ()
        {
            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.LineList,
                 pointList,
                0,  // vertex buffer offset to add to each element of the index buffer
                6,  // number of vertices in pointList
                lineListIndices,  // the index buffer
                0,  // first index element to read
                3   // number of primitives to draw
            );

        }
        private void InitializeEffect()
        {
            basicEffect = new BasicEffect(GraphicsDevice, null);


            basicEffect.VertexColorEnabled = true;
            basicEffect.World = Camera.World;
            basicEffect.View = Camera.View;
            basicEffect.Projection = Camera.Projection;
        }
        private void InitializeLineList()
        {
            lineListIndices = new short[(points * 2) - 2];

            for (int i = 0; i < points; i++)
            {
                lineListIndices[i] = (short)(i);
            }
        }
        private void InitializePointList()
        {
            vertexDeclaration = new VertexDeclaration(
                GraphicsDevice,
                VertexPositionColor.VertexElements
                );

            pointList = new VertexPositionColor[points];
            #region Points
            pointList[0] = new VertexPositionColor(
                            new Vector3(-100f, 0f, 0f), Color.Red);
            pointList[1] = new VertexPositionColor(
                            new Vector3(100f, 0f, 0f), Color.Red);
            pointList[2] = new VertexPositionColor(
                            new Vector3(0f, 100f, 0f), Color.Blue);
            pointList[3] = new VertexPositionColor(
                            new Vector3(0f, -100f, 0f), Color.Blue);
            pointList[4] = new VertexPositionColor(
                            new Vector3(0f, 0f, 100f), Color.Yellow);
            pointList[5] = new VertexPositionColor(
                            new Vector3(0f, 0f, -100f), Color.Yellow);

            for (int i = 0; i < 20; i += 2)
            {
                pointList[i + 6] = new VertexPositionColor(
                            new Vector3(-100f, 0f, (float)(-10 - 10 * i / 2)), Color.Green);

                pointList[i + 7] = new VertexPositionColor(
                            new Vector3(100f, 0f, (float)(-10 - 10 * i / 2)), Color.Pink);

            }
            for (int i = 0; i < 20; i += 2)
            {
                pointList[i + 26] = new VertexPositionColor(
                            new Vector3(-100f, 0f, (float)(10 + 10 * i / 2)), Color.Green);
                pointList[i + 27] = new VertexPositionColor(
                            new Vector3(100f, 0f, (float)(10 + 10 * i / 2)), Color.Pink);
            }

            for (int i = 0; i < 20; i += 2)
            {
                pointList[i + 46] = new VertexPositionColor(
                            new Vector3((float)(-10 - 10 * i / 2), 0f, -100f), Color.Green);

                pointList[i + 47] = new VertexPositionColor(
                            new Vector3((float)(-10 - 10 * i / 2), 0f, 100f), Color.Pink);

            }
            for (int i = 0; i < 20; i += 2)
            {
                pointList[i + 66] = new VertexPositionColor(
                            new Vector3((float)(10 + 10 * i / 2), 0f, -100f), Color.Green);
                pointList[i + 67] = new VertexPositionColor(
                            new Vector3((float)(10 + 10 * i / 2), 0f, 100f), Color.Pink);
            }

            #endregion
            vertexBuffer = new VertexBuffer(GraphicsDevice,
                  VertexPositionColor.SizeInBytes * (pointList.Length),
                  BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColor>(pointList);
        }
        #endregion
    }
}