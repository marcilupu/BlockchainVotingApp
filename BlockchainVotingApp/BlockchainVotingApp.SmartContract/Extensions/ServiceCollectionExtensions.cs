using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockchainVotingApp.SmartContract.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmartContractService(this IServiceCollection services)
        {
            services
                .AddSingleton<PathsLookup>()
                .AddSingleton<IContractConfiguration, ContractConfiguration>()
                .AddSingleton<IVotingContractGenerator, VotingContractGenerator>()
                .AddSingleton<IRegisterContractGenerator, RegisterContractGenerator>()
                .AddSingleton<IVotingContractServiceFactory, VotingContractServiceFactory>()
                .AddSingleton<IRegisterContractServiceFactory, RegisterContractServiceFactory>();

            return services;
        }
    }
}
