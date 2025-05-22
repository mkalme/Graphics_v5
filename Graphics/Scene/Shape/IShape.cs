using Vectors.Vectors3D;

namespace Graphics
{
    public interface IShape {
        Vec3f Location { get; set; }

        float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc);
    }
}
