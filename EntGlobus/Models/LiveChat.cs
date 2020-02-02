using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class LiveChat
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime? MessDate { get; set; }

        [ForeignKey("PodLiveLesson")]
        public Guid PodLiveLessonId { get; set; }
        public PodLiveLesson PodLiveLesson { get; set; }
    }
}
