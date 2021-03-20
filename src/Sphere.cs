using System;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    public class Sphere:Hittable
    {
        float radius;
        Vector3 center;

        public HitData? CastRay(Ray r, float minT = 0, float maxT = float.MaxValue)
        {
            if (maxT < minT)
                throw new Exception(" maxT is smaller than minT !!!");

            //一元二次方程求解, 射线和球相交
            var a = Vector3.Dot(r.Direction, r.Direction);
            var aMc = r.Origin - center;
            var baMc = Vector3.Dot(r.Direction, aMc);

            var b = 2 * baMc;
            var c = Vector3.Dot(aMc, aMc) - radius * radius;
            var discriminant = b * b - 4 * a * c;

            //无解
            if (discriminant < 0)
                return null;

            HitData hitdata = new HitData();
            var sqrtOfDiscriminant = (float)Math.Sqrt(discriminant);

            //检验最小根是否满足
            var mint = (-b - sqrtOfDiscriminant) / 2 * a;
            if (mint >= minT && mint <= maxT)
            {
                hitdata.t = mint; 
                hitdata.point = r.At(mint);
                //记录光线从里或者外部射入以及法线
                hitdata.SetFaceNormal(r,hitdata.point - center);
            }

            //检验最大根是否满足
            var maxt = (-b + sqrtOfDiscriminant) / 2 * a;
            if (maxt >= minT && maxt <= maxT)
            {
                hitdata.t = maxt; 
                hitdata.point = r.At(maxt);
                //记录光线从里或者外部射入以及法线
                hitdata.SetFaceNormal(r,hitdata.point - center);
            }

            //没有符合条件的交点
            return null;
        }
    }
}
