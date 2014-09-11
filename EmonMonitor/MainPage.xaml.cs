using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EmonMonitor.ViewModel;
using System.Windows.Threading;
using EmonMonitor.Model;
using System.Threading.Tasks;

namespace EmonMonitor
{
    public partial class MainPage : PhoneApplicationPage
    {
        private FeedsDashboardViewModel feedsDashboardVM;
        private StatusBarViewModel statusBarVM;
        DispatcherTimer refreshDashboardTimer = new DispatcherTimer();

        public MainPage()
        {
            InitializeComponent();

            feedsDashboardVM = new FeedsDashboardViewModel();
            FeedsDashboardViewOnPage.DataContext = feedsDashboardVM.FeedDashboardCollection;
            statusBarVM = new StatusBarViewModel();
            StatusBarViewOnPage.DataContext = statusBarVM;
            
            refreshDashboardTimer.Interval = TimeSpan.FromSeconds(GetRefreshRate());
            refreshDashboardTimer.Tick += OnRefreshDashboardTimerTick;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            refreshDashboardTimer.Interval = TimeSpan.FromSeconds(GetRefreshRate());
            await RefreshDashboard(false);
            refreshDashboardTimer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            refreshDashboardTimer.Stop();
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as Panorama).SelectedIndex)
            {
                case 0:
                    refreshDashboardTimer.Start();
                    this.ApplicationBar.IsVisible = true;
                    break;
                default:
                    refreshDashboardTimer.Stop();
                    this.ApplicationBar.IsVisible = false;
                    break;
            }
        }

        private async void OnRefreshDashboardTimerTick(object sender, EventArgs e)
        {
            refreshDashboardTimer.Stop();
            await RefreshDashboard(true);
            refreshDashboardTimer.Start();
        }

        private int GetRefreshRate()
        {
            return Convert.ToInt32(App.configVM.ConfigurationCollection.FirstOrDefault(o => o.Type == ConfigType.API && o.Name == "Refresh (s)").Value);
        }

        private async Task RefreshDashboard(bool SyncWithServer)
        {
            statusBarVM.SetStatusProgressBar(null,true);
            bool success = true;
            if (SyncWithServer)
                success = await App.configVM.RefreshFeedsAsync();
            if (success)
            {
                feedsDashboardVM.LoadDashboard();
                statusBarVM.ClearStatusProgressBar();
            }
            else
            {
                statusBarVM.SetStatusMessageNotificationAsync("Some error occured, check internet connection.");
            }
        }

        private void AppBarSetupButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SetupPage.xaml", UriKind.Relative));
        }
    }
}