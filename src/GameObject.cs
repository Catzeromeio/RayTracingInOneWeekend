using System.Numerics;

namespace RayTracingInOneWeekend
{
    public class GameObject
    {
        private Vector3 position;
        private Vector3 velocity;

        public GameObject(Vector3 initPosition, Vector3? theVelocity = null)
        {
            position = initPosition;

            if (theVelocity == null)
                velocity = Vector3.Zero;
            else
                velocity = (Vector3)theVelocity;
        }

        public Vector3 CurrentPosition(float time)
        {
            return position + (time - SimulationTime.startTime) * velocity;
        }
    }
}
