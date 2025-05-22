using Vectors.Vectors3D;

namespace Graphics {
    public class SurfaceResult {
        public Vec3f Color { get; init; }
        public float ReflectionIndex { get; init; }
        public float RefractiveIndex { get; init; }
        public float FresnelIndex { get; init; }
        public int SpecularPow { get; init; }

        public SurfaceResult() {
            Color = 0;
            ReflectionIndex = 0;
            RefractiveIndex = 0;
            FresnelIndex = 0;
            SpecularPow = 0;
        }
        public SurfaceResult(Vec3f color, float reflectionIndex = 0, float fresnelIndex = 0, int specularPow = 0) {
            Color = color;
            ReflectionIndex = reflectionIndex;
            RefractiveIndex = 0;
            FresnelIndex = fresnelIndex;
            SpecularPow = specularPow;
        }
    }
}
