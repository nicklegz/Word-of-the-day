using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using word_of_the_day.Models;
using Newtonsoft.Json.Serialization;
using word_of_the_day.Interfaces;
using word_of_the_day.Extensions;
using word_of_the_day.Repos;
using word_of_the_day.Config;

namespace word_of_the_day
{
    public class Startup
    {
        private static readonly string corsPolicyName = "CorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //configure Cors policy
            CorsPolicyConfig.ConfigureCorsPolicy(services, corsPolicyName);

            services.AddDbContext<WordOfTheDayContext>(
                options => options.UseNpgsql(
                    Configuration.GetConnectionString(
                        "DevConnection")));
            services.AddTransient<IWordRepository, WordRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

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

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "word_of_the_day v1"));
            }

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseCors(corsPolicyName);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
