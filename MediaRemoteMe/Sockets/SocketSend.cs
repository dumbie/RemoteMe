using ArnoldVinkCode;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace MediaRemoteMe
{
    partial class MainPage
    {
        //Send socket message to ArnoldVinkTools
        async Task SocketSendArnold(string RemoteData)
        {
            try
            {
                //Check if not sending a socket already
                if (!vSendingSocketMsg)
                {
                    //Check if server ip adres is valid
                    string ServerIp = Convert.ToString(vApplicationSettings["ServerIp"]);
                    string ServerPort = Convert.ToString(vApplicationSettings["ServerPort"]);
                    if (!await ValidateRemoteIpAdres(ServerIp)) { return; }

                    //Set the remote socket variables
                    vSendingSocketMsg = true;

                    //Update the status user interface
                    txt_AppStatusDesc.Text = "Sending remote command...";
                    app_StatusBar.Visibility = Visibility.Visible;

                    using (StreamSocket StreamSocket = new StreamSocket())
                    {
                        using (CancellationTokenSource CancelToken = new CancellationTokenSource())
                        {
                            CancelToken.CancelAfter(2500);
                            IAsyncAction SocketConnectAsync = StreamSocket.ConnectAsync(new HostName(ServerIp), ServerPort);
                            await SocketConnectAsync.AsTask(CancelToken.Token);
                        }
                        using (DataWriter DataWriter = new DataWriter(StreamSocket.OutputStream))
                        {
                            DataWriter.WriteString(RemoteData);
                            await DataWriter.StoreAsync();
                        }
                    }

                    vSendingSocketMsg = false;
                    app_StatusBar.Visibility = Visibility.Collapsed;
                    txt_ErrorConnect.Visibility = Visibility.Collapsed;
                    txt_ErrorConnect2.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                vSendingSocketMsg = false;
                app_StatusBar.Visibility = Visibility.Collapsed;
                if (!AVFunctions.DevMobile())
                {
                    txt_ErrorConnect.Visibility = Visibility.Visible;
                    txt_ErrorConnect2.Visibility = Visibility.Visible;
                }

                await new MessageDialog("Failed to connect to the Arnold Vink Tools application on your PC please check this app settings, your network connection and make sure that Arnold Vink Tools is running on the target PC.", App.vApplicationName).ShowAsync();
            }
        }

        //Send socket message to AmbiPro
        async Task SocketSendAmbiPro(string RemoteData)
        {
            try
            {
                //Check if not sending a socket already
                if (!vSendingSocketMsg)
                {
                    //Check if server ip adres is valid
                    string ServerIp = Convert.ToString(vApplicationSettings["ServerIp"]);
                    string ServerPort = Convert.ToString(vApplicationSettings["ServerPortAmbiPro"]);
                    if (!await ValidateRemoteIpAdres(ServerIp)) { return; }

                    //Set the remote socket variables
                    vSendingSocketMsg = true;

                    //Update the status user interface
                    txt_AppStatusDesc.Text = "Sending remote command...";
                    app_StatusBar.Visibility = Visibility.Visible;

                    using (StreamSocket StreamSocket = new StreamSocket())
                    {
                        using (CancellationTokenSource CancelToken = new CancellationTokenSource())
                        {
                            CancelToken.CancelAfter(2500);
                            IAsyncAction SocketConnectAsync = StreamSocket.ConnectAsync(new HostName(ServerIp), ServerPort);
                            await SocketConnectAsync.AsTask(CancelToken.Token);
                        }
                        using (DataWriter DataWriter = new DataWriter(StreamSocket.OutputStream))
                        {
                            DataWriter.WriteString(RemoteData);
                            await DataWriter.StoreAsync();
                        }
                    }

                    vSendingSocketMsg = false;
                    app_StatusBar.Visibility = Visibility.Collapsed;
                    txt_ErrorConnect.Visibility = Visibility.Collapsed;
                    txt_ErrorConnect2.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                vSendingSocketMsg = false;
                app_StatusBar.Visibility = Visibility.Collapsed;
                if (!AVFunctions.DevMobile())
                {
                    txt_ErrorConnect.Visibility = Visibility.Visible;
                    txt_ErrorConnect2.Visibility = Visibility.Visible;
                }

                await new MessageDialog("Failed to connect to the AmbiPro application on your PC please check this app settings, your network connection and make sure that AmbiPro is running on the target PC.", App.vApplicationName).ShowAsync();
            }
        }

        //Check if the server ip adres is valid
        async Task<bool> ValidateRemoteIpAdres(string IpAdres)
        {
            try
            {
                if (IpAdres.EndsWith(".") || IpAdres.EndsWith(",") || IpAdres.Count(x => x == '.') < 3)
                {
                    menuButtonSettings_Tapped(null, null);
                    await Task.Delay(250);
                    txtbox_ServerIp.Focus(FocusState.Programmatic);
                    txtbox_ServerIp.SelectionStart = txtbox_ServerIp.Text.Length;
                    return false;
                }
                else { return true; }
            }
            catch { }
            return false;
        }
    }
}