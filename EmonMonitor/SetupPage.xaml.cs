using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EmonMonitor.Resources;
using EmonMonitor.ViewModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;



namespace EmonMonitor
{

    public partial class SetupPage : PhoneApplicationPage
    {

        private FeedsSubscribedPreviewViewModel feedsSubscribedPreviewVM;
        private StatusBarViewModel statusBarVM;

        // Constructor
        public SetupPage()
        {
            InitializeComponent();

            feedsSubscribedPreviewVM = new FeedsSubscribedPreviewViewModel();
            FeedsSubscribedViewOnPage.DataContext = feedsSubscribedPreviewVM.FeedSubscribedPreviewCollection;
            statusBarVM = new StatusBarViewModel();
            StatusBarViewOnPage.DataContext = statusBarVM;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ConfigApiViewOnPage.DataContext = from o in App.configVM.ConfigurationCollection where o.Type == Model.ConfigType.API select o;
            ConfigFeedViewOnPage.DataContext = from o in App.configVM.ConfigurationCollection where o.Type == Model.ConfigType.FEED select o;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (this.State.ContainsKey("ApiConfigViewModel"))
            {
                this.State["ApiConfigViewModel"] = App.configVM;
            }
            else
            {
                this.State.Add("ApiConfigViewModel", App.configVM);
            }
            App.configVM.SaveConfigurations();
        }

        private async void ButtonTryConnect_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            statusBarVM.SetStatusProgressBar("Connecting...");
            bool success = await App.configVM.RefreshFeedsAsync();
            if (success)
            {
                ConfigFeedViewOnPage.DataContext = from o in App.configVM.ConfigurationCollection where o.Type == Model.ConfigType.FEED select o;
                feedsSubscribedPreviewVM.LoadSubscribedPreview();
                statusBarVM.SetStatusMessageNotificationAsync("Successfully synchronized. Choose subscriptions by sweeping to feeds.");
            }
            else
            {
                statusBarVM.SetStatusMessageNotificationAsync("Some error occured, check url or api key.");
            }
        }

        private async void ButtonRefreshPreview_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            statusBarVM.SetStatusProgressBar("Refreshing...");
            bool success = await App.configVM.RefreshFeedsAsync();
            if (success)
            {
                ConfigFeedViewOnPage.DataContext = from o in App.configVM.ConfigurationCollection where o.Type == Model.ConfigType.FEED select o;
                feedsSubscribedPreviewVM.LoadSubscribedPreview();
                statusBarVM.ClearStatusProgressBar();
            }
            else
            {
                statusBarVM.SetStatusMessageNotificationAsync("Some error occured, check url ou api key.");
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as Pivot).SelectedIndex)
            {
                case 2:
                    feedsSubscribedPreviewVM.LoadSubscribedPreview();
                    break;
                default:
                    break;
            }
        }



        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }

}