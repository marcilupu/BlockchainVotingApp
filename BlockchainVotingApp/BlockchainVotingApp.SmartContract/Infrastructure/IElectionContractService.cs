using Nethereum.Contracts;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public record ExecutionResult(bool IsSuccess, string? Message = null);

    public record ExecutionResult<ValueType>(ValueType? Value, bool IsSuccess, string? Message = null) : ExecutionResult(IsSuccess, Message);

    public interface IElectionContractService
    {
        Task<ExecutionResult<DataType>> Get<DataType>(string contractAddress, string functionName, Func<Function, Task<DataType>> executor, DataType? defaultValue = default);

        Task<ExecutionResult> Post(string contractAddress, string functionName, params object[]? parameters);
    }
}
