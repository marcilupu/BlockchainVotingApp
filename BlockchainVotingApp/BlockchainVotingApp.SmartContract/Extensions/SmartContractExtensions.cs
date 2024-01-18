using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockchainVotingApp.SmartContract.Extensions
{
    public static class SmartContractExtensions
    {
        public static IServiceCollection AddSmartContractService(this IServiceCollection services)
        {
            services
                .AddSingleton<ISmartContractConfiguration, SmartContractConfiguration>()
                .AddSingleton<ISmartContractGenerator, SmartContractGenerator>()
                .AddSingleton<ISmartContractServiceFactory, SmartContractServiceFactory>();

            return services;
        }
    }
}
