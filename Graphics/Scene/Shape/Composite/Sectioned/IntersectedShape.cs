using Vectors.Vectors3D;

namespace Graphics {
    public class IntersectedShape : IShape, ISectionedShape {
        public Vec3f Location { get => FirstShape.Location; set => FirstShape.Location = value; }
        public ISectionedShape FirstShape { get; set; }
        public ISectionedShape SecondShape { get; set; }

        public IntersectedShape(ISectionedShape firstShape, ISectionedShape secondShape) { 
            FirstShape = firstShape;
            SecondShape = secondShape;
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            float t = FirstShape.Intersect(ray, out normalFunc, out surfaceFunc);
            if (t > 0) {
                Vec3f p = ray.Origin + ray.Direction * t;
                if (SecondShape.IsInside(p)) return t;
            }

            t = SecondShape.Intersect(ray, out normalFunc, out surfaceFunc);
            if (t > 0) {
                Vec3f p = ray.Origin + ray.Direction * t;
                if (FirstShape.IsInside(p)) return t;
            }

            return -1;
        }

        public bool IsInside(Vec3f point) {
            return FirstShape.IsInside(point) && SecondShape.IsInside(point);
        }
    }
}
