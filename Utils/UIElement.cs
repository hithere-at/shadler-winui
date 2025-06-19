using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;

namespace Shadler.UI
{
    public static class ShadlerUIElement
    {
        public static Button CreateContentButton(string title, string year, string url)
        {
            // main items
            // content banner
            Image contentImage = new Image
            {
                Source = new BitmapImage(new Uri(url)),
                Width = 150,
                Height = 240,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            // content title
            TextBlock contentTitle = new TextBlock
            {
                Text = title,
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // content release year
            TextBlock contentYear = new TextBlock
            {
                Text = year,
                FontSize = 13,
                HorizontalAlignment = HorizontalAlignment.Center,
                Opacity = 0.6
            };

            StackPanel baseLayout = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            baseLayout.Children.Add(contentImage);
            baseLayout.Children.Add(contentTitle);
            baseLayout.Children.Add(contentYear);

            Button button = new Button
            {
                Content = baseLayout,
                Margin = new Thickness(0,0,12,0)
            };

            return button;
        }
    }
}
