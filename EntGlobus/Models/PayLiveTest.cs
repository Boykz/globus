using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models
{
    public class PayLiveTest
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUsern User { get; set; }

        [ForeignKey("LiveLesson")]
        public int LiveLessonId { get; set; }
        public LiveLesson LiveLesson { get; set; }

        public int Price { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public PayLiveTestType PayLiveTestType { get; set; }
    }

    public enum PayLiveTestType
    {
        Paybox = 1,
        Qiwi = 2,
        Default = 3
    }
}
