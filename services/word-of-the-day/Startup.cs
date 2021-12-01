using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using word_of_the_day.Models;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api.Authorization;
using word_of_the_day.Interfaces;
using word_of_the_day.Extensions;
using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace word_of_the_day
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //     options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            // })            
            // .AddCookie(o =>
            // {
            //     // o.EventsType = typeof(CustomCookieAuthenticationEvents);
            //     o.Cookie.SameSite = SameSiteMode.None;
            //     o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //     o.Cookie.HttpOnly = true;
                
            // })
            // .AddOpenIdConnect("Auth0", options => ConfigureOpenIdConnect(options));

            // services.Configure<CookiePolicyOptions>(options =>
            // {
            //     options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            //     options.OnAppendCookie = cookieContext =>
            //         CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            //     options.OnDeleteCookie = cookieContext =>
            //         CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                
            // });

            services.AddHttpClient();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(
                        "https://worddujour.herokuapp.com", 
                        "http://worddujour.herokuapp.com",
                        "http://worddujour.ca",
                        "https://worddujour.ca",
                        "https://www.worddujour.ca",
                        "http://www.worddujour.ca",
                        "http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader());
            });

            // var authentication = services
            //     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer("Bearer", c =>
            //     {
            //         c.Authority = $"https://{Configuration["Auth0:Domain"]}";
            //         c.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateAudience = true,
            //         ValidAudiences = Configuration["Auth0:Audience"].Split(";"),
            //         ValidateIssuer = true,
            //         ValidIssuer = $"https://{Configuration["Auth0:Domain"]}"
            //     };
            // });

            services.AddDbContext<WordOfTheDayContext>(
                options => options.UseNpgsql(
                    Configuration.GetConnectionString(
                        "DevConnection")));
            services.AddTransient<IWordRepository, WordExtension>();
            services.AddTransient<IUserRepository, UserExtension>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                var resolver = options.SerializerSettings.ContractResolver;
                if (resolver != null)
                    (resolver as DefaultContractResolver).NamingStrategy = null;

            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "word_of_the_day", Version = "v1" });
            });

        //     services.AddAuthorization(o =>
        //     {
        //         o.AddPolicy("read:word", p => p.
        //             RequireAuthenticatedUser().
        //             RequireScope("read:word"));
        //     });
        // }

        // private void ConfigureOpenIdConnect(OpenIdConnectOptions options)
        // {
        //     // Set the authority to your Auth0 domain
        //     options.Authority = $"https://{Configuration["Auth0:Domain"]}";

        //     // Configure the Auth0 Client ID and Client Secret
        //     options.ClientId = Configuration["Auth0:ClientId"];
        //     options.ClientSecret = Configuration["Auth0:ClientSecret"];

        //     // Set response type to code
        //     options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

        //     options.ResponseMode = OpenIdConnectResponseMode.FormPost;

        //     // Configure the scope
        //     options.Scope.Clear();
        //     options.Scope.Add("openid");
        //     options.Scope.Add("offline_access");
        //     options.Scope.Add("read:word");
            
        //     options.CallbackPath = new PathString("/callback");

        //     // Configure the Claims Issuer to be Auth0
        //     options.ClaimsIssuer = "Auth0";

        //     // This saves the tokens in the session cookie
        //     options.SaveTokens = true;
            
        //     options.Events = new OpenIdConnectEvents
        //     {
        //         // handle the logout redirection
        //         OnRedirectToIdentityProviderForSignOut = (context) =>
        //         {
        //             var logoutUri = $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";

        //             var postLogoutUri = context.Properties.RedirectUri;
        //             if (!string.IsNullOrEmpty(postLogoutUri))
        //             {
        //                 if (postLogoutUri.StartsWith("/"))
        //                 {
        //                     // transform to absolute
        //                     var request = context.Request;
        //                     postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
        //                 }
        //                 logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
        //             }
        //             context.Response.Redirect(logoutUri);
        //             context.HandleResponse();

        //             return Task.CompletedTask;
        //         },
        //         OnRedirectToIdentityProvider = context => {
        //             context.ProtocolMessage.SetParameter("audience", Configuration["Auth0:Audience"]);
        //             return Task.CompletedTask;
        //         }
                
        //     };
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "word_of_the_day v1"));
            }

            // app.UseHttpsRedirection();
            
            // app.UseCookiePolicy();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }

        private void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                if (DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }
        public static bool DisallowsSameSiteNone(string userAgent)
        {
            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking
            // stack.
            if (userAgent.Contains("CPU iPhone OS 12") ||
                userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack. 
            // This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions, 
            // but pre-Chromium Edge does not require SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}
