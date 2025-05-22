using Vectors.Vectors2D;

namespace Graphics {
    public class CustomFlatSurface : IFlatSurface {
        public Func<Vec2f, SurfaceResult> SurfaceFunction { get; set; }

        public CustomFlatSurface(Func<Vec2f, SurfaceResult> surfaceFunction) {
            SurfaceFunction = surfaceFunction;
        }

        public SurfaceResult GetSurface(Vec2f point) {
            return SurfaceFunction(point);
        }
    }
}
