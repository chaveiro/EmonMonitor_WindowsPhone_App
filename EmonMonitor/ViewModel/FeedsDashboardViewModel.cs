using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EmonMonitor.Resources;
using EmonMonitor.Model;
using EmonMonitor.DataAccess;
using EmonMonitor.DataAccess.Contracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmonMonitor.Utils;

namespace EmonMonitor.ViewModel
{
    public class FeedsDashboardViewModel : INotifyPropertyChanged
    {
        public FeedsDashboardViewModel()
        {
        }

        ObservableCollection<FeedSingle> _feedDashboardCollection = new ObservableCollection<FeedSingle>();
        public ObservableCollection<FeedSingle> FeedDashboardCollection
        {
            get { 
                return _feedDashboardCollection; 
            }
            private set
            {
                if (value != _feedDashboardCollection)
                {
                    _feedDashboardCollection = value;
                    NotifyPropertyChanged("FeedDashboardCollection");
                }
            }
        }


        public bool LoadDashboard()
        {
            string api_url = App.configVM.ConfigurationCollection.FirstOrDefault(c => c.Name == "Api Url").Value;
            string api_key = App.configVM.ConfigurationCollection.FirstOrDefault(c => c.Name == "Api Key").Value;

            this.FeedDashboardCollection.Clear();
            foreach (var feedSubscribed in App.configVM.ConfigurationCollection.Where( o => o.Type == ConfigType.FEED && o.Subscribed && o.ShowOnDash))
            {
                //ApiJsonClient client = new ApiJsonClient();
                //Decimal value = await client.GetFeedValueAsync(api_url, api_key, feed.Id);
                this.FeedDashboardCollection.Add(
                    new FeedSingle()
                    {
                                    Name = feedSubscribed.Name,
                                    TimeStamp = feedSubscribed.LastTimeStamp,
                                    Value = Util.ConvertToDecimal(feedSubscribed.Value)
                    }
                );
            }
            return true;
        }


        //public bool LoadSubscribedPreview()
        //{
        //    string api_url = App.configVM.ConfigurationCollection.FirstOrDefault(c => c.Name == "Api Url").Value;
        //    string api_key = App.configVM.ConfigurationCollection.FirstOrDefault(c => c.Name == "Api Key").Value;

        //    this.FeedSubscribedPreviewCollection.Clear();
        //    foreach (var feedSunscribed in App.configVM.ConfigurationCollection.Where(o => o.Type == ConfigType.FEED && o.Subscribed))
        //    {
        //        //ApiJsonClient client = new ApiJsonClient();
        //        //Decimal value = await client.GetFeedValueAsync(api_url, api_key, feed.Id);
        //        Feed feedOne = new Feed()
        //        {
        //            TimeStamp = feedSunscribed.LastTimeStamp,
        //            Value = Convert.ToDecimal(feedSunscribed.Value)
        //        };

        //        this.FeedSubscribedPreviewCollection.Add(
        //            new Feeds()
        //            {
        //                Id = feedSunscribed.Id,
        //                Name = feedSunscribed.Name,
        //                FeedList = new List<Feed>() { feedOne }
        //            }
        //        );
        //    }
        //    return true;
        //}


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}