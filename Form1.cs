using Lidgren.Network;
using System.Media;
using System.Security.Cryptography;

namespace netchat
{
    public partial class netchatform : Form
    {
        private static NetServer netServer;
        private static NetClient netClient;

        public netchatform()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void host_Click(object sender, EventArgs e)
        {
            if (portfield.Text.Length < 4)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("Needs to be a valid port.", "Warning.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                host.Enabled = false;
                join.Enabled = false;
                ipfield.Enabled = false;
                portfield.Enabled = false;
                Server();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void input_TextChanged(object sender, EventArgs e)
        {

        }

        private void Server()
        {

        }

        private void Client()
        {

        }
    }
}
