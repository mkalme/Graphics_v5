using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics {
    public class Sphere : IShape, ISectionedShape {
        public Vec3f Location { get; set; }
        public float Radius { get; set; }

        public IFlatSurface Surface { get; set; }

        public Sphere() {
            Location = 0;
            Radius = 1;
            Surface = new CheckedSphericalSurface();
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            normalFunc = null;
            surfaceFunc = null;

            Vec3f v = ray.Origin - Location;

            float a = ray.Direction.Dot(ray.Direction);
            float b = 2 * v.Dot(ray.Direction);
            float c = v.Dot(v) - Radius * Radius;

            if (!MathUtilities.DetermineRoot(a, b, c, out float t)) return -1;

            normalFunc = () => (ray.Origin + ray.Direction * t - Location) / Radius;
            surfaceFunc = () => {
                Vec3f n = (ray.Origin + ray.Direction * t - Location) / Radius;
                return Surface.GetSurface(GetUV(n));
            };

            return t;
        }
        private static Vec2f GetUV(Vec3f n) {
            float u = (float)(Math.Atan2(-n.X, -n.Z) / (2 * Math.PI));
            float v = 1 - (0.5F + (float)(Math.Atan2(n.Y, new Vec2f(n.X, n.Z).Length) / Math.PI));

            return new Vec2f(u, v);
        }

        public bool IsInside(Vec3f point) {
            return (point - Location).Dot(point - Location) < Radius * Radius;
        }
    }
}
