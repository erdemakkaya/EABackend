

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EA.Application.Common.Api;
using EA.Application.Common.Api.Base;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;
using EA.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EA.Application.WebApi.Controllers
{
    [ApiController]
    [Route("Role")]
    public class RoleController: ApiBase<ApplicationRole, ApplicationRoleDto, RoleController>, IRoleController
    {
        #region Variables
        /// <summary>
        /// Identity alt yapısı içerisinde bulunan UserManager sınıfı ile kullanıcı işlemlerini yapacağız.
        /// </summary>
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IMapper _mapper;
        #endregion

        #region Contructor
        public RoleController(IServiceProvider service, IMapper mapper) : base(service, mapper)
        {
            _roleManager = service.GetService<RoleManager<ApplicationRole>>();
            _mapper = mapper;
        }
        #endregion

        public override async Task<ApiResult<ApplicationRoleDto>> AddAsync(ApplicationRoleDto item)
        {
            
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var role = _mapper.Map<ApplicationRole>(item);
                role.CreateDate = DateTime.UtcNow;
                identityResult = await _roleManager.CreateAsync(role).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<ApplicationRoleDto>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Role Added Successfully." : sbErrors.ToString()),
                    Data = identityResult.Succeeded ? _mapper.Map<ApplicationRoleDto>(role):null
            };

                _logger.LogInformation($"Add Role with roleId:{role.Id } role Name:{item.Name} role Desc:{item.Description} result:{result.Message}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Role Add : name:{item.Name} desc:{item.Description} result:Error - {ex.Message}");

                return new ApiResult<ApplicationRoleDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }
    }
}
 