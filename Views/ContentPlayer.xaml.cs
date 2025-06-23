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
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using WinRT.Interop;

using Shadler.DataStructure;
using Shadler.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Shadler.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPlayer : Page
    {
        public ContentPlayer()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs args)
        {
            base.OnNavigatedTo(args);

            if (args.Parameter is ShadlerPlayerContent contentInfo)
            {
                string videoUrl;

                using (HttpClient client = new HttpClient())
                {
                    ShadlerHttp.SetDefaultHeader(client);
                    HttpResponseMessage response = await client.GetAsync(contentInfo.StreamsUrl);

                    string matchedString;

                    if (response.IsSuccessStatusCode)
                    {
                        string regexPattern = "apivtwo/[^\"]*";

                        string streamsResponse = await response.Content.ReadAsStringAsync();

                        Regex regex = new Regex(regexPattern);
                        Match match = regex.Match(streamsResponse);
                        matchedString = match.Value.Replace("clock", "clock.json").Replace("/download", "");

                    }
                    else
                    {
                        return;
                    }

                    response = await client.GetAsync("https://blog.allanime.day/" + matchedString);

                    if (response.IsSuccessStatusCode)
                    {
                        string videoResponse = await response.Content.ReadAsStringAsync();

                        JsonDocument doc = JsonDocument.Parse(videoResponse);
                        JsonElement root = doc.RootElement;
                        videoUrl = root.GetProperty("links")[0].GetProperty("link").ToString();

                    }
                    else
                    {
                        return;
                    }

                }

                PlayerTitle.Text = contentInfo.Title;
                PlayerYear.Text = contentInfo.Year;

                ContentPlayerElement.Source = MediaSource.CreateFromUri(new Uri(videoUrl));
            }
        }

        private void FullScreenClick(object sender, RoutedEventArgs args)
        {
            Window? window = App.MainWindow;
            IntPtr hwnd = WindowNative.GetWindowHandle(window);
            AppWindow appWindow = AppWindow.GetFromWindowId(Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd));

            if (appWindow.Presenter.Kind == AppWindowPresenterKind.FullScreen)
            {
                appWindow.SetPresenter(AppWindowPresenterKind.Default);
            }
            else
            {
                appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
            }
        }
    }
}
