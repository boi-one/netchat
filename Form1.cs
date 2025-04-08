using Lidgren.Network;
using System.Media;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace netchat
{
    public partial class netchatform : Form
    {
        private static NetServer netServer;
        private static NetClient netClient;

        public netchatform()
        {
            InitializeComponent();
            input.KeyDown += new KeyEventHandler(BoxKeyDown);
        }

        private void BoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && input.Text.Length > 0)
            {
                SendMessage(input.Text);
                input.Text = "";
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
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
                input.Enabled = false; //remove if the host also wants to talk
                new Thread(() => Server("127.0.0.1", Convert.ToInt32(portfield.Text))).Start();
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

        private void SendMessage(string message)
        {
            netClient.SendMessage(netClient.CreateMessage(message), NetDeliveryMethod.ReliableOrdered);
        }

        private void LocalNewLine(string text, Color textColor = default)
        {
            if (textColor == default) textColor = Color.Black;

            chatbox.SelectionStart = chatbox.TextLength;
            chatbox.SelectionLength = 0;
            chatbox.SelectionColor = textColor;

            chatbox.AppendText(text + "\r\n");
            chatbox.SelectionColor = RichTextBox.DefaultForeColor;
        }

        private void Server(string ip, int port)
        {
            if (InvokeRequired) Invoke(() => LocalNewLine("hosting...", Color.Yellow));

            NetPeerConfiguration config = new NetPeerConfiguration("netchat");
            config.MaximumConnections = 5;
            config.Port = 5839;
            netServer = new NetServer(config);

            netServer.Start();

            NetIncomingMessage im;

            while (true)
            {
                while ((im = netServer.ReadMessage()) != null)
                {
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                            string reason = im.ReadString();
                            if (InvokeRequired) Invoke(() => LocalNewLine(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason, Color.YellowGreen));
                            Console.WriteLine(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason, Color.YellowGreen);

                            if (status == NetConnectionStatus.Connected)
                            {
                                if (InvokeRequired) Invoke(() => LocalNewLine("Client connected", Color.Green));
                                Console.WriteLine("Client connected");
                            }

                            UpdateConnectionsList();
                            break;
                        case NetIncomingMessageType.Data:
                            string c = im.ReadString();
                            LocalNewLine("Broadcasting '" + c + "'", Color.Gray);
                            Console.WriteLine(("Broadcasting '" + c + "'", Color.Gray));
                            foreach (NetConnection nc in netServer.Connections)
                            {
                                //if (im.SenderConnection == nc) continue;
                                netServer.SendMessage(netServer.CreateMessage(c), nc, NetDeliveryMethod.ReliableOrdered);
                            }
                            List<NetConnection> all = netServer.Connections;
                            all.Remove(im.SenderConnection);

                            if (all.Count > 0)
                            {
                                NetOutgoingMessage om = netServer.CreateMessage();
                                om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + c);
                                netServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                            }
                            break;
                        default:
                            if (InvokeRequired) Invoke(() => LocalNewLine("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel, Color.Red));
                            Console.WriteLine("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel, Color.Red);
                            break;
                    }
                }
                //Thread.Sleep(1);
            }
        }

        private void Client()
        {
            netClient = new NetClient(new("netchat"));
            netClient.Start();
            netClient.Connect(ip.Text, Convert.ToInt32(port.Text));
            new Thread(() =>
            {
                ListenForMessages();
            }).Start();
            LocalNewLine("successfuly joined!", Color.Blue);
        }

        private void ListenForMessages()
        {
            NetIncomingMessage im;
            while (true)
            {
                while ((im = netClient.ReadMessage()) != null)
                {
                    // handle incoming message
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            string text = im.ReadString();
                            LocalNewLine(text);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                            string reason = im.ReadString();
                            Console.WriteLine(status.ToString() + ": " + reason);

                            break;
                        case NetIncomingMessageType.Data:
                            string chat = im.ReadString();
                            LocalNewLine(chat);
                            break;
                        default:
                            LocalNewLine("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes", Color.Red);
                            break;
                    }
                    netClient.Recycle(im);
                }
                Thread.Sleep(1);
            }
        }

        private static void UpdateConnectionsList()
        {
            foreach (NetConnection c in netServer.Connections)
            {
                string s = NetUtility.ToHexString(c.RemoteUniqueIdentifier) + " from " + c.RemoteEndPoint.ToString() + " [" + c.Status + "]";
            }
        }

        private void portfield_TextChanged(object sender, EventArgs e)
        {

        }

        private void chatbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
