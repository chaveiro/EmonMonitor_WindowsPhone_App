using System;
using System.ComponentModel;

namespace EmonMonitor.Model
{
    public enum ConfigType { API, FEED, FEED_TO_DELETE };

    public class Configuration : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public ConfigType Type { get; set; }

        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        private DateTime _lastTimeStamp;
        public DateTime LastTimeStamp
        {
            get
            {
                return _lastTimeStamp;
            }
            set
            {
                _lastTimeStamp = value;
                RaisePropertyChanged("LastTimeStamp");
            }
        }

        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }

        private string _inputScope;
        public string InputScope
        {
            get
            {
                return _inputScope;
            }
            set
            {
                _inputScope = value;
                RaisePropertyChanged("InputScope");
            }
        }

        private bool _subscribed = true;
        public bool Subscribed
        {
            get
            {
                return _subscribed;
            }
            set
            {
                _subscribed = value;
                RaisePropertyChanged("Subscribed");
            }
        }

        private bool _showOnDash;
        public bool ShowOnDash
        {
            get
            {
                return _showOnDash;
            }
            set
            {
                _showOnDash = value;
                if (this.Subscribed == false)
                    _showOnDash = false;
                RaisePropertyChanged("ShowOnDash");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        // Create a copy to save.
        // If your object is databound, this copy is not databound.
        public Configuration GetCopy()
        {
            Configuration copy = (Configuration)this.MemberwiseClone();
            return copy;
        }
    }
}
