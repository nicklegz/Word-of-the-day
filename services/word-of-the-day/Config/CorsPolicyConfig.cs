using Microsoft.Extensions.DependencyInjection;

namespace word_of_the_day.Config
{
    public static class CorsPolicyConfig
    {
        public static void ConfigureCorsPolicy(IServiceCollection services, string policyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder.WithOrigins(
                        "https://worddujour.herokuapp.com", 
                        "http://worddujour.herokuapp.com",
                        "http://localhost:4200"
                    )
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader());
            });
        }
    }
}