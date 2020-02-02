using EntGlobus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ApiServece
{
    public class PodLiveWithHistoryModel
    {
        public ModelPodLiveWithHistoryModel Live { get; set; }
        public List<ModelPodLiveWithHistoryModel> History { get; set; }
    }

    public class ModelPodLiveWithHistoryModel
    {
        public Guid Id { get; set; }
        public string UrlVideo { get; set; }
        public string UrlPhoto { get; set; }

        public string Nuska { get; set; }
        public bool? Status { get; set; }
        public TypeLiveLesson? TypeVideo { get; set; }
        public DateTime? StartDate { get; set; }

        public int LiveLessonId { get; set; }
    }
}
