using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EA.Application.Common.Api.Base;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;

namespace EA.Application.WebApi.Interfaces
{
    public interface IOrganizationController : IApiController<OrganizationDto, Organization>
    {
    }
}
