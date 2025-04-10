using Lidgren.Network;
using System.Media;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace netchat
{
    struct Address
    {
        public string ip;
        public int port;

        public Address(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
    }

    public partial class NetChatForm : Form
    {
        private static NetServer netServer;
        private static NetClient netClient;
        private static string username;

        public NetChatForm()
        {
            InitializeComponent();
            input.KeyDown += new KeyEventHandler(BoxKeyDown);        }

        private void BoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && input.Text.Length > 0)
            {
                SendMessage($"{username}: { input.Text}");
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

        Address SetAddress(string ip, int port)
        {
            if (portfield.Text.Length > 0) port = Convert.ToInt32(portfield.Text);
            if (ip.Length < 7) ip = ipfield.PlaceholderText;
            if (port < 1000) port = Convert.ToInt32(portfield.PlaceholderText);

            return new Address(ip, port);
        }

        private void host_Click(object sender, EventArgs e)
        {
            host.Enabled = false;
            join.Enabled = false;
            ipfield.Enabled = false;
            portfield.Enabled = false;
            input.Enabled = false; //remove if the host also wants to talk

            string ip = ipfield.Text;
            int port = 0;
            Address address = SetAddress(ip, port);        

            Thread serverThread = new Thread(() => Server(address.ip, address.port));
            serverThread.IsBackground = true;
            serverThread.Start();
            ipfield.Text = ip;
        }

        private void join_Click(object sender, EventArgs e)
        {
            host.Enabled = false;
            join.Enabled = false;
            ipfield.Enabled = false;
            portfield.Enabled = false;
            if (namefield.Text.Length < 1) username = namefield.PlaceholderText;
            else username = namefield.Text;
            Client();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void input_TextChanged(object sender, EventArgs e)
        {

        }

        private void SendMessage(string message)
        {
            netClient.SendMessage(netClient.CreateMessage(message), NetDeliveryMethod.ReliableOrdered);
            ScrollDown();
        }

        private void ScrollDown()
        {
            chatbox.SelectionStart = chatbox.Text.Length;
            chatbox.ScrollToCaret();
        }

        private void LocalNewLine(string text, Color textColor = default)
        {
            if (textColor == default) textColor = Color.Black;

            chatbox.SelectionStart = chatbox.TextLength;
            chatbox.SelectionLength = 0;
            chatbox.SelectionColor = textColor;

            chatbox.AppendText(text + "\r\n");
            Console.WriteLine(text);
            chatbox.SelectionColor = DefaultForeColor;
        }

        private void Server(string ip, int port)
        {
            if (InvokeRequired) Invoke(() => LocalNewLine($"hosting at {ip} on port: {port}...", Color.DarkGoldenrod));

            NetPeerConfiguration config = new NetPeerConfiguration("netchat");
            config.MaximumConnections = 5;
            config.Port = port;
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
                            }

                            UpdateConnectionsList();
                            break;
                        case NetIncomingMessageType.Data:
                            string c = im.ReadString();
                            if (InvokeRequired) Invoke(() => LocalNewLine("Broadcasting '" + c + "'", Color.Gray));
                            if (InvokeRequired) Invoke(() => ScrollDown());

                            foreach (NetConnection nc in netServer.Connections)
                            {
                                //if (im.SenderConnection == nc) continue;
                                netServer.SendMessage(netServer.CreateMessage(c), nc, NetDeliveryMethod.ReliableOrdered);
                            }
                            List<NetConnection> all = netServer.Connections;
                            all.Remove(im.SenderConnection);
                            break;
                        default:
                            if (InvokeRequired) Invoke(() => LocalNewLine("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel, Color.Red));
                            break;
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void Client()
        {
            netClient = new NetClient(new("netchat"));
            netClient.Start();
            string ip = ipfield.Text;
            int port = 0;
            Address address = SetAddress(ip, port);
            netClient.Connect(address.ip, address.port);
            Thread clientThread = new Thread(() =>
            {
                ListenForMessages();
            });
            clientThread.IsBackground = true;
            clientThread.Start();
        }

        static int i;
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
                            if (InvokeRequired) Invoke(() => LocalNewLine(text));
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                            string reason = im.ToString();
                            if (InvokeRequired) Invoke(() => LocalNewLine(status.ToString() + ": " + reason, Color.Blue));
                            break;
                        case NetIncomingMessageType.Data:
                            string chat = im.ReadString();
                            if (InvokeRequired) Invoke(() => LocalNewLine(chat));
                            break;
                        default:
                            if (InvokeRequired) Invoke(() => LocalNewLine("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes", Color.DarkRed));
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

        private void ipfield_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
