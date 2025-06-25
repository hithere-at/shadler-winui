using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Shadler.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FullScreenPlayer : Window
    {
        public MediaPlayer currentPlayer = new ();
        
        public FullScreenPlayer()
        {
            this.InitializeComponent();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
        }


        public void SetMediaPlayerSource(MediaPlayer player)
        {
            currentPlayer = player;
            ContentPlayer.SetMediaPlayer(player);
        }

        private void ContentPlayerGrid_KeyPressed(object sender, KeyRoutedEventArgs args)
        {
            if (args.Key == Windows.System.VirtualKey.Escape)
            {
                this.Close();
            }
        }
    }
}
