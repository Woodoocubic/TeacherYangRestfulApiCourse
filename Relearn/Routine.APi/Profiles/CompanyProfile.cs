using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Routine.APi.Entities;
using Routine.APi.Models;

namespace Routine.APi.Profiles
{
    public class CompanyProfile: Profile
    {

        public CompanyProfile()
        {
            // origin: Company -> CompanyDto
            // prop name consistent
            // auto ignore reference
            CreateMap<Company, CompanyDto>();
            // manually
            //CreateMap<Company, CompanyDto>()
            //    .ForMember(dest => dest.Name,
            //        opt => opt.MapFrom(
            //            src => src.Name));
        }
    }
}
