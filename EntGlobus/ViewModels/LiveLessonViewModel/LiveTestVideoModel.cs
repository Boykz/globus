using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels.LiveLessonViewModel
{
    public class LiveTestVideoModel
    {
        public Models.PodLiveLesson PodLiveLesson { get; set; }
        public List<Models.PodLiveLesson> HistoryPodLiveLesson { get; set; }
    }
}
