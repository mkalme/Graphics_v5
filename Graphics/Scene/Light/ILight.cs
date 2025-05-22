using Vectors.Vectors3D;

namespace Graphics {
    public interface ILight {
        Vec3f Location { get; set; }
        Vec3f Color { get; set; }
        float Lumens { get; set; }
    }
}
