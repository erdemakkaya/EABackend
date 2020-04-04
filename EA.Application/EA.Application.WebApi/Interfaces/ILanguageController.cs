using EA.Application.Common.Api.Base;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EA.Application.WebApi.Interfaces
{
    public interface ILanguageController : IApiController<LanguageDto, Language>
    {
      bool CheckDublicateLanguage(string culture);
    }
}