using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using EA.Application.Common.Api;
using EA.Application.Common.Api.Base;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EA.Application.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("Organization")]
    public class OrganizationController : ApiBase<Organization, OrganizationDto, OrganizationController>
    {
        public OrganizationController(IServiceProvider service,IMapper mapper) : base(service, mapper)
        {
        }

        /// <summary>
        /// Unit of work 'ün çalışması ve kayıtların veritabanına ulaşması için Add,Update,Delete metotlarını override ediyoruz
        /// Bu bir zorunluluk değil eğer unitofwork'ü ApiBase içerisinde savechanges yapacak şekilde kullanırsanız bu metotları override etmek zorunda kalmazsınız
        /// Ancak o zaman unit of work mantığı boş yere bu sisteme eklenmiş gibi olacak 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override ApiResult<string> Add([FromBody] OrganizationDto item)
        {
            var result = base.Add(item);
            _uow.SaveChanges(false);
            return result;
        }

        public override ApiResult<string> Update([FromBody] OrganizationDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] OrganizationDto item)
        {
            var result = base.Delete(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> DeleteById(Guid id)
        {
            var result = base.DeleteById(id);
            _uow.SaveChanges(true);
            return result;
        }
    }
}