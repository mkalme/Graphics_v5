using Vectors.Vectors3D;

namespace Graphics {
    public class Scene {
        public IList<IShape> Shapes { get; set; }
        public IList<ILight> LightSources { get; set; }
        public Vec3f FogColor { get; set; }
        public Vec3f AmbientLight { get; set; }
        public bool SkylightEnabled { get; set; } = true;
        public float SkylightIntensity { get; set; } = 1 / 3F;

        public ISky Sky { get; set; }
        public ICamera Camera { get; set; }

        public Scene() { 
            Shapes = new List<IShape>();
            LightSources = new List<ILight>();
            AmbientLight = 0;

            Sky = new VoidSky();
            Camera = new Camera();
        }
    }
}
