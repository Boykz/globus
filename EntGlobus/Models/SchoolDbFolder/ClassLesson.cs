using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.SchoolDbFolder
{
    public class ClassLesson
    {
        public int Id { get; set; }

        public string LessonName { get; set; }

        public string Url { get; set; }
        public int SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; }
    }
}
