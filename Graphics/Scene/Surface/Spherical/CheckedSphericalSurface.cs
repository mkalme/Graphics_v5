using System.Drawing;
using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics {
    public class CheckedSphericalSurface : IFlatSurface {
        public SurfaceResult FirstResult { get; set; }
        public SurfaceResult SecondResult { get; set; }
        public int USquares { get; set; } = 20 * 2;
        public int VSquares { get; set; } = 20;

        public CheckedSphericalSurface() {
            FirstResult = new SurfaceResult(Color.Gold.ToVec3f(), 0.25F, 0.75F);
            SecondResult = new SurfaceResult(Color.PaleGoldenrod.ToVec3f(), 0.25F, 0.75F);
        }

        public SurfaceResult GetSurface(Vec2f uv) {
            return Math.Floor(uv.X * USquares) % 2 == 0 ^ Math.Floor(uv.Y * VSquares) % 2 == 0 ? FirstResult : SecondResult;
        }
    }
}
