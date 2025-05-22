using Vectors.Vectors3D;

namespace Graphics {
    public interface ISky {
        Vec3f TraceColor(Vec3f normal);
    }
}
