using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Data.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;

using Shadler.DataStructure;
using Shadler.Utils;
using Shadler.UI;

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

        List<List<string>> episodePages = new List<List<string>>();
        int pageIndex = 0;

        protected override async void OnNavigatedTo(NavigationEventArgs args) {

            base.OnNavigatedTo(args);

            if (args.Parameter is ShadlerContent currentContent)
            {

                using (HttpClient client = new HttpClient())
                {
                    ShadlerHttp.SetDefaultHeader(client);

                    HttpResponseMessage response = await client.GetAsync(currentContent.DetailUrl);

                    if (!(response.IsSuccessStatusCode))
                    {
                        ContentDetails.Children.Clear();
                        ContentDetails.Children.Add(new TextBlock
                        {
                            Text = "Failed to load content details.",
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                        });
                    }

                    string responseData = await response.Content.ReadAsStringAsync();

                    using (JsonDocument doc = JsonDocument.Parse(responseData))
                    {
                        string contentIdentifierWhyCantTheyJustHaveAStableAPI = currentContent.ContentType == "Anime" ? "show" : "manga";
                        string contentEpisodesChaptersWhatever = currentContent.ContentType == "Anime" ? "availableEpisodesDetail" : "availableChaptersDetail";
                        int count = 0;

                        JsonElement root = doc.RootElement;
                        string contentDesciption = root
                            .GetProperty("data")
                            .GetProperty(contentIdentifierWhyCantTheyJustHaveAStableAPI)
                            .GetProperty("description").ToString();

                        ContentDescription.Text = WebUtility.HtmlDecode(contentDesciption).Replace("<br>", "");

                        JsonElement episodeStrings = root
                            .GetProperty("data")
                            .GetProperty(contentIdentifierWhyCantTheyJustHaveAStableAPI)
                            .GetProperty(contentEpisodesChaptersWhatever)
                            .GetProperty("sub");

                        int episodesLength = episodeStrings.GetArrayLength() - 1;
                        List<string> pageHelper = new List<string>();

                        foreach (JsonElement episode in episodeStrings.EnumerateArray())
                        {
                            pageHelper.Add(episode.ToString());

                            if ((count != 0 && count % 15 == 0) || count == episodesLength)
                            {
                                episodePages.Add(new List<string>(pageHelper));
                                pageHelper.Clear();
                            }

                            count++;
                        }

                        foreach (string episodeString in episodePages[0])
                        {
                            EpisodeSelector.Children.Add(ShadlerUIElement.CreateShadlerEpisodeButton(episodeString));
                        }
                    }
                }

                ContentTitle.Text = currentContent.Title;
                ContentYear.Text = currentContent.Year;
                ContentThumbnail.Source = currentContent.Thumbnail;

            }
        }

        private void ContentPage_KeyPressed(object sender, KeyRoutedEventArgs args)
        {
            if (args.Key == Windows.System.VirtualKey.Enter)
            {
                var pageBox = sender as TextBox;

                if (int.TryParse(pageBox?.Text, out pageIndex)) {

                    pageIndex -= 1;
                    EpisodeSelector.Children.Clear();

                    foreach (string episodeString in episodePages[pageIndex])
                    {
                        Console.WriteLine(episodeString);
                        EpisodeSelector.Children.Add(ShadlerUIElement.CreateShadlerEpisodeButton(episodeString));
                    }

                    pageBox.Text = pageIndex.ToString();

                } else
                {
                    return;
                }
            }
        }

        private void NextPageClick(object sender, RoutedEventArgs args)
        {
            pageIndex += 1;
            EpisodeSelector.Children.Clear();

            foreach (string episodeString in episodePages[pageIndex])
            {
                EpisodeSelector.Children.Add(ShadlerUIElement.CreateShadlerEpisodeButton(episodeString));
            }
        }

        private void PreviousPageClick(object sender, RoutedEventArgs args)
        {
            pageIndex -= 1;
            EpisodeSelector.Children.Clear();

            foreach (string episodeString in episodePages[pageIndex])
            {
                EpisodeSelector.Children.Add(ShadlerUIElement.CreateShadlerEpisodeButton(episodeString));
            }
        }
    }
}