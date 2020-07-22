using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Models.AnaliticaDbFolder
{
    public class ForUserCallAnalitics
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Number { get; set; }

        public bool Call { get; set; }
        public string Comment { get; set; }
        public CallType Result { get; set; }
    }

    public enum CallType
    {
        Бізде_Оқып_жатыр = 1,
        Баска_курста_окиды=2,
        Тагы_хабарласып_коремын=3,

    }
}
