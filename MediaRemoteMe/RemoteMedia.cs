using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MediaRemoteMe
{
    public partial class MainPage : Page
    {
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
                await SocketSendArnold("Me" + TapTag);
            }
            catch { }
        }
    }
}