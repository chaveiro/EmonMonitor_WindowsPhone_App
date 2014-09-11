using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EmonMonitor.ViewModel
{
    public class StatusBarViewModel:INotifyPropertyChanged 
    {
        public async void SetStatusMessageNotificationAsync(string msg, bool isTransparent = false)
        {
            this.IsTransparent = isTransparent;
            this.StatusMsg = msg;
            this.ShowStatusPanel = true;
            this.ShowStatusBar = false;
            this.ShowStatusMsg = true;
            await Task.Run(() => Thread.Sleep(4000));
            this.ShowStatusPanel = false;
            this.ShowStatusBar = false;
            this.ShowStatusMsg = false;
        }

        public void SetStatusProgressBar(string msg = null, bool isTransparent = false)
        {
            this.IsTransparent = isTransparent;
            if (msg == null) {
                this.StatusMsg = string.Empty;
                this.ShowStatusPanel = true;
                this.ShowStatusBar = true;
                this.ShowStatusMsg = false;
            }
            else
            {
                this.StatusMsg = msg;
                this.ShowStatusPanel = true;
                this.ShowStatusBar = true;
                this.ShowStatusMsg = true;
            }
        }

        public void ClearStatusProgressBar()
        {
            this.StatusMsg = string.Empty;
            this.ShowStatusPanel = false;
            this.ShowStatusBar = false;
            this.ShowStatusMsg = false;
        }


        private bool _isTransparent = false;
        public bool IsTransparent
        {
            get { return _isTransparent; }
            set
            {
                if (_isTransparent != value)
                {
                    _isTransparent = value;
                    NotifyPropertyChanged("IsTransparent");
                }
            }
        }

        private string _statusMsg;
        public string StatusMsg
        {
            get { return _statusMsg; }
            set
            {
                if (_statusMsg != value)
                {
                    _statusMsg = value;
                    NotifyPropertyChanged("StatusMsg");
                }
            }
        }

        private bool _showStatusMsg = false;
        public bool ShowStatusMsg
        {
            get { return _showStatusMsg; }
            set
            {
                if (_showStatusMsg != value)
                {
                    _showStatusMsg = value;
                    NotifyPropertyChanged("ShowStatusMsg");
                }
            }
        }

        private bool _showStatusBar = false;
        public bool ShowStatusBar
        {
            get { return _showStatusBar; }
            set
            {
                if (_showStatusBar != value)
                {
                    _showStatusBar = value;
                    NotifyPropertyChanged("ShowStatusBar");
                }
            }
        }

        private bool _showStatusPanel = false;
        public bool ShowStatusPanel
        {
            get { return _showStatusPanel; }
            set
            {
                if (_showStatusPanel != value)
                {
                    _showStatusPanel = value;
                    NotifyPropertyChanged("ShowStatusPanel");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
