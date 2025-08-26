namespace Test.Client
{
	partial class MainForm
	{
		/// <summary>
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 設計工具產生的程式碼

		/// <summary>
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
			this.Account = new System.Windows.Forms.TextBox();
			this.Password = new System.Windows.Forms.TextBox();
			this.Login = new System.Windows.Forms.Button();
			this.SendMessage = new System.Windows.Forms.Button();
			this.SendText = new System.Windows.Forms.TextBox();
			this.ConnectLabel = new System.Windows.Forms.Label();
			this.Logout = new System.Windows.Forms.Button();
			this.LoginStatusLabel = new System.Windows.Forms.Label();
			this.Chat = new System.Windows.Forms.RichTextBox();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// Account
			// 
			this.Account.Location = new System.Drawing.Point(24, 23);
			this.Account.Name = "Account";
			this.Account.Size = new System.Drawing.Size(100, 22);
			this.Account.TabIndex = 0;
			this.Account.TextChanged += new System.EventHandler(this.Account_TextChanged);
			// 
			// Password
			// 
			this.Password.Location = new System.Drawing.Point(24, 63);
			this.Password.Name = "Password";
			this.Password.Size = new System.Drawing.Size(100, 22);
			this.Password.TabIndex = 1;
			this.Password.TextChanged += new System.EventHandler(this.Password_TextChanged);
			// 
			// Login
			// 
			this.Login.Location = new System.Drawing.Point(173, 23);
			this.Login.Name = "Login";
			this.Login.Size = new System.Drawing.Size(75, 23);
			this.Login.TabIndex = 2;
			this.Login.Text = "Login";
			this.Login.UseVisualStyleBackColor = true;
			this.Login.Click += new System.EventHandler(this.Login_Click);
			// 
			// SendMessage
			// 
			this.SendMessage.Location = new System.Drawing.Point(304, 105);
			this.SendMessage.Name = "SendMessage";
			this.SendMessage.Size = new System.Drawing.Size(75, 23);
			this.SendMessage.TabIndex = 3;
			this.SendMessage.Text = "Send";
			this.SendMessage.UseVisualStyleBackColor = true;
			this.SendMessage.Click += new System.EventHandler(this.SendMessage_Click);
			// 
			// SendText
			// 
			this.SendText.Location = new System.Drawing.Point(24, 107);
			this.SendText.Name = "SendText";
			this.SendText.Size = new System.Drawing.Size(257, 22);
			this.SendText.TabIndex = 5;
			this.SendText.TextChanged += new System.EventHandler(this.SendText_TextChanged);
			// 
			// ConnectLabel
			// 
			this.ConnectLabel.AutoSize = true;
			this.ConnectLabel.Location = new System.Drawing.Point(302, 28);
			this.ConnectLabel.Name = "ConnectLabel";
			this.ConnectLabel.Size = new System.Drawing.Size(23, 12);
			this.ConnectLabel.TabIndex = 6;
			this.ConnectLabel.Text = "link";
			// 
			// Logout
			// 
			this.Logout.Location = new System.Drawing.Point(173, 63);
			this.Logout.Name = "Logout";
			this.Logout.Size = new System.Drawing.Size(75, 23);
			this.Logout.TabIndex = 7;
			this.Logout.Text = "Logout";
			this.Logout.UseVisualStyleBackColor = true;
			this.Logout.Click += new System.EventHandler(this.Logout_Click);
			// 
			// LoginStatusLabel
			// 
			this.LoginStatusLabel.AutoSize = true;
			this.LoginStatusLabel.Location = new System.Drawing.Point(302, 63);
			this.LoginStatusLabel.Name = "LoginStatusLabel";
			this.LoginStatusLabel.Size = new System.Drawing.Size(29, 12);
			this.LoginStatusLabel.TabIndex = 8;
			this.LoginStatusLabel.Text = "login";
			// 
			// Chat
			// 
			this.Chat.Location = new System.Drawing.Point(24, 135);
			this.Chat.Name = "Chat";
			this.Chat.Size = new System.Drawing.Size(257, 292);
			this.Chat.TabIndex = 9;
			this.Chat.Text = "";
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(440, 135);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(227, 292);
			this.listBox1.TabIndex = 10;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(440, 95);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 22);
			this.textBox1.TabIndex = 11;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(588, 95);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 20);
			this.comboBox1.TabIndex = 12;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.Chat);
			this.Controls.Add(this.LoginStatusLabel);
			this.Controls.Add(this.Logout);
			this.Controls.Add(this.ConnectLabel);
			this.Controls.Add(this.SendText);
			this.Controls.Add(this.SendMessage);
			this.Controls.Add(this.Login);
			this.Controls.Add(this.Password);
			this.Controls.Add(this.Account);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox Account;
		private System.Windows.Forms.TextBox Password;
		private System.Windows.Forms.Button Login;
		private System.Windows.Forms.Button SendMessage;
		private System.Windows.Forms.TextBox SendText;
		private System.Windows.Forms.Label ConnectLabel;
		private System.Windows.Forms.Button Logout;
		private System.Windows.Forms.Label LoginStatusLabel;
		private System.Windows.Forms.RichTextBox Chat;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ComboBox comboBox1;
	}
}

