using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadler.DataStructure
{
    public struct ShadlerContent
    {
        public string ContentType { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public BitmapImage Thumbnail { get; set; }
        public string DetailUrl { get; set; }
        public ShadlerContent(string contentType, string id, string title, string year, BitmapImage thumbnail, string detailUrl)
        {
            ContentType = contentType;
            Id = id;
            Title = title;
            Year = year;
            Thumbnail = thumbnail;
            DetailUrl = detailUrl;
        }
    };
   
}
