using Microsoft.Extensions.DependencyInjection;

namespace word_of_the_day.Config
{
    public class CorsPolicyConfig
    {
        private readonly IServiceCollection _services;
        public CorsPolicyConfig(IServiceCollection services)
        {
            _services = services;
        }

        public void ConfigureCorsPolicy()
        {
            
        }
    }
}