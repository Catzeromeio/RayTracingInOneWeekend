using System;
using System.Collections.Generic;

namespace RayTracingInOneWeekend
{
    public class HittableList
    {
        List<Hittable> hittableList = new List<Hittable>();

        public void Add(Hittable hittale)
        {
            hittableList.Add(hittale);
        }

        public void Remove(Hittable hittale)
        {
            hittableList.Remove(hittale);
        }

        public void Clear()
        {
            hittableList.Clear();
        }

        public bool CastRay(Ray ray, ref HitData hitdata, float minT = 0, float maxT = float.MaxValue)
        {
            float closestSoFar = maxT;

            bool isHitSomething = false;

            foreach (var item in hittableList)
            {
                if(item.CastRay(ray,ref hitdata,minT,closestSoFar))
                {
                    closestSoFar = hitdata.t;
                    isHitSomething = true;
                }
            }

            return isHitSomething;
        }
    }
}
