﻿using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SanjeshP.RDC.Common;
using SanjeshP.RDC.Common.Exceptions;
using SanjeshP.RDC.Common.Utilities;
using SanjeshP.RDC.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SanjeshP.RDC.Entities.User;
using Microsoft.AspNetCore.Http;
using SanjeshP.RDC.Data.Contracts.Users;
using SanjeshP.RDC.Data.Repositories.Users;
using Microsoft.AspNetCore.Authentication;

namespace WebFramework.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                //Tips
                //Automatic client evaluation is no longer supported. This event is no longer generated.
                //This line is no longer needed.
                //.ConfigureWarnings(warning => warning.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        }

        public static void AddMinimalMvc(this IServiceCollection services)
        {
            //https://github.com/aspnet/AspNetCore/blob/0303c9e90b5b48b309a78c2ec9911db1812e6bf3/src/Mvc/Mvc/src/MvcServiceCollectionExtensions.cs
            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter()); //Apply AuthorizeFilter as global filter to all actions

                //Like [ValidateAntiforgeryToken] attribute but dose not validatie for GET and HEAD http method
                //You can ingore validate by using [IgnoreAntiforgeryToken] attribute
                //Use this filter when use cookie 
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                //options.UseYeKeModelBinder();
            }).AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.Converters.Add(new StringEnumConverter());
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //option.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            //services.AddSwaggerGenNewtonsoftSupport();

            #region Old way (We don't need this from ASP.NET Core 3.0 onwards)
            ////https://github.com/aspnet/Mvc/blob/release/2.2/src/Microsoft.AspNetCore.Mvc/MvcServiceCollectionExtensions.cs
            //services.AddMvcCore(options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter());

            //    //Like [ValidateAntiforgeryToken] attribute but dose not validatie for GET and HEAD http method
            //    //You can ingore validate by using [IgnoreAntiforgeryToken] attribute
            //    //Use this filter when use cookie 
            //    //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

            //    //options.UseYeKeModelBinder();
            //})
            //.AddApiExplorer()
            //.AddAuthorization()
            //.AddFormatterMappings()
            //.AddDataAnnotations()
            //.AddJsonOptions(option =>
            //{
            //    //option.JsonSerializerOptions
            //})
            //.AddNewtonsoftJson(/*option =>
            //{
            //    option.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //    option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            //}*/)

            ////Microsoft.AspNetCore.Mvc.Formatters.Json
            ////.AddJsonFormatters(/*options =>
            ////{
            ////    options.Formatting = Newtonsoft.Json.Formatting.Indented;
            ////    options.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            ////}*/)

            //.AddCors()
            //.SetCompatibilityVersion(CompatibilityVersion.Latest); //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            #endregion
        }

        public static void AddElmahCore(this IServiceCollection services, IConfiguration configuration, SiteSettings siteSetting)
        {
            services.AddElmah<SqlErrorLog>(options =>
            {
                options.Path = siteSetting.ElmahPath;
                options.ConnectionString = configuration.GetConnectionString("Elmah");
                //options.CheckPermissionAction = httpContext => httpContext.User.Identity.IsAuthenticated;
            });
        }
        public static void AddCustomAuthentication(this IServiceCollection services,AuthenticationSettings authenticationSettings, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = authenticationSettings.LoginPath;
                options.LogoutPath = authenticationSettings.LogoutPath;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(authenticationSettings.ExpireTimeSpanInMinutes);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                options.Events = new CookieAuthenticationEvents
                {
                    // رویداد در صورت شکست احراز هویت
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api"))
                        {
                            context.Response.Clear();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        }

                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    },

                    // رویداد هنگام معتبرسازی کوکی
                    OnValidatePrincipal = async context =>
                    {
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

                        if (claimsIdentity.Claims?.Any() != true)
                        {
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        }

                        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
                        {
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            return;
                        }

                        // اگر مقدار userGuid معتبر است، ادامه دهید
                        var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userGuid);
                        if (user == null || !user.IsActive)
                        {
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        }
                    },

                    // رویداد در زمان چالش
                    OnRedirectToAccessDenied = context =>
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    }
                };
            });
        }



        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie()
            .AddJwtBearer(options =>
            {
                var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var encryptionKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = jwtSettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("Authentication failed.", context.Exception);

                        if (context.Exception != null)
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                        if (!securityStamp.HasValue())
                            context.Fail("This token has no security stamp");

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.GetUserId();
                        var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, new Guid(userId));

                        //if (user.SecurityStamp != Guid.Parse(securityStamp))
                        //    context.Fail("Token security stamp is not valid.");

                        var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                        if (validatedUser == null)
                            context.Fail("Token security stamp is not valid.");

                        if (!user.IsActive)
                            context.Fail("User is not active.");

                        await userRepository.UpdateUserLastLoginDateAsync(user, context.HttpContext.RequestAborted);
                    },
                    OnChallenge = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);

                        if (context.AuthenticateFailure != null)
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authenticate failure.", HttpStatusCode.Unauthorized, context.AuthenticateFailure, null);
                        throw new AppException(ApiResultStatusCode.UnAuthorized, "You are unauthorized to access this resource.", HttpStatusCode.Unauthorized);

                        //return Task.CompletedTask;
                    }
                };
            });
        }

        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            //services.AddApiVersioning(options =>
            //{
            //    //url segment => {version}
            //    options.AssumeDefaultVersionWhenUnspecified = true; //default => false;
            //    options.DefaultApiVersion = new ApiVersion(1, 0); //v1.0 == v1
            //    options.ReportApiVersions = true;

            //    //ApiVersion.TryParse("1.0", out var version10);
            //    //ApiVersion.TryParse("1", out var version1);
            //    //var a = version10 == version1;

            //    //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            //    // api/posts?api-version=1

            //    //options.ApiVersionReader = new UrlSegmentApiVersionReader();
            //    // api/v1/posts

            //    //options.ApiVersionReader = new HeaderApiVersionReader(new[] { "Api-Version" });
            //    // header => Api-Version : 1

            //    //options.ApiVersionReader = new MediaTypeApiVersionReader()

            //    //options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"), new UrlSegmentApiVersionReader())
            //    // combine of [querystring] & [urlsegment]
            //});
        }
    }
}
