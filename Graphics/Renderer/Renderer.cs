using System.Runtime.InteropServices;
using Vectors.Vectors3D;

namespace Graphics {
    public class Renderer : IRenderer {
        public Scene Scene { get; set; }

        public int Depth { get; set; } = 10;
        public float MaxLumens { get; set; } = 1000;

        public bool Render(LockedBitmap output) {
            Ray[,] rays = Scene.Camera.Cast();

            Parallel.For(0, Scene.Camera.HeightPx, y => {
                for (int x = 0; x < Scene.Camera.WidthPx; x++) {
                    Ray ray = rays[x, y];

                    output.SetPixel(Trace(ray).ToColor(), x, y);
                }
            });

            //Trace(rays[483, 387]);

            return true;
        }

        public Vec3f Trace(Ray ray, int inRefraction = 0, int depth = 1) {
            float t = Intersect(ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc);
            if (t < 0) return Scene.Sky.TraceColor(ray.Direction);

            Vec3f intersectionPoint = ray.Origin + ray.Direction * (t * 0.9999F);
            Vec3f normal = normalFunc();
            SurfaceResult surface = surfaceFunc();

            Vec3f output = surface.Color * Shade(intersectionPoint, normal, ray.Direction, 1, out Vec3f specular);

            if ((surface.ReflectionIndex > 0 || surface.FresnelIndex > 0) && depth < Depth) {
                Vec3f reflect = Reflect(normal, ray.Direction);
                Vec3f color = Trace(new Ray(intersectionPoint, reflect), inRefraction, depth + 1);

                float angle = 1 - Math.Abs(reflect.Dot(normal));
                float reflection = surface.ReflectionIndex + (1 - surface.ReflectionIndex) * surface.FresnelIndex * angle;

                output = output.Mix(color, Math.Clamp(reflection, 0, 1));
            }

            if (inRefraction < 200 && surface.RefractiveIndex > 0) {
                Vec3f refract = Refract(ray.Direction, normal, surface.RefractiveIndex);
                if (refract != 0) {
                    Vec3f color = Trace(new Ray(ray.Origin + ray.Direction * (t * 1.0001F), refract), inRefraction + 1, depth + 1);

                    output = color;
                }
            }

            output = (output + specular).Clamp(0, 1);

            if (t > 20) output = output.Mix(Scene.FogColor, Math.Clamp((float)Math.Sqrt(t - 20) * 6 / 100, 0, 1));

            return output;
        }
        private Vec3f Shade(Vec3f point, Vec3f normal, Vec3f rayD, int ks, out Vec3f specular) {
            Vec3f diffuse = 0, ambient = 0;
            specular = 0;

            for (int i = 0; i < Scene.LightSources.Count; i++) {
                ILight lightSource = Scene.LightSources[i];

                Vec3f lightD = lightSource.Location - point;
                float length = lightD.Length;

                if (Intersect(new Ray(point, lightD / length), out _, out _, length) >= 0) {
                    continue;
                }

                float angle = Math.Abs(Vec3f.Divide(lightD, length).Dot(normal));
                float lumens = lightSource.Lumens * angle / (length * length);
                lumens = lumens > MaxLumens ? MaxLumens : lumens;

                diffuse += lightSource.Color * (lumens / MaxLumens);

                Vec3f r = Reflect(normal, lightD / length);
                specular += lightSource.Color * ks * (float)Math.Pow(Math.Max(0, r.Dot(rayD)), 100);
            }

            if (Scene.SkylightEnabled && Intersect(new Ray(point, new Vec3f(0, 1, 0)), out _, out _) >= 0) {
                ambient = Scene.AmbientLight * Scene.SkylightIntensity;
            } else {
                ambient = Scene.AmbientLight;
            }

            return (ambient + diffuse).Clamp(0, 1);
        }
        private static Vec3f Reflect(Vec3f n, Vec3f d) {
            return d - (2 * d.Dot(n) * n);
        }
        private static Vec3f Refract(Vec3f I, Vec3f N, float ior) {
            float cosi = Math.Clamp(Vec3f.Dot(I, N), -1, 1);
            float etai = 1, etat = ior;

            Vec3f n = N;
            if (cosi < 0) {
                cosi = -cosi;
            } else {
                Swap(ref etai, ref etat);
                n = -N;
            }

            float eta = etai / etat;
            float k = 1 - eta * eta * (1 - cosi * cosi);

            return k < 0 ? 0 : eta * I + (eta * cosi - (float)Math.Sqrt(k)) * n;
        }
        private float Intersect(Ray ray, out Func<Vec3f> normalFunc, out Func<SurfaceResult> surfaceFunc, float l = float.MaxValue) {
            float minT = float.MaxValue;
            bool intersects = false;
            normalFunc = null;
            surfaceFunc = null;

            for (int i = 0; i < Scene.Shapes.Count; i++) {
                float t = Scene.Shapes[i].Intersect(ray, out Func<Vec3f> normal, out Func<SurfaceResult> surface);
                if (t < 0 || t >= l) continue;

                if (t < minT) {
                    minT = t;
                    intersects = true;
                    normalFunc = normal;
                    surfaceFunc = surface;
                }
            }

            return intersects ? minT : -1;
        }

        private static void Swap(ref float a, ref float b) {
            float temp = a;

            a = b;
            b = temp;
        }
    }
}
