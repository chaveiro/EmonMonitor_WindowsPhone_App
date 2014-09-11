using System;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Linq;
using EmonMonitor.Model;
using EmonMonitor.DataAccess;
using EmonMonitor.DataAccess.Contracts;
using System.Threading.Tasks;
using EmonMonitor.Utils;

namespace EmonMonitor.ViewModel
{

    public class ConfigurationViewModel 
    {
        public ObservableCollection<Configuration> ConfigurationCollection { get; set; }


        public void GetConfigurations()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Count > 0)
            {
                GetSavedConfigurations();
            }
            else
            {
                GetDefaultConfigurations();
            }
        }

        public void SaveConfigurations()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            foreach (Configuration a in ConfigurationCollection)
            {
                string keyName = a.Id + a.Name;
                if (settings.Contains(keyName) && (a.Type == ConfigType.FEED_TO_DELETE))
                {
                    settings.Remove(keyName);
                }
                else if (settings.Contains(keyName))
                {
                    settings[keyName] = a;
                }
                else
                {
                    settings.Add(keyName, a.GetCopy());
                }
            }

            settings.Save();
            //MessageBox.Show("Finished saving config");
        }


        private void GetDefaultConfigurations()
        {
            ObservableCollection<Configuration> a = new ObservableCollection<Configuration>();

            // Api defaults
            //a.Add(new Configuration() { Name = "Api Url", Type = ConfigType.API, Value = @"http://emon.gtronica.com" , InputScope = "Url"});
            a.Add(new Configuration() { Name = "Api Url", Type = ConfigType.API, Value = @"http://emoncms.org", InputScope = "Url" });
            a.Add(new Configuration() { Name = "Api Key", Type = ConfigType.API, Value = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", InputScope = "AlphanumericFullWidth" });
            a.Add(new Configuration() { Name = "Refresh (s)", Type = ConfigType.API, Value = "4" , InputScope="Number"});

            // Feeds default
            a.Add(new Configuration() { Name = "Run Setup !", Type = ConfigType.FEED, Id = 1, Subscribed = true, ShowOnDash = true});
            a.Add(new Configuration() { Name = "Run Setup !", Type = ConfigType.FEED, Id = 2, Subscribed = true, ShowOnDash = true });
            a.Add(new Configuration() { Name = "Run Setup !", Type = ConfigType.FEED, Id = 3, Subscribed = true, ShowOnDash = false });

            ConfigurationCollection = a;
            //MessageBox.Show("Got config from default");
        }

        private void GetSavedConfigurations()
        {
            ObservableCollection<Configuration> a = new ObservableCollection<Configuration>();

            foreach (Object o in IsolatedStorageSettings.ApplicationSettings.Values)
            {
                a.Add((Configuration)o);
            }

            ConfigurationCollection = a;
            //MessageBox.Show("Got config from storage");
        }

        public async Task<bool> RefreshFeedsAsync()
        {
            try
            {
                string api_url = App.configVM.ConfigurationCollection.FirstOrDefault(c => c.Name == "Api Url").Value;
                string api_key = App.configVM.ConfigurationCollection.FirstOrDefault(c => c.Name == "Api Key").Value;

                ApiJsonClient client = new ApiJsonClient();
                List<FeedItem> resp = await client.GetFeedsListAsync(api_url, api_key);

                var confFeeds = from o in App.configVM.ConfigurationCollection where o.Type == Model.ConfigType.FEED select o;
                foreach (var confFeed in confFeeds)
                {
                    confFeed.Type = Model.ConfigType.FEED_TO_DELETE;
                }

                foreach (var feed in resp)
                {
                    Model.Configuration existingFeedConf = App.configVM.ConfigurationCollection.FirstOrDefault((o) => o.Type == Model.ConfigType.FEED_TO_DELETE && o.Id == feed.Id);
                    if (existingFeedConf != null)
                    {
                        existingFeedConf.Name = feed.Name;
                        existingFeedConf.Type = Model.ConfigType.FEED;
                        existingFeedConf.LastTimeStamp = Util.FromApiTime(feed.Time);
                        existingFeedConf.Value = feed.Value;
                    }
                    else
                    {
                        App.configVM.ConfigurationCollection.Add(
                            new Model.Configuration
                            {
                                Name = feed.Name,
                                Id = feed.Id,
                                Type = Model.ConfigType.FEED,
                                LastTimeStamp = Util.FromApiTime(feed.Time),
                                Value = feed.Value
                            }
                        );
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

       
    }
}
