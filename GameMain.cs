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
            BlitFrameBufferToScreen();
        }

        public static void ClearAllBuffers()
        {
            ClearColorBuffer();
            ClearDepthBuffer();
        }

        public static void ClearColorBuffer()
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

            for (int i = 0; i < Screen.Width-1; i++)
            {

                for (int j = 0; j < Screen.Height-1; j++)
                {
                    var index = Screen.Width * j + i;
                    var color = frameBuffer[index];
                    color = 255 * color;
                    blitMap.SetPixel(i,Screen.Height-1-j, Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z));
                }
            }

            graphic.DrawImage(blitMap,0,0);
        }
    }
}
