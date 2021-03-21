using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingInOneWeekend
{
    public static class Utilities
    {
        static public float PI = 3.1415926f; 
        static public float DegreesToRadians(float degrees)
        {
            return degrees * PI / 180.0f;
        }
    }
}
