using System.Collections.Concurrent;
using System.Drawing;
using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics {
    public class SquareGeneratedSphericalSurface : IFlatSurface {
        public Vec3f FirstColor { get; set; }
        public Vec3f SecondColor { get; set; }

        public SurfaceResult SurfaceResult { get; set; }

        public float USquares { get; set; } = 50 * 2;
        public float VSquares { get; set; } = 50;

        private ConcurrentDictionary<Vec2f, Vec3f> _savedColors;
        private Random _random;

        public SquareGeneratedSphericalSurface() {
            FirstColor = Color.ForestGreen.ToVec3f();
            SecondColor = Color.PaleGreen.ToVec3f();

            SurfaceResult = new SurfaceResult(0, 0.25F, 0.75F);

            _savedColors = new ConcurrentDictionary<Vec2f, Vec3f>();
            _random = new Random();
        }

        public SurfaceResult GetSurface(Vec2f uv) {
            float x = (float)Math.Floor(uv.X * USquares);
            float y = (float)Math.Floor(uv.Y * VSquares);

            Vec2f xy = new Vec2f(x, y);

            if (!_savedColors.TryGetValue(xy, out Vec3f color)) {
                color = FirstColor.Mix(SecondColor, (float)_random.NextDouble());
                _savedColors.TryAdd(xy, color);
            }

            return new SurfaceResult(color, SurfaceResult.ReflectionIndex, SurfaceResult.FresnelIndex);
        }
    }
}
