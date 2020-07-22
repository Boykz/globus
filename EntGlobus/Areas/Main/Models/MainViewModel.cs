using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Areas.Main.Models
{
    public class MainViewModel
    {
        public IEnumerable<MainSlider> MainSliders { get; set; }
        public IEnumerable<MainPost> MainPosts { get; set; }
    }

    public class MainSlider
    {
        public string PhotoUrl { get; set; }

    }

    public class MainPost
    {
        public string PhotoUrl { get; set; }
        public string Title { get; set; }
        public string Hashtag { get; set; }
        public int ViewCount { get; set; }
        public DateTime Date { get; set; }
    }
}
