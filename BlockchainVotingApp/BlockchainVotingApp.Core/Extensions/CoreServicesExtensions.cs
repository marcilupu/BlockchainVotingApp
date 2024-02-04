using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockchainVotingApp.Core.Extensions
{
    public static class CoreServicesExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<IElectionService, ElectionService>();
            services.AddScoped<IElectionModule, ElectionModule>();
            services.AddSingleton<IQRGenerator, QRGenerator>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddScoped<IRegisterService, RegisterService>();
            
            //This service is used in order to change the election state
            services.AddScoped<IElectionModule, ElectionModule>();

            return services;
        }
    }
}
