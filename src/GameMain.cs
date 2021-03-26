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
            randomer = new Random(1991);
            form.KeyDown += KeyDown;

            graphic = form.CreateGraphics();
            blackBrush = new SolidBrush(Color.Black);
            blitMap = new Bitmap(Screen.Width , Screen.Height);

            timeStart_s = DateTime.Now.Ticks/ TimeSpan.TicksPerSecond;
            timeSinceStartUp_s = 0;
            deltaTime_s = 0;
            timeLastFrameStart_s = 0;
            frameRate = 60;

            Vector3 lookFrom = new Vector3(13, 2, 3);
            Vector3 lookAtPoint = new Vector3(0, 0, 0);
            float focusDist = 10;
            mainCamera = new Camera(lookFrom,lookAtPoint,new Vector3(0,1,0),Screen.Width,Screen.Height,20,0.1f,focusDist);

            world =  RandomScene();
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

        static Vector3 RandomColor()
        {
            return new Vector3((float)randomer.NextDouble(),(float)randomer.NextDouble(),(float)randomer.NextDouble());
        }

        static HittableList RandomScene()
        {
            HittableList world = new HittableList();
            var ground_material = new Lambertian(new Vector3(0.5f, 0.5f, 0.5f));
            world.Add(new Sphere(new Vector3(0f, -1000f, 0), 1000, ground_material));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    var choose_mat = randomer.NextDouble();
                    Vector3 center = new Vector3(a + 0.9f * (float)randomer.NextDouble(), 0.2f, b + 0.9f * (float)randomer.NextDouble());

                    if ((center - new Vector3(4, 0.2f, 0)).Length() > 0.9f)
                    {
                        Material sphere_material;

                        if (choose_mat < 0.8)
                        {
                            // diffuse
                            var albedo = RandomColor() * RandomColor();
                            sphere_material = new Lambertian(albedo);
                            world.Add(new Sphere(center, 0.2f, sphere_material));
                        }
                        else if (choose_mat < 0.95)
                        {
                            // metal
                            var albedo = (RandomColor() * 0.5f) + new Vector3(0.5f, 0.5f, 0.5f);
                            var fuzz = (float)randomer.NextDouble() * 0.5f;
                            sphere_material = new Metal(albedo, fuzz);
                            world.Add(new Sphere(center, 0.2f, sphere_material));
                        }
                        else
                        {
                            // glass
                            sphere_material = new Dielectric(1.5f);
                            world.Add(new Sphere(center, 0.2f, sphere_material));
                        }
                    }
                }

            }
            var material1 = new Dielectric(1.5f);
            world.Add(new Sphere(new Vector3(0, 1, 0), 1.0f, material1));

            var material2 = new Lambertian(new Vector3(0.4f, 0.2f, 0.1f));
            world.Add(new Sphere(new Vector3(-4, 1, 0), 1.0f, material2));

            var material3 = new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0);
            world.Add(new Sphere(new Vector3(4, 1, 0), 1.0f, material3));

            return world;
        }
    }
}
