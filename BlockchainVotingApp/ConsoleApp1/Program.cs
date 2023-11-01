// See https://aka.ms/new-console-template for more information
using BlockchainVotingApp.SmartContract.Extensions;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

services.AddSingleton(configuration);
services.AddSmartContractService();


var serviceProvider = services.BuildServiceProvider();

var scopedServiceProvider = serviceProvider.CreateScope();

try
{
    var smartContractService = scopedServiceProvider.ServiceProvider.GetRequiredService<ISmartContractService>();

    //List<int> voters = new List<int>() { 3, 4, 5 };
    //var result = await smartContractService.AddCandidate(1, "0x3deFb5Ca921e9D90836c57E31d6615c5Beb58E80");
    //var result1 = await smartContractService.AddVoters(voters, "0x6697DB0a3134826c2399755fc727CA83E15F69c5");
    //var result2 = await smartContractService.ChangeElectionState("0x3deFb5Ca921e9D90836c57E31d6615c5Beb58E80");
    //var result3 = await smartContractService.Vote(1, 1, "0x3deFb5Ca921e9D90836c57E31d6615c5Beb58E80");
    //var result3 = await smartContractService.HasUserVoted(1, "0x3deFb5Ca921e9D90836c57E31d6615c5Beb58E80");
    //var result = await smartContractService.GetUserVote(1, "0x3deFb5Ca921e9D90836c57E31d6615c5Beb58E80");

    var deployedContract = await smartContractService.DeploySmartContract("0xedb941642abbea89723f4c11cb960427ecabc6fa8a540c7c16612a40eb0753b6");

}
catch (Exception e)
{

}