using EntGlobus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels.LiveLessonViewModel
{
    public class PodLiveLessonModel
    {
        public Guid Id { get; set; }
        public string UrlVideo { get; set; }
        public string UrlPhoto { get; set; }
        public bool? Status { get; set; }
        public TypeLiveLesson? TypeVideo { get; set; }
        public DateTime? StartDate { get; set; }

        public int LiveLessonId { get; set; }
        public LiveLesson LiveLesson { get; set; }
    }
}
