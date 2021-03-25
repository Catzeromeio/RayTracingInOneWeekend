using System;
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

        public static float s = 1e-8f; 
        static public bool CloseToZero(Vector3 vec)
        {
            return Math.Abs(vec.X) < s && Math.Abs(vec.Y) < s && Math.Abs(vec.Z) < s;
        }

        // 仅计算入射方向与法线在同一半球的情况
        // eta 为入射区域的折射率
        // eta_primary 为出射区域的折射率
        // 最终计算折射反向
        public static bool Refract(Vector3 inDir,Vector3 normal,float eta, float eta_primary, out Vector3 refractDir)
        {
            var refractRatio = eta / eta_primary;
            var neg_inDirDotNormal = Vector3.Dot(-inDir, normal);
            var checkValue = 1.0f - refractRatio * refractRatio * (1.0f - neg_inDirDotNormal * neg_inDirDotNormal);
            if (checkValue > 0)
            {
                refractDir = ((float)(refractRatio * neg_inDirDotNormal - Math.Sqrt(checkValue))) * normal + refractRatio * inDir;
                return true;
            }

            refractDir = Vector3.Zero;
            return false;
        }
    }
}
