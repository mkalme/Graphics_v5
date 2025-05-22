namespace Graphics {
    public static class MathUtilities {
        public static bool DetermineRoot(float a, float b, float c, out float t) {
            t = -1;
            
            float d = b * b - 4 * a * c;
            if (d < 0) return false;

            float sqrtD = (float)Math.Sqrt(d);

            float t0 = (-b + sqrtD) / (2 * a);
            float t1 = (-b - sqrtD) / (2 * a);

            if (t1 < 0) t = t0;
            else if (t0 < 0) t = t1;
            else t = t1 < t0 ? t1 : t0;

            return true;
        }
    }
}
