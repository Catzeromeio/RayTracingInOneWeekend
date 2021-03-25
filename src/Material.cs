using System.Numerics;

namespace RayTracingInOneWeekend
{
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
            if (Utilities.CloseToZero(scatter_direction))
                scatter_direction = hitdata.normal;

            scatter = new Ray(hitdata.point,scatter_direction);
            attenuation = albedo;
            return true;
        }
    }

    public class Metal: Material
    {
        Vector3 albedo;
        float fuzzy;

        public Metal(Vector3 theAlbedo, float thefuzzy = 0)
        {
            albedo = theAlbedo;
            fuzzy = thefuzzy;
        }

        public override bool Scatter(Ray ray, HitData hitdata, out Vector3 attenuation, out Ray scatter)
        {
            var reflectDir = Utilities.Reflect(ray.Direction,hitdata.normal);
            scatter = new Ray(hitdata.point, reflectDir + fuzzy * GameMain.RandomInUnitSphere());
            attenuation = albedo; 
            return (Vector3.Dot(hitdata.normal, scatter.Direction) > 0);
        }
    }

    public class Dielectric : Material
    {
        private float eta;

        public Dielectric(float theEta)
        {
            eta = theEta;
        }

        public override bool Scatter(Ray ray, HitData hitdata, out Vector3 attenuation, out Ray scatter)
        {
            //假设无衰减
            attenuation = Vector3.One;
            var theEta = hitdata.isHitFromOutside ? 1.0f : eta;
            var theEta_primary = hitdata.isHitFromOutside ? eta : 1.0f;

            Vector3 refractDir; 

            //尝试折射，如果失败则进行反射
            if(!Utilities.Refract(ray.Direction,hitdata.normal,theEta,theEta_primary,out refractDir))
                refractDir =  Utilities.Reflect(ray.Direction,hitdata.normal);

            scatter = new Ray(hitdata.point,refractDir);
            return true;
        }
    }
}
