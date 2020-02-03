using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MediaRemoteMe
{
    public partial class MainPage : Page
    {
        async void Btn_PrimaryMonitor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Monitor: Switch to primary");
                await SocketSendArnold("SwitchMonitor‡Primary");
            }
            catch { }
        }

        async void Btn_SecondaryMonitor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Monitor: Switch to Secondary");
                await SocketSendArnold("SwitchMonitor‡Secondary");
            }
            catch { }
        }

        async void Btn_ExtendMonitor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Monitor: Switch to Extend");
                await SocketSendArnold("SwitchMonitor‡Extend");
            }
            catch { }
        }

        async void Btn_DuplicateMonitor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Monitor: Switch to Duplicate");
                await SocketSendArnold("SwitchMonitor‡Duplicate");
            }
            catch { }
        }
    }
}