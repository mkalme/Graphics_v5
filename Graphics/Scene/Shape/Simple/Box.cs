using Vectors.Vectors3D;

namespace Graphics {
    public class Box : IShape, ISectionedShape {
        public Vec3f Location {
            get => _location;
            set {
                _location = value;
                SetTriangleMesh();
            }
        }
        private Vec3f _location;
        
        public Vec3f SecondPoint {
            get => _secondPoint;
            set {
                _secondPoint = value;
                SetTriangleMesh();
            }
        }
        private Vec3f _secondPoint;

        public IFlatSurface Surface {
            get => _surface;
            set {
                _surface = value;

                for (int i = 0; i < _triangleMesh.Shapes.Count; i++) { 
                    ((Triangle)_triangleMesh.Shapes[i]).Surface = value;
                }
            }
        }
        private IFlatSurface _surface;

        private Mesh _triangleMesh;

        public Box() {
            _triangleMesh = new Mesh();

            Location = 0;
            SecondPoint = 0;
            Surface = new CheckedFlatSurface();

            SetTriangleMesh();
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            return _triangleMesh.Intersect(ray, out normalFunc, out surfaceFunc);
        }
        private void SetTriangleMesh() {
            _triangleMesh.Shapes.Clear();

            Vec3f p0 = Location;
            Vec3f p1 = new Vec3f(SecondPoint.X, Location.Y, Location.Z);
            Vec3f p2 = new Vec3f(SecondPoint.X, Location.Y, SecondPoint.Z);
            Vec3f p3 = new Vec3f(Location.X, Location.Y, SecondPoint.Z);

            Vec3f p4 = new Vec3f(Location.X, SecondPoint.Y, Location.Z);
            Vec3f p5 = new Vec3f(SecondPoint.X, SecondPoint.Y, Location.Z);
            Vec3f p6 = new Vec3f(SecondPoint.X, SecondPoint.Y, SecondPoint.Z);
            Vec3f p7 = new Vec3f(Location.X, SecondPoint.Y, SecondPoint.Z);

            List<Vec3f[]> triangles = new List<Vec3f[]>(12) { 
                new Vec3f[]{ p0, p1, p2, new Vec3f(0, -1, 0) },
                new Vec3f[]{ p0, p2, p3, new Vec3f(0, -1, 0) },
                new Vec3f[]{ p4, p5, p6, new Vec3f(0, 1, 0) },
                new Vec3f[]{ p4, p6, p7, new Vec3f(0, 1, 0) },

                new Vec3f[]{ p0, p3, p7, new Vec3f(1, 0, 0) },
                new Vec3f[]{ p0, p4, p7, new Vec3f(1, 0, 0) },
                new Vec3f[]{ p1, p2, p6, new Vec3f(-1, 0, 0) },
                new Vec3f[]{ p1, p5, p6, new Vec3f(-1, 0, 0) },

                new Vec3f[]{ p3, p2, p6, new Vec3f(0, 0, 1) },
                new Vec3f[]{ p3, p7, p6, new Vec3f(0, 0, 1) },
                new Vec3f[]{ p0, p4, p5, new Vec3f(0, 0, -1) },
                new Vec3f[]{ p0, p1, p5, new Vec3f(0, 0, -1) },
            };

            for (int i = 0; i < triangles.Count; i++) {
                Triangle triangle = new Triangle() {
                    Location = triangles[i][0],
                    SecondPoint = triangles[i][1],
                    ThirdPoint = triangles[i][2],
                    Surface = Surface
                };
                triangle.SetCustomNormal(triangles[i][3]);

                _triangleMesh.Shapes.Add(triangle);
            }
        }

        public bool IsInside(Vec3f point) {
            return IsInRange(point.X, Location.X, SecondPoint.X)
                && IsInRange(point.Y, Location.Y, SecondPoint.Y)
                && IsInRange(point.Z, Location.Z, SecondPoint.Z);
        }
        private static bool IsInRange(float x, float a, float b) {
            return x > a && x < b || x < a && x > b;
        }
    }
}
