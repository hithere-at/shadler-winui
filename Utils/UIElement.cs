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

using Shadler.DataStructure;
namespace Shadler.UI
{
    public static class ShadlerUIElement
    {
        public static Button CreateShadlerContent(string title, string year, BitmapImage image, string tag)
        {
            // main items
            // content banner
            Image contentImage = new Image
            {
                Source = image,
                Stretch = Stretch.UniformToFill,
            };

            // border
            Border contentImageContainer = new Border
            {
                Width = 150,
                Height = 240,
                HorizontalAlignment = HorizontalAlignment.Center,
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 0, 0, 9),
            };

            contentImageContainer.Child = contentImage;

            // content title
            // very stupid hack because manga author cant make a proper title lmaooooooo
            // seriously, what the heck is ryoushin no shakkin wo katagawari shite morau jouken bla bla :sob: (good manga btw, very sweet)
            // 
            string shortTitle = title;

            if (title.Length > 22)
            {
                shortTitle = title.Substring(0, 19) + "...";
            }

            TextBlock contentTitle = new TextBlock
            {
                Text = shortTitle,
                TextWrapping = TextWrapping.WrapWholeWords,
                FontSize = 15,
                Margin = new Thickness(4, 0, 4, 0),
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

            baseLayout.Children.Add(contentImageContainer);
            baseLayout.Children.Add(contentTitle);
            baseLayout.Children.Add(contentYear);

            Button button = new Button
            {
                Width = 200,
                Height = 300,
                Content = baseLayout,
                Margin = new Thickness(0, 0, 12, 12),
                Tag = tag
            };

            return button;
        }
    }
}
