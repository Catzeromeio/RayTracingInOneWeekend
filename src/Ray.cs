using System.Numerics;
namespace RayTracingInOneWeekend
{
    public class Ray
    {
        public Vector3 position;
        private Vector3 direction;
        public float time;

        public Vector3 Direction {
            get { return direction; }
            set
            {
                direction = Vector3.Normalize(value);
            }
        }

        public Ray(Vector3 pos, Vector3 dir, float rayTime)
        {
            position = pos;
            direction = Vector3.Normalize(dir);
            time = rayTime;
        }

        public Vector3 At(float t)
        {
            return position + t * direction;
        }
    }
}
