using Vectors.Vectors2D;

namespace Graphics {
    public class CheckedFlatSurface : IFlatSurface {
        public SurfaceResult FirstResult { get; set; }
        public SurfaceResult SecondResult { get; set; }
        public float Zoom { get; set; }

        public CheckedFlatSurface() {
            FirstResult = new SurfaceResult(0.4F, 0.25F * 0, 0.75F);
            SecondResult = new SurfaceResult(0.7F, 0.25F * 0, 0.75F);
            Zoom = 5;
        }

        public SurfaceResult GetSurface(Vec2f point) {
            return Math.Floor(point.X * Zoom) % 2 == 0 ^ Math.Floor(point.Y * Zoom) % 2 == 0 ? FirstResult : SecondResult;
        }
    }
}
