using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics {
    public class HorizontallyRotatedShape : IShape {
        public IShape Shape { get; set; }
        public Vec3f Location {
            get => Shape.Location;
            set => Shape.Location = value;
        }

        public Vec3f Axis { get; set; }
        public Rotation Rotation { get; set; }

        public HorizontallyRotatedShape(IShape shape) {
            Shape = shape;
            Axis = Shape.Location;
            Rotation = Rotation.FromRadians(0);
        }

        public float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc) {
            Vec2f newO = Rotation.RotateNegative((ray.Origin.Z - Axis.Z, ray.Origin.X - Axis.X));
            Vec2f newD = Rotation.RotateNegative((ray.Direction.Z, ray.Direction.X));

            float t = Shape.Intersect(new Ray(new Vec3f(newO.Y + Axis.X, ray.Origin.Y, newO.X + Axis.Z), new Vec3f(newD.Y, ray.Direction.Y, newD.X)), out Func<Vec3f> nFunc, out surfaceFunc);

            normalFunc = () => {
                Vec3f n = nFunc();
                Vec2f nxy = Rotation.Rotate((n.Z, n.X));

                return new Vec3f(nxy.Y, n.Y, nxy.X);
            };

            return t;
        }
    }
}
