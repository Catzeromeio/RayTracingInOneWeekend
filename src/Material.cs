using System.Numerics;

namespace RayTracingInOneWeekend
{
    //todo add fuzzy
    public abstract class Material
    {
        abstract public bool Scatter(Ray ray, HitData hitdata, out Vector3 attenuation, out Ray scatter);
    }

    public class Lambertian: Material
    {
        Vector3 albedo;
        public Lambertian(Vector3 theAlbedo)
        {
            albedo = theAlbedo;
        }

        public override bool Scatter(Ray ray, HitData hitdata, out Vector3 attenuation, out Ray scatter)
        {
            var scatter_direction = GameMain.RandomInHemisphere(hitdata.normal);
            //todo check is close to zero, if true set normal

            scatter = new Ray(hitdata.point,scatter_direction);
            attenuation = albedo;
            return true;
        }
    }

    public class Metal: Material
    {
        Vector3 albedo;

        public Metal(Vector3 theAlbedo)
        {
            albedo = theAlbedo;
        }

        public override bool Scatter(Ray ray, HitData hitdata, out Vector3 attenuation, out Ray scatter)
        {
            var reflectDir = Utilities.Reflect(ray.Direction,hitdata.normal);
            scatter = new Ray(hitdata.point, reflectDir);
            attenuation = albedo; 
            return (Vector3.Dot(hitdata.normal, scatter.Direction) > 0);
        }
    }
}
