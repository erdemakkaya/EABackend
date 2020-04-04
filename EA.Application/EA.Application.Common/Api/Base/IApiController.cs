﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EA.Application.Common.Pagging;

namespace EA.Application.Common.Api.Base
{
    public interface IApiController<TDto, T>
    {
        #region GetMethods
        ApiResult<List<TDto>> GetAll();
        ApiResult<TDto> Find(Guid id);
        ApiResult GetAllWithPaging(PagingParams pagingParams);
        #endregion

        #region PostMethods
        ApiResult<TDto> Add(TDto item);
        Task<ApiResult<TDto>> AddAsync(TDto item);
        ApiResult<TDto> Update(TDto item);
        Task<ApiResult<TDto>> UpdateAsync(TDto item);
        ApiResult<string> Delete(TDto item);
        Task<ApiResult<string>> DeleteAsync(TDto item);
        ApiResult<string> DeleteById(Guid Id);
        Task<ApiResult<string>> DeleteByIdAsync(Guid id);
        #endregion
    }
}
