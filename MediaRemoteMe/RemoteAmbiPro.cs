using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace MediaRemoteMe
{
    public partial class MainPage : Page
    {
        async void Btn_AmbiPro_OnOff_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("AmbiPro: Turning the leds on or off.");
                await SocketSendAmbiPro("LedSwitch");
            }
            catch { }
        }

        async void Slider_AmbiPro_Brightness_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            try
            {
                Slider Slider = (Slider)sender;
                if (e.OldValue != 0)
                {
                    Debug.WriteLine("AmbiPro: Changing led brightness to " + Slider.Value);
                    await SocketSendAmbiPro("LedBrightness‡" + Slider.Value);
                }
            }
            catch { }
        }

        async void Combobox_AmbiPro_Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox ComboBox = (ComboBox)sender;
                Debug.WriteLine("AmbiPro: Changing led mode to " + ComboBox.SelectedIndex);
                await SocketSendAmbiPro("LedMode‡" + ComboBox.SelectedIndex);
            }
            catch { }
        }
    }
}