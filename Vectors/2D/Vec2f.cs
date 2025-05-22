namespace Vectors.Vectors2D {
    public struct Vec2f {
        public readonly float X { get; init; }
        public readonly float Y { get; init; }

        public float Length {
            get {
                return GetLength(this);
            }
        }

        public Vec2f() {
            X = 0;
            Y = 0;
        }
        public Vec2f(float x, float y) {
            X = x;
            Y = y;
        }

        public static implicit operator Vec2f(float a) {
            return new Vec2f(a, a);
        }
        public static implicit operator Vec2f((float, float) a) {
            return new Vec2f(a.Item1, a.Item2);
        }

        public static bool operator ==(Vec2f a, Vec2f b) {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Vec2f a, Vec2f b) {
            return a.X != b.X || a.Y != b.Y;
        }
        public static Vec2f operator -(Vec2f a) {
            return new Vec2f(-a.X, -a.Y);
        }
        public static Vec2f operator +(Vec2f a, float value) {
            return Add(a, value);
        }
        public static Vec2f operator +(float value, Vec2f a) {
            return Add(a, value);
        }
        public static Vec2f operator +(Vec2f a, Vec2f b) {
            return Add(a, b);
        }
        public static Vec2f operator -(Vec2f a, float value) {
            return Subtract(a, value);
        }
        public static Vec2f operator -(float value, Vec2f a) {
            return Subtract(a, value);
        }
        public static Vec2f operator -(Vec2f a, Vec2f b) {
            return Subtract(a, b);
        }
        public static Vec2f operator *(Vec2f a, float value) {
            return Multiply(a, value);
        }
        public static Vec2f operator *(float value, Vec2f a) {
            return Multiply(a, value);
        }
        public static Vec2f operator *(Vec2f a, Vec2f b) {
            return Multiply(a, b);
        }
        public static Vec2f operator /(Vec2f a, float value) {
            return Divide(a, value);
        }
        public static Vec2f operator /(float value, Vec2f a) {
            return Divide(a, value);
        }
        public static Vec2f operator /(Vec2f a, Vec2f b) {
            return Divide(a, b);
        }

        public Vec2f Add(Vec2f b) {
            return Add(this, b);
        }
        public Vec2f Add(float value) {
            return Add(this, value);
        }
        public Vec2f Subtract(Vec2f b) {
            return Subtract(this, b);
        }
        public Vec2f Subtract(float value) {
            return Subtract(this, value);
        }
        public Vec2f Multiply(Vec2f b) {
            return Multiply(this, b);
        }
        public Vec2f Multiply(float value) {
            return Multiply(this, value);
        }
        public Vec2f Divide(Vec2f b) {
            return Divide(this, b);
        }
        public Vec2f Divide(float value) {
            return Divide(this, value);
        }
        public Vec2f Normalize() {
            return Normalize(this);
        }
        public float Dot(Vec2f b) {
            return Dot(this, b);
        }

        public static Vec2f Add(Vec2f a, Vec2f b) {
            return new Vec2f(a.X + b.X, a.Y + b.Y);
        }
        public static Vec2f Add(Vec2f a, float value) {
            return new Vec2f(a.X + value, a.Y + value);
        }
        public static Vec2f Subtract(Vec2f a, Vec2f b) {
            return new Vec2f(a.X - b.X, a.Y - b.Y);
        }
        public static Vec2f Subtract(Vec2f a, float value) {
            return new Vec2f(a.X - value, a.Y - value);
        }
        public static Vec2f Multiply(Vec2f a, Vec2f b) {
            return new Vec2f(a.X * b.X, a.Y * b.Y);
        }
        public static Vec2f Multiply(Vec2f a, float value) {
            return new Vec2f(a.X * value, a.Y * value);
        }
        public static Vec2f Divide(Vec2f a, Vec2f b) {
            return new Vec2f(a.X / b.X, a.Y / b.Y);
        }
        public static Vec2f Divide(Vec2f a, float value) {
            return new Vec2f(a.X / value, a.Y / value);
        }
        public static Vec2f Normalize(Vec2f a) {
            return Divide(a, GetLength(a));
        }
        public static float Dot(Vec2f a, Vec2f b) {
            return a.X * b.X + a.Y * b.Y;
        }
        public static float GetLength(Vec2f a) {
            return (float)Math.Sqrt(a.X * a.X + a.Y * a.Y);
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            return obj is Vec2f && (Vec2f)obj == this;
        }
        public override string ToString() {
            return $"Vec2f [X={X}, Y={Y}]";
        }
        public override int GetHashCode() {
            unchecked {
                int hash = 17;

                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();

                return hash;
            }
        }
    }
}
