using System;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    public partial class GameMain
    {
        public static bool hit_sphere(Vector3 center, float radius, Ray r)
        {
            //二次方程求解，判断是否有根，有根即为相交
            var a = Vector3.Dot(r.Direction, r.Direction);
            var aMc = r.Origin - center;
            var baMc = Vector3.Dot(r.Direction, aMc);

            var b = 2 * baMc;
            var c = Vector3.Dot(aMc, aMc) - radius * radius;

            return b * b - 4 * a * c > 0;
        }

        public static Vector3 Ray_Color(Ray ray)
        {
            if (hit_sphere(new Vector3(0, 0, -1), 0.5f, ray))
                return new Vector3(1,0,0);

            var t = 0.5f * (ray.Direction.Y + 1.0f);
            return (1.0f - t) * Vector3.One + t * (new Vector3(0.5f, 0.7f, 1.0f));
        }
    }
}
