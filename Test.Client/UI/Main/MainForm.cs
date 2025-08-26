using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Protocol;
using Test.Client.Chat.Application;
using Test.Client.Chat.Domain;
using Test.Client.Client;
using Test.Client.Extension;
using Test.Client.Login.Application;
using Test.Client.Login.Domain;
using Test.Client.View;
using Test.Common;

namespace Test.Client
{
	public partial class MainForm : BaseForm
	{
		private readonly LoginInfo loginInfo;
		private readonly MainInfo mainInfo;

		public MainForm(LoginInfo loginInfo, MainInfo mainInfo) : base()
		{
			this.loginInfo = loginInfo;
			this.mainInfo = mainInfo;

			InitializeComponent();			

			SetDataBinding();
		}

		private void SetDataBinding()
		{
			BindingSource mainInfoBindingSource = new BindingSource();
			mainInfoBindingSource.DataSource = mainInfo;
			ConnectLabel.DataBindings.Add(nameof(ConnectLabel.Text), mainInfoBindingSource, nameof(mainInfo.IsConnectedText));
			ConnectLabel.DataBindings.Add(nameof(ConnectLabel.ForeColor), mainInfoBindingSource, nameof(mainInfo.IsConnectedForeColor));
			LoginStatusLabel.DataBindings.Add(nameof(LoginStatusLabel.Text), mainInfoBindingSource, nameof(mainInfo.IsOnlineText));
			LoginStatusLabel.DataBindings.Add(nameof(LoginStatusLabel.ForeColor), mainInfoBindingSource, nameof(mainInfo.IsOnlineForeColor));

			SendText.DataBindings.Add(nameof(SendText.Text), mainInfoBindingSource, nameof(mainInfo.SendMessage));
			Chat.DataBindings.Add(nameof(Chat.Text), mainInfoBindingSource, nameof(mainInfo.ChatCommit));
		}

		private void Account_TextChanged(object sender, EventArgs e)
		{
			var textBox = sender as TextBox;
			loginInfo.Account = textBox.Text;
		}

		private void Password_TextChanged(object sender, EventArgs e)
		{
			var textBox = sender as TextBox;
			loginInfo.Password = textBox.Text;
		}

		private void Login_Click(object sender, EventArgs e)
		{
			Invoke<ILogin>((mainForm) =>
			{
				mainForm.OnClickLogin();				
			});
		}

		private void Logout_Click(object sender, EventArgs e)
		{
			Invoke<ILogin>((mainForm) =>
			{
				mainForm.OnClickLogout();
			});
		}

		private void SendMessage_Click(object sender, EventArgs e)
		{
			Invoke<IChatSend>((chatSend) =>
			{
				chatSend.OnClickSendMessage();
			});
		}

		private void SendText_TextChanged(object sender, EventArgs e)
		{
			var textBox = sender as TextBox;
			mainInfo.SendMessage = textBox.Text;
		}
	}
}
