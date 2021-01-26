using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EA.Application.Data.Entitites;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EA.Application.Data.Context;
using EA.Application.Common.UnitOfWork;
using EA.Application.Common.Pagging.infrastructure;
using EA.Application.WebApi.Controllers;
using EA.Application.Common.Pagging;
using EA.Application.Dto.Mapper;
using EA.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using EA.Application.WebApi.Swagger;
using FluentMigrator.Runner;

namespace EA.Application.WebApi
{
    public class Startup
    {
        #region Variables
        IConfiguration _config { get; }
        #endregion

        #region Constructor
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region JwtTokenSection

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(5),
                        RoleClaimType = "Roles",
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidIssuer = _config["Tokens:Issuer"],
                        ValidateIssuer = true,
                        ValidAudience = _config["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                    };
                });
            services.ConfigureApplicationCookie(options => options.LoginPath = "/api/Token");
            #endregion
            #region LoggingSection
            services.AddLogging(builder => builder.AddFile(options =>
            {
                options.FileName = _config["Logging:Options:FilePrefix"]; 
                options.LogDirectory = _config["Logging:Options:LogDirectory"]; 
                options.FileSizeLimit = int.Parse(_config["Logging:Options:FileSizeLimit"]); 
            }));
            #endregion

            #region ApplicationDbContextSection
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<DbContext, ApplicationDbContext>();
            #endregion
            services.AddFluentMigratorCore()
                .ConfigureRunner(builder => builder
                    .AddSqlServer()
                    .WithGlobalConnectionString(_config.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(ApplicationDbContext).Assembly).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole());
            #region DependencyInjectionSection
            services.AddScoped<IUnitofWork, UnitofWork<ApplicationDbContext>>();
            services.AddTransient(typeof(IPagingLinks<>), typeof(PagingLinks<>));
            services.AddScoped<ILanguageController, LanguageController>();
            services.AddScoped<IUserController, UserController>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                                           .ActionContext;
                return new UrlHelper(actionContext);
            });
            #endregion

            #region IdentitySection
            //Identity yapısını ekliyoruz ve kendi oluşturduğumuz sınıfları veriyoruz.
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //Identity ile ilgili kurallar
            services.Configure<IdentityOptions>(options =>
            {
                // Parola ayarları
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Kullanıcının kilitlenmesi ile ilgili kurallar
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;

                // Yeni kullanıcı kuralları
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            });
            #endregion

            #region AutoMapperSection
            //Auto mapper'ı ekliyoruz
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region CorsSection
            //Cors ayarları 
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        //.WithOrigins("http://localhost:4200")
                        .AllowAnyOrigin()
                        //.WithMethods("GET", "POST")
                        .AllowAnyMethod()
                        //.WithHeaders("accept", "content-type", "origin", "No-Auth")
                        .AllowAnyHeader());
            });
            #endregion
            #region CahceSection
            services.AddMemoryCache();
            services.AddResponseCaching();
            #endregion

            #region SwaggerSection

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "EA.Application.WebApi",
                    Description = "Core identity, jwt token, paging, file logging, repository patter kullanılacak oluşturulmuştur.",
                    Contact = new OpenApiContact()
                    {
                        
                        Name = "Erdem Akkaya",
                        Email = "erdemdakkaya@gmail.com",
                        Url = new Uri("http://www.erdemakkaya.com")
                    }
                });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.OperationFilter<HeaderFiltersForSwagger>();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            #endregion

            #region MvcSection

            services.AddMvc(option => option.EnableEndpointRouting = false);

            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region EnvironmentSection
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #endregion

            #region IdentitySection
            app.UseAuthentication();
            #endregion

            #region CorsSection
            app.UseCors("CorsPolicy");
            #endregion
            app.UseStatusCodePages();

            #region CacheSection
            app.UseResponseCaching();
            #endregion

            #region  Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            #endregion


            #region MvcSection
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller}/{action}");
            });


            #endregion


        }
    }
}