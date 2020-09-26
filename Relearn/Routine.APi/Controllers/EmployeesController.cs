using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.APi.Entities;
using Routine.APi.Models;
using Routine.APi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Routine.APi.DtoParameters;

namespace Routine.APi.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController: ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;

        public EmployeesController(ICompanyRepository companyRepository, 
            IMapper mapper, 
            IPropertyMappingService propertyMappingService)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService =
                propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(
            Guid companyId,
            [FromQuery]EmployeeDtoParameters parameters )
        {
            //validate iquery string 's orderby 
            if (! _propertyMappingService.ValidMappingExistsFor<CompanyDto, Company>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (await _companyRepository.CompanyExistAsync(companyId))
            {
                var employees = await _companyRepository.GetEmployeesAsync(companyId, parameters);
                var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
                return Ok(employeeDtos);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{employeeId}", Name = nameof(GetEmployeeForCompany))]
        public async Task<ActionResult<EmployeeDto>> 
            GetEmployeeForCompany(Guid companyId, 
                Guid employeeId)
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

        [HttpPut("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeForCompany(
            Guid companyId,
            Guid employeeId,
            EmployeeUpdateDto employee)
        {
            if (! await  _companyRepository.CompanyExistAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employeeEntity == null)
            {
                // not allow client side for Guid
                //return NotFound();

                //allow client side for Guid
                var employeeToAddEntity = _mapper.Map<Employee>(employee);
                employeeToAddEntity.Id = employeeId;
                _companyRepository.AddEmployee(companyId, employeeToAddEntity);
                await _companyRepository.SaveAsync();
                var returnDto = _mapper.Map<EmployeeDto>(employeeToAddEntity);
                return CreatedAtRoute(nameof(GetEmployeeForCompany),
                    new {companyId = companyId, employeeId = employeeId},
                    returnDto);
            }
            // updateDto to entity
            _mapper.Map(employee, employeeEntity);
            _companyRepository.UpdateEmployee(employeeEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(
            Guid companyId,
            Guid employeeId,
            JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            if ( ! await  _companyRepository.CompanyExistAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employeeEntity == null)
            {
                //not allow client side Guid;
                //return NotFound();

                //allow client side Guid 
                var employeeDto = new EmployeeUpdateDto();
                //send to ModelState AND Validate
                patchDocument.ApplyTo(employeeDto, ModelState);
                if (!TryValidateModel(employeeDto))
                {
                    return ValidationProblem(ModelState);
                }

                var employeeToAdd = _mapper.Map<Employee>(employeeDto);
                employeeToAdd.Id = employeeId;
                _companyRepository.AddEmployee(companyId, employeeToAdd);
                await _companyRepository.SaveAsync();
                var dtoToReturn = _mapper.Map<Employee>(employeeToAdd);

                return CreatedAtRoute(nameof(GetEmployeeForCompany),
                    new {companyId = companyId, employeeId = employeeId},
                    dtoToReturn);
            }

            var dtoToPatch = _mapper.Map<EmployeeUpdateDto>(employeeEntity);
            
            // need validation
            patchDocument.ApplyTo(dtoToPatch);
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(dtoToPatch, employeeEntity);
            _companyRepository.UpdateEmployee(employeeEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employeeEntity == null)
            {
                return NotFound();
            }

            _companyRepository.DeleteEmployee(employeeEntity);

            await _companyRepository.SaveAsync();
            return NoContent();
        }

        public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult) options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }




}
