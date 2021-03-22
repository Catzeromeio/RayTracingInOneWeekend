using System.Numerics;

namespace RayTracingInOneWeekend
{
    public static class Utilities
    {
        static public float PI = 3.1415926f; 
        static public float DegreesToRadians(float degrees)
        {
            return degrees * PI / 180.0f;
        }

        //计算反射光线方向
        static public Vector3 Reflect(Vector3 inDir, Vector3 normal)
        {
            normal = Vector3.Normalize(normal);
            var reflect = inDir - 2.0f * (Vector3.Dot(normal, inDir)) * normal;
            return Vector3.Normalize(reflect);
        }
    }
}
