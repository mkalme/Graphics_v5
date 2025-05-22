using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics {
    public class Camera : ICamera {
        public int WidthPx { get; set; }
        public int HeightPx { get; set; }
        public float FocalLength { get; set; }

        public Vec3f Location { get; set; }
        public Rotation Rotation { get; set; }
        public Vec3f Direction {
            get => _direction;
            set {
                _direction = value;

                _hRotation = Rotation.FromRadians((float)Math.Atan2(value.X, value.Z));
                _vRotation = Rotation.FromRadians((float)Math.Atan2(value.Y, new Vec2f(value.X, value.Z).Length));
            }
        }

        private Vec3f _direction;
        private Rotation _hRotation;
        private Rotation _vRotation;

        public Camera() {
            WidthPx = 1000;
            HeightPx = 1000;
            FocalLength = 1;

            Location = 0;
            Direction = new Vec3f(0, 0, 1).Normalize();
            Rotation = Rotation.FromRadians(0);
        }

        public Ray[,] Cast(){
            Ray[,] rays = new Ray[WidthPx, HeightPx];

            float height = HeightPx / (float)WidthPx;
            float dx = 1F / WidthPx, dy = height / HeightPx;

            Parallel.For(0, HeightPx, yPx => {
                for (int xPx = 0; xPx < WidthPx; xPx++) {
                    float x = 0.5F - xPx * dx - dx / 2;
                    float y = height / 2 - yPx * dy - dy / 2;

                    Vec3f point = new Vec3f(x, y, FocalLength);
                    point = RotatePoint(point);
                    point = RotatePointToDirection(point);

                    rays[xPx, yPx] = new Ray(Location, point.Normalize());
                }
            });

            return rays;
        }

        private Vec3f RotatePoint(Vec3f point){
            Vec2f d = Rotation.Rotate(new Vec2f(-point.X, point.Y));

            return new Vec3f(-d.X, d.Y, point.Z);
        }
        private Vec3f RotatePointToDirection(Vec3f point){
            Vec2f v = _vRotation.Rotate(new Vec2f(point.Z, point.Y));
            Vec2f h = _hRotation.Rotate(new Vec2f(v.X, point.X));

            return new Vec3f(h.Y, v.Y, h.X);
        }

        public void LookAt(Vec3f point) {
            Direction = (point - Location).Normalize();
        }
    }
}
