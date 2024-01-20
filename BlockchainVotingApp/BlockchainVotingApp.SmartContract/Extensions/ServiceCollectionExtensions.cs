﻿using BlockchainVotingApp.SmartContract.Infrastructure;
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
                .AddSingleton<ISmartContractConfiguration, SmartContractConfiguration>()
                .AddSingleton<ISmartContractGenerator, SmartContractGenerator>()
                .AddSingleton<ISmartContractServiceFactory, SmartContractServiceFactory>();

            return services;
        }
    }
}