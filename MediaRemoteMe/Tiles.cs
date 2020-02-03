using System;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;

namespace MediaRemoteMe
{
    partial class MainPage
    {
        //Check if live tiles are pinned
        void TilesLoad()
        {
            try
            {
                if (SecondaryTile.Exists("MediaRemoteMePlayPause")) { btn_PinPlayPause.Content = "Unpin Play or Pause"; }
                if (SecondaryTile.Exists("MediaRemoteMePrevious")) { btn_PinPrevious.Content = "Unpin Previous Item"; }
                if (SecondaryTile.Exists("MediaRemoteMeNext")) { btn_PinNext.Content = "Unpin Next Item"; }
                if (SecondaryTile.Exists("MediaRemoteMeVolUp")) { btn_PinVolUp.Content = "Unpin Volume Up"; }
                if (SecondaryTile.Exists("MediaRemoteMeVolDown")) { btn_PinVolDown.Content = "Unpin Volume Down"; }
                if (SecondaryTile.Exists("MediaRemoteMeVolMute")) { btn_PinVolMute.Content = "Unpin Volume Mute"; }
                if (SecondaryTile.Exists("MediaRemoteMeArrowLeft")) { btn_PinArrowLeft.Content = "Unpin Arrow Left"; }
                if (SecondaryTile.Exists("MediaRemoteMeArrowRight")) { btn_PinArrowRight.Content = "Unpin Arrow Right"; }
                if (SecondaryTile.Exists("MediaRemoteMeFullscreen")) { btn_PinFullscreen.Content = "Unpin Fullscreen"; }
                if (SecondaryTile.Exists("MediaRemoteMeAmbiProOnOff")) { btn_PinAmbiProOnOff.Content = "Unpin AmbiPro On/Off"; }
            }
            catch { }
        }

        //Unpin all the live tiles
        async void btn_UnpinAllTiles_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                foreach (SecondaryTile SecondaryTile in await SecondaryTile.FindAllAsync())
                { await SecondaryTile.RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below); }

                btn_PinPlayPause.Content = "Pin Play or Pause";
                btn_PinPrevious.Content = "Pin Previous Item";
                btn_PinNext.Content = "Pin Next Item";
                btn_PinVolUp.Content = "Pin Volume Up";
                btn_PinVolDown.Content = "Pin Volume Down";
                btn_PinVolMute.Content = "Pin Volume Mute";
                btn_PinArrowLeft.Content = "Pin Arrow Left";
                btn_PinArrowRight.Content = "Pin Arrow Right";
                btn_PinFullscreen.Content = "Pin Fullscreen";

                await new MessageDialog("All the remote tiles have been unpinned from your start screen.", App.vApplicationName).ShowAsync();
            }
            catch { }
        }

        //Pin or unpin Play or Pause
        async void btn_PinPlayPause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMePlayPause";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinPlayPause.Content = "Pin Play or Pause"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "Play or Pause", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/PlayPause.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinPlayPause.Content = "Unpin Play or Pause"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Pin or unpin Previous Item
        async void btn_PinPrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMePrevious";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinPrevious.Content = "Pin Previous Item"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "Previous Item", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/Previous.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinPrevious.Content = "Unpin Previous Item"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Pin or unpin Next Item
        async void btn_PinNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMeNext";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinNext.Content = "Pin Next Item"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "Next Item", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/Next.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinNext.Content = "Unpin Next Item"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Pin or unpin Volume Up
        async void btn_PinVolUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMeVolUp";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinVolUp.Content = "Pin Volume Up"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "Volume Up", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/VolUp.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinVolUp.Content = "Unpin Volume Up"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Pin or unpin Volume Down
        async void btn_PinVolDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMeVolDown";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinVolDown.Content = "Pin Volume Down"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "Volume Down", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/VolDown.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinVolDown.Content = "Unpin Volume Down"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Pin or unpin Volume Mute
        async void btn_PinVolMute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMeVolMute";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinVolMute.Content = "Pin Volume Mute"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "Volume Mute", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/VolMute.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinVolMute.Content = "Unpin Volume Mute"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Pin or unpin Arrow Left
        async void btn_PinArrowLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMeArrowLeft";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinArrowLeft.Content = "Pin Arrow Left"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "Arrow Left", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/ArrowLeft.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinArrowLeft.Content = "Unpin Arrow Left"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Pin or unpin Arrow Right
        async void btn_PinArrowRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMeArrowRight";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinArrowRight.Content = "Pin Arrow Right"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "Arrow Right", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/ArrowRight.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinArrowRight.Content = "Unpin Arrow Right"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Pin or unpin Fullscreen
        async void btn_PinFullscreen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMeFullscreen";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinFullscreen.Content = "Pin Fullscreen"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "Fullscreen", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/Fullscreen.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinFullscreen.Content = "Unpin Fullscreen"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Pin or unpin AmbiPro On/Off
        async void btn_PinAmbiProOnOff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "MediaRemoteMeAmbiProOnOff";
                if (SecondaryTile.Exists(SecondaryTileId))
                {
                    bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (UnPinned) { btn_PinAmbiProOnOff.Content = "Pin AmbiPro On/Off"; }
                }
                else
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "AmbiPro On/Off", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/AmbiPro.png"), TileSize.Square150x150);
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned) { btn_PinAmbiProOnOff.Content = "Unpin AmbiPro On/Off"; }
                }
            }
            catch { await new MessageDialog("Failed to pin or unpin the tile, please try again.", App.vApplicationName).ShowAsync(); }
        }

        //Render Secondary Live Tile
        Rect GetElementRect(FrameworkElement FrameworkElement)
        {
            return new Rect(FrameworkElement.TransformToVisual(null).TransformPoint(new Point()), new Size(FrameworkElement.ActualWidth, FrameworkElement.ActualHeight));
        }
    }
}