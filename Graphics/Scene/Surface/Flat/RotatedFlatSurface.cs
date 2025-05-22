using System;
using Vectors.Vectors2D;

namespace Graphics
{
    public class RotatedFlatSurface : IFlatSurface {
        public IFlatSurface FlatSurface { get; set; }
        public Rotation Rotation { get; set; }

        public RotatedFlatSurface(IFlatSurface flatSurface) {
            FlatSurface = flatSurface;
            Rotation = Rotation.FromRadians(0);
        }
        public RotatedFlatSurface(IFlatSurface flatSurface, Rotation rotation) {
            FlatSurface = flatSurface;
            Rotation = rotation;
        }

        public SurfaceResult GetSurface(Vec2f point) {
            return FlatSurface.GetSurface(Rotation.Rotate(point));
        }
    }
}
