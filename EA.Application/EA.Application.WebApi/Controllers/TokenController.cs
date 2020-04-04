using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EA.Application.Common.Api;
using EA.Application.Data.Entitites;
using EA.Application.Dto.DTOS;
using EA.Application.WebApi.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EA.Application.WebApi.Controllers
{
    /// <summary>
    /// Token oluşturulmasını ve login işlemlerinin gerçekleşmesini sağlayan sınıf.
    /// Route Authentication diyerek bu url üzerinde çalışacağını bildiriyoruz burası size kalmış ahmet, mehmet bile yazabilirsiniz
    /// </summary>
    [Route("Authentication")]
    public class TokenController : ControllerBase
    {
        #region Variables

        //Kullanıcı işlemleri için usermanager sınıfı (identity alt yapsından gelmektedir.)
        private readonly UserManager<ApplicationUser> _userManager;

        //Oturum işlemleri için sign in manager sınıfı (identity alt yapsından gelmektedir.)
        private readonly SignInManager<ApplicationUser> _signinManager;

        //Config den okuma işlemi yapmak için kullanacağımız sınıf (DI ile türemektedir bu nedenle interface)
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Yapıcı metod
        /// </summary>
        /// <param name="service"></param>
        public TokenController(IServiceProvider service,IMapper mapper)
        {
            _userManager = service.GetService<UserManager<ApplicationUser>>();
            _signinManager = service.GetService<SignInManager<ApplicationUser>>();
            _config = service.GetService<IConfiguration>();
            _mapper = mapper;
        }

        #endregion Constructor

        #region BusinessSection

        /// <summary>
        /// Login işleminin gerçekleşeceği metot.
        /// Route LoginAsync diyerek bu link ile erişilmesini söylüyoruz.
        /// </summary>
        /// <param name="loginModel">Kullanıcı işlemleri için gerekli bilgileri barındıran sınıf</param>
        /// <returns></returns>
        [HttpPost("LoginAsync")]
        [AllowAnonymous]
        public async Task<ApiResult> LoginAsync([FromBody]LoginModel loginModel)
        {
            try
            {
                //ilk etapta geriye döneceğiz http status codu no content olarak ayarladım size kalmış zorunlu değil
                var StatusCode = StatusCodes.Status204NoContent;
                var ResultMessage = "";
                object ResultData = "";

                //gönderilen mail adresinin doğruluğunu kontrol ediyorum
                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                //mail dogru ise user nesnesi ApplicationUser sınıfının bir instance olarak dolacaktır.
                if (user != null)
                {
                    //parola kontrolü yapıyoruz
                    var checkPwd = await _signinManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
                    //parola doğru ise işlemlere devam
                    if (checkPwd.Succeeded)
                    {
                        //kullanıcını rollerini sistemden alıyoruz
                       // var roles = await _userManager.GetRolesAsync(user);

                        //kullancının claimlerini dolduruyoruz.
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                        };

                        //claim leri ClaimsIdentity sınıfı içine dolduruyoruz
                        var claimsIdentity = new ClaimsIdentity(claims, "Token");
                        //daha sonra rolleride ayrı bir claim olarak basıyoruz
                       // claimsIdentity.AddClaims(roles.Select(role => new Claim("roles", role)));

                        //Jwt token secret keyi configden okuyoruz
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        //secret key in şifreleme algoritmasını belirliyoruz.
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        //token için Issuer bilgisini configden alıyoruz, tokeni oluşturan app'in url bilgisi olarak atadım
                        var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                        _config["Tokens:Issuer"], //token oluştuan
                        claimsIdentity.Claims,//kullanıcıya özel bilgiler (body)
                        expires: DateTime.Now.AddMinutes(30),//expire tarihi
                        signingCredentials: creds);

                        var tokenHandler = new { token = new JwtSecurityTokenHandler().WriteToken(token) };

                        //Tokeni geriye dönüyoruz ve status codu 200 yapyıoruz
                        ResultData = new TokenModel
                        {
                            Token = tokenHandler.token,
                            UserDto = _mapper.Map<ApplicationUser, ApplicationUserDto>(user)
                        };
                        ResultMessage = "Token created successfully";
                        StatusCode = StatusCodes.Status200OK;
                    }
                    else
                    {
                        ResultData = null;
                        ResultMessage = "Password is not correct!";
                        StatusCode = StatusCodes.Status406NotAcceptable;
                    }
                }
                else
                {
                    ResultData = null;
                    ResultMessage = "No such user!";
                    StatusCode = StatusCodes.Status406NotAcceptable;
                }

                return new ApiResult
                {
                    StatusCode = StatusCode,
                    Message = ResultMessage,
                    Data = ResultData
                };
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        #endregion BusinessSection
    }
}
