using AutoMapper;
using EntGlobus.Models;
using EntGlobus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.Helpers
{
    public class ProfMapper : Profile
    {
        public ProfMapper()
        {
            CreateMap<RegisterViewModel, AppUsern>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.TelNum)); ;
        }
    }
}
