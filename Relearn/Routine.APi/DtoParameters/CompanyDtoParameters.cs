using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.APi.Services;

namespace Routine.APi.DtoParameters
{
    public class CompanyDtoParameters
    {
        private const int MaxPageSize = 20;
        public string CompanyName { get; set; }
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1; // by default is 1
        public string OrderBy { get; set; } = "Name";
        public string Fields { get; set; }
        private int _pageSize = 5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize ? MaxPageSize : value);
        }
    }
}
