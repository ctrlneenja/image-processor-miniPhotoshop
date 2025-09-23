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
    public partial class Form1 : Form
    {
        private WebCamLib.DeviceManager deviceManager;
        private WebCamLib.Device webcamDevice;
        private Panel panelWebcam;

        public Form1()
        {
            InitializeComponent();

            // Create and add panelWebcam if not using the designer
            panelWebcam = new Panel();
            panelWebcam.Name = "panelWebcam";
            panelWebcam.Size = new Size(320, 240); // Set your preferred size
            panelWebcam.Location = new Point(10, 10); // Set your preferred location
            this.Controls.Add(panelWebcam);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFileDialog.FileName);
                Form2 form2 = new Form2();
                form2.SetImage(img);
                form2.Show();
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var devices = WebCamLib.DeviceManager.GetAllDevices();
            if (devices != null && devices.Length > 0)
            {
                webcamDevice = devices[0]; // Use first webcam

                Form3 form3 = new Form3(webcamDevice); // pass device to Form3
                form3.Show();

                webcamDevice.ShowWindow(form3.pictureBox1); // show webcam preview

                this.Hide();


            }
            else
            {
                MessageBox.Show("No webcam found.");
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();

        }
    }
}
