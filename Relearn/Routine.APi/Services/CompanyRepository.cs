using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Routine.APi.Data;
using Routine.APi.DtoParameters;
using Routine.APi.Entities;
using Routine.APi.Helpers;
using Routine.APi.Models;

namespace Routine.APi.Services
{
    public class CompanyRepository: ICompanyRepository
    {
        private readonly RoutineDbContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        public CompanyRepository(RoutineDbContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ??
                                      throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            company.Id = Guid.NewGuid();
            if (company.Employees !=null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }

            _context.Companies.Add(company);
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.CompanyId = companyId;
            _context.Employees.Add(employee);
        }

        public async Task<bool> CompanyExistAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _context.Companies.AnyAsync(x => x.Id == companyId);
        }

        public void DeleteCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            _context.Remove(company);
        }

        public void DeleteEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
        }

        public async Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var queryExpression = _context.Companies as IQueryable<Company>;
            if (!string.IsNullOrWhiteSpace(parameters.CompanyName))
            {
                parameters.CompanyName = parameters.CompanyName.Trim();
                queryExpression = queryExpression.Where(x => x.Name 
                                                             == parameters.CompanyName);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                parameters.SearchTerm = parameters.CompanyName.Trim();
                queryExpression = queryExpression.Where(x => x.Name.Contains(parameters.SearchTerm)
                                                             || x.Introduction.Contains(parameters.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                var mappingDictionary = _propertyMappingService.GetPropertyMapping<CompanyDto, Company>();
                queryExpression = queryExpression.ApplySort(parameters.OrderBy, mappingDictionary);
            }
            //return await queryExpression.Skip((parameters.PageNumber - 1) * parameters.PageSize)
            //    .Take(parameters.PageSize)
            //    .ToListAsync();


            return await PagedList<Company>.CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }

            return await _context.Companies
                .Where(x => companyIds.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            return await _context.Employees
                .Where(x => x.Id == employeeId && x.CompanyId == companyId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,
            EmployeeDtoParameters parameters)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            var queryExpression = _context.Employees.Where(x => x.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(parameters.Gender))
            {
                parameters.Gender = parameters.Gender.Trim();
                var gender = Enum.Parse<Gender>(parameters.Gender);
                queryExpression = queryExpression.Where(x => x.Gender == gender);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                parameters.Q = parameters.Q.Trim();
                queryExpression = queryExpression.Where(x => x.EmployeeNo.Contains(parameters.Q)
                                                             || x.FirstName.Contains(parameters.Q)
                                                             || x.LastName.Contains(parameters.Q));
            }

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                var mappingDictionary = _propertyMappingService.GetPropertyMapping<EmployeeDto, Employee>();
                queryExpression = queryExpression.ApplySort(parameters.OrderBy, mappingDictionary);
            }

            return await queryExpression.ToListAsync();
        }

        public void UpdateCompany(Company company)
        {
            //no need to show the code 
            //_context.Entry(company).State = EntityState.Modified;
        }

        public void UpdateEmployee(Employee employee)
        {
            // the same, no need to show the code
            //_context.Entry(employee).State = EntityState.Modified;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >=0;
        }
    }
}
