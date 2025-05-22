using System.ComponentModel.DataAnnotations;
using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics {
    public class Cylinder : IShape, ISectionedShape {
        public Vec3f Location { get; set; }
        public float Radius { get; set; }
        public IFlatSurface Surface { get; set; }

        public Cylinder() {
            Location = 0;
            Radius = 1;
            Surface = new CheckedSphericalSurface();
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            normalFunc = null;
            surfaceFunc = null;

            Vec2f o = new Vec2f(ray.Origin.Z, ray.Origin.X);
            Vec2f d = new Vec2f(ray.Direction.Z, ray.Direction.X);
            Vec2f v = new Vec2f(ray.Origin.Z, ray.Origin.X) - new Vec2f(Location.Z, Location.X);

            float dLength = d.Length;
            d /= dLength;

            float a = d.Dot(d);
            float b = 2 * v.Dot(d);
            float c = v.Dot(v) - Radius * Radius;

            if (!MathUtilities.DetermineRoot(a, b, c, out float t)) return -1;

            Vec2f point = o + d * t;

            t /= dLength;

            normalFunc = () => new Vec3f(point.Y - Location.X, 0, point.X - Location.Z) / Radius;
            surfaceFunc = () => {
                Vec3f n = new Vec3f(point.Y - Location.X, ray.Origin.Y + ray.Direction.Y * t, point.X - Location.Z) / Radius;
                return Surface.GetSurface(GetUV(n));
            };

            return t;
        }
        private Vec2f GetUV(Vec3f n) {
            float u = (float)(Math.Atan2(-n.X, -n.Z) / (2 * Math.PI));

            return new Vec2f(u, n.Y / (float)Math.PI);
        }

        public bool IsInside(Vec3f point) {
            Vec2f v = new Vec2f(point.X - Location.X, point.Z - Location.Z);
            return v.Dot(v) < Radius * Radius;
        }
    }
}
