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

    public class GameMain
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

        public static float scale = 0;

        //deal with input like this
        public static void KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.A)
            {
                scale += 0.02f;
            }
        }


        public static void Update()
        {

        }

        public static void Render()
        {
            ClearFrameBuffer();
            ClearDepthBuffer();

            //var scale = DateTime.Now.Millisecond/1000.0f;

            // do renderer logic
            for (int i = 0; i < Screen.Width; i++)
            {
                for (int j = 0; j < Screen.Height; j++)
                {
                    var index = GetIndex(i, j);
                    var pixel = frameBuffer[index];
                    pixel.X = i * 1.0f/Screen.Width;
                    pixel.Y = j * 1.0f/Screen.Height;
                    pixel.Z = 0;
                    pixel =  pixel * scale;
                    frameBuffer[index] = pixel;
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
