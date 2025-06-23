using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadler.DataStructure
{
    public struct ShadlerGeneralContent
    {
        public string ContentType { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public BitmapImage Thumbnail { get; set; }
        public string DetailUrl { get; set; }
        public ShadlerGeneralContent(string contentType, string id, string title, string year, BitmapImage thumbnail, string detailUrl)
        {
            ContentType = contentType;
            Id = id;
            Title = title;
            Year = year;
            Thumbnail = thumbnail;
            DetailUrl = detailUrl;
        }
    };

    public struct ShadlerPlayerContent
    {
        public string ContentType { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Episode { get; set; }
        public string StreamsUrl { get; set; }

        public ShadlerPlayerContent()
        {
            ContentType = string.Empty;
            Id = string.Empty;
            Title = string.Empty;
            Year = string.Empty;
            Episode = string.Empty;
            StreamsUrl = string.Empty;
        }
        public ShadlerPlayerContent(string contentType, string id, string title, string year, string episode, string contentUrl)
        {
            ContentType = contentType;
            Id = id;
            Title = title;
            Year = year;
            Episode = episode;
            StreamsUrl = contentUrl;
        }
    }
   
}
