using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace image_processor_miniPhotoshop
{
    public partial class Form2 : Form
    {
        Bitmap bitmap;
        Bitmap newPhoto;
        public Form2()
        {
            InitializeComponent();
        }

        public void SetImage(Image img)
        {
            pictureBox1.Image = img;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
           bitmap = new Bitmap(pictureBox1.Image);
           newPhoto = new Bitmap(bitmap.Width, bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    newPhoto.SetPixel(x, y, pixelColor);
                }
            }

            pictureBox2.Image = newPhoto;

        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Image);
            newPhoto = new Bitmap(bitmap.Width, bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    int grey = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    Color greyColor = Color.FromArgb(grey, grey, grey);
                    newPhoto.SetPixel(x, y, greyColor);
                }
            }

            pictureBox2.Image = newPhoto;
        }


        public static Color InvertColor(Color color)
        {
            return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
             bitmap = new Bitmap(pictureBox1.Image);
             newPhoto = new Bitmap(bitmap.Width, bitmap.Height);  
             
            for (int x = 0; x < bitmap.Width; x++)
            {
                for(int y = 0; y < bitmap.Height; y++)
                {
                    Color originalColor = bitmap.GetPixel(x, y);
                    Color invertedColor = InvertColor(originalColor);
                    newPhoto.SetPixel(x, y, invertedColor);
                }
            }
            pictureBox2.Image = newPhoto;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Image);
            int[] histogram = new int[256];

            // Calculate histogram
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    int grey = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    histogram[grey]++;
                }
            }

            // Draw histogram directly
            int histWidth = 256;
            int histHeight = 100;
            Bitmap histImage = new Bitmap(histWidth, histHeight);

            int max = histogram.Max();

            using (Graphics g = Graphics.FromImage(histImage))
            {
                g.Clear(Color.White);
                for (int x = 0; x < histWidth; x++)
                {
                    int value = histogram[x];
                    int barHeight = max > 0 ? (int)((value / (float)max) * (histHeight - 1)) : 0;
                    g.DrawLine(Pens.Black, x, histHeight - 1, x, histHeight - 1 - barHeight);
                }
            }

            pictureBox2.Image = histImage;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Image);
            newPhoto = new Bitmap(bitmap.Width, bitmap.Height);

            for(int y = 0; y < bitmap.Height; y++)
            {
                for(int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    int tr = (int)(0.393 * pixelColor.R + 0.769 * pixelColor.G + 0.189 * pixelColor.B);
                    int tg = (int)(0.349 * pixelColor.R + 0.686 * pixelColor.G + 0.168 * pixelColor.B);
                    int tb = (int)(0.272 * pixelColor.R + 0.534 * pixelColor.G + 0.131 * pixelColor.B);
                    tr = Math.Min(255, tr);
                    tg = Math.Min(255, tg);
                    tb = Math.Min(255, tb);
                    Color sepiaColor = Color.FromArgb(tr, tg, tb);
                    newPhoto.SetPixel(x, y, sepiaColor);
                }
            }
                pictureBox2.Image = newPhoto;
        }

        private void savePhotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("There is no processed image to save!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Save Processed Photo";
                saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                saveDialog.DefaultExt = "png";
                saveDialog.FileName = "ProcessedPhoto";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Determine the format based on extension
                        var ext = System.IO.Path.GetExtension(saveDialog.FileName).ToLower();
                        System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;

                        if (ext == ".jpg" || ext == ".jpeg") format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        else if (ext == ".bmp") format = System.Drawing.Imaging.ImageFormat.Bmp;

                        pictureBox2.Image.Save(saveDialog.FileName, format);
                        MessageBox.Show("Photo saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving photo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.Show(); 
            }
        }

        private void chooseNewPhotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Select a Photo";
                openDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Dispose previous image if it exists
                        if (pictureBox1.Image != null)
                            pictureBox1.Image.Dispose();

                        // Load the new image
                        Bitmap newPhoto = new Bitmap(openDialog.FileName);
                        pictureBox1.Image = newPhoto;

                        // Optionally, clear processed photo box
                        if (pictureBox2.Image != null)
                        {
                            pictureBox2.Image.Dispose();
                            pictureBox2.Image = null;
                        }

                        toolStripStatusLabel1.Text = "New photo loaded!";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to load photo: " + ex.Message);
                    }
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public class ConvMatrix

        {

            public int TopLeft = 0, TopMid = 0, TopRight = 0;

            public int MidLeft = 0, Pixel = 1, MidRight = 0;

            public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;

            public int Factor = 1;

            public int Offset = 0;

            public void SetAll(int nVal)

            {

                TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight =

                          BottomLeft = BottomMid = BottomRight = nVal;

            }

        }

        private void filters20ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void shrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("No image loaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bitmap b = new Bitmap(pictureBox1.Image); // source image
            Bitmap bSrc = (Bitmap)b.Clone();
            Bitmap newBitmap = new Bitmap(b.Width, b.Height);

            // Example convolution matrix (shrink/blur-like effect)
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = 1;
            m.Factor = 9; // average of 9 pixels

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
                        for (int color = 0; color < 3; color++) // B, G, R channels
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

            pictureBox2.Image = b;
        }

        private void smoothingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            int nWeight = 1; // default smoothing weight

            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.Factor = nWeight + 8;

            // Apply convolution
            BitmapFilter.Conv3x3(bitmap, m);

            pictureBox2.Image = bitmap; // show processed image
        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            BitmapFilter.GaussianBlur(bitmap);

            pictureBox2.Image = bitmap;
        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            BitmapFilter.Sharpen(bitmap);

            pictureBox2.Image = bitmap;
        }

        private void meanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            BitmapFilter.MeanRemoval(bitmap);

            pictureBox2.Image = bitmap;
        }

        private void embossLaplascianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            BitmapFilter.EmbossLaplacian(bitmap);

            pictureBox2.Image = bitmap;
        }

        private void horzVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            BitmapFilter.Embossy_HV(bitmap);

            pictureBox2.Image = bitmap;
        }

        private void allDirectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            BitmapFilter.Embossy_All(bitmap);

            pictureBox2.Image = bitmap;
        }

        private void lossyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            BitmapFilter.Embossy_Lossy(bitmap);

            pictureBox2.Image = bitmap;
        }

        private void horizontalOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            BitmapFilter.Embossy_Horizontal(bitmap);

            pictureBox2.Image = bitmap;
        }

        private void verticalOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            BitmapFilter.Embossy_Vertical(bitmap);

            pictureBox2.Image = bitmap;
        }
    }
}

namespace image_processor_miniPhotoshop
{
    public static class BitmapFilter
    {
        public static bool Conv3x3(Bitmap b, Form2.ConvMatrix m)
        {
            if (m.Factor == 0)
                return false;

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
                        for (int color = 0; color < 3; color++) // process B, G, R
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




