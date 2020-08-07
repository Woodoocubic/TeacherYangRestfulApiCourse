using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.APi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using Routine.APi.Models;

namespace Routine.APi.Profiles
{
    public class EmployeeProfile: Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest =>dest.Name, opt => opt.MapFrom(src =>$"{src.FirstName} {src.LastName}"))
                .ForMember(dest =>dest.GenderDisplay, opt=>opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>GetAge(src.DateOfBirth)));
        }

        private int GetAge(DateTime dateOfBirth)
        {
            DateTime dateOfNow = DateTime.Now;
            if (dateOfBirth > dateOfNow)
            {
                throw new ArgumentOutOfRangeException(nameof(dateOfBirth));
            }

            int age = dateOfNow.Year - dateOfBirth.Year;
            if (dateOfNow.Month < dateOfBirth.Month)
            {
                age--;
            }
            else if (dateOfNow.Month == dateOfBirth.Month && dateOfNow.Day < dateOfBirth.Day)
            {
                age--;
            }

            return age;
        }
    }
}
