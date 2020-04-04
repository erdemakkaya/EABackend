using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EA.Application.Common.Api;
using EA.Application.WebApi.VM;
using Microsoft.AspNetCore.Mvc;

namespace EA.Application.WebApi.Interfaces
{
    public interface ITokenController
    {
        Task<ApiResult> LoginAsync([FromBody] LoginModel loginModel);
    }
}