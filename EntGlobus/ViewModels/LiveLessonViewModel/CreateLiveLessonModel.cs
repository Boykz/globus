using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels.LiveLessonViewModel
{
    public class CreateLiveLessonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Information { get; set; }

        public bool? OpenClose { get; set; }

        public string Photo { get; set; }
        public string Icon { get; set; }
    }
}
