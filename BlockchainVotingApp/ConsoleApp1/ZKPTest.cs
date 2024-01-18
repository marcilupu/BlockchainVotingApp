using BlockchainVotingApp.SmartContract.Extensions;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainApp.Test;

internal static class ZKPTest
{
    public static async Task Test()
    {
        IServiceCollection services = new ServiceCollection();

        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        services.AddSingleton(configuration);
        services.AddSmartContractService();

        var serviceProvider = services.BuildServiceProvider();

        var generator = serviceProvider.GetRequiredService<ISmartContractGenerator>();
        var userIds = new List<int>() { 1, 2, 3, 4, 5, 6 };
        var contextIdentifier = "DE_TEST";

        var contractMetadata = await generator.CreateSmartContractContext(contextIdentifier, userIds);

        var proof = await generator.GenerateProof(contextIdentifier, 2);

        Console.WriteLine("Done");
    }
}
