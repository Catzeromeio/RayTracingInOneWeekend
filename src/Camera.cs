using System.Drawing;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    //摄像机局部坐标系为右手坐标系
    //摄像机看向-z
    public class Camera:GameObject
    {
        public Vector3[] frameBuffer;
        public Vector3[] depthBuffer;
        private float focalLength;
        private float viewPortHeight;
        private Size resolution;

        public Size Resolution
        {
            get { return resolution; }
            set
            {
                resolution = value;
                OnResolutionChange();
            }
        }

        public float AspectRatio
        {
            get { return resolution.Width / resolution.Height; }
        }

        public Camera(int resolutionW,int resolutionH,float theFocalLength = 1,float theViewPortHeight = 2)
        {
            resolution = new Size(resolutionW, resolutionH);
            focalLength = theFocalLength;
            viewPortHeight = theViewPortHeight;

            frameBuffer = new Vector3[resolution.Width * resolution.Height];
            depthBuffer = new Vector3[resolution.Width * resolution.Height];
        }

        public void OnResolutionChange()
        {
            frameBuffer = new Vector3[resolution.Width * resolution.Height];
            depthBuffer = new Vector3[resolution.Width * resolution.Height];
        }

        public void Render(HittableList scene)
        {
            //ClearDepthBuffer();
            //ClearFrameBuffer();

            for (int i = 0; i < resolution.Width; i++)
            {
                for (int j = 0; j < resolution.Height; j++)
                {
                    var index = GetIndex(i, j);
                    //计算像素中心
                    var pixelP = new Vector2(i + 0.5f, j + 0.5f);
                    //中心平移到原点(2D)
                    pixelP = pixelP - new Vector2(resolution.Width / 2.0f, resolution.Height / 2.0f);
                    //缩放到视口大小
                    pixelP = viewPortHeight / resolution.Height * pixelP;
                    //从眼睛位置到3D空间缩放后像素发射射线
                    var ray = new Ray(position, new Vector3(pixelP.X, pixelP.Y, position.Z -focalLength) - position);
                    frameBuffer[index] = GameMain.Ray_Color(ray, scene);
                }
            }
        }

        public  void BlitFrameBufferToBitMap(Bitmap outputMap)
        {
            if (outputMap.Width != resolution.Width || outputMap.Height != resolution.Height)
                throw new System.Exception("Wrong size !!!!");

            for (int i = 0; i < resolution.Width; i++)
            {
                for (int j = 0; j < resolution.Height; j++)
                {
                    var color = frameBuffer[GetIndex(i,j)];
                    color = 255 * color;
                    outputMap.SetPixel(i,resolution.Height-1-j, Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z));
                }
            }
        }

        int GetIndex(int x, int y)
        {
            return resolution.Width * y + x;
        }

        public void ClearFrameBuffer()
        {
            for (int i = 0; i < frameBuffer.Length; i++)
            {
                frameBuffer[i].X = 0;
                frameBuffer[i].Y = 0;
                frameBuffer[i].Z = 0;
            }
        }

        public void ClearDepthBuffer()
        {
            for (int i = 0; i < depthBuffer.Length; i++)
            {
                depthBuffer[i].X = 0;
                depthBuffer[i].Y = 0;
                depthBuffer[i].Z = 0;
            }
        }
    }
}
