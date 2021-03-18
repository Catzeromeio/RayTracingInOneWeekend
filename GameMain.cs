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
        public static Bitmap blitMap;
        public static Vector3[] frameBuffer;
        public static Vector3[] depthBuffer;


        public static void GameInit(Form form)
        {
            GameMain.graphic = form.CreateGraphics();
            frameBuffer = new Vector3[Screen.Width * Screen.Height];
            depthBuffer = new Vector3[Screen.Width * Screen.Height];
            blitMap = new Bitmap(Screen.Width , Screen.Height);
        }

        public static void GameMainProc()
        {
            while (true)
            {
                Update();
                Render();
            }
        }

        public static void Update()
        {

        }

        public static void Render()
        {
            ClearAllBuffers();

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
                    frameBuffer[index] = pixel;
                }
            }

            BlitFrameBufferToScreen();
        }

        public static void ClearAllBuffers()
        {
            ClearFrameBuffer();
            ClearDepthBuffer();
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
                frameBuffer[i].Z = 0;
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
    }
}
