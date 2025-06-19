using ABI.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic;
using Shadler.Utils;
using Shadler.UI;
using System;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Shadler.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Browser : Page
    {
        string contentType = "Anime";
        public Browser()
        {
            this.InitializeComponent();
        }

        private async void Search_Query(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            Console.WriteLine("Search_Query called");
            string query = sender.Text;
            
            if (string.IsNullOrEmpty(query))
            {
                ContentGrid.Children.Clear();
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/112.0");
                client.DefaultRequestHeaders.Add("Referer", "https://allmanga.to");
                string url;

                if (contentType == "Anime")
                {
                    url = Shadler.Utils.Anime.GetQueryUrl(query);
                }
                else
                {
                    url = Shadler.Utils.Manga.GetQueryUrl(query);
                }

                Console.WriteLine($"Requesting URL: {url}");
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(responseData))
                    {
                        string what = contentType == "Anime" ? "shows" : "mangas";
                        JsonElement root = doc.RootElement;
                        JsonElement contentResults = root.GetProperty("data").GetProperty(what).GetProperty("edges");
                        int count = 0;

                        ContentGrid.Children.Clear();

                        foreach (JsonElement content in contentResults.EnumerateArray())
                        {
                            string thumbnailPath = content.GetProperty("thumbnail").GetString();
                            thumbnailPath = !(thumbnailPath.StartsWith("https://")) ? "https://aln.youtube-anime.com/" + thumbnailPath : thumbnailPath;
                            string title = content.GetProperty("name").GetString();
                            JsonElement date = content.GetProperty("airedStart");   
                            string year;

                            if (date.TryGetProperty("year", out JsonElement yearElement))
                            {
                                year = yearElement.ToString();
                            } else
                            {
                                year = " ";
                            }

                            Button contentButton = ShadlerUIElement.CreateContentButton(title, year, thumbnailPath);
                            ContentGrid.Children.Add(contentButton);

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
            ContentGrid.Children.Clear();

            if (sender is MenuFlyoutItem item)
            {
                switch (item.Text)
                {
                    case "Anime":
                        contentType = "Anime";
                        ContentTypeDropDown.Content = "Anime";
                        break;

                    case "Manga":
                        contentType = "Manga";
                        ContentTypeDropDown.Content = "Manga"; 
                        break;
                }
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }
    }
}
