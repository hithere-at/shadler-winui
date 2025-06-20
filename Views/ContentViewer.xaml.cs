using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Shadler.DataStructure;
using Microsoft.UI.Xaml.Media.Imaging;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Shadler.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentViewer : Page
    {
        public ContentViewer()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs args) {

            base.OnNavigatedTo(args);

            if (args.Parameter is ShadlerContent currentContent)
            {
                ContentViewerTitle.Text = currentContent.Title;
                ContentViewerYear.Text = currentContent.Year;
                ContentViewerThumbnail.Source = new BitmapImage(new Uri(currentContent.ThumbnailUrl));
            }
        
        }
    }

}