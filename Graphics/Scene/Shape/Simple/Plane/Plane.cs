using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics
{
    public class Plane : IShape
    {
        public Vec3f Location { get; set; }
        public Vec3f Normal
        {
            get => _normal;
            set
            {
                _normal = value;

                float normalAngle = (float)Math.Atan2(value.X, value.Z);
                if (normalAngle != 0) normalAngle = (float)Math.PI - normalAngle;

                Rotation rotateN = Rotation.FromRadians(normalAngle);

                float normalYAngle = (float)Math.Atan2(value.Y, rotateN.Rotate(new Vec2f(value.Z, value.X)).X);

                _hRotation = Rotation.FromRadians(normalAngle * 0);
                _vRotation = Rotation.FromRadians(-(normalYAngle - (float)Math.PI / 2));
            }
        }
        private Vec3f _normal;

        private Rotation _hRotation;
        private Rotation _vRotation;

        public IFlatSurface FlatSurface { get; set; }

        public Plane()
        {
            Location = new Vec3f();
            Normal = new Vec3f(0, 1, 0);
            FlatSurface = new CheckedFlatSurface();
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc)
        {
            normalFunc = null;
            surfaceFunc = null;

            float t;

            float denom = Normal.Dot(ray.Direction);
            if (Math.Abs(denom) > 0.0001f)
            {
                t = (Location - ray.Origin).Dot(Normal) / denom;
            }
            else
            {
                return -1;
            }

            if (t < 0) return -1;

            normalFunc = () => Normal;
            surfaceFunc = () => FlatSurface.GetSurface(TransformPoint(ray.Origin + ray.Direction * t - Location));

            return t;
        }

        private Vec2f TransformPoint(Vec3f point)
        {
            Vec2f h = _hRotation.Rotate(new Vec2f(point.Z, point.X));
            Vec2f v = _vRotation.Rotate(new Vec2f(h.X, point.Y));

            return new Vec2f(h.Y, v.X);
        }
    }
}
