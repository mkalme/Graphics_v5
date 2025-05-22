using Vectors.Vectors3D;

namespace Graphics {
    public class Mesh : IShape {
        public Vec3f Location { get; set; }
        public IList<IShape> Shapes { get; set; }

        public Mesh() { 
            Location = new Vec3f();
            Shapes = new List<IShape>();
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            float minT = float.MaxValue;
            bool intersects = false;
            normalFunc = null;
            surfaceFunc = null;

            for (int i = 0; i < Shapes.Count; i++) {
                float t = Shapes[i].Intersect(ray, out Func<Vec3f> normal, out Func<SurfaceResult> surface);
                if (t < 0) continue;

                if (t < minT) {
                    minT = t;
                    intersects = true;
                    normalFunc = normal;
                    surfaceFunc = surface;
                }
            }

            return intersects ? minT : -1;
        }
    }
}
