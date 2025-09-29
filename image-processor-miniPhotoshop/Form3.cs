using System;
using System.Drawing;
using System.Linq; // Needed for the Histogram .Max() method
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WebCamLib;
using static image_processor_miniPhotoshop.Form2;

namespace image_processor_miniPhotoshop
{
    public partial class Form3 : Form
    {
        // The webcam device passed from the main form
        private Device webcamDevice;

        // This will hold the single photo captured from the webcam.
        // All filters will work on this image.
        private Bitmap capturedPhoto;

        #region Clipboard P/Invoke Functions
        // These are required to read the image from the clipboard
        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        private const uint CF_BITMAP = 2;
        #endregion

        private bool isCopying = false;

        public Form3(Device device)
        {
            InitializeComponent();
            this.webcamDevice = device;
        }

        // When the form loads, start the live preview.
        private void Form3_Load(object sender, EventArgs e)
        {
            if (webcamDevice != null)
            {
                // Start the live preview in pictureBox1.
                // Note: pictureBox1.Image will remain NULL.
                webcamDevice.ShowWindow(pictureBox1);
            }
            else
            {
                MessageBox.Show("Webcam device not found!");
                this.Close();
            }
        }

        // When "Copy" is clicked, grab one frame from the webcam.
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Invert the isCopying flag
            isCopying = !isCopying;

            if (isCopying)
            {
                // Start the timer to continuously capture frames
                timer1.Start();
                // Update the menu text to show the next possible action
                copyToolStripMenuItem.Text = "Stop Copying";
            }
            else
            {
                // Stop the timer
                timer1.Stop();
                // Change the text back
                copyToolStripMenuItem.Text = "Copy";
            }
        }

        // When the form closes, release the webcam. VERY IMPORTANT!
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (webcamDevice != null)
            {
                webcamDevice.Stop();
            }
            // Also dispose of the bitmap to free memory
            if (capturedPhoto != null)
            {
                capturedPhoto.Dispose();
            }
        }

        #region ======== Image Processing Filters ========
        // All filters below now use 'capturedPhoto' as the source.

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if a photo has been captured first
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap newPhoto = new Bitmap(capturedPhoto.Width, capturedPhoto.Height);
            for (int y = 0; y < capturedPhoto.Height; y++)
            {
                for (int x = 0; x < capturedPhoto.Width; x++)
                {
                    Color pixelColor = capturedPhoto.GetPixel(x, y);
                    int grey = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    Color greyColor = Color.FromArgb(grey, grey, grey);
                    newPhoto.SetPixel(x, y, greyColor);
                }
            }
            pictureBox2.Image = newPhoto;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap newPhoto = new Bitmap(capturedPhoto.Width, capturedPhoto.Height);
            for (int x = 0; x < capturedPhoto.Width; x++)
            {
                for (int y = 0; y < capturedPhoto.Height; y++)
                {
                    Color originalColor = capturedPhoto.GetPixel(x, y);
                    Color invertedColor = Color.FromArgb(255 - originalColor.R, 255 - originalColor.G, 255 - originalColor.B);
                    newPhoto.SetPixel(x, y, invertedColor);
                }
            }
            pictureBox2.Image = newPhoto;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            int[] histogram = new int[256];
            for (int y = 0; y < capturedPhoto.Height; y++)
            {
                for (int x = 0; x < capturedPhoto.Width; x++)
                {
                    Color pixelColor = capturedPhoto.GetPixel(x, y);
                    int grey = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    histogram[grey]++;
                }
            }

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
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap newPhoto = new Bitmap(capturedPhoto.Width, capturedPhoto.Height);
            for (int y = 0; y < capturedPhoto.Height; y++)
            {
                for (int x = 0; x < capturedPhoto.Width; x++)
                {
                    Color pixelColor = capturedPhoto.GetPixel(x, y);
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

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (webcamDevice == null)
            {
                MessageBox.Show("Webcam is not initialized!");
                return;
            }

            try
            {
                // Tell webcam to copy its current frame to the clipboard
                webcamDevice.Sendmessage();

                // Read the image from the clipboard
                if (OpenClipboard(IntPtr.Zero))
                {
                    IntPtr hBitmap = GetClipboardData(CF_BITMAP);
                    if (hBitmap != IntPtr.Zero)
                    {
                        // Get the image and store it in our 'capturedPhoto' variable
                        using (Bitmap clipboardBitmap = Image.FromHbitmap(hBitmap))
                        {
                            // Dispose of the old photo before creating a new one
                            if (capturedPhoto != null)
                                capturedPhoto.Dispose();
                            capturedPhoto = (Bitmap)clipboardBitmap.Clone();
                        }

                        // Display the newly captured photo in pictureBox2
                        Image oldImage = pictureBox2.Image;
                        pictureBox2.Image = (Bitmap)capturedPhoto.Clone(); // Show a copy
                        if (oldImage != null)
                            oldImage.Dispose();
                    }
                    CloseClipboard();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to capture frame: " + ex.Message);
            }
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide(); // hides Form3
            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.Show(); // show Form1
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void shrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);

            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = 1;
            m.Factor = 9; // average

            BitmapFilter.Conv3x3(bitmap, m);
            pictureBox2.Image = bitmap;
        }

        private void smoothinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);

            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = 1;
            m.Factor = 9;

            BitmapFilter.Conv3x3(bitmap, m);
            pictureBox2.Image = bitmap;
        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);
            BitmapFilter.GaussianBlur(bitmap);
            pictureBox2.Image = bitmap;
        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);
            BitmapFilter.Sharpen(bitmap);
            pictureBox2.Image = bitmap;
        }

        private void meanRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);
            BitmapFilter.MeanRemoval(bitmap);
            pictureBox2.Image = bitmap;
        }

        private void embossLaplascianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);
            BitmapFilter.EmbossLaplacian(bitmap);
            pictureBox2.Image = bitmap;
        }

        private void horzVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);
            BitmapFilter.Embossy_HV(bitmap);
            pictureBox2.Image = bitmap;
        }

        private void allDirectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);
            BitmapFilter.Embossy_All(bitmap);
            pictureBox2.Image = bitmap;
        }

        private void lossyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);
            BitmapFilter.Embossy_Lossy(bitmap);
            pictureBox2.Image = bitmap;
        }

        private void horizontalOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);
            BitmapFilter.Embossy_Horizontal(bitmap);
            pictureBox2.Image = bitmap;
        }

        private void verticalOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capturedPhoto == null)
            {
                MessageBox.Show("Please capture a photo first (File -> Copy).");
                return;
            }

            Bitmap bitmap = new Bitmap(capturedPhoto);
            BitmapFilter.Embossy_Vertical(bitmap);
            pictureBox2.Image = bitmap;
        }
    }
}