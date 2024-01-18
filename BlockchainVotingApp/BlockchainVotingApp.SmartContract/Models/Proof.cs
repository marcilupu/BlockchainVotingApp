namespace BlockchainVotingApp.SmartContract.Models
{
    /// <summary>
    /// Represents the ZKP proof of the user generated with zokrates.
    /// </summary>
    public sealed class Proof
    {
        public string AX { get; init; } = null!;

        public string AY { get; init; } = null!;

        public string B0X { get; init; } = null!;

        public string B0Y { get; init; } = null!;

        public string B1X { get; init; } = null!;

        public string B1Y { get; init; } = null!;

        public string CX { get; init; } = null!;

        public string CY { get; init; } = null!;
    }
}
