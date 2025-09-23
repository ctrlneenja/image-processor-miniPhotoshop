using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}

