using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadler.DataStructure
{
    public struct ShadlerContent
    {
        public Button Button { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string ThumbnailUrl { get; set; }
        public ShadlerContent(Button button, string id, string title, string year, string thumbnailUrl)
        {
            Button = button;
            Title = title;
            Year = year;
            ThumbnailUrl = thumbnailUrl;
        }
    };
   
}
