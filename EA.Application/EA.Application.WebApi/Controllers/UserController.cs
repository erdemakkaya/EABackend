using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EA.Application.Common.Api;
using EA.Application.Common.Api.Base;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;
using EA.Application.WebApi.Interfaces;
using EA.Application.WebApi.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


namespace EA.Application.WebApi.Controllers
{
    /// <summary>
    /// Identity Alt yapısını kullanarak kullanıcı işlemleri yapacağımız sınıf
    /// </summary>
    [ApiController]
    [Route("User")]
    //[Authorize(Roles = "Admin")]
    public class UserController : ApiBase<ApplicationUser, ApplicationUserDto, LanguageController>, IUserController
    {
      
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMapper _mapper;

        public UserController(IServiceProvider service,IMapper mapper) : base(service,mapper)
        {
            _userManager = service.GetService<UserManager<ApplicationUser>>();
            _mapper = mapper;
        }

        public override async Task<ApiResult<string>> AddAsync(ApplicationUserDto item)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = _mapper.Map<ApplicationUser>(item);
                user.CreateDate = DateTime.UtcNow;
                identityResult = await _userManager.CreateAsync(user, password: user.PasswordHash).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<string>
                {
                    //ekleme işlemi başarılı ise http200 ile başarılı değil ise gelen verilerde bir sorun olduğunuz belirtmek için 406 kabul edilemez yanıtını vereceğiz. Bu yanıtların tipleri size kalmış ben bu ikisini kullanmayı tercih ettim.
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    //Mesaj kısmında başarılı ise bir mesaj gönderiyoruz değil ise hataları içeren stringimizi gönderiyoruz.
                    Message = (identityResult.Succeeded ? "User Added Successfully." : sbErrors.ToString()),
                    Data =  null
                };

                //logger ile sonucu logluyoruz.
                _logger.LogInformation($"Add User with userid:{user.Id } mail:{item.Email} username:{item.UserName} result:{result.Message}");
                //hazırladığımız result değişkenimizi dönüyoruz.
                return result;
            }
            catch (Exception ex)
            {
                //Hata çıkarsa logluyoruz.
                _logger.LogError($"User Add : mail:{item.Email} username:{item.UserName} result:Error - {ex.Message}");

                //geriye hatayı içeren bir dönüş yapıyoruz.
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        
        public override ApiResult<string> Add([FromBody] ApplicationUserDto item)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to Add a user to database",
                Data = null
            };
        }

      
        public override async Task<ApiResult<string>> UpdateAsync([FromBody] ApplicationUserDto item)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString()).ConfigureAwait(false);

                _logger.LogInformation($"Update User : userid:{user.Id} oldusername:{item.UserName} oldphonenumber:{item.PhoneNumber} oldtitle:{user.Title}");

                user.UserName = item.UserName;
                user.PhoneNumber = item.PhoneNumber;
                user.Title = item.Title;
                user.LanguageId = item.LanguageId;
                user.Email = item.Email;

                identityResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<string>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Update User Success" : sbErrors.ToString()),
                    Data = null
                };

                _logger.LogInformation($"Update User : userid:{user.Id} newusername:{item.UserName} newphonenumber:{item.PhoneNumber} newtitle:{user.Title} result :{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Update User : newusername:{item.UserName} newphonenumber:{item.PhoneNumber} newtitle:{item.Title} result:{ex.Message}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        public override ApiResult<string> Update([FromBody] ApplicationUserDto item)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to Update a user",
                Data = null
            };
        }

     
        public override ApiResult<string> DeleteById(Guid id)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to delete a user from database",
                Data = null
            };
        }

        
        public override async Task<ApiResult<string>> DeleteByIdAsync(Guid id)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
                identityResult = await _userManager.DeleteAsync(user).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<string>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Deleted Successfully." : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"User deleted. userid:{id} email:{user.Email} username:{user.UserName}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Delete user error. userid:{id} error:{ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }

       
        public override ApiResult<string> Delete([FromBody] ApplicationUserDto item)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to delete a user from database",
                Data = null
            };
        }

   
        public override Task<ApiResult<string>> DeleteAsync([FromBody] ApplicationUserDto item)
        {
            return DeleteByIdAsync(item.Id);
        }

       
        public override ApiResult<ApplicationUserDto> Find(Guid id)
        {
            try
            {
                var a = GetQueryable().Include(x => x.UserRoles).ThenInclude(t => t.Role).Include(y => y.Language)
                    .FirstOrDefault(x => x.Id == id);
                var dto = _mapper.Map<ApplicationUser, ApplicationUserDto>(a);

                var result = new ApiResult<ApplicationUserDto>
                {
                    StatusCode = (dto != null ? StatusCodes.Status200OK : StatusCodes.Status404NotFound),
                    Message = (dto != null ? "User Founded Successfully." : "No such user!"),
                    Data = dto
                };

                _logger.LogInformation($"Find user success username:{dto.UserName} email:{dto.Email}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Find user error! Code:{ex}");
                return new ApiResult<ApplicationUserDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        public override ApiResult<List<ApplicationUserDto>> GetAll()
        {
            try
            {
                var list = _userManager.Users.Include(x => x.UserRoles).ThenInclude(t => t.Role).Include(x => x.Language).Select(x => _mapper.Map<ApplicationUserDto>(x)).ToList();

                var result = new ApiResult<List<ApplicationUserDto>>
                {
                    StatusCode = (list.Count >= 1 ? StatusCodes.Status200OK : StatusCodes.Status404NotFound),
                    Message = (list.Count >= 1 ? "Users Founded Successfully." : "There is no user!"),
                    Data = list
                };

                _logger.LogInformation($"Getall users success Total user count:{list.Count}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Getall users error! Code:{ex}");
                return new ApiResult<List<ApplicationUserDto>>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

     
        [HttpPost("ChangePasswordAsAdminAsync")]
        public async Task<ApiResult> ChangePasswordAsAdminAsync([FromBody] ChangePasswordModel model)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString()).ConfigureAwait(false);

                var validator = new PasswordValidator<ApplicationUser>();

                var validatorResult = await validator.ValidateAsync(_userManager, user, model.NewPassword).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", validatorResult.Errors.Select(x => x.Code).ToList()));

                if (validatorResult.Succeeded)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                    identityResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Change Password Success" : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"Change Password : userid:{user.Id } mail:{user.Email} username:{user.UserName} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Change Password : id:{model.UserId} result:Error - {ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("ChangePasswordAsync")]
        public async Task<ApiResult> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString()).ConfigureAwait(false);

                var oldPasswordCheck = await _userManager.CheckPasswordAsync(user, model.OldPassword).ConfigureAwait(false);

                if (oldPasswordCheck)
                {
                    var validatorResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", validatorResult.Errors.Select(x => x.Code).ToList()));
                }
                else
                {
                    sbErrors.Append("Old Password Invalid");
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Change Password Success" : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"Change Password : userid:{user.Id } mail:{user.Email} username:{user.UserName} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Change Password : UserId:{model.UserId} Error:{ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }

     
        [HttpPost("AddUserRoleAsync")]
        public async Task<ApiResult> AddUserRoleAsync([FromBody] UserRoleModel userRole)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            var roleManager = _service.GetService<RoleManager<ApplicationRole>>();
            try
            {
                var user = await _userManager.FindByIdAsync(userRole.User.ToString()).ConfigureAwait(false);
                var role = await roleManager.FindByIdAsync(userRole.Role.ToString()).ConfigureAwait(false);

                if (role != null)
                {
                    identityResult = await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));
                }
                else
                {
                    sbErrors.Append("No Such Role!");
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Role Added Successfully." : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"User Role Add to userid:{userRole.User} role:{userRole.Role} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"User Role Add to userid:{userRole.User} role:{userRole.Role} result:Error - {ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }

      
        [HttpPost("DeleteUserRoleAsync")]
        public async Task<ApiResult> DeleteUserRoleAsync(Guid userid, Guid roleid)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            var roleManager = _service.GetService<RoleManager<ApplicationRole>>();
            try
            {
                var user = await _userManager.FindByIdAsync(userid.ToString()).ConfigureAwait(false);
                var role = await roleManager.FindByIdAsync(roleid.ToString()).ConfigureAwait(false);

                if (role != null)
                {
                    identityResult = await _userManager.RemoveFromRoleAsync(user, role.Name).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));
                }
                else
                {
                    sbErrors.Append("No Such Role!");
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Role Deleted Successfully." : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"User Role Delete to userid:{userid} role:{roleid} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"User Role Delete to userid:{userid} role:{roleid} result:Error - {ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }
    }
}