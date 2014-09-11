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
    public class FeedsSubscribedPreviewViewModel : INotifyPropertyChanged
    {
        public FeedsSubscribedPreviewViewModel()
        {
        }

        ObservableCollection<FeedSingle> _feedSubscribedPreviewCollection = new ObservableCollection<FeedSingle>();
        public ObservableCollection<FeedSingle> FeedSubscribedPreviewCollection
        {
            get { 
                return _feedSubscribedPreviewCollection; 
            }
            private set
            {
                if (value != _feedSubscribedPreviewCollection)
                {
                    _feedSubscribedPreviewCollection = value;
                    NotifyPropertyChanged("FeedSubscribedPreviewCollection");
                }
            }
        }


        public bool LoadSubscribedPreview()
        {
            string api_url = App.configVM.ConfigurationCollection.FirstOrDefault(c => c.Name == "Api Url").Value;
            string api_key = App.configVM.ConfigurationCollection.FirstOrDefault(c => c.Name == "Api Key").Value;

            this.FeedSubscribedPreviewCollection.Clear();
            foreach (var feedSunscribed in App.configVM.ConfigurationCollection.Where( o => o.Type == ConfigType.FEED && o.Subscribed))
            {
                //ApiJsonClient client = new ApiJsonClient();
                //Decimal value = await client.GetFeedValueAsync(api_url, api_key, feed.Id);
                this.FeedSubscribedPreviewCollection.Add(
                    new FeedSingle()
                    {
                                    Name = feedSunscribed.Name,
                                    TimeStamp = feedSunscribed.LastTimeStamp,
                                    Value = Util.ConvertToDecimal(feedSunscribed.Value)
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