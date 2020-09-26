using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.APi.Services
{
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
