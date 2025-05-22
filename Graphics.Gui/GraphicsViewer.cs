using System.Diagnostics;
using Vectors.Vectors2D;
using Vectors.Vectors3D;

namespace Graphics.Gui
{
    public partial class GraphicsViewer : Form {
        public Size ImageSize { get; set; } = new Size(1000, 1000);
        public bool Zoom { get; set; } = false;

        public GraphicsViewer() {
            InitializeComponent();

            PictureBox.SizeMode = Zoom ? PictureBoxSizeMode.Zoom : PictureBoxSizeMode.AutoSize;
            WindowState = FormWindowState.Maximized;
        }

        private void GraphicsViewer_Load(object sender, EventArgs e) {
            BeginRender();
        }

        private void BeginRender() {
            LockedBitmap bitmap = new LockedBitmap(Color.Black, ImageSize.Width, ImageSize.Height);

            bool finished = false;

            new Thread(() => {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                Renderer renderer = new Renderer() {
                    Scene = LoadScene3()
                };

                finished = renderer.Render(bitmap);

                //for (int i = 0; i < 100; i++) {
                //    finished = renderer.Render(bitmap);
                //}

                watch.Stop();
                Console.WriteLine($"Elapsed milliseconds: {watch.ElapsedMilliseconds}");
            }).Start();

            new Thread(() => {
                while (!finished) {
                    UpdatePictureBox(bitmap.CreateBitmapCopy());
                    if(!finished) Thread.Sleep(50);
                }

                bitmap.Unlock();
                Invoke(new Action(() => {
                    Clipboard.SetImage(bitmap.Source);
                }));

                Console.WriteLine("Done");

                UpdatePictureBox(bitmap.Source);
            }).Start();
        }
        private void UpdatePictureBox(Image image) {
            Invoke(new Action(() => {
                PictureBox.Image = image;
            }));
        }

        private Scene LoadScene0() {
            Scene scene = new Scene();

            BitmapSky sky = new BitmapSky(new LockedBitmap((Bitmap)Image.FromFile("D:\\PanoramicImage.jpg"))) {
                HorizontalRotation = Rotation.FromDegrees(0),
                FogColor = Color.FromArgb(125, 143, 165).ToVec3f(),
                FogEnabled = true
            };

            scene.Sky = sky;
            scene.AmbientLight = sky.Image.GetAverageColor() / 3;
            scene.SkylightEnabled = false;
            scene.FogColor = sky.FogColor;

            Camera camera = new Camera() { 
                WidthPx = ImageSize.Width,
                HeightPx = ImageSize.Height,
                Rotation = Rotation.FromDegrees(45),
                Direction = new Vec3f(0, 0, 1).Normalize(),
                Location = new Vec3f(6, 5, -2)
            };

            camera.LookAt(new Vec3f(0, 3, 11));

            scene.Camera = camera;

            scene.Shapes.Add(new Plane() {
                Location = new Vec3f(0, -1.5F, 0),
                Normal = new Vec3f(0, 1, 0).Normalize(),
                FlatSurface = new RotatedFlatSurface(
                    new CheckedFlatSurface() {
                        Zoom = 4
                    },
                    Rotation.FromDegrees(45))
            });

            Mesh sphereCube = new Mesh();

            for (int z = 0; z < 3; z++) {
                for (int y = 0; y < 3; y++) {
                    sphereCube.Shapes.Add(new Sphere() {
                        Location = new Vec3f(3, y * 3, 8 + z * 3),
                        Radius = 1.5F,
                        Surface = new SquareGeneratedSphericalSurface() {
                            FirstColor = Color.LightBlue.ToVec3f(),
                            SecondColor = Color.DodgerBlue.ToVec3f()
                        }
                    });

                    sphereCube.Shapes.Add(new Sphere() {
                        Location = new Vec3f(0, y * 3, 8 + z * 3),
                        Radius = 1.5F,
                        Surface = new SquareGeneratedSphericalSurface() {
                            FirstColor = Color.Red.ToVec3f(),
                            SecondColor = Color.DarkRed.ToVec3f()
                        }
                    });

                    sphereCube.Shapes.Add(new Sphere() {
                        Location = new Vec3f(-3, y * 3, 8 + z * 3),
                        Radius = 1.5F,
                        Surface = new SquareGeneratedSphericalSurface() {
                            FirstColor = Color.ForestGreen.ToVec3f(),
                            SecondColor = Color.PaleGreen.ToVec3f()
                        }
                    });
                }
            }

            HorizontallyRotatedShape rotated = new HorizontallyRotatedShape(sphereCube) {
                Axis = new Vec3f(0, 3, 11),
                Rotation = Rotation.FromDegrees(270 - 45)
            };

            scene.Shapes.Add(rotated);

            scene.LightSources.Add(new PointLight() {
                Location = new Vec3f(2, -0.5F, -1),
                Lumens = 25000,
                Color = 1
            });

            scene.LightSources.Add(new PointLight() {
                Location = new Vec3f(-2, -0.5F, -1),
                Lumens = 25000,
                Color = 1
            });

            scene.LightSources.Add(new PointLight() {
                Location = new Vec3f(-4, 2, 18),
                Lumens = 25000,
                Color = 1
            });

            scene.LightSources.Add(new PointLight() {
                Location = new Vec3f(7, 2, 18),
                Lumens = 25000,
                Color = 1
            });

            return scene;
        }
        private Scene LoadScene1() {
            Scene scene = new Scene();

            BitmapSky sky = new BitmapSky(new LockedBitmap((Bitmap)Image.FromFile("Assets\\Sky\\PanoramicImage.png"))) {
                HorizontalRotation = Rotation.FromDegrees(0),
                FogColor = Color.FromArgb(125, 143, 165).ToVec3f(),
                FogEnabled = true
            };

            scene.Sky = sky;
            scene.AmbientLight = sky.Image.GetAverageColor() / 3;
            scene.SkylightEnabled = false;
            scene.FogColor = sky.FogColor;

            Camera camera = new Camera() {
                WidthPx = ImageSize.Width,
                HeightPx = ImageSize.Height,
                Rotation = Rotation.FromDegrees(0),
                Direction = new Vec3f(0, 0, 1).Normalize(),
                Location = new Vec3f(0, 0, 0)
            };

            scene.Camera = camera;

            scene.Shapes.Add(new Plane() {
                Location = new Vec3f(0, -1.5F, 0),
                Normal = new Vec3f(0, 1, 0).Normalize(),
                FlatSurface = new RotatedFlatSurface(
                    new CheckedFlatSurface() {
                        Zoom = 4
                    },
                    Rotation.FromDegrees(45))
            });

            //scene.Shapes.Add(new Sphere() {
            //    Location = new Vec3f(0, 0 - 1.5F, 100),
            //    Radius = 1.5F * 20,
            //    Surface = new SquareGeneratedSphericalSurface() {
            //        FirstColor = Color.ForestGreen.ToVec3f(),
            //        SecondColor = Color.PaleGreen.ToVec3f(),
            //        SurfaceResult = new SurfaceResult(0, 0.25F, 0.75F)
            //    }
            //});

            scene.Shapes.Add(new Sphere() {
                Location = new Vec3f(0, 0, 8),
                Radius = 1.5F,
                Surface = new SquareGeneratedSphericalSurface() {
                    FirstColor = Color.ForestGreen.ToVec3f(),
                    SecondColor = Color.PaleGreen.ToVec3f(),
                    SurfaceResult = new SurfaceResult(0, 0.25F, 0.75F)
                }
            });

            scene.Shapes.Add(new Cylinder() {
                Location = new Vec3f(2.65F, 0, 8),
                Radius = 1.15F,
                Surface = new SquareGeneratedSphericalSurface() {
                    FirstColor = Color.White.ToVec3f(),
                    SecondColor = Color.DarkRed.ToVec3f(),
                    SurfaceResult = new SurfaceResult(0, 0.25F, 0.75F)
                }
            });

            scene.Shapes.Add(new Cone() {
                Location = new Vec3f(-2.375F, 1.8F, 8),
                HalfAngle = (float)(Math.PI / 8),
                V = new Vec3f(0, -1, 0)
            });

            scene.LightSources.Add(new PointLight() {
                Location = new Vec3f(2, -0.5F, -1),
                Lumens = 20000,
                Color = 1
            });

            scene.LightSources.Add(new PointLight() {
                Location = new Vec3f(-2, -0.5F, -1),
                Lumens = 20000,
                Color = 1
            });

            return scene;
        }
        private Scene LoadScene2() {
            Scene scene = new Scene();

            BitmapSky sky = new BitmapSky(new LockedBitmap((Bitmap)Image.FromFile("Assets\\Sky\\PanoramicImage.png"))) {
                HorizontalRotation = Rotation.FromDegrees(0),
                FogColor = Color.FromArgb(125, 143, 165).ToVec3f(),
                FogEnabled = false
            };

            scene.Sky = sky;
            scene.AmbientLight = sky.Image.GetAverageColor();
            scene.SkylightEnabled = false;
            scene.FogColor = sky.FogColor;

            Camera camera = new Camera() {
                WidthPx = ImageSize.Width,
                HeightPx = ImageSize.Height,
                Rotation = Rotation.FromDegrees(0),
                Direction = new Vec3f(0, 0, 1).Normalize(),
                Location = new Vec3f(0, 0, 0)
            };

            scene.Camera = camera;

            //scene.Shapes.Add(new Plane() {
            //    Location = new Vec3f(0, -1.5F, 0),
            //    Normal = new Vec3f(0, 1, 0).Normalize(),
            //    FlatSurface = new RotatedFlatSurface(
            //        new CheckedFlatSurface() {
            //            Zoom = 4
            //        },
            //        Rotation.FromDegrees(45))
            //});

            //scene.Shapes.Add(new Sphere() {
            //    Location = new Vec3f(0, 0, 8),
            //    Radius = 1.5F,
            //    Surface = new SimpleSurface() {
            //        SurfaceResult = new SurfaceResult(1, 0.25F, 0.75F) {
            //            RefractiveIndex = 1.3F
            //        }
            //    }
            //});

            scene.Shapes.Add(new Plane() {
                Location = new Vec3f(0, 0, 13),
                Normal = new Vec3f(0, 0, -1).Normalize(),
                FlatSurface = new RotatedFlatSurface(
                    new CustomFlatSurface(point => {
                        SurfaceResult firstResult = new SurfaceResult(0.4F, 0.25F, 0.75F);
                        SurfaceResult secondResult = new SurfaceResult(0.7F, 0.25F, 0.75F);

                        return Math.Floor(point.Y * 4) % 2 == 0 ? firstResult : secondResult;
                    }),
                    Rotation.FromDegrees(32))
            });

            scene.Shapes.Add(new Cylinder() {
                Location = new Vec3f(0, 0, 8),
                Radius = 1.15F,
                Surface = new SimpleSurface() {
                    SurfaceResult = new SurfaceResult(1, 0.25F, 0.75F) {
                        RefractiveIndex = 1.3F
                    }
                }
            });

            //scene.Shapes.Add(new Box() {
            //    Location = new Vec3f(1, -1, 8),
            //    SecondPoint = new Vec3f(-1, 1, 8.2F),
            //    Surface = new SimpleSurface() {
            //        SurfaceResult = new SurfaceResult(1, 0.25F, 0.75F) {
            //            RefractiveIndex = 0.5F
            //        }
            //    }
            //});

            //scene.LightSources.Add(new PointLight() {
            //    Location = new Vec3f(2, -0.5F, -1),
            //    Lumens = 30000,
            //    Color = 1
            //});

            //scene.LightSources.Add(new PointLight() {
            //    Location = new Vec3f(-2, -0.5F, -1),
            //    Lumens = 30000,
            //    Color = 1
            //});

            return scene;
        }
        private Scene LoadScene3() {
            Scene scene = new Scene();

            BitmapSky sky = new BitmapSky(new LockedBitmap((Bitmap)Image.FromFile("Assets\\Sky\\PanoramicImage.png"))) {
                HorizontalRotation = Rotation.FromDegrees(0),
                FogColor = Color.FromArgb(125, 143, 165).ToVec3f(),
                FogEnabled = true
            };

            scene.Sky = sky;
            scene.AmbientLight = sky.Image.GetAverageColor();
            scene.SkylightEnabled = false;
            scene.FogColor = sky.FogColor;

            Camera camera = new Camera() {
                WidthPx = ImageSize.Width,
                HeightPx = ImageSize.Height,
                Rotation = Rotation.FromDegrees(0),
                Direction = new Vec3f(0, 0, 1).Normalize(),
                Location = new Vec3f(0, 0, 0)
            };

            scene.Camera = camera;

            scene.Shapes.Add(new Plane() {
                Location = new Vec3f(0, -1.5F, 0),
                Normal = new Vec3f(0, 1, 0).Normalize(),
                FlatSurface = new RotatedFlatSurface(
                    new CheckedFlatSurface() {
                        Zoom = 4
                    },
                    Rotation.FromDegrees(45))
            });

            Cylinder cylinder = new Cylinder() {
                Location = new Vec3f(0, 0, 8),
                Radius = 1.15F,
                Surface = new SquareGeneratedSphericalSurface() {
                    FirstColor = Color.White.ToVec3f(),
                    SecondColor = Color.DarkRed.ToVec3f(),
                    SurfaceResult = new SurfaceResult(0, 0.25F, 0.75F)
                }
            };

            Sphere sphere = new Sphere() {
                Location = new Vec3f(0, 0, 8),
                Radius = 1.5F,
                Surface = new SquareGeneratedSphericalSurface() {
                    FirstColor = Color.White.ToVec3f(),
                    SecondColor = Color.DarkRed.ToVec3f(),
                    SurfaceResult = new SurfaceResult(0, 0.25F, 0.75F)
                }
            };

            IntersectedShape intersectedShape = new IntersectedShape(cylinder, sphere);

            Sphere sphere1 = new Sphere() {
                Location = new Vec3f(1, -1, 8),
                Radius = 1.5F,
                Surface = new SquareGeneratedSphericalSurface() {
                    FirstColor = Color.White.ToVec3f(),
                    SecondColor = Color.DarkRed.ToVec3f(),
                    SurfaceResult = new SurfaceResult(0, 0.25F, 0.75F)
                }
            };

            scene.Shapes.Add(new OffsetShape(new IntersectedShape(intersectedShape, sphere1), new Vec3f(0, 1, 0)));

            //scene.Shapes.Add(new Box() {
            //    Location = new Vec3f(1, -1, 8),
            //    SecondPoint = new Vec3f(-1, 1, 10F),
            //    Surface = new SimpleSurface() {
            //        SurfaceResult = new SurfaceResult(1, 0.25F, 0.75F) {
            //            RefractiveIndex = 1.5F
            //        }
            //    }
            //});

            scene.LightSources.Add(new PointLight() {
                Location = new Vec3f(2, -0.5F, -1),
                Lumens = 20000,
                Color = 1
            });

            scene.LightSources.Add(new PointLight() {
                Location = new Vec3f(-2, -0.5F, -1),
                Lumens = 20000,
                Color = 1
            });

            return scene;
        }
    }
}