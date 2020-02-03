using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCodeCamp.Data
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile()
        {
            CreateMap<Camp, CampModel>().ForMember(c=>c.VenueName, opt=> opt.MapFrom(m =>m.Location.VenueName));
        }
    }
}