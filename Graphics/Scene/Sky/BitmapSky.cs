using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics {
    public class BitmapSky : ISky {
        public LockedBitmap Image { get; set; }
        public Rotation HorizontalRotation { get; set; }

        public Vec3f FogColor { get; set; }
        public bool FogEnabled { get; set; }

        public BitmapSky(LockedBitmap image) {
            Image = image;
            HorizontalRotation = Rotation.FromRadians(0);
            FogEnabled = false;
        }

        public Vec3f TraceColor(Vec3f normal) {
            Vec2f n2d = HorizontalRotation.Rotate(new Vec2f(normal.Z, normal.X));
            Vec3f n = new Vec3f(n2d.Y, normal.Y, n2d.X);

            float h = 1 - (float)(Math.Atan2(-n.X, -n.Z) / (Math.PI * 2));
            float v = 1 - (0.5F + (float)(Math.Atan2(n.Y, new Vec2f(n.X, n.Z).Length) / Math.PI));

            Vec3f output = Image.GetPixel((int)(Image.Width * h), (int)(Image.Height * v)).ToVec3f();

            if (FogEnabled && normal.Y >= 0 && normal.Y <= 0.01F) {
                output = output.Mix(FogColor, 1 - normal.Y / 0.01F);
            }

            return output;
        }
    }
}
