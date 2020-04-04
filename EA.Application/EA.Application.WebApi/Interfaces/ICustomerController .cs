using EA.Application.Common.Api.Base;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;

namespace EA.Application.WebApi.Interfaces
{
    public interface ICustomerController : IApiController<CustomerDto, Customer>
    {
    }
}