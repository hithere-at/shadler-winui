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
                CornerRadius = new CornerRadius(8),
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

        public static Grid CreateShadlerEpisodeButton(string episode, RoutedEventHandler playButtonEvent)
        {
            Grid episodeViewerGrid = new Grid
            {
                CornerRadius = new CornerRadius(4),
                Margin = new Thickness(0, 6, 0, 0),
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0x0F, 0xFF, 0x0FF, 0x0FF))
            };

            episodeViewerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) });
            episodeViewerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            episodeViewerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Auto) });

            TextBlock episodeString = new TextBlock
            {
                Margin = new Thickness(12, 0, 0, 0),
                Text = "Episode " + episode,
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };

            Button playButton = new Button
            {
                Tag = episode,
                Margin = new Thickness(0, 12, 12, 12),
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = new FontIcon
                {
                    FontFamily = new FontFamily("Segoe Fluent Icons"),
                    Glyph = "\uE768"
                }
            };

            Button downloadButton = new Button
            {
                Tag = episode,
                Margin = new Thickness(0, 12, 12, 12),
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = new FontIcon
                {
                    FontFamily = new FontFamily("Segoe Fluent Icons"),
                    Glyph = "\uE896"
                }
            };

            playButton.Click += playButtonEvent;

            Grid.SetColumn(episodeString, 0);
            Grid.SetColumn(playButton, 1);
            Grid.SetColumn(downloadButton, 2);

            episodeViewerGrid.Children.Add(episodeString);
            episodeViewerGrid.Children.Add(playButton);
            episodeViewerGrid.Children.Add(downloadButton);

            return episodeViewerGrid;
        }
    }
}
