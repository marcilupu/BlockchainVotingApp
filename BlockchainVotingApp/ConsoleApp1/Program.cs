// See https://aka.ms/new-console-template for more information
using BlockchainVotingApp.Core.Extensions;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Ef.Config;
using BlockchainVotingApp.SmartContract.Extensions;
using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

services.AddSingleton(configuration);
services.AddSmartContractService();
services.AddDataAccessLayer(configuration);
services.AddCoreServices();


var serviceProvider = services.BuildServiceProvider();

var scopedServiceProvider = serviceProvider.CreateScope();

try
{
    //===========================ELECTION state=====================================
    var electionModule = scopedServiceProvider.ServiceProvider.GetRequiredService<IElectionModule>();

    var electionState = await electionModule.UpdateElectionsState();

    //===========================ELECTION flow=======================================

    //var smartContractService = scopedServiceProvider.ServiceProvider.GetRequiredService<ISmartContractService>();

    //var deployedContract = await smartContractService.DeploySmartContract("0xedb941642abbea89723f4c11cb960427ecabc6fa8a540c7c16612a40eb0753b6");

    //List<int> voters = new List<int>() { 1, 2, 3 };
    //var result = await smartContractService.AddCandidate(1, deployedContract);
    //var result1 = await smartContractService.AddVoters(voters, deployedContract);
    //var result2 = await smartContractService.ChangeElectionState(false, deployedContract);
    //var result3 = await smartContractService.Vote(1, 1, deployedContract);
    //var result4 = await smartContractService.HasUserVoted(1, deployedContract);
    //var result5 = await smartContractService.GetUserVote(1, deployedContract);
    //Console.WriteLine(deployedContract);
    //Console.WriteLine($"{result5.VoterId} {result5.CandidateId}");

    //var deployedContract1 = await smartContractService.DeploySmartContract("0xedb941642abbea89723f4c11cb960427ecabc6fa8a540c7c16612a40eb0753b6");

    //var result10 = await smartContractService.AddCandidate(2, deployedContract1);
    //var result11 = await smartContractService.AddVoters(voters, deployedContract1);
    //var result21 = await smartContractService.ChangeElectionState(false, deployedContract1);
    //var result31 = await smartContractService.Vote(1, 2, deployedContract1);
    //var result41 = await smartContractService.HasUserVoted(1, deployedContract1);
    //var result51 = await smartContractService.GetUserVote(1, deployedContract1);
    //Console.WriteLine();
    //Console.WriteLine(deployedContract1);
    //Console.WriteLine($"{result51.VoterId} {result51.CandidateId}");

    //Console.WriteLine();
    //Console.WriteLine(deployedContract);
    //var result33 = await smartContractService.Vote(2, 1, deployedContract);
    //var result6 = await smartContractService.GetUserVote(2, deployedContract);
    //Console.WriteLine($"{result6.VoterId} {result6.CandidateId}");


}
catch (Exception e)
{

}