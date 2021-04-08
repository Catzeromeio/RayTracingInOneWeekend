using System;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    public class Sphere:GameObject,Hittable
    {
        float radius;
        Material material;

        public Sphere(Vector3 initPosition, float sRadius, Material sMaterial, Vector3? theVelocity = null):base(initPosition, theVelocity)
        {
            radius = sRadius;
            material = sMaterial;
        }

        public bool CastRay(Ray r, ref HitData hitdata, float minT = 0, float maxT = float.MaxValue)
        {
            if (maxT < minT)
                throw new Exception(" maxT is smaller than minT !!!");

            //一元二次方程求解, 射线和球相交
            var a = Vector3.Dot(r.Direction, r.Direction);
            var aMc = r.position - CurrentPosition(r.time);
            var baMc = Vector3.Dot(r.Direction, aMc);

            var b = 2 * baMc;
            var c = Vector3.Dot(aMc, aMc) - radius * radius;
            var discriminant = b * b - 4 * a * c;

            //无解
            if (discriminant < 0)
                return false;

            var sqrtOfDiscriminant = (float)Math.Sqrt(discriminant);

            //检验最小根是否满足
            var mint = (-b - sqrtOfDiscriminant) / 2 * a;
            if (mint >= minT && mint <= maxT)
            {
                hitdata.t = mint; 
                hitdata.point = r.At(mint);
                hitdata.material = material;
                //记录光线从里或者外部射入以及法线
                hitdata.SetFaceNormal(r,(hitdata.point - CurrentPosition(r.time))/radius);
                return true;
            }

            //检验最大根是否满足
            var maxt = (-b + sqrtOfDiscriminant) / 2 * a;
            if (maxt >= minT && maxt <= maxT)
            {
                hitdata.t = maxt; 
                hitdata.point = r.At(maxt);
                hitdata.material = material;
                //记录光线从里或者外部射入以及法线
                hitdata.SetFaceNormal(r,(hitdata.point - CurrentPosition(r.time))/radius);
                return true;
            }

            //没有符合条件的交点
            return false;
        }
    }
}
