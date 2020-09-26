using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.APi.DtoParameters;
using Routine.APi.Entities;
using Routine.APi.Helpers;
using Routine.APi.Models;
using Routine.APi.Services;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Routine.APi.Controllers
{
    /*
     * HTTP请求：
     * GET      - 查询
     * POST     - 创建/添加
     * PATCH    - 局部修改/更新
     * PUT      - 如果存在就替换，不存在则创建
     * DELETE   - 移除/删除
     */

    /*
     * 返回状态码：
     * 
     * 2xx 成功
     * 200 - OK 请求成功
     * 201 - Created 请求成功并创建了资源
     * 204 - No Content 请求成功，但是不应该返回任何东西，例如删除操作
     * 
     * 4xx 客户端错误
     * 400 - Bad Request API消费者发送到服务器的请求是有错误的
     * 401 - Unauthorized 没有提供授权信息或者提供的授权信息不正确
     * 403 - Forbidden 身份认证已经成功，但是已认证的用户却无法访问请求的资源
     * 404 - Not Found 请求的资源不存在
     * 405 - Method Not Allowed 尝试发送请求到资源的时候，使用了不被支持的HTTP方法
     * 406 - Not Acceptable API消费者请求的表述格式并不被Web API所支持，并且API不
     *       会提供默认的表述格式
     * 409 - Conflict 请求与服务器当前状态冲突（通常指更新资源时发生的冲突）
     * 415 - Unsupported Media Type 与406正好相反，有一些请求必须带着数据发往服务器，
     *       这些数据都属于特定的媒体格式，如果API不支持该媒体类型格式，415就会被返回
     * 422 - Unprocessable Entity 它是HTTP拓展协议的一部分，它说明服务器已经懂得了
     *       Content Type，实体的语法也没有问题，但是服务器仍然无法处理这个实体数据 
     * 
     * 5xx 服务器错误
     * 500 - Internal Server Error 服务器出现错误
     */
    [ApiController]
    [Route("api/companies")]
    //[Route("api/[controller]")]
    public class CompaniesController: ControllerBase
    {
        /*[ApiController]属性并不是强制要求的，但是它会使开发体验更好
         * 它会启用以下行为：
         * 1.要求使用属性路由（Attribute Routing）
         * 2.自动HTTP 400响应
         * 3.推断参数的绑定源
         * 4.Multipart/form-data请求推断
         * 5.错误状态代码的问题详细信息
         */
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;
        public CompaniesController(ICompanyRepository companyRepository, 
            IMapper mapper, 
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _companyRepository = companyRepository
                                 ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService
                                      ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService
                                      ?? throw new ArgumentNullException(nameof(propertyCheckerService));
        }

        [HttpGet(Name = nameof(GetCompanies))]
        [HttpHead]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyDtoParameters parameters)
        //public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            if (!_propertyMappingService.ValidMappingExistsFor<CompanyDto, Company>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<CompanyDto>(parameters.Fields))
            {
                return BadRequest();
            }

            var companies = 
                await _companyRepository.GetCompaniesAsync(parameters);

            var previousPageLink = companies.HasPrevious
                ? CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = companies.HasNext
                ? CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = companies.TotalCount,
                pageSize = companies.PageSize,
                currentPage = companies.CurrentPage,
                totalPage = companies.TotalPages,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("X-Pagination", JsonSerializer
                .Serialize(
                paginationMetadata, 
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));
            
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            
            return Ok(companyDtos.ShapeData(parameters.Fields));
        }
         
        [HttpGet("{companyId}", Name = nameof(GetCompany))] //or [Route("{companyId}")] api/Companies/{companyId}
        public async Task<IActionResult> GetCompany(Guid companyId, string fields)
        {
            if (!_propertyCheckerService.TypeHasProperties<CompanyDto>(fields))
            {
                return BadRequest();
            }

            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CompanyDto>(company).shapeData(fields));
        }
        
        //Task<IActionResult> = Task<ActionResult<CompanyDto>>
        [HttpPost]
        public async Task<IActionResult> CreateCompany(
            [FromBody] CompanyAddDto company)
        {
            // old version should use:
            // ApiController attribute, no need for check
            //if (company == null)
            //{ 
            //    return BadRequest();
            //}
            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();
            var retrunDto = _mapper.Map<CompanyDto>(entity);
            //return 201
            //using CreateAtRoute  return the header add the Location
            return CreatedAtRoute(nameof(GetCompany), new {companyId = retrunDto.Id}, 
                retrunDto); 
        }

        [HttpDelete("{companyId}")]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var companyEntity = await _companyRepository.GetCompanyAsync(companyId);
            if (companyEntity == null)
            {
                return NotFound();
            }

            await _companyRepository.GetEmployeesAsync(companyId, new EmployeeDtoParameters());
            
            _companyRepository.DeleteCompany(companyEntity);
            await _companyRepository.SaveAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "DELETE, GET, PATCH, PUT, OPTIONS");
            return Ok();
        }

        private string CreateCompaniesResourceUri(CompanyDtoParameters parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(
                        nameof(GetCompanies),
                        new
                        {
                             fields = parameters.Fields,
                             orderBy = parameters.OrderBy,
                             pageNumber = parameters.PageNumber-1,
                             pageSize = parameters.PageSize,
                             companyName = parameters.CompanyName,
                             searchTerm = parameters.SearchTerm
                        });
                case ResourceUriType.NextPage:
                    return Url.Link(
                        nameof(GetCompanies),
                        new
                        {
                            fields = parameters.Fields,
                            pageNumber = parameters.PageNumber+1,
                            pageSize = parameters.PageSize,
                            companyName = parameters.CompanyName,
                            searchTerm = parameters.SearchTerm
                        });
                default:
                    return Url.Link(nameof(GetCompanies),
                        new
                        {
                            fields = parameters.Fields,
                            pageNumber = parameters.PageNumber,
                            pageSize = parameters.PageSize,
                            companyName = parameters.CompanyName,
                            searchTerm = parameters.SearchTerm
                        });
            }
        }
    }
}
