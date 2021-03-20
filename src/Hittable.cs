using System.Numerics;

namespace RayTracingInOneWeekend
{
    public struct HitData
    {
        public float t;
        public Vector3 point;

        //外表面法线朝外
        //里表面法线朝内
        public Vector3 normal;

        public bool isHitFromOutside;

        public void SetFaceNormal(Ray ray, Vector3 outwardNormal)
        {
            outwardNormal = Vector3.Normalize(outwardNormal);
            isHitFromOutside = Vector3.Dot(ray.Direction, outwardNormal) < 0;
            normal = isHitFromOutside ? outwardNormal : -outwardNormal;
        }
    }

    public interface Hittable
    {
         //射线测试，如果没有交点或者在[minT,maxT]区间内的交点，返回值为null
         //否则返回最先相交的交点
         bool CastRay(Ray ray,ref HitData hitdata, float minT = 0, float maxT = float.MaxValue);
    }
}
