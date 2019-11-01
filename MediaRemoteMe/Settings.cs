using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MediaRemoteMe
{
    partial class MainPage
    {
        //Check - Application Settings
        async Task SettingsCheck()
        {
            try
            {
                //Check - Screen always on
                if (!vApplicationSettings.ContainsKey("ScreenAlwaysOn")) { vApplicationSettings["ScreenAlwaysOn"] = false; }

                //Check - Volume Skip Amount
                if (!vApplicationSettings.ContainsKey("VolSkip")) { vApplicationSettings["VolSkip"] = "2"; }

                //Check - Remote Screen Location
                if (!vApplicationSettings.ContainsKey("RemoteLocation"))
                {
                    if (AVFunctions.DevMobile()) { vApplicationSettings["RemoteLocation"] = 2; }
                    else { vApplicationSettings["RemoteLocation"] = 0; }
                }

                //Check - Close app Tile/Voice
                if (!vApplicationSettings.ContainsKey("CmdCloseApp")) { vApplicationSettings["CmdCloseApp"] = false; }

                //Check - Server ip adres
                if (!vApplicationSettings.ContainsKey("ServerIp")) { vApplicationSettings["ServerIp"] = "192.168.0."; }

                //Check - Server port
                if (!vApplicationSettings.ContainsKey("ServerPort")) { vApplicationSettings["ServerPort"] = "1000"; }

                //Load and check - Voice Commands for Cortana - Not awaiting to avoid slow startup
                try
                {
                    VoiceCommandManager.InstallCommandSetsFromStorageFileAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///VoiceCommands.xml")));
                    Debug.WriteLine("Successfully registered the Cortana voice commands.");
                }
                catch
                {
                    Debug.WriteLine("Failed to register the Cortana voice commands.");
                }
            }
            catch (Exception Ex) { await new MessageDialog("SettingsCheckError: " + Ex.Message, "MediaRemoteMe").ShowAsync(); }
        }

        //Load - Application Settings
        async Task SettingsLoad()
        {
            try
            {
                //Load - Screen always on
                cb_ScreenAlwaysOn.IsChecked = (bool)vApplicationSettings["ScreenAlwaysOn"];
                if ((bool)vApplicationSettings["ScreenAlwaysOn"])
                {
                    //Prevent application lock screen
                    try { vDisplayRequest.RequestActive(); } catch { }
                }
                else
                {
                    //Allow application lock screen
                    try { vDisplayRequest.RequestRelease(); } catch { }
                }

                //Load - Volume Skip Amount
                try { sldr_Volume.Value = (Convert.ToInt32(vApplicationSettings["VolSkip"].ToString()) * 2); } catch { }

                //Load - Remote Screen Location
                lp_SettingRemoteLocation.SelectedIndex = (int)vApplicationSettings["RemoteLocation"];

                //Load - Close app Tile/Voice
                cb_CmdCloseApp.IsChecked = (bool)vApplicationSettings["CmdCloseApp"];

                //Load - Server ip adres
                txtbox_ServerIp.Text = vApplicationSettings["ServerIp"].ToString();
                if (String.IsNullOrWhiteSpace(txtbox_ServerIp.Text)) { txtbox_ServerIp.BorderBrush = new SolidColorBrush(Colors.Red); }
                if (!txtbox_ServerIp.Text.Contains(".") || txtbox_ServerIp.Text.Contains("-") || txtbox_ServerIp.Text.Contains(",") || txtbox_ServerIp.Text.Contains("..") || txtbox_ServerIp.Text.Contains("...") || txtbox_ServerIp.Text.EndsWith(".")) { txtbox_ServerIp.BorderBrush = new SolidColorBrush(Colors.Red); }
                if (!(txtbox_ServerIp.Text.Split('.').Length == 4)) { txtbox_ServerIp.BorderBrush = new SolidColorBrush(Colors.Red); }

                //Load - Server port
                txtbox_ServerPort.Text = vApplicationSettings["ServerPort"].ToString();
                if (String.IsNullOrWhiteSpace(txtbox_ServerPort.Text)) { txtbox_ServerPort.BorderBrush = new SolidColorBrush(Colors.Red); }
                if (Regex.IsMatch(txtbox_ServerPort.Text, "(\\D+)")) { txtbox_ServerPort.BorderBrush = new SolidColorBrush(Colors.Red); }
                if (Convert.ToInt32(txtbox_ServerPort.Text) < 1 || Convert.ToInt32(txtbox_ServerPort.Text) > 65535) { txtbox_ServerPort.BorderBrush = new SolidColorBrush(Colors.Red); }
            }
            catch (Exception Ex) { await new MessageDialog("SettingsLoadError: " + Ex.Message, "MediaRemoteMe").ShowAsync(); }
        }

        //Save Events - Application Settings
        async Task SettingsSave()
        {
            try
            {
                //Save - Screen always on
                cb_ScreenAlwaysOn.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["ScreenAlwaysOn"] = true;
                        //Prevent application lock screen
                        try { vDisplayRequest.RequestActive(); } catch { }
                    }
                    else
                    {
                        vApplicationSettings["ScreenAlwaysOn"] = false;
                        //Allow application lock screen
                        try { vDisplayRequest.RequestRelease(); } catch { }
                    }
                };

                //Save - Volume Skip Amount
                sldr_Volume.ValueChanged += (sender, e) =>
                {
                    Slider Slider = (Slider)sender;
                    vApplicationSettings["VolSkip"] = (Slider.Value / 2);
                };

                //Save - Remote Screen Location
                lp_SettingRemoteLocation.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["RemoteLocation"] != ComboBox.SelectedIndex)
                    {
                        vApplicationSettings["RemoteLocation"] = ComboBox.SelectedIndex;
                        if (ComboBox.SelectedIndex == 0) { stackpanel_Remote.VerticalAlignment = VerticalAlignment.Top; stackpanel_Keyboard.VerticalAlignment = VerticalAlignment.Top; }
                        else if (ComboBox.SelectedIndex == 1) { stackpanel_Remote.VerticalAlignment = VerticalAlignment.Center; stackpanel_Keyboard.VerticalAlignment = VerticalAlignment.Center; }
                        else if (ComboBox.SelectedIndex == 2) { stackpanel_Remote.VerticalAlignment = VerticalAlignment.Bottom; stackpanel_Keyboard.VerticalAlignment = VerticalAlignment.Bottom; }
                    }
                };

                //Save - Close app Tile/Voice
                cb_CmdCloseApp.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["CmdCloseApp"] = true; }
                    else { vApplicationSettings["CmdCloseApp"] = false; }
                };

                //Save - Server ip adres
                txtbox_ServerIp.TextChanged += (sender, e) =>
                {
                    TextBox TextBox = (TextBox)sender;
                    if (String.IsNullOrWhiteSpace(TextBox.Text)) { TextBox.BorderBrush = new SolidColorBrush(Colors.Red); return; }
                    if (TextBox.Text.Contains("-")) { TextBox.Text = TextBox.Text.Replace("-", "."); TextBox.SelectionStart = TextBox.Text.Length; }
                    if (TextBox.Text.Contains(",")) { TextBox.Text = TextBox.Text.Replace(",", "."); TextBox.SelectionStart = TextBox.Text.Length; }
                    if (TextBox.Text.Count(x => x == '.') < 3 || TextBox.Text.Count(x => x == '.') > 3) { TextBox.BorderBrush = new SolidColorBrush(Colors.Red); return; }
                    if (!TextBox.Text.Contains(".") || TextBox.Text.Contains("..") || TextBox.Text.Contains("...") || TextBox.Text.EndsWith(".")) { TextBox.BorderBrush = new SolidColorBrush(Colors.Red); return; }

                    TextBox.BorderBrush = new SolidColorBrush(Colors.Green);
                    vApplicationSettings["ServerIp"] = TextBox.Text;
                };

                //Save - Server port
                txtbox_ServerPort.TextChanged += (sender, e) =>
                {
                    TextBox TextBox = (TextBox)sender;
                    if (String.IsNullOrWhiteSpace(TextBox.Text)) { TextBox.BorderBrush = new SolidColorBrush(Colors.Red); return; }
                    if (TextBox.Text.Contains("-")) { TextBox.Text = TextBox.Text.Replace("-", ""); TextBox.SelectionStart = TextBox.Text.Length; }
                    if (TextBox.Text.Contains(",")) { TextBox.Text = TextBox.Text.Replace(",", ""); TextBox.SelectionStart = TextBox.Text.Length; }
                    if (TextBox.Text.Contains(".")) { TextBox.Text = TextBox.Text.Replace(".", ""); TextBox.SelectionStart = TextBox.Text.Length; }
                    if (Regex.IsMatch(TextBox.Text, "(\\D+)")) { TextBox.BorderBrush = new SolidColorBrush(Colors.Red); return; }
                    if (Convert.ToInt32(TextBox.Text) < 1 || Convert.ToInt32(txtbox_ServerPort.Text) > 65535) { TextBox.BorderBrush = new SolidColorBrush(Colors.Red); return; }

                    TextBox.BorderBrush = new SolidColorBrush(Colors.Green);
                    vApplicationSettings["ServerPort"] = TextBox.Text;
                };
            }
            catch (Exception Ex) { await new MessageDialog("SettingsSaveError: " + Ex.Message, "MediaRemoteMe").ShowAsync(); }
        }

        //Open WiFi Settings
        async void SettingsOpenWiFiSettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:network-wifi")); }

        //Open NFC Settings
        async void SettingsOpenNFCSettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:proximity")); }

        //Open Project Website
        async void SettingsOpenProjectWebsite_Click(object sender, RoutedEventArgs e)
        {
            if (AVFunctions.DevMobile()) { await Launcher.LaunchUriAsync(new Uri("http://m.arnoldvink.com/?p=projects")); }
            else { await Launcher.LaunchUriAsync(new Uri("http://projects.arnoldvink.com")); }
        }

        //Open Donation Page
        async void SettingsOpenDonationPage_Click(object sender, RoutedEventArgs e)
        {
            if (AVFunctions.DevMobile()) { await Launcher.LaunchUriAsync(new Uri("http://m.arnoldvink.com/?p=donation")); }
            else { await Launcher.LaunchUriAsync(new Uri("http://donation.arnoldvink.com")); }
        }

        //Open Privacy Policy
        async void SettingsOpenPrivacyPolicy_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("http://privacy.arnoldvink.com")); }
    }
}