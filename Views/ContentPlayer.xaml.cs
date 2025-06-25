using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Shadler.DataStructure;
using Shadler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.Spi;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.Media.Core;
using Windows.Media.Streaming.Adaptive;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Shadler.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPlayer : Page
    {
        List<string> availableEpisodes = new List<string>();
        int episodeIndex;
        string contentId;
        string title;
        string year;

        public ContentPlayer()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs args)
        {
            base.OnNavigatedTo(args);

            if (args.Parameter is ShadlerPlayerContent contentInfo)
            {
                contentId = contentInfo.Id;
                title = contentInfo.Title;
                year = contentInfo.Year;
                availableEpisodes = new List<string>(contentInfo.AvailableEpisodes);

                string videoUrl = await GetVideoUrl(contentInfo.StreamsUrl);

                episodeIndex = availableEpisodes.IndexOf(contentInfo.Episode);
                
                PlayerTitle.Text = $"{title} Episode {availableEpisodes[episodeIndex]}";
                PlayerYear.Text = year;

                if (availableEpisodes.Count == 1)
                {
                    NextButton.IsEnabled = false;
                    PreviousButton.IsEnabled = false;

                } else if (episodeIndex == availableEpisodes.Count - 1)
                {
                    PreviousButton.IsEnabled = false;

                } else if (episodeIndex == 0) {
                    NextButton.IsEnabled = false;

                }

                    ContentPlayerElement.Source = MediaSource.CreateFromUri(new Uri(videoUrl));
            }
        }

        private void FullScreenClick(object sender, RoutedEventArgs args)
        {
            FullScreenPlayer fullScreenPlayer = new FullScreenPlayer();
            fullScreenPlayer.SetMediaPlayerSource(ContentPlayerElement.MediaPlayer);
            fullScreenPlayer.Activate();
        }


        private void NextEpisodeClick(object sender, RoutedEventArgs args)
        {
            episodeIndex -= 1;

            if (!(episodeIndex == 0)) // not in last episode
            {
                PreviousButton.IsEnabled = true;
                NextButton.IsEnabled = true;
            } else
            {
                NextButton.IsEnabled = false;
            }

            PlayCurrentEpisode();
        }

        private void PreviousEpisodeClick(object sender, RoutedEventArgs args)
        {
            episodeIndex += 1;

            if (!(episodeIndex == availableEpisodes.Count - 1)) // not in first episode
            {
                PreviousButton.IsEnabled = true;
                NextButton.IsEnabled = true;
            }
            else
            {
                PreviousButton.IsEnabled = false;
            }

            PlayCurrentEpisode();
        }

        // this function sets videoUrl variable when finished executing
        private async Task<string> GetVideoUrl(string streamUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                ShadlerHttp.SetDefaultHeader(client);
                HttpResponseMessage response = await client.GetAsync(streamUrl);

                string matchedString;
                string videoUrl = string.Empty;

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
                    return string.Empty;
                }

                response = await client.GetAsync("https://blog.allanime.day/" + matchedString);

                if (response.IsSuccessStatusCode)
                {
                    string videoResponse = await response.Content.ReadAsStringAsync();

                    JsonDocument doc = JsonDocument.Parse(videoResponse);
                    JsonElement root = doc.RootElement;
                    videoUrl = root.GetProperty("links")[0].GetProperty("link").ToString();

                    return videoUrl;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private async void PlayCurrentEpisode()
        {
            string episode = availableEpisodes[episodeIndex];
            string streamUrl = Anime.GetStreamUrl(contentId, episode);
            string videoUrl = await GetVideoUrl(streamUrl);

            PlayerTitle.Text = $"{title} Episode {episode}";
            PlayerYear.Text = year;

            ContentPlayerElement.Source = MediaSource.CreateFromUri(new Uri(videoUrl));
        }
    }
}
