namespace netchat
{
    partial class NetChatForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetChatForm));
            ip = new Label();
            port = new Label();
            ipfield = new TextBox();
            portfield = new TextBox();
            join = new Button();
            host = new Button();
            input = new TextBox();
            name = new Label();
            namefield = new TextBox();
            chatbox = new RichTextBox();
            SuspendLayout();
            // 
            // ip
            // 
            ip.AutoSize = true;
            ip.Location = new Point(524, 15);
            ip.Name = "ip";
            ip.Size = new Size(17, 15);
            ip.TabIndex = 0;
            ip.Text = "ip";
            ip.Click += label1_Click;
            // 
            // port
            // 
            port.AutoSize = true;
            port.Location = new Point(653, 15);
            port.Name = "port";
            port.Size = new Size(29, 15);
            port.TabIndex = 1;
            port.Text = "port";
            // 
            // ipfield
            // 
            ipfield.Location = new Point(547, 12);
            ipfield.Name = "ipfield";
            ipfield.PlaceholderText = "127.0.0.1";
            ipfield.Size = new Size(100, 23);
            ipfield.TabIndex = 2;
            ipfield.TextChanged += ipfield_TextChanged;
            // 
            // portfield
            // 
            portfield.Location = new Point(688, 12);
            portfield.Name = "portfield";
            portfield.PlaceholderText = "8421";
            portfield.Size = new Size(100, 23);
            portfield.TabIndex = 3;
            portfield.TextChanged += portfield_TextChanged;
            // 
            // join
            // 
            join.Location = new Point(443, 12);
            join.Name = "join";
            join.Size = new Size(75, 23);
            join.TabIndex = 5;
            join.Text = "join";
            join.UseVisualStyleBackColor = true;
            join.Click += join_Click;
            // 
            // host
            // 
            host.Location = new Point(362, 12);
            host.Name = "host";
            host.Size = new Size(75, 23);
            host.TabIndex = 6;
            host.Text = "host";
            host.UseVisualStyleBackColor = true;
            host.Click += host_Click;
            // 
            // input
            // 
            input.Location = new Point(12, 420);
            input.Name = "input";
            input.Size = new Size(776, 23);
            input.TabIndex = 7;
            input.TextChanged += input_TextChanged;
            // 
            // name
            // 
            name.AutoSize = true;
            name.Location = new Point(15, 16);
            name.Name = "name";
            name.Size = new Size(37, 15);
            name.TabIndex = 8;
            name.Text = "name";
            // 
            // namefield
            // 
            namefield.Location = new Point(58, 13);
            namefield.Name = "namefield";
            namefield.PlaceholderText = "Default Johnson";
            namefield.Size = new Size(100, 23);
            namefield.TabIndex = 9;
            // 
            // chatbox
            // 
            chatbox.Enabled = false;
            chatbox.Location = new Point(15, 42);
            chatbox.Name = "chatbox";
            chatbox.Size = new Size(773, 372);
            chatbox.TabIndex = 10;
            chatbox.Text = "";
            chatbox.TextChanged += richTextBox1_TextChanged;
            // 
            // NetChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(chatbox);
            Controls.Add(namefield);
            Controls.Add(name);
            Controls.Add(input);
            Controls.Add(host);
            Controls.Add(join);
            Controls.Add(portfield);
            Controls.Add(ipfield);
            Controls.Add(port);
            Controls.Add(ip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "NetChatForm";
            Text = "netchat";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ip;
        private Label port;
        private TextBox ipfield;
        private TextBox portfield;
        private Button join;
        private Button host;
        private TextBox input;
        private Label name;
        private TextBox namefield;
        private RichTextBox chatbox;
    }
}
