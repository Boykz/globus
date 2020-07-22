using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class PodLiveLesson
    {
        public Guid Id { get; set; }

        public string UrlVideo { get; set; }
        public string UrlPhoto { get; set; }
        public string Nuska { get; set; }

        public string Title { get; set; }
        public bool? Status { get; set; }
        public TypeLiveLesson? TypeVideo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DurationTime { get; set; }


        [ForeignKey("LiveLesson")]
        public int LiveLessonId { get; set; }
        public LiveLesson LiveLesson { get; set; }
    }


    public enum TypeLiveLesson
    {
        Video = 1,
        Youtube = 2,
        Live = 3
    }
}
