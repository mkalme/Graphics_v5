using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Vectors.Vectors3D;

namespace Graphics {
    public class LockedBitmap {
        public Bitmap Source { get; set; }
        public BitmapData Data { get; set; }
        public byte[] RGBValues { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public LockedBitmap(Color color, int width, int height) {
            Bitmap bitmap = new Bitmap(width, height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.Clear(color);

            Lock(bitmap);
        }
        public LockedBitmap(Bitmap bitmap) {
            Lock(bitmap);
        }

        public void SetPixel(Color color, int x, int y) {
            int index = (y * Width + x) * 4;

            RGBValues[index + 3] = color.A;
            RGBValues[index + 2] = color.R;
            RGBValues[index + 1] = color.G;
            RGBValues[index] = color.B;
        }
        public Color GetPixel(int x, int y) {
            int index = (y * Width + x) * 4;

            return Color.FromArgb(RGBValues[index + 3], RGBValues[index + 2], RGBValues[index + 1], RGBValues[index]);
        }

        public Vec3f GetAverageColor() {
            Vec3f output = new Vec3f();
            
            for (int y = 0; y < Height / 2; y++) {
                for (int x = 0; x < Width; x++) {
                    output += GetPixel(x, y).ToVec3f();
                }
            }

            return output / (Width * Height / 2);
        }

        public void Lock(Bitmap bitmap) {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(data.Stride) * data.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(data.Scan0, rgbValues, 0, bytes);

            Source = bitmap;
            Data = data;
            RGBValues = rgbValues;
            Width = bitmap.Width;
            Height = bitmap.Height;
        }
        public void Unlock() {
            if (Data == null) {
                using (MemoryStream stream = new MemoryStream(RGBValues)) {
                    Source = new Bitmap(stream);
                }
            } else {
                Marshal.Copy(RGBValues, 0, Data.Scan0, RGBValues.Length);
                Source.UnlockBits(Data);
            }
        }

        public Bitmap CreateBitmapCopy() {
            Bitmap output = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);

            Rectangle boundsRect = new Rectangle(0, 0, Width, Height);
            BitmapData bmpData = output.LockBits(boundsRect, ImageLockMode.WriteOnly, output.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int skipByte = bmpData.Stride - Width * 4;
            byte[] newBuff = new byte[RGBValues.Length + skipByte * Height];
            for (int j = 0; j < Height; j++) {
                Buffer.BlockCopy(RGBValues, j * Width * 4, newBuff, j * (Width * 4 + skipByte), Width * 4);
            }

            Marshal.Copy(newBuff, 0, ptr, newBuff.Length);
            output.UnlockBits(bmpData);

            return output;
        }
    }
}