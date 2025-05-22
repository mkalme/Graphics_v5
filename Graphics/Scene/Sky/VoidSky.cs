using Vectors.Vectors3D;

namespace Graphics {
    public class VoidSky : ISky {
        public Vec3f TraceColor(Vec3f normal) {
            return 0;
        }
    }
}
