using System;
using System.Drawing;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    //摄像机局部坐标系为右手坐标系
    //摄像机看向-z
    public class Camera:GameObject
    {
        static readonly float focalLength = 1;

        private Size resolution;
        private float vFov;

        private Vector3 forward;
        private Vector3 up;
        private Vector3 left;

        private Vector3 leftBottomViewPortCorner;

        public Vector3[] frameBuffer;
        public Vector3[] depthBuffer;
        private float viewPortHeight;
        private float viewPortWidth;

        public int sampleRoot = 2;
        public int rayTracingDepth = 500;

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


        //vFov 度数
        public Camera(Vector3 pos, Vector3 theLookAtPoint,Vector3 theUp, int resolutionW,int resolutionH,float theVFov)
        {
            position = pos;

            forward = Vector3.Normalize(position - theLookAtPoint);
            left = Vector3.Normalize(Vector3.Cross(theUp,forward));
            up = Vector3.Normalize(Vector3.Cross(forward, left));

            resolution = new Size(resolutionW, resolutionH);
            vFov = theVFov;

            viewPortHeight = 2 * ((float)Math.Sin((Utilities.DegreesToRadians(vFov/2))));
            viewPortWidth = viewPortHeight * AspectRatio;

            leftBottomViewPortCorner = pos - (viewPortHeight / 2) * up -(viewPortWidth/2)*left - focalLength * forward;

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
            ClearFrameBuffer();

            for (int i = 0; i < resolution.Width; i++)
            {
                for (int j = 0; j < resolution.Height; j++)
                {
                    var index = GetIndex(i, j);
                    var ss = 1.0f / sampleRoot;
                    var ss_half = ss / 2.0f;

                    //将像素分割成sampleRoot * sampleRoot个像素进行采样
                    for (int si = 0; si < sampleRoot; si++)
                    {
                        for (int sj = 0; sj < sampleRoot; sj++)
                        {
                            //计算像素中心
                            var pixelP = new Vector2(i + si *ss + ss_half , j + sj*ss + ss_half);

                            var ray = GetRay(pixelP.X/resolution.Width,pixelP.Y/resolution.Height);

                            //叠加
                            frameBuffer[index] += GameMain.Ray_Color(ray, scene,rayTracingDepth);
                        }
                    }

                    frameBuffer[index] = 1.0f/(sampleRoot * sampleRoot) * frameBuffer[index];
                    //gamma correction with gamma = 2
                    frameBuffer[index].X = (float)Math.Sqrt(frameBuffer[index].X);
                    frameBuffer[index].Y = (float)Math.Sqrt(frameBuffer[index].Y);
                    frameBuffer[index].Z = (float)Math.Sqrt(frameBuffer[index].Z);
                }
            }
        }

        public Ray GetRay(float u, float v)
        {
            var dir = leftBottomViewPortCorner + u * viewPortWidth * left + v * viewPortHeight * up - position; 
            return new Ray(position,Vector3.Normalize(dir));
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
