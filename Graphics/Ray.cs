using Vectors.Vectors3D;

namespace Graphics {
    public readonly struct Ray {
        public readonly Vec3f Origin { get; init; }
        public readonly Vec3f Direction { get; init; }

        public Ray() {
            Origin = new Vec3f();
            Direction = new Vec3f();
        }
        public Ray(Vec3f origin, Vec3f direction) { 
            Origin = origin;
            Direction = direction;
        }
    }
}