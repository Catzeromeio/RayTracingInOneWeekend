using System;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    public partial class GameMain
    {
        public static Vector3 Ray_Color(Ray ray)
        {
            var t = 0.5f * (ray.Direction.Y + 1.0f);
            return (1.0f - t) * Vector3.One + t * (new Vector3(0.5f, 0.7f, 1.0f));
        }
    }
}
