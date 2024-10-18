using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Form2 form2;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None; // Remove the border and X button
            this.ShowInTaskbar = false; // Hide Form1 from the taskbar
            this.TopMost = true; // Make Form1 stay on top
            this.Load += Form1_Load;
            this.MouseDown += Form1_MouseDown; // Subscribe to MouseDown event
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Get the screen's working area (excluding taskbars, etc.)
            var workingArea = Screen.PrimaryScreen.WorkingArea;

            // Position the form in the bottom-right corner, 0.5 cm higher
            int offset = (int)(18.9); // 0.5 cm in pixels (approximately)
            this.Location = new Point(workingArea.Right - this.Width, workingArea.Bottom - this.Height - offset);

            // Wait for 2 seconds before displaying Form2
            await Task.Delay(2000);

            // Create and show Form2 centered
            form2 = new Form2();
            form2.TopMost = true; // Ensure Form2 stays on top
            form2.StartPosition = FormStartPosition.CenterScreen; // Ensure Form2 appears at the center
            form2.Show();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if form2 is not null and currently visible
            if (form2 != null && form2.Visible)
            {
                VibrateForm2(); // Call the method to vibrate Form2
            }
        }

        private async void VibrateForm2()
        {
            int vibrationAmount = 10; // Number of pixels to move Form2
            int vibrationCount = 10; // Number of vibration iterations

            Point originalLocation = form2.Location;

            for (int i = 0; i < vibrationCount; i++)
            {
                // Move Form2 slightly to the left
                form2.Location = new Point(originalLocation.X - vibrationAmount, originalLocation.Y);
                await Task.Delay(20); // Wait 20 milliseconds

                // Move Form2 slightly to the right
                form2.Location = new Point(originalLocation.X + vibrationAmount, originalLocation.Y);
                await Task.Delay(20); // Wait 20 milliseconds
            }

            // Reset to the original position
            form2.Location = originalLocation;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // PictureBox click logic (if any)
        }
    }
}
