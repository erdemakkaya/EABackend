using System;
using System.Linq;
using System.Net;
using AutoMapper;
using EA.Application.Common.Api;
using EA.Application.Common.Api.Base;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;
using EA.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EA.Application.WebApi.Controllers
{

  
    [ApiController]
    [Route("lang")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LanguageController : ApiBase<Language, LanguageDto, LanguageController>, ILanguageController
    {
        #region Constructor

        public LanguageController(IServiceProvider service,IMapper mapper) : base(service, mapper)
        {
        }

        #endregion Constructor

        
        public override ApiResult<string> Add([FromBody] LanguageDto item)
        {
            //çift kayıt kontrolü yapan metot false dönerse işlemler yapılacak.
            if (!CheckDublicateLanguage(item.Culture))
            {
                //base deki ekleme işlemi yapılıyor
                var result = base.Add(item);
                //sıralı işlemler olmadığı için save changes diyerek veritabanında değişiklikler yapılıyor
                _uow.SaveChanges(false);
                //geriye base de bulunan add metodundan dönen değer döndürülüyor
                return result;
            }
            else
            {
                //çift kayıt varsa ekleme işlemi yapılmıyor ve kullanıcıya cevap dönülüyor
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status406NotAcceptable,
                    Message = "This culture is exist!",
                    Data = null
                };
            }
        }

       
        [HttpGet("CheckDublicateLanguage")]
        public bool CheckDublicateLanguage(string culture)
        {
            var result = GetQueryable().Where(x => x.Culture == culture).ToList().Count;
            return result > 0;
        }
       
        public override ApiResult<string> Update([FromBody] LanguageDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] LanguageDto item)
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
