using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes; 
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

using Shadler.UI;
using Shadler.Utils;
using Shadler.DataStructure;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Shadler.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Browser : Page
    {
        string currentContentType = "Anime";
        string currentQuery = string.Empty;
        List<ShadlerGeneralContent> shadlerContents = new List<ShadlerGeneralContent>();
        
        public Browser()
        {
            this.InitializeComponent();
        }

        private void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                currentQuery = sender.Text;
                Search_Query(currentQuery);
            }
        }

        private void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Search_Query(args.QueryText);
        }

        private async void Search_Query(string query)
        {

            if (string.IsNullOrEmpty(query))
            {
                ContentViewerFrame.BackStack.Clear();
                ContentViewerFrame.Content = null;
                ContentGrid.Children.Clear();
                shadlerContents.Clear();
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                ShadlerHttp.SetDefaultHeader(client);

                string queryUrl;

                if (currentContentType == "Anime")
                {
                    queryUrl = Anime.GetQueryUrl(query);
                }
                else
                {
                    queryUrl = Manga.GetQueryUrl(query);
                }

                HttpResponseMessage response = await client.GetAsync(queryUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(responseData))
                    {
                        string what = currentContentType == "Anime" ? "shows" : "mangas";
                        int count = 0;

                        JsonElement root = doc.RootElement;
                        JsonElement contentResults;

                        ContentViewerFrame.BackStack.Clear();
                        ContentViewerFrame.Content = null;
                        ContentGrid.Children.Clear();
                        shadlerContents.Clear();

                        if (root.TryGetProperty("data", out contentResults))
                        {
                            contentResults = contentResults.GetProperty(what).GetProperty("edges");
                        }
                        else
                        {
                            // silently fail if the data doesnt exist because API not API'ing
                            return;
                        }

                        foreach (JsonElement content in contentResults.EnumerateArray())
                        {

                            string? id = content.GetProperty("_id").GetString();
                            string? title = content.GetProperty("name").GetString();
                            string year;

                            string? thumbnailUrl = content.GetProperty("thumbnail").GetString();
                            thumbnailUrl = !thumbnailUrl.StartsWith("https://")
                                ? "https://aln.youtube-anime.com/" + thumbnailUrl
                                : thumbnailUrl;

                            string detailUrl = currentContentType == "Anime"
                                ? Anime.GetDetailUrl(content.GetProperty("_id").GetString())
                                : Manga.GetDetailUrl(content.GetProperty("_id").GetString());


                            JsonElement date = content.GetProperty("airedStart");   

                            if (date.TryGetProperty("year", out JsonElement yearElement))
                            {
                                year = yearElement.ToString();
                            } else
                            {
                                year = " ";
                            }

                            BitmapImage thumbnailImage = new BitmapImage(new Uri(thumbnailUrl));

                            Button currContentButton = ShadlerUIElement.CreateShadlerContent(title, year, thumbnailImage, count.ToString());
                            ShadlerGeneralContent currContent = new ShadlerGeneralContent(currentContentType, id, title, year, thumbnailImage, detailUrl);

                            currContentButton.Click += SelectContent_Event;
                            ContentGrid.Children.Add(currContentButton);
                            shadlerContents.Add(currContent);

                            count++;
                        }
                    }
                }
                else
                {
                    ContentGrid.Children.Add(new TextBlock
                    {
                        Text = "Uh oh. Something has gone really wrong. Please try again.",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 16
                    });
                }
            }
        }

        private void ContentMenu_Click(object sender, RoutedEventArgs args)
        {

            if (sender is MenuFlyoutItem item)
            {
                if (item.Text == currentContentType)
                {
                    return; // dont reload if the selected content type is the same as the current one

                } else {

                    ContentViewerFrame.BackStack.Clear();
                    ContentViewerFrame.Content = null;
                    ContentGrid.Children.Clear();
                    shadlerContents.Clear();

                    switch (item.Text)
                    {
                        case "Anime":
                            currentContentType = "Anime";
                            ContentTypeDropDown.Content = "Anime";
                            break;

                        case "Manga":
                            currentContentType = "Manga";
                            ContentTypeDropDown.Content = "Manga";
                            break;

                        default:
                            return;
                    }

                    Search_Query(currentQuery);

                }
            }
        }

        private void SelectContent_Event(object sender, RoutedEventArgs args)
        {
            ContentGrid.Children.Clear();

            if (sender is Button shadlerContentButton)
            { 
                int index = int.Parse(shadlerContentButton.Tag.ToString());

                ContentViewerFrame.Navigate(typeof(ContentViewer), shadlerContents[index]);

            }

        }
    }
}
