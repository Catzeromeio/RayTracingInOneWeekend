using System.Numerics;
namespace RayTracingInOneWeekend
{
    public class Ray
    {
        public Vector3 Origin { get; set; }
        private Vector3 direction;
        public Vector3 Direction {
            get { return direction; }
            set
            {
                direction = Vector3.Normalize(value);
            }
        }

        public Ray(Vector3 origin, Vector3 dir)
        {
            Origin = origin;
            direction = Vector3.Normalize(dir);
        }

        public Vector3 At(float t)
        {
            return Origin + t * direction;
        }
    }
}
