using Vectors.Vectors3D;

namespace Graphics {
    public interface ISectionedShape : IShape {
        bool IsInside(Vec3f point);
    }
}
