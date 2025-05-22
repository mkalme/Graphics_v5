using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics {
    public class Cone : IShape, ISectionedShape {
        public Vec3f Location { get; set; }
        public Vec3f V { get; set; }
        
        public float HalfAngle {
            get => _halfAngle;
            set {
                _halfAngle = value;

                _cosine = (float)Math.Cos(value);
                _rotation = Rotation.FromRadians(value);
            }
        }
        private float _halfAngle;

        private float _cosine;
        private Rotation _rotation;

        public IFlatSurface Surface { get; set; }

        public Cone() {
            Location = 0;
            HalfAngle = (float)(Math.PI / 8);
            Surface = new CheckedSphericalSurface();
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            normalFunc = null;
            surfaceFunc = null;

            Vec3f co = ray.Origin - Location;

            float a = ray.Direction.Dot(V) * ray.Direction.Dot(V) - _cosine * _cosine;
            float b = 2 * (ray.Direction.Dot(V) * co.Dot(V) - ray.Direction.Dot(co) * _cosine * _cosine);
            float c = co.Dot(V) * co.Dot(V) - co.Dot(co) * _cosine * _cosine;

            if (!MathUtilities.DetermineRoot(a, b, c, out float t)) return -1;

            Vec3f p = ray.Origin + ray.Direction * t;

            float angle = (float)(Math.PI / 2) / (p - Location).Dot(V);
            if (angle <= HalfAngle && angle <= 0) return -1;

            normalFunc = () => {
                float xzLength = new Vec2f(p.X - Location.X, p.Z - Location.Z).Length;

                Vec2f rotated = _rotation.Rotate(new Vec2f(xzLength, 0));
                float multiply = rotated.X / xzLength;

                return new Vec3f((p.X - Location.X) * multiply, rotated.Y, (p.Z - Location.Z) * multiply).Normalize();
            };
            surfaceFunc = () => {
                return Surface.GetSurface(GetUV(p));
            };

            return t;
        }

        private Vec2f GetUV(Vec3f p) {
            Vec2f n = new Vec2f(p.X - Location.X, p.Z - Location.Z);
            float u = (float)(Math.Atan2(-n.X, -n.Y) / (2 * Math.PI));

            return new Vec2f(u, (Location - p).Length);
        }

        public bool IsInside(Vec3f point) {
            return point.Cross(V).Length < point.Dot(V) * _cosine;
        }
    }
}
