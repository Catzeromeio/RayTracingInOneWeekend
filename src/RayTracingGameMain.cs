using System;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    public partial class GameMain
    {
        public static float hit_sphere(Vector3 center, float radius, Ray r)
        {
            //一元二次方程求解，判断是否有根，有根即为相交
            var a = Vector3.Dot(r.Direction, r.Direction);
            var aMc = r.Origin - center;
            var baMc = Vector3.Dot(r.Direction, aMc);

            var b = 2 * baMc;
            var c = Vector3.Dot(aMc, aMc) - radius * radius;
            var discriminant = b * b - 4 * a * c;

            //无解
            if (discriminant < 0)
                return -1;
            else
                //如果有解, 返回最小t
                return (-b - (float)Math.Sqrt(discriminant)) / 2 * a;
        }


        public static Vector3 sphereCenter = new Vector3(0, 0, -1);
        public static float radius = 0.5f;

        public static Vector3 Ray_Color(Ray ray)
        {
            var t = hit_sphere(sphereCenter, radius, ray);
            if(t >= 0)
            {
                var hit = ray.At(t);
                //+Vector3.One 保证x,y,z为正
                //*0.5保证可以转化为颜色
                return 0.5f * (Vector3.Normalize(hit - sphereCenter) + Vector3.One) ;
            }

            t = 0.5f * (ray.Direction.Y + 1.0f);
            return (1.0f - t) * Vector3.One + t * (new Vector3(0.5f, 0.7f, 1.0f));
        }
    }
}
