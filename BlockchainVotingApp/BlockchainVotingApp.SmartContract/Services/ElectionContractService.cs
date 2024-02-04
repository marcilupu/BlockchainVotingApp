using BlockchainVotingApp.SmartContract.Extensions;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using System.Numerics;

namespace BlockchainVotingApp.SmartContract.Services
{
    internal abstract class ElectionContractService : IElectionContractService
    {
        protected readonly IContractConfiguration _configuration;
        protected readonly ContractMetadata _metadata;

        public ElectionContractService(ContractMetadata metadata, IContractConfiguration configuration)
        {
            _configuration = configuration;
            _metadata = metadata;
        }

        public async Task<ExecutionResult<DataType>> Get<DataType>(string contractAddress, string functionName, Func<Function, Task<DataType>> executor, DataType? defaultValue = default)
        {
            var web3 = new Web3(_configuration.BlockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(_metadata.Abi, contractAddress);

                var function = contract.GetFunction(functionName);

                var result = await executor(function);

                return new ExecutionResult<DataType>(result, true);
            }
            catch (RpcResponseException ex)
            {
                Console.WriteLine($" [RpcResponseException]. Message: {ex.Message}");

                return new ExecutionResult<DataType>(defaultValue, false, ex.Message);
            }
            catch
            {
                return new ExecutionResult<DataType>(defaultValue, false);
            }
        }

        public async Task<ExecutionResult> Post(string contractAddress, string functionName, params object[]? parameters)
        {
            var web3 = new Web3(_configuration.BlockchainNetworkUrl);

            try
            {
                var contract = web3.Eth.GetContract(_metadata.Abi, contractAddress);

                var function = contract.GetFunction(functionName);

                // Estimate the gas required to execute the smart contract function.
                var estimatedGas = await EstimateGas(function);

                if (estimatedGas.Gas != null)
                {
                    var result = await function.SendTransactionAsync(
                        from: _configuration.AdminDefaultAccountAddress,
                        gas: estimatedGas.Gas,
                        value: new HexBigInteger(new BigInteger(0)),
                        parameters
                    );

                    return new ExecutionResult(true, result);
                }

                return new ExecutionResult(false, estimatedGas.Message);
            }
            catch (RpcResponseException response)
            {
                return new ExecutionResult(false, response.RpcError.GetDisplayMessage());
            }
            catch
            {
                return new ExecutionResult(false);
            }

            async Task<(HexBigInteger? Gas, string? Message)> EstimateGas(Function function)
            {
                try
                {
                    return (await function.EstimateGasAsync(parameters), null);
                }
                catch (SmartContractRevertException response)
                {
                    return (null, response.RevertMessage);
                }
                catch
                {
                    return (new HexBigInteger(new BigInteger(50000)), null);
                }
            }
        }
    }
}
