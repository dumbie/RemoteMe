using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MediaRemoteMe
{
    partial class MainPage
    {
        //Load - Help text
        void HelpLoad()
        {
            try
            {
                if (!sp_Help.Children.Any())
                {
                    sp_Help.Children.Add(new TextBlock() { Text = "How do I setup MediaRemoteMe?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "After installing the client and server, lookup your PC ip on your PC by opening Arnold Vink Tools from the traybar and click on 'Show IP' now enter the servers PC ip adres on your device on the Settings page and you are ready.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWhere can I download the server?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "You can download the Arnold Vink Tools (server) from the project website found below.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWhere can I find my server IP adres?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "On your PC open the running Arnold Vink Tools from the traybar and click on the 'Show IP' button.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nMy media player does not respond", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Only media players that support the mediakeys from a keyboard will work, for example: Zune Software, Windows Media Player, Groove Music, Xbox Music, Foobar2000 and iTunes. When you are using Winamp manually enable 'Enable default media key support' in the Global Hotkeys Preferences.\r\n\r\nSome applications like an active 'File Explorer' might block a few media key commands from been executed to your media player, making those applications inactive may fix the issue.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWhy does my player not go fullscreen?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "When the fullscreen media command does not work make sure that your video player is already the active window on your PC otherwise the command will not be received.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nMy remote fails to connect to my PC", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Make sure that the server is allowed in the firewall and your PC ip adres and port number is set correctly, some users might need to run the server as Administrator.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWindows PC client fails to connect", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "When you are using a Windows PC as both server and client you will also need to run Arnold Vink Tools once on the client PC as Administrator to allow the remote controls to be sent out to your server.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nCortana voice command support", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "This application also supports various Cortana voice commands, you can check them all out by telling 'What can I say' to Cortana to see a list of capable application commands.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nSupport and bug reporting", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "When you are walking into any problem or bug you can goto the support page here: http://help.arnoldvink.com", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nDevelopment donation support", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Feel free to make a donation on: http://donation.arnoldvink.com", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    PackageVersion AppVersion = Package.Current.Id.Version;
                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nApplication made by Arnold Vink", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Version: v" + AppVersion.Major + "." + AppVersion.Minor + "." + AppVersion.Build + "." + AppVersion.Revision, Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });
                }
            }
            catch { }
        }
    }
}