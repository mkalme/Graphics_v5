using Vectors.Vectors3D;

namespace Graphics {
    public class InvertedShape : IShape, ISectionedShape {
        public Vec3f Location { get => FirstShape.Location; set => FirstShape.Location = value; }
        public ISectionedShape FirstShape { get; set; }
        public ISectionedShape SecondShape { get; set; }

        public InvertedShape(ISectionedShape firstShape, ISectionedShape secondShape) {
            FirstShape = firstShape;
            SecondShape = secondShape;
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            if (FirstShape.IsInside(ray.Origin) && SecondShape.IsInside(ray.Origin)) {
                float t = FirstShape.Intersect(ray, out normalFunc, out surfaceFunc);
                if(t > 0) return t;

                return SecondShape.Intersect(ray, out normalFunc, out surfaceFunc);
            }

            normalFunc = null;
            surfaceFunc = null;

            return -1;
        }

        public bool IsInside(Vec3f point) {
            return FirstShape.IsInside(point) ^ SecondShape.IsInside(point);
        }
    }
}
