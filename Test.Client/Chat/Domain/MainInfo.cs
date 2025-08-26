using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Test.Client.Chat.Domain
{
	public class MainInfo : INotifyPropertyChanged
	{
		private Color isConnectedForeColorValue = default;
		public Color IsConnectedForeColor
		{
			get { return isConnectedForeColorValue; }
			private set
			{
				isConnectedForeColorValue = value;
				NotifyPropertyChanged();
			}
		}
		private string isConnectedTextValue = string.Empty;
		public string IsConnectedText
		{
			get { return isConnectedTextValue; }
			private set
			{
				isConnectedTextValue = value;
				NotifyPropertyChanged();
			}
		}
		private Color isOnlineForeColorValue = default;
		public Color IsOnlineForeColor
		{
			get { return isOnlineForeColorValue; }
			private set
			{
				isOnlineForeColorValue = value;
				NotifyPropertyChanged();
			}
		}
		private string isOnlineTextValue = string.Empty;
		public string IsOnlineText
		{
			get { return isOnlineTextValue; }
			private set
			{
				isOnlineTextValue = value;
				NotifyPropertyChanged();
			}
		}

		private string sendMessage = string.Empty;
		public string SendMessage
		{
			get { return sendMessage; }
			set
			{
				sendMessage = value;
				NotifyPropertyChanged();
			}
		}

		private string chatCommit = string.Empty;
		public string ChatCommit
		{
			get { return chatCommit; }
			set
			{
				chatCommit = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		internal void SetConnectStatus(bool isConnected)
		{
			IsConnectedText = isConnected ? "連線中" : "等待連線";
			IsConnectedForeColor = isConnected ? Color.Green : Color.Red;
		}

		internal void SetOnlineStatus(bool isOnline)
		{
			IsOnlineText = isOnline ? "在線中" : "離線中";
			IsOnlineForeColor = isOnline ? Color.Green : Color.Red;
		}

		internal void AddChatCommit(string name, string text)
		{
			ChatCommit += $"{name} : {text}\n";
		}
	}
}
