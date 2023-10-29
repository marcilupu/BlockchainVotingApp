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

            return services;
        }
    }
}
