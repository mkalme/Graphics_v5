using Vectors.Vectors2D;

namespace Graphics {
    public class SimpleSurface : IFlatSurface {
        public SurfaceResult SurfaceResult { get; set; }

        public SimpleSurface() {
            SurfaceResult = new SurfaceResult(1, 0, 0, 0);
        }

        public SurfaceResult GetSurface(Vec2f point) {
            return SurfaceResult;
        }
    }
}
