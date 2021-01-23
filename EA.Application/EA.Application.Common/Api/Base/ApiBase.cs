using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EA.Application.Common.Enttiy;
using EA.Application.Common.Pagging;
using EA.Application.Common.Pagging.infrastructure;
using EA.Application.Common.Repository;
using EA.Application.Common.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EA.Application.Common.Api.Base
{
    public class ApiBase<T, TDto, TController> : ControllerBase where T : class,IEntity where TDto : class where TController : ControllerBase  
    {
        #region Variables

        /// <summary>
        /// Sınıf içerisinde kullanacağımız değişkenlerimizi tanımlıyoruz. Unitofwork ve generic repository private yani sadece bu sınıf için erişilebilirken logger tüm sınıflardan erişilebilsin diye public bırakıyoruz.
        /// Bu sınıfı kalıtım alan tüm sınırlarımızda loglama işlemi yapacağımız için logger public..
        /// </summary>
        public readonly IUnitofWork _uow;
        public readonly IServiceProvider _service;
        public readonly ILogger<TController> _logger;
        private readonly IRepository<T> _repository;
        public readonly IMapper _mapper;

        #endregion Variables

       

     
        public ApiBase(IServiceProvider service,IMapper mapper)
        {
            //Get Service diyerek dependencylerimizi ınject ediyoruz
            _logger = service.GetService<ILogger<TController>>();
            _uow = service.GetService<IUnitofWork>();
            _repository = _uow.GetDefaultRepo<T>();
            _mapper = mapper;
            _service = service;
        }

        

        

        [HttpGet("Find")]
        public virtual ApiResult<TDto> Find(Guid id)
        {
            try
            {
                _logger.LogInformation($"Find record from the {typeof(T)} table with id:{id}");
                return new ApiResult<TDto>
                {
                    Message = "Success",
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<T, TDto>(_repository.Find(id))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Find record error from the {typeof(T)} table with id:{id} error:{ex}");
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet("FindAsync")]
        public virtual async Task<ApiResult<TDto>> FindAsync(Guid id)
        {
            return Find(id);
        }

       
        [HttpGet("GetAll")]
        public virtual ApiResult<List<TDto>> GetAll()
        {
            try
            {
                var entities = _repository.GetAll().ToList();
                _logger.LogInformation($"Getall records from the {typeof(T)} table");
                return new ApiResult<List<TDto>>
                {
                    Message = "Success",
                    StatusCode = StatusCodes.Status200OK,
                    Data = entities.Select(x => _mapper.Map<TDto>(x)).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Getall records error from the {typeof(T)} table");
                return new ApiResult<List<TDto>>
                {
                    Message = $"Error:{ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null
                };
            }
        }

      
        [HttpGet("GetQueryable")]
        public virtual IQueryable<T> GetQueryable()
        {
            try
            {
                _logger.LogInformation($"GetQueryable from the {typeof(T)} table");
                return _repository.GetMany();
            }
            catch (Exception Ex)
            {
                _logger.LogError($"GetQueryable error from the {typeof(T)} table. Error:{Ex}");
                return null;
            }
        }

        [HttpPost("GetAllWithPaging")]
        public virtual ApiResult GetAllWithPaging(PagingParams pagingParams)
        {
            try
            {
                _logger.LogInformation($"GetAllWithPaging from the {typeof(T)} table");
                var pagingLinks = _service.GetService<IPagingLinks<T>>();

                var model = new PagedList<T>(
                    GetQueryable(), pagingParams.PageNumber, pagingParams.PageSize);

                Response.Headers.Add("X-Pagination", model.GetHeader().ToJson());

                var outputModel = new OutputModel<T>
                {
                    Paging = model.GetHeader(),
                    Links = pagingLinks.GetLinks(model),
                    Items = model.List.Select(m => m).ToList(),
                };

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = outputModel
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllWithPaging error from the {typeof(T)} table. Data: {String.Join(',', pagingParams.GetType().GetProperties().Select(x => $" - {x.Name} : {x.GetValue(pagingParams)} - ").ToList())} exception:{ex}");
                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        [HttpDelete("DeleteById")]
        public virtual ApiResult<string> DeleteById(Guid id)
        {
            try
            {
                _repository.Delete(id);
                _logger.LogInformation($"Record deleted from the {typeof(T)} table with id:{id}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete error for the:{typeof(T)} table with id:{id} result:Error - {ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        [HttpDelete("DeleteByIdAsync")]
        public virtual async Task<ApiResult<string>> DeleteByIdAsync(Guid id)
        {
            return DeleteById(id);
        }
        [HttpDelete("Delete")]
        public virtual ApiResult<string> Delete([FromBody] TDto item)
        {
            var resolvedItem = String.Join(',', item.GetType().GetProperties().Select(x => $" - {x.Name} : {x.GetValue(item)} - ").ToList());
            try
            {
                _repository.Delete(_mapper.Map<T>(item));
                _logger.LogInformation($"Record deleted from the {typeof(T)} table. Data:{resolvedItem}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete record error from the {typeof(T)} table. Data: {resolvedItem} exception:{ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        [HttpDelete("DeleteAsync")]
        public virtual async Task<ApiResult<string>> DeleteAsync([FromBody] TDto item)
        {
            return Delete(item);
        }

        [HttpPost("Add")]
        public virtual ApiResult<string> Add([FromBody] TDto item)
        {
            var resolvedItem = String.Join(',', item.GetType().GetProperties().Select(x => $" - {x.Name} : {x.GetValue(item)} - ").ToList());
            try
            {
                var sd = _mapper.Map<T>(item);
                _repository.Insert(sd);
                _logger.LogInformation($"Add record to the {typeof(T)} table. Data:{resolvedItem}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Add record error to the {typeof(T)} table. Data: {resolvedItem} exception:{ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

       
        [HttpPost("AddAsync")]
        public virtual async Task<ApiResult<string>> AddAsync([FromBody] TDto item)
        {
            return Add(item);
        }

     
        [HttpPut("Update")]
        public virtual ApiResult<string> Update([FromBody] TDto item)
        {
            var resolvedItem = String.Join(',', item.GetType().GetProperties().Select(x => $" - {x.Name} : {x.GetValue(item)} - ").ToList());
            try
            {
                _repository.Update(_mapper.Map<T>(item));
                _logger.LogInformation($"Update record to the {typeof(T)} table. Data:{resolvedItem}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update record error to the {typeof(T)} table. Data: {resolvedItem} exception:{ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPut("UpdateAsync")]
        public virtual async Task<ApiResult<string>> UpdateAsync([FromBody] TDto item)
        {
            return Update(item);
        }

        private void Save()
        {
            _uow.SaveChanges(true);
        }
    }
}