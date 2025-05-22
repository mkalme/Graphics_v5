using Vectors.Vectors3D;

namespace Graphics {
    public class OffsetShape : IShape {
        public Vec3f Location { get => Shape.Location; set => Shape.Location = value; }
        public IShape Shape { get; set; }
        public Vec3f Offset { get; set; }

        public OffsetShape(IShape shape, Vec3f offset) { 
            Shape = shape;
            Offset = offset;
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            return Shape.Intersect(new Ray(ray.Origin - Offset, ray.Direction), out normalFunc, out surfaceFunc);
        }
    }
}
