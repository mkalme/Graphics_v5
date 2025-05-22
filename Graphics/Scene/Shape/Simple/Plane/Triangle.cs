using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics {
    public class Triangle : IShape {
        public Vec3f Location { get; set; }
        public Vec3f SecondPoint { get; set; }
        public Vec3f ThirdPoint { get; set; }

        public IFlatSurface Surface { get; set; }

        private Vec3f _customNormal;

        public Triangle() {
            Location = 0;
            SecondPoint = 0;
            ThirdPoint = 0;

            Surface = new CheckedFlatSurface();
            _customNormal = 0;
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            normalFunc = null;
            surfaceFunc = null;

            Vec3f n = (SecondPoint - Location).Cross(ThirdPoint - Location);

            float t = -(n.Dot(ray.Origin) - n.Dot(Location)) / n.Dot(ray.Direction);
            if (t < 0) return -1;

            Vec3f intersectionPoint = ray.Origin + t * ray.Direction;

            Vec3f firstEdge = SecondPoint - Location;
            Vec3f v1 = intersectionPoint - Location;
            Vec3f c = firstEdge.Cross(v1);
            if (n.Dot(c) < 0) return -1;

            Vec3f secondEdge = ThirdPoint - SecondPoint;
            Vec3f v2 = intersectionPoint - SecondPoint;
            c = secondEdge.Cross(v2);
            if (n.Dot(c) < 0) return -1;

            Vec3f thirdEdge = Location - ThirdPoint;
            Vec3f v3 = intersectionPoint - ThirdPoint;
            c = thirdEdge.Cross(v3);
            if (n.Dot(c) < 0) return -1;

            normalFunc = () => {
                if (_customNormal == 0) {
                    Vec3f pvec = Vec3f.Cross(ray.Direction, ThirdPoint - Location);
                    float det = Vec3f.Dot(SecondPoint - Location, pvec);
                    bool backfacing = det < float.Epsilon;

                    return backfacing ? -n.Normalize() : n.Normalize();
                } else {
                    return _customNormal;
                }
            };
            surfaceFunc = () => Surface.GetSurface(new Vec2f(intersectionPoint.X, intersectionPoint.Z));

            return t;
        }

        public void SetCustomNormal(Vec3f normal) {
            _customNormal = normal;
        }
    }
}
