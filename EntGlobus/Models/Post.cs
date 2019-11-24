using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string subject { get; set; }
        public string hashtag { get; set; }
        public string text { get; set; }
        public string pathimg { get; set; }
        public string pathvideo { get; set; }
        public DateTime date { get; set; }
        public int watch { get; set; }
    }
}
