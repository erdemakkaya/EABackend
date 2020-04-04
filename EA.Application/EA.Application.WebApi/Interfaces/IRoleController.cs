using EA.Application.Common.Api.Base;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;

namespace EA.Application.WebApi.Interfaces
{
    interface IRoleController:IApiController<ApplicationRoleDto, ApplicationRole>
    {
    }
}
