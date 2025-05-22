using System.Drawing;

namespace Vectors.Vectors3D {
    public static class ColorExtensions {
        public static Color ToColor(this Vec3f color) {
            byte r = (byte)(color.X < 0 ? 0 : color.X > 1 ? 1 : color.X * 255);
            byte g = (byte)(color.Y < 0 ? 0 : color.Y > 1 ? 1 : color.Y * 255);
            byte b = (byte)(color.Z < 0 ? 0 : color.Z > 1 ? 1 : color.Z * 255);

            return Color.FromArgb(r, g, b);
        }
        public static Vec3f ToVec3f(this Color color) {
            return new Vec3f(color.R / 255F, color.G / 255F, color.B / 255F);
        }
        public static Vec3f Mix(this Vec3f main, Vec3f other, float intensity) {
            float r = (other.X - main.X) * intensity + main.X;
            float g = (other.Y - main.Y) * intensity + main.Y;
            float b = (other.Z - main.Z) * intensity + main.Z;

            return new Vec3f(r, g, b);
        }
        public static Vec3f Clamp(this Vec3f a, float min, float max) {
            return new Vec3f() {
                X = a.X < min ? min : a.X > max ? max : a.X,
                Y = a.Y < min ? min : a.Y > max ? max : a.Y,
                Z = a.Z < min ? min : a.Z > max ? max : a.Z
            };
        }
    }
}
