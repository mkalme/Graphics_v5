namespace Vectors.Vectors3D {
    public readonly struct Vec3f {
        public readonly float X { get; init; }
        public readonly float Y { get; init; }
        public readonly float Z { get; init; }

        public float Length {
            get {
                return GetLength(this);
            }
        }

        public Vec3f() {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Vec3f(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator Vec3f(float a) {
            return new Vec3f(a, a, a);
        }
        public static implicit operator Vec3f((float, float, float) a) {
            return new Vec3f(a.Item1, a.Item2, a.Item3);
        }

        public static bool operator ==(Vec3f a, Vec3f b) {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }
        public static bool operator !=(Vec3f a, Vec3f b) {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }
        public static Vec3f operator -(Vec3f a) {
            return new Vec3f(-a.X, -a.Y, -a.Z);
        }
        public static Vec3f operator +(Vec3f a, float value) {
            return Add(a, value);
        }
        public static Vec3f operator +(float value, Vec3f a) {
            return Add(a, value);
        }
        public static Vec3f operator +(Vec3f a, Vec3f b) {
            return Add(a, b);
        }
        public static Vec3f operator -(Vec3f a, float value) {
            return Subtract(a, value);
        }
        public static Vec3f operator -(float value, Vec3f a) {
            return Subtract(a, value);
        }
        public static Vec3f operator -(Vec3f a, Vec3f b) {
            return Subtract(a, b);
        }
        public static Vec3f operator *(Vec3f a, float value) {
            return Multiply(a, value);
        }
        public static Vec3f operator *(float value, Vec3f a) {
            return Multiply(a, value);
        }
        public static Vec3f operator *(Vec3f a, Vec3f b) {
            return Multiply(a, b);
        }
        public static Vec3f operator /(Vec3f a, float value) {
            return Divide(a, value);
        }
        public static Vec3f operator /(float value, Vec3f a) {
            return Divide(a, value);
        }
        public static Vec3f operator /(Vec3f a, Vec3f b) {
            return Divide(a, b);
        }

        public Vec3f Add(Vec3f b) {
            return Add(this, b);
        }
        public Vec3f Add(float value) {
            return Add(this, value);
        }
        public Vec3f Subtract(Vec3f b) {
            return Subtract(this, b);
        }
        public Vec3f Subtract(float value) {
            return Subtract(this, value);
        }
        public Vec3f Multiply(Vec3f b) {
            return Multiply(this, b);
        }
        public Vec3f Multiply(float value) {
            return Multiply(this, value);
        }
        public Vec3f Divide(Vec3f b) {
            return Divide(this, b);
        }
        public Vec3f Divide(float value) {
            return Divide(this, value);
        }
        public Vec3f Normalize() {
            return Normalize(this);
        }
        public Vec3f Cross(Vec3f b) {
            return Cross(this, b);
        }
        public float Dot(Vec3f b) {
            return Dot(this, b);
        }

        public static Vec3f Add(Vec3f a, Vec3f b) {
            return new Vec3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z); 
        }
        public static Vec3f Add(Vec3f a, float value) {
            return new Vec3f(a.X + value, a.Y + value, a.Z + value);
        }
        public static Vec3f Subtract(Vec3f a, Vec3f b) {
            return new Vec3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static Vec3f Subtract(Vec3f a, float value) {
            return new Vec3f(a.X - value, a.Y - value, a.Z - value);
        }
        public static Vec3f Multiply(Vec3f a, Vec3f b) {
            return new Vec3f(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }
        public static Vec3f Multiply(Vec3f a, float value) {
            return new Vec3f(a.X * value, a.Y * value, a.Z * value);
        }
        public static Vec3f Divide(Vec3f a, Vec3f b) {
            return new Vec3f(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }
        public static Vec3f Divide(Vec3f a, float value) {
            return new Vec3f(a.X / value, a.Y / value, a.Z / value);
        }
        public static Vec3f Normalize(Vec3f a) {
            return Divide(a, GetLength(a));
        }
        public static Vec3f Cross(Vec3f a, Vec3f b) {
            return new Vec3f() {
                X = a.Y * b.Z - a.Z * b.Y,
                Y = -(a.X * b.Z - a.Z * b.X),
                Z = a.X * b.Y - a.Y * b.X
            };
        }
        public static float Dot(Vec3f a, Vec3f b) {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        public static float GetLength(Vec3f a) {
            return (float)Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            return obj is Vec3f && (Vec3f)obj == this;
        }
        public override string ToString() {
            return $"Vec3f [X={X}, Y={Y}, Z={Z}]";
        }
        public override int GetHashCode() {
            unchecked{
                int hash = 17;
                
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Z.GetHashCode();

                return hash;
            }
        }
    }
}