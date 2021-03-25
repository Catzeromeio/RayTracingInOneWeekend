using System;
using System.Drawing;
using System.Windows.Forms;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    public static class Screen
    {
        public static int Width = 640;
        public static int Height = 360;
    }

    public struct Coord
    {
        public int x;
        public int y;
        public Coord(int xx, int yy)
        {
            x = xx;
            y = yy;
        }
    }

     public partial class GameMain
    {
        public static Graphics graphic;
        public static SolidBrush blackBrush;
        public static Bitmap blitMap;

        public static Camera mainCamera;

        public static double timeSinceStartUp_s;
        public static double timeStart_s;
        public static double deltaTime_s;
        public static double timeLastFrameStart_s;
        public static float frameRate;

        public static HittableList world;

        public static Random randomer;

        public static void GameInit(Form form)
        {
            form.KeyDown += KeyDown;

            graphic = form.CreateGraphics();
            blackBrush = new SolidBrush(Color.Black);
            blitMap = new Bitmap(Screen.Width , Screen.Height);

            timeStart_s = DateTime.Now.Ticks/ TimeSpan.TicksPerSecond;
            timeSinceStartUp_s = 0;
            deltaTime_s = 0;
            timeLastFrameStart_s = 0;
            frameRate = 60;

            mainCamera = new Camera(Screen.Width,Screen.Height,1,2);
            mainCamera.position = Vector3.Zero;

            var material_ground = new Lambertian(new  Vector3(0.8f, 0.8f, 0.0f));

            var material_left = new Dielectric(1.5f);
            var material_center = new Lambertian(new  Vector3(0.1f, 0.2f, 0.5f));
            var material_right = new Metal(new  Vector3(0.8f, 0.6f, 0.2f),0);

            world = new HittableList();
            world.Add(new Sphere(new Vector3(0,-100.5f,-1),100f,material_ground));
            world.Add(new Sphere(new Vector3(0,0,-1),0.5f,material_center));
            world.Add(new Sphere(new Vector3(-1,0,-1),0.5f,material_left));
            world.Add(new Sphere(new Vector3(-1,0,-1),-0.4f,material_left));
            world.Add(new Sphere(new Vector3(1,0,-1),0.5f,material_right));

            randomer = new Random(1991);
        }

        public static void GameMainProc()
        {
            while (true)
            {
                if (TimeCheck())
                {
                    Update();
                    Render();
                }
            }
        }

        //deal with input like this
        public static void KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.A)
                mainCamera.position += new Vector3(0.1f,0,0);

            if(e.KeyCode == Keys.D)
                mainCamera.position -= new Vector3(0.1f,0,0);

            if(e.KeyCode == Keys.W)
                mainCamera.position += new Vector3(0,0.1f,0);

            if(e.KeyCode == Keys.S)
                mainCamera.position -= new Vector3(0,0.1f,0);
        }


        public static void Update()
        {

        }

        public static void Render()
        {
            // do renderer logic
            mainCamera.Render(world);
            mainCamera.BlitFrameBufferToBitMap(blitMap);
            graphic.DrawImage(blitMap,0,0);
            graphic.DrawString("FPS " + 1/deltaTime_s ,new Font("Arial", 16), blackBrush,0,0);
        }
       
        public static bool TimeCheck()
        {
            var now = DateTime.Now.Ticks / (double)TimeSpan.TicksPerSecond ;
            timeSinceStartUp_s =  now - timeStart_s;
            deltaTime_s = now - timeLastFrameStart_s;
            if (deltaTime_s  > 1.0/frameRate)
            {
                timeLastFrameStart_s = now;
                return true;
            }

            return false;
        }
    }
}
