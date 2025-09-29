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

        private Bitmap ApplyPixelFilter(Bitmap src, Func<Color, Color> transform)
        {
            Bitmap result = new Bitmap(src.Width, src.Height);
            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    result.SetPixel(x, y, transform(src.GetPixel(x, y)));
                }
            }
            return result;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Image);
            pictureBox2.Image = ApplyPixelFilter(bitmap, c => c);

        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Image);
            pictureBox2.Image = ApplyPixelFilter(bitmap, c => {
                int g = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                return Color.FromArgb(g, g, g);
            });
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Image);
            pictureBox2.Image = ApplyPixelFilter(bitmap, c => Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
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
            pictureBox2.Image = ApplyPixelFilter(bitmap, c => {
                int tr = (int)(0.393 * c.R + 0.769 * c.G + 0.189 * c.B);
                int tg = (int)(0.349 * c.R + 0.686 * c.G + 0.168 * c.B);
                int tb = (int)(0.272 * c.R + 0.534 * c.G + 0.131 * c.B);
                return Color.FromArgb(Math.Min(255, tr), Math.Min(255, tg), Math.Min(255, tb));
            });
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

            Bitmap bitmap = new Bitmap(pictureBox1.Image);

            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = 1;
            m.Factor = 9; // average blur

            BitmapFilter.Conv3x3(bitmap, m);
            pictureBox2.Image = bitmap;
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




