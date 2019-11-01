using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.System.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MediaRemoteMe
{
    public partial class MainPage : Page
    {
        //Application Variables
        public static IDictionary<string, object> vApplicationSettings = ApplicationData.Current.LocalSettings.Values;
        public static DisplayRequest vDisplayRequest = new DisplayRequest();
        public static ApplicationViewTitleBar vTitleBar;
        public static StatusBar vStatusBar;
        public static bool vSendingSocketMsg = false;

        //Handle application page loading
        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            try
            {
                Loaded += async delegate
                {
                    //Set application startup arguments
                    if (args.Parameter != null) { App.vApplicationLaunchArgs = args.Parameter.ToString(); }

                    //Handle application page startup
                    await ApplicationStartup();
                };
            }
            catch { }
        }

        //Handle application page startup
        async Task ApplicationStartup()
        {
            try
            {
                //Set Application Styles
                this.RequestedTheme = ElementTheme.Light;
                if (AVFunctions.DevMobile())
                {
                    //Set Phone StatusBar
                    vStatusBar = StatusBar.GetForCurrentView();
                    vStatusBar.ForegroundColor = Colors.White;
                    vStatusBar.BackgroundColor = Color.FromArgb(255, 29, 29, 29);
                    vStatusBar.BackgroundOpacity = 1;
                    await vStatusBar.ShowAsync();

                    //Set Application Display Orientation
                    DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait | DisplayOrientations.PortraitFlipped;

                    //Set Application Menu Orientation - Vertical
                    grid_Main.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid_Main.RowDefinitions.Add(new RowDefinition { Height = new GridLength(85, GridUnitType.Pixel) });
                    page_Menu.SetValue(Grid.RowProperty, 1);
                    page_Main.SetValue(Grid.RowProperty, 0);
                    menu_ScrollViewer.Style = (Style)App.Current.Resources["ScrollViewerHorizontal"];
                    menu_StackPanel.Orientation = Orientation.Horizontal;
                }
                else
                {
                    //Set Desktop TitleBar
                    vTitleBar = ApplicationView.GetForCurrentView().TitleBar;
                    vTitleBar.ButtonBackgroundColor = Color.FromArgb(255, 29, 29, 29);
                    vTitleBar.BackgroundColor = Color.FromArgb(255, 29, 29, 29);
                    vTitleBar.ButtonForegroundColor = Colors.White;
                    vTitleBar.ForegroundColor = Colors.White;

                    //Set Application Minimum Window Size
                    ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(360, 340));

                    //Set Application Display Orientation
                    DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped;

                    //Set Application Menu Orientation - Horizontal
                    grid_Main.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(85, GridUnitType.Pixel) });
                    grid_Main.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    page_Menu.SetValue(Grid.ColumnProperty, 0);
                    page_Main.SetValue(Grid.ColumnProperty, 1);
                    menu_ScrollViewer.Style = (Style)App.Current.Resources["ScrollViewerVertical"];
                    menu_StackPanel.Orientation = Orientation.Vertical;
                }

                //Load Application Settings
                await SettingsCheck();
                await SettingsLoad();
                await SettingsSave();

                //Set the remote buttons location
                if (AVFunctions.DevMobile())
                {
                    stackpanel_Remote.HorizontalAlignment = HorizontalAlignment.Center;
                    stackpanel_Keyboard.HorizontalAlignment = HorizontalAlignment.Center;
                }

                switch ((int)vApplicationSettings["RemoteLocation"])
                {
                    case 0: { stackpanel_Remote.VerticalAlignment = VerticalAlignment.Top; stackpanel_Keyboard.VerticalAlignment = VerticalAlignment.Top; break; }
                    case 1: { stackpanel_Remote.VerticalAlignment = VerticalAlignment.Center; stackpanel_Keyboard.VerticalAlignment = VerticalAlignment.Center; break; }
                    case 2: { stackpanel_Remote.VerticalAlignment = VerticalAlignment.Bottom; stackpanel_Keyboard.VerticalAlignment = VerticalAlignment.Bottom; break; }
                }

                //Monitor user touch swipe
                tab_Keyboard.ManipulationMode = ManipulationModes.TranslateX;
                tab_Keyboard.ManipulationStarted += tab_Keyboard_ManipulationStarted;
                tab_Keyboard.ManipulationCompleted += tab_Keyboard_ManipulationCompleted;

                //Register application events, timers and interactions
                ApplicationEventsRegister();

                //Enable user interface
                grid_Main.Opacity = 1;
                grid_Main.IsHitTestVisible = true;

                //Application Startup Navigation
                await ApplicationNavigation();
            }
            catch { }
        }

        //Handle application navigation
        async Task ApplicationNavigation()
        {
            try
            {
                //Check if server ip adres is set
                if (await ValidateRemoteIpAdres(vApplicationSettings["ServerIp"].ToString())) { menuButtonRemote_Tapped(null, null); } else { return; }

                //Tile Startup
                if (App.vLaunchTileActivatedCommand == "MediaRemoteMePlayPause") { await SocketSender("MePlayPause"); }
                else if (App.vLaunchTileActivatedCommand == "MediaRemoteMePrevious") { await SocketSender("MePrevSong"); }
                else if (App.vLaunchTileActivatedCommand == "MediaRemoteMeNext") { await SocketSender("MeNextSong"); }
                else if (App.vLaunchTileActivatedCommand == "MediaRemoteMeVolUp") { await SocketSender("MeVolUp" + vApplicationSettings["VolSkip"].ToString()); }
                else if (App.vLaunchTileActivatedCommand == "MediaRemoteMeVolDown") { await SocketSender("MeVolDown" + vApplicationSettings["VolSkip"].ToString()); }
                else if (App.vLaunchTileActivatedCommand == "MediaRemoteMeVolMute") { await SocketSender("MeVolMute"); }
                else if (App.vLaunchTileActivatedCommand == "MediaRemoteMeArrowLeft") { await SocketSender("MeArrowLeft"); }
                else if (App.vLaunchTileActivatedCommand == "MediaRemoteMeArrowRight") { await SocketSender("MeArrowRight"); }
                else if (App.vLaunchTileActivatedCommand == "MediaRemoteMeFullscreen") { await SocketSender("MeFullscreen"); }
                //Voice Startup
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMePlay") { await SocketSender("MePlayPause"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeResume") { await SocketSender("MePlayPause"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMePause") { await SocketSender("MePlayPause"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeStop") { await SocketSender("MePlayPause"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMePrevious") { await SocketSender("MePrevSong"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeBack") { await SocketSender("MePrevSong"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeLeft") { await SocketSender("MePrevSong"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeNext") { await SocketSender("MeNextSong"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeForward") { await SocketSender("MeNextSong"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeRight") { await SocketSender("MeNextSong"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeVolUp") { await SocketSender("MeVolUp" + vApplicationSettings["VolSkip"].ToString()); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeVolUpShort") { await SocketSender("MeVolUp" + vApplicationSettings["VolSkip"].ToString()); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeVolDown") { await SocketSender("MeVolDown" + vApplicationSettings["VolSkip"].ToString()); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeVolDownShort") { await SocketSender("MeVolDown" + vApplicationSettings["VolSkip"].ToString()); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeVolMute") { await SocketSender("MeVolMute"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeVolMuteShort") { await SocketSender("MeVolMute"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeVolUnmute") { await SocketSender("MeVolMute"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeVolUnmuteShort") { await SocketSender("MeVolMute"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeArrowLeft") { await SocketSender("MeArrowLeft"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeArrowRight") { await SocketSender("MeArrowRight"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeFullscreen") { await SocketSender("MeFullscreen"); }
                else if (App.vLaunchVoiceActivatedCommand == "MediaRemoteMeWindow") { await SocketSender("MeFullscreen"); }
                //NFC Startup
                else if (App.vApplicationLaunchArgs == "NfcPlayPause") { await SocketSender("MePlayPause"); }
                else if (App.vApplicationLaunchArgs == "NfcPrevious") { await SocketSender("MePrevSong"); }
                else if (App.vApplicationLaunchArgs == "NfcNext") { await SocketSender("MeNextSong"); }
                else if (App.vApplicationLaunchArgs == "NfcVolUp") { await SocketSender("MeVolUp" + vApplicationSettings["VolSkip"].ToString()); }
                else if (App.vApplicationLaunchArgs == "NfcVolDown") { await SocketSender("MeVolDown" + vApplicationSettings["VolSkip"].ToString()); }
                else if (App.vApplicationLaunchArgs == "NfcVolMute") { await SocketSender("MeVolMute"); }
                else if (App.vApplicationLaunchArgs == "NfcArrowLeft") { await SocketSender("MeArrowLeft"); }
                else if (App.vApplicationLaunchArgs == "NfcArrowRight") { await SocketSender("MeArrowRight"); }
                else if (App.vApplicationLaunchArgs == "NfcFullscreen") { await SocketSender("MeFullscreen"); }

                //Check if the application needs to be closed
                if ((!String.IsNullOrEmpty(App.vLaunchTileActivatedCommand) || !String.IsNullOrEmpty(App.vLaunchVoiceActivatedCommand) || !String.IsNullOrEmpty(App.vApplicationLaunchArgs)) && App.vLaunchTileActivatedCommand != "App" && (bool)vApplicationSettings["CmdCloseApp"]) { Application.Current.Exit(); }

                //Reset the application startup variables
                App.vLaunchTileActivatedCommand = "";
                App.vLaunchVoiceActivatedCommand = "";
                App.vLaunchVoiceActivatedSpoken = "";
                App.vApplicationLaunchArgs = "";
                return;
            }
            catch { }
        }

        //Application Media Buttons
        async void MediaBox_Tap(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                FrameworkElement FrameworkElement = (FrameworkElement)sender;
                string TapTag = FrameworkElement.Tag.ToString();

                //Add volume information to the tag
                if (TapTag == "VolUp" || TapTag == "VolDown") { TapTag += vApplicationSettings["VolSkip"].ToString(); }

                Debug.WriteLine("Clicked on button: " + TapTag);
                await SocketSender("Me" + TapTag);
            }
            catch { }
        }

        //Keyboard Switch Functions
        void KeyboardSwitch()
        {
            if (stackpanel_KeyboardNumeric.Visibility == Visibility.Visible)
            {
                stackpanel_KeyboardNumeric.Visibility = Visibility.Collapsed;
                stackpanel_KeyboardArrows.Visibility = Visibility.Visible;
            }
            else
            {
                stackpanel_KeyboardNumeric.Visibility = Visibility.Visible;
                stackpanel_KeyboardArrows.Visibility = Visibility.Collapsed;
            }
        }
        private void btn_KeyboardSwitchNumeric_Click(object sender, RoutedEventArgs e)
        {
            stackpanel_KeyboardNumeric.Visibility = Visibility.Visible;
            stackpanel_KeyboardArrows.Visibility = Visibility.Collapsed;
        }

        private void btn_KeyboardSwitchArrows_Click(object sender, RoutedEventArgs e)
        {
            stackpanel_KeyboardNumeric.Visibility = Visibility.Collapsed;
            stackpanel_KeyboardArrows.Visibility = Visibility.Visible;
        }

        //Menu Button Functions
        void menuButtonRemote_Tapped(object sender, TappedRoutedEventArgs e)
        {
            tab_Remote.Visibility = Visibility.Visible;
            menuButtonRemote.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));

            tab_Keyboard.Visibility = Visibility.Collapsed;
            menuButtonKeyboard.Background = new SolidColorBrush(Colors.Transparent);

            tab_Tiles.Visibility = Visibility.Collapsed;
            menuButtonTiles.Background = new SolidColorBrush(Colors.Transparent);

            tab_Nfc.Visibility = Visibility.Collapsed;
            menuButtonNfc.Background = new SolidColorBrush(Colors.Transparent);

            tab_Settings.Visibility = Visibility.Collapsed;
            menuButtonSettings.Background = new SolidColorBrush(Colors.Transparent);

            tab_Help.Visibility = Visibility.Collapsed;
            menuButtonHelp.Background = new SolidColorBrush(Colors.Transparent);
        }
        void menuButtonKeyboard_Tapped(object sender, TappedRoutedEventArgs e)
        {
            tab_Remote.Visibility = Visibility.Collapsed;
            menuButtonRemote.Background = new SolidColorBrush(Colors.Transparent);

            tab_Keyboard.Visibility = Visibility.Visible;
            menuButtonKeyboard.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));

            tab_Tiles.Visibility = Visibility.Collapsed;
            menuButtonTiles.Background = new SolidColorBrush(Colors.Transparent);

            tab_Nfc.Visibility = Visibility.Collapsed;
            menuButtonNfc.Background = new SolidColorBrush(Colors.Transparent);

            tab_Settings.Visibility = Visibility.Collapsed;
            menuButtonSettings.Background = new SolidColorBrush(Colors.Transparent);

            tab_Help.Visibility = Visibility.Collapsed;
            menuButtonHelp.Background = new SolidColorBrush(Colors.Transparent);
        }
        void menuButtonTiles_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TilesLoad();

            tab_Remote.Visibility = Visibility.Collapsed;
            menuButtonRemote.Background = new SolidColorBrush(Colors.Transparent);

            tab_Keyboard.Visibility = Visibility.Collapsed;
            menuButtonKeyboard.Background = new SolidColorBrush(Colors.Transparent);

            tab_Tiles.Visibility = Visibility.Visible;
            menuButtonTiles.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));

            tab_Nfc.Visibility = Visibility.Collapsed;
            menuButtonNfc.Background = new SolidColorBrush(Colors.Transparent);

            tab_Settings.Visibility = Visibility.Collapsed;
            menuButtonSettings.Background = new SolidColorBrush(Colors.Transparent);

            tab_Help.Visibility = Visibility.Collapsed;
            menuButtonHelp.Background = new SolidColorBrush(Colors.Transparent);
        }
        void menuButtonNfc_Tapped(object sender, TappedRoutedEventArgs e)
        {
            tab_Remote.Visibility = Visibility.Collapsed;
            menuButtonRemote.Background = new SolidColorBrush(Colors.Transparent);

            tab_Keyboard.Visibility = Visibility.Collapsed;
            menuButtonKeyboard.Background = new SolidColorBrush(Colors.Transparent);

            tab_Tiles.Visibility = Visibility.Collapsed;
            menuButtonTiles.Background = new SolidColorBrush(Colors.Transparent);

            tab_Nfc.Visibility = Visibility.Visible;
            menuButtonNfc.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));

            tab_Settings.Visibility = Visibility.Collapsed;
            menuButtonSettings.Background = new SolidColorBrush(Colors.Transparent);

            tab_Help.Visibility = Visibility.Collapsed;
            menuButtonHelp.Background = new SolidColorBrush(Colors.Transparent);
        }
        void menuButtonSettings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                //Check if there is an internet connection
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    //Check if Wi-Fi or Ethernet is connected
                    uint IanaInterfaceType = NetworkInformation.GetInternetConnectionProfile().NetworkAdapter.IanaInterfaceType;
                    if (IanaInterfaceType != 71 && IanaInterfaceType != 6) { txt_ErrorWifi.Visibility = Visibility.Visible; }
                    else { txt_ErrorWifi.Visibility = Visibility.Collapsed; }
                }
                else
                { txt_ErrorWifi.Visibility = Visibility.Visible; }
            }
            catch { }

            //Check if a Windows PC is used as client
            if (!AVFunctions.DevMobile()) { txt_ErrorPC.Visibility = Visibility.Visible; }
            else { txt_ErrorPC.Visibility = Visibility.Collapsed; }

            tab_Remote.Visibility = Visibility.Collapsed;
            menuButtonRemote.Background = new SolidColorBrush(Colors.Transparent);

            tab_Keyboard.Visibility = Visibility.Collapsed;
            menuButtonKeyboard.Background = new SolidColorBrush(Colors.Transparent);

            tab_Tiles.Visibility = Visibility.Collapsed;
            menuButtonTiles.Background = new SolidColorBrush(Colors.Transparent);

            tab_Nfc.Visibility = Visibility.Collapsed;
            menuButtonNfc.Background = new SolidColorBrush(Colors.Transparent);

            tab_Settings.Visibility = Visibility.Visible;
            menuButtonSettings.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));

            tab_Help.Visibility = Visibility.Collapsed;
            menuButtonHelp.Background = new SolidColorBrush(Colors.Transparent);
        }
        void menuButtonHelp_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HelpLoad();

            tab_Remote.Visibility = Visibility.Collapsed;
            menuButtonRemote.Background = new SolidColorBrush(Colors.Transparent);

            tab_Keyboard.Visibility = Visibility.Collapsed;
            menuButtonKeyboard.Background = new SolidColorBrush(Colors.Transparent);

            tab_Tiles.Visibility = Visibility.Collapsed;
            menuButtonTiles.Background = new SolidColorBrush(Colors.Transparent);

            tab_Nfc.Visibility = Visibility.Collapsed;
            menuButtonNfc.Background = new SolidColorBrush(Colors.Transparent);

            tab_Settings.Visibility = Visibility.Collapsed;
            menuButtonSettings.Background = new SolidColorBrush(Colors.Transparent);

            tab_Help.Visibility = Visibility.Visible;
            menuButtonHelp.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }

        //Keyboard page swipe action
        private Point TouchStartingPoint;
        private double TouchSwipingLimitHeight = 80;
        private double TouchSwipingRange = 80;

        private void tab_Keyboard_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            try
            {
                TouchStartingPoint = e.Position;
            }
            catch { }
        }
        private void tab_Keyboard_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            try
            {
                double CurrentPointHorizontal = e.Position.X;
                double CurrentVerticalDifference = Math.Abs(e.Position.Y - TouchStartingPoint.Y);

                //From the left to right
                if (CurrentVerticalDifference < TouchSwipingLimitHeight && CurrentPointHorizontal - TouchStartingPoint.X > TouchSwipingRange)
                {
                    KeyboardSwitch();
                }
            }
            catch { }
        }
    }
}