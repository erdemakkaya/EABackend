using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EA.Application.Common.Api;
using EA.Application.Common.Api.Base;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EA.Application.WebApi.Controllers
{
    /// <summary>
    /// Müşteri tablosu işlemleri için kullanılacak sınıf.
    /// </summary>
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("Customer")]
    public class CustomerController : ApiBase<Customer, CustomerDto, CustomerController>
    {
        
        public CustomerController(IServiceProvider service,IMapper mapper) : base(service,mapper)
        {
        }

        /// <summary>
        /// Include işlemi olduğu için Find metodunu override ediyoruz.
        /// </summary>
        /// <param name="id">İstenen kaydın Id bilgisi</param>
        /// <returns></returns>
        public override ApiResult<CustomerDto> Find(Guid id)
        {
            return new ApiResult<CustomerDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "User founded",
                Data = _mapper.Map<Customer, CustomerDto>(GetQueryable().Include(x => x.Organization).FirstOrDefault(x => x.Id == id))
            };
        }

        /// <summary>
        /// Include işlemi olduğu için getall metodunu override ediyoruz.
        /// </summary>
        /// <returns></returns>
        public override ApiResult<List<CustomerDto>> GetAll()
        {
            return new ApiResult<List<CustomerDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "User founded",
                Data = GetQueryable().Include(x => x.Organization).ToList().Select(x => _mapper.Map<CustomerDto>(x)).ToList()
            };
        }

        /// <summary>
        /// Unit of work 'ün çalışması ve kayıtların veritabanına ulaşması için Add,Update,Delete metotlarını override ediyoruz
        /// Bu bir zorunluluk değil eğer unitofwork'ü ApiBase içerisinde savechanges yapacak şekilde kullanırsanız bu metotları override etmek zorunda kalmazsınız
        /// Ancak o zaman unit of work mantığı boş yere bu sisteme eklenmiş gibi olacak 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override ApiResult<string> Add([FromBody] CustomerDto item)
        {
            var result = base.Add(item);
            _uow.SaveChanges(false);
            return result;
        }

        public override ApiResult<string> Update([FromBody] CustomerDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] CustomerDto item)
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