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
        public static Vector3[] frameBuffer;
        public static Vector3[] depthBuffer;

        public static double timeSinceStartUp_s;
        public static double timeStart_s;
        public static double deltaTime_s;
        public static double timeLastFrameStart_s;
        public static float frameRate;

        public static float focalLength;
        public static Vector3 eyePosition;
        public static float viewPortHeight;

        public static HittableList world;


        public static void GameInit(Form form)
        {
            form.KeyDown += KeyDown;

            graphic = form.CreateGraphics();
            blackBrush = new SolidBrush(Color.Black);
            frameBuffer = new Vector3[Screen.Width * Screen.Height];
            depthBuffer = new Vector3[Screen.Width * Screen.Height];
            blitMap = new Bitmap(Screen.Width , Screen.Height);

            timeStart_s = DateTime.Now.Ticks/ TimeSpan.TicksPerSecond;
            timeSinceStartUp_s = 0;
            deltaTime_s = 0;
            timeLastFrameStart_s = 0;
            frameRate = 60;

            //观察位置到视口平面距离
            //注：这里看向 -z
            focalLength = 1.0f;
            eyePosition = Vector3.Zero;
            viewPortHeight = 2;

            world = new HittableList();
            world.Add(new Sphere(new Vector3(0,0,-1),0.5f));
            world.Add(new Sphere(new Vector3(0,-100.5f,-1),100));
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

        }


        public static void Update()
        {

        }

        public static void Render()
        {
            ClearFrameBuffer();
            ClearDepthBuffer();

            // do renderer logic
            // 右手坐标系
            //注：眼睛看向 -z
            for (int i = 0; i < Screen.Width; i++)
            {
                for (int j = 0; j < Screen.Height; j++)
                {
                    var index = GetIndex(i, j);
                    //计算像素中心
                    var pixelP = new Vector2(i+0.5f, j+0.5f);
                    //中心平移到原点(2D)
                    pixelP = pixelP - new Vector2(Screen.Width / 2.0f, Screen.Height / 2.0f);
                    //缩放到视口大小
                    pixelP = viewPortHeight/Screen.Height * pixelP ;
                    //从眼睛位置到3D空间缩放后像素发射射线
                    var ray = new Ray(eyePosition, new Vector3(pixelP.X, pixelP.Y, -focalLength) - eyePosition);
                    frameBuffer[index] = Ray_Color(ray);
                }
            }

            BlitFrameBufferToScreen();
        }

        public static void ClearFrameBuffer()
        {
            for (int i = 0; i < frameBuffer.Length; i++)
            {
                frameBuffer[i].X = 0;
                frameBuffer[i].Y = 0;
                frameBuffer[i].Z = 0;
            }
        }

        public static void ClearDepthBuffer()
        {
            for (int i = 0; i < depthBuffer.Length; i++)
            {
                depthBuffer[i].X = 0;
                depthBuffer[i].Y = 0;
                depthBuffer[i].Z = 0;
            }
        }

        public static void BlitFrameBufferToScreen()
        {

            for (int i = 0; i < Screen.Width; i++)
            {

                for (int j = 0; j < Screen.Height; j++)
                {
                    var color = frameBuffer[GetIndex(i,j)];
                    color = 255 * color;
                    blitMap.SetPixel(i,Screen.Height-1-j, Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z));
                }
            }

            graphic.DrawImage(blitMap,0,0);
            graphic.DrawString("FPS " + 1/deltaTime_s ,new Font("Arial", 16), blackBrush,0,0);
        }

        public static int GetIndex(int x, int y)
        {
            return Screen.Width * y + x;
        }
        public static Coord IndexToCoord(int index)
        {
            int y = index % Screen.Width;
            int x = index - Screen.Width * y;

            return new Coord(x,y);
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
