using Vectors.Vectors3D;

namespace Graphics {
    public class SubtractedShape : IShape, ISectionedShape {
        public Vec3f Location {
            get => MainShape.Location;
            set => MainShape.Location = value;
        }

        public ISectionedShape MainShape { get; set; }
        public ISectionedShape Subtractor { get; set; }

        public SubtractedShape(ISectionedShape mainShape, ISectionedShape subtractor) {
            MainShape = mainShape;
            Subtractor = subtractor;
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            Ray currentRay = ray;
            float t = 0;
            Func<Vec3f> nFunc;

            bool checkIfInside = true, isInside = false;
            while (true) {
                if (isInside || checkIfInside && Subtractor.IsInside(currentRay.Origin)) {
                    t += Subtractor.Intersect(currentRay, out nFunc, out surfaceFunc);
                    currentRay = new Ray(ray.Origin + ray.Direction * t, ray.Direction);

                    if (MainShape.IsInside(currentRay.Origin)) {
                        normalFunc = () => -nFunc();
                        return t;
                    }

                    checkIfInside = false;
                    isInside = false;
                } else {
                    float tc = MainShape.Intersect(currentRay, out normalFunc, out surfaceFunc);
                    if (tc < 0) return -1;
                    t += tc;

                    currentRay = new Ray(ray.Origin + ray.Direction * t, ray.Direction);

                    if (!Subtractor.IsInside(currentRay.Origin)) {
                        return t;
                    }

                    isInside = true;
                }
            }
        }

        public bool IsInside(Vec3f point) {
            return MainShape.IsInside(point) && !Subtractor.IsInside(point);
        }
    }
}
