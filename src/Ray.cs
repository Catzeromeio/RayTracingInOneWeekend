using System.Numerics;
namespace RayTracingInOneWeekend
{
    public class Ray:GameObject
    {
        private Vector3 direction;
        public Vector3 Direction {
            get { return direction; }
            set
            {
                direction = Vector3.Normalize(value);
            }
        }

        public Ray(Vector3 pos, Vector3 dir)
        {
            position = pos;
            direction = Vector3.Normalize(dir);
        }

        public Vector3 At(float t)
        {
            return position + t * direction;
        }
    }
}
