using System;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    public partial class GameMain
    {
        public static Vector3 Ray_Color(Ray ray, HittableList scene, int depth = 1000)
        {
            if (depth < 0)
                return Vector3.Zero;

            depth -= 1;

            HitData hitdata = new HitData();
            if(scene.CastRay(ray,ref hitdata,0.001f))
            {
                var randomDir = RandomInHemisphere(hitdata.normal);
                var newRay = new Ray(hitdata.point,randomDir);
                return 0.5f * Ray_Color(newRay,scene, depth);
            }

            var t = 0.5f * (ray.Direction.Y + 1.0f);
            return (1.0f - t) * Vector3.One + t * (new Vector3(0.5f, 0.7f, 1.0f));
        }

        //使用极坐标
        private static Vector3 RandomInUnitSphere()
        {
            float a = (float)randomer.NextDouble() * 2 *Utilities.PI;
            float b = (float)randomer.NextDouble() * 2 *Utilities.PI;
            float r = 1.0f;

            float x = r *(float)(Math.Cos(a) * Math.Sin(b));
            float y = r *(float)(Math.Cos(b) * Math.Sin(a));
            float z = r * (float)Math.Cos(b);

            return new Vector3(x,y,z);
        }

        private static Vector3 RandomInHemisphere(Vector3 normal)
        {
            var random = RandomInUnitSphere();
            if (Vector3.Dot(random, normal) > 0)
                return random;
            else
                return -random;
        }
    }
}
