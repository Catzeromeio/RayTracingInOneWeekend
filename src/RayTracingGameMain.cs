using System;
using System.Numerics;

namespace RayTracingInOneWeekend
{
    public partial class GameMain
    {
        public static Vector3 Ray_Color(Ray ray)
        {
            HitData hitdata = new HitData();
            if(world.CastRay(ray,ref hitdata))
                //+Vector3.One 保证x,y,z为正
                //*0.5保证可以转化为颜色
                return 0.5f * (hitdata.normal + Vector3.One) ;

            var t = 0.5f * (ray.Direction.Y + 1.0f);
            return (1.0f - t) * Vector3.One + t * (new Vector3(0.5f, 0.7f, 1.0f));
        }
    }
}
