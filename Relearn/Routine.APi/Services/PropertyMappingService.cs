using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.APi.Entities;
using Routine.APi.Models;

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
        //reflection relation list AND empty node error for new List..
        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<CompanyDto, Company>(_companyPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping
                = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            var propertyMapping = matchingMapping.ToList();
            if (propertyMapping.Count == 1)
            {
                return propertyMapping.First().MappingDictionary;
            }
            throw new Exception($"cannot find the 1 to 1 relation: {typeof(TSource)}, {typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping
                = GetPropertyMapping<TSource, TDestination>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldAfterSplit = fields.Split(",");
            foreach (var field in fieldAfterSplit)
            {
                var trimmedField = field.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }


    }
}
