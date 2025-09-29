using System.Drawing;
using System.Drawing.Imaging;
using static image_processor_miniPhotoshop.Form2;

namespace image_processor_miniPhotoshop
{
    public static class BitmapFilter
    {
        public static bool Conv3x3(Bitmap b, ConvMatrix m)
        {
            if (m.Factor == 0) return false;

            Bitmap bSrc = (Bitmap)b.Clone();

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                           ImageLockMode.ReadWrite,
                                           PixelFormat.Format24bppRgb);

            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height),
                                             ImageLockMode.ReadWrite,
                                             PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;

            unsafe
            {
                byte* p = (byte*)(void*)bmData.Scan0;
                byte* pSrc = (byte*)(void*)bmSrc.Scan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        for (int color = 0; color < 3; color++) // B, G, R
                        {
                            int nPixel =
                                (
                                    (pSrc[color] * m.TopLeft) +
                                    (pSrc[color + 3] * m.TopMid) +
                                    (pSrc[color + 6] * m.TopRight) +
                                    (pSrc[color + stride] * m.MidLeft) +
                                    (pSrc[color + stride + 3] * m.Pixel) +
                                    (pSrc[color + stride + 6] * m.MidRight) +
                                    (pSrc[color + stride2] * m.BottomLeft) +
                                    (pSrc[color + stride2 + 3] * m.BottomMid) +
                                    (pSrc[color + stride2 + 6] * m.BottomRight)
                                ) / m.Factor + m.Offset;

                            if (nPixel < 0) nPixel = 0;
                            if (nPixel > 255) nPixel = 255;

                            p[color + stride] = (byte)nPixel;
                        }

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

            public static bool GaussianBlur(Bitmap b)
            {
                Form2.ConvMatrix m = new Form2.ConvMatrix();

                m.TopLeft = 1; m.TopMid = 2; m.TopRight = 1;
                m.MidLeft = 2; m.Pixel = 4; m.MidRight = 2;
                m.BottomLeft = 1; m.BottomMid = 2; m.BottomRight = 1;

                m.Factor = 16;  // sum of kernel weights
                m.Offset = 0;

                return Conv3x3(b, m);
            }

            public static bool Sharpen(Bitmap b)
            {
                Form2.ConvMatrix m = new Form2.ConvMatrix();

                m.TopLeft = 0; m.TopMid = -2; m.TopRight = 0;
                m.MidLeft = -2; m.Pixel = 11; m.MidRight = -2;
                m.BottomLeft = 0; m.BottomMid = -2; m.BottomRight = 0;

                m.Factor = 3;   // normalization factor
                m.Offset = 0;   // no brightness adjustment

                return Conv3x3(b, m);
            }

            public static bool MeanRemoval(Bitmap b)
            {
                Form2.ConvMatrix m = new Form2.ConvMatrix();

                m.TopLeft = -1; m.TopMid = -1; m.TopRight = -1;
                m.MidLeft = -1; m.Pixel = 9; m.MidRight = -1;
                m.BottomLeft = -1; m.BottomMid = -1; m.BottomRight = -1;

                m.Factor = 1;   // normalization factor
                m.Offset = 0;   // no brightness adjustment

                return Conv3x3(b, m);
            }

            public static bool EmbossLaplacian(Bitmap b)
            {
                Form2.ConvMatrix m = new Form2.ConvMatrix();

                m.TopLeft = -1; m.TopMid = 0; m.TopRight = -1;
                m.MidLeft = 0; m.Pixel = 4; m.MidRight = 0;
                m.BottomLeft = -1; m.BottomMid = 0; m.BottomRight = -1;

                m.Factor = 1;    // no normalization
                m.Offset = 127;  // shift grayscale to middle range

                return Conv3x3(b, m);
            }

            public static bool Embossy_HV(Bitmap b)
            {
                Form2.ConvMatrix m = new Form2.ConvMatrix();

                m.TopLeft = 0; m.TopMid = -1; m.TopRight = 0;
                m.MidLeft = -1; m.Pixel = 4; m.MidRight = -1;
                m.BottomLeft = 0; m.BottomMid = -1; m.BottomRight = 0;

                m.Factor = 1;
                m.Offset = 127;

                return Conv3x3(b, m);
            }

            public static bool Embossy_All(Bitmap b)
            {
                Form2.ConvMatrix m = new Form2.ConvMatrix();

                m.TopLeft = -1; m.TopMid = -1; m.TopRight = -1;
                m.MidLeft = -1; m.Pixel = 8; m.MidRight = -1;
                m.BottomLeft = -1; m.BottomMid = -1; m.BottomRight = -1;

                m.Factor = 1;
                m.Offset = 127;

                return Conv3x3(b, m);
            }

            public static bool Embossy_Lossy(Bitmap b)
            {
                Form2.ConvMatrix m = new Form2.ConvMatrix();

                m.TopLeft = 1; m.TopMid = -2; m.TopRight = 1;
                m.MidLeft = -2; m.Pixel = 4; m.MidRight = -2;
                m.BottomLeft = -2; m.BottomMid = 1; m.BottomRight = -2;

                m.Factor = 1;
                m.Offset = 127;

                return Conv3x3(b, m);
            }

            public static bool Embossy_Horizontal(Bitmap b)
            {
                Form2.ConvMatrix m = new Form2.ConvMatrix();

                m.TopLeft = 0; m.TopMid = 0; m.TopRight = 0;
                m.MidLeft = -1; m.Pixel = 2; m.MidRight = -1;
                m.BottomLeft = 0; m.BottomMid = 0; m.BottomRight = 0;

                m.Factor = 1;
                m.Offset = 127;

                return Conv3x3(b, m);
            }

            public static bool Embossy_Vertical(Bitmap b)
            {
                Form2.ConvMatrix m = new Form2.ConvMatrix();

                m.TopLeft = 0; m.TopMid = -1; m.TopRight = 0;
                m.MidLeft = 0; m.Pixel = 0; m.MidRight = 0;
                m.BottomLeft = 0; m.BottomMid = 1; m.BottomRight = 0;

                m.Factor = 1;
                m.Offset = 127;

                return Conv3x3(b, m);
            }
    }
}


