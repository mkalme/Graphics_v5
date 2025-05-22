using System.Drawing;
using Vectors.Vectors3D;

namespace Graphics {
    public class HorizontalPlane : IShape {
        public Vec3f Location { get; set; }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            normalFunc = () => new Vec3f(0, 1, 0);
            surfaceFunc = null;

            if (ray.Direction.Y == 0) return -1;
            float t = (-ray.Origin.Y + Location.Y) / ray.Direction.Y;

            if (t < 0) return -1;

            surfaceFunc = () => {
                Vec3f intersectionPoint = ray.Origin + ray.Direction * t;

                return new SurfaceResult() {
                    Color = Math.Floor(intersectionPoint.X * 5) % 2 == 0 ^ Math.Floor(intersectionPoint.Z * 5) % 2 == 0 ? Color.DarkCyan.ToVec3f() : Color.DarkGray.ToVec3f()
                };
            };

            return t;
        }
    }
}
