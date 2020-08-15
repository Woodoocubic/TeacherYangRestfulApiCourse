using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.APi.Entities;
using Routine.APi.Models;
using Routine.APi.Services;

namespace Routine.APi.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController: ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService 

        public EmployeesController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,
            [FromQuery(Name = "gender")] string genderDisplay, [FromQuery] string q)
        {
            if (await _companyRepository.CompanyExistAsync(companyId))
            {
                var employees = await _companyRepository.GetEmployeesAsync(companyId, genderDisplay, q);
                var employDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
                return Ok(employDtos);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{employeeId}", Name = nameof(GetEmployeeForCompany))]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            if (await _companyRepository.CompanyExistAsync(companyId))
            {
                var employee = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
                if (employee == null)
                {
                    return NotFound();
                }

                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return Ok(employeeDto);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany([FromRoute] Guid companyId, 
            [FromBody] EmployeeAddDto employee)
        {
            if (! await _companyRepository.CompanyExistAsync(companyId))
            {
                return NotFound();
            }

            var entity = _mapper.Map<Employee>(employee);
            _companyRepository.AddEmployee(companyId, entity);
            await _companyRepository.SaveAsync();
            var returnDto = _mapper.Map<EmployeeDto>(entity);

            return CreatedAtRoute(nameof(GetEmployeeForCompany),
                new {companyId = returnDto.CompanyId, employeeId = returnDto.Id},
                returnDto);
        }
    }
}
