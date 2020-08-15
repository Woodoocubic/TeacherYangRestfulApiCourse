using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.APi.Services
{
    public class PropertyMappingService: IPropertyMappingService
    {
        //reflect relation dict
        //one prop to multi props
        //company relation dict
        //stringComparer.OrdinalIgnoreCse to key lower case capital case same 
        private readonly Dictionary<string, PropertyMappingValue> _companyPropertyMapping 
        = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>{"Id"}) },
            {"Name", new PropertyMappingValue(new List<string>{"Name"}) },
            {"Country", new PropertyMappingValue(new List<string>{"Country"}) },
            {"Industry",new PropertyMappingValue(new List<string>{"Industry"}) },
            {"Product", new PropertyMappingValue(new List<string>{"Product"}) },
            {"Introduction", new PropertyMappingValue(new List<string>{"Introduction"}) }
        };

        //Employee reflection dict
        private readonly Dictionary<string, PropertyMappingValue> _employeePropertyMapping
        = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>{"Id"}) },
            {"CompanyId", new PropertyMappingValue(new List<string>{"CompanyId"}) },
            {"EmployeeNo", new PropertyMappingValue(new List<string>{"EmployeeNo"}) },
            {"Name", new PropertyMappingValue(new List<string>{"FirstName", "LastName"})},
            {"GenderDisplay", new PropertyMappingValue(new List<string>{"Gender"}) },
            {"Age", new PropertyMappingValue(new List<string>{"DateOfBirth"}, true)}
        };
        //reflection relation list
        private IList<IPropertyMapping>
    }
}
