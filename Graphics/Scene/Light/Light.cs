using Vectors.Vectors3D;

namespace Graphics {
    public class PointLight : ILight {
        public Vec3f Location { get; set; }
        public Vec3f Color { get; set; }
        public float Lumens { get; set; }
    }
}
