using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace BlockchainVotingApp.SmartContract.Models
{
    /// <summary>
    /// Represents the ZKP proof of the user generated with zokrates.
    /// </summary>
    public sealed class Proof
    {
        public Proof() { }

        /// <summary>
        /// Attempt to read a new instance of <see cref="Proof"/> from a zokrates proof.json file.
        /// </summary>
        public static bool TryRead(string proofFile, [NotNullWhen(true)] out Proof? proof)
        {
            proof = null;

            try
            {

                if (File.Exists(proofFile))
                {
                    var jsonRaw = File.ReadAllText(proofFile);

                    var json = JObject.Parse(jsonRaw);
                    var proofNode = json["proof"];

                    proof = new Proof()
                    {
                        AX = proofNode["a"][0].ToString(),
                        AY = proofNode["a"][1].ToString(),
                        B0X = proofNode["b"][0][0].ToString(),
                        B1X = proofNode["b"][0][1].ToString(),
                        B0Y = proofNode["b"][1][0].ToString(),
                        B1Y = proofNode["b"][1][1].ToString(),
                        CX = proofNode["c"][0].ToString(),
                        CY = proofNode["c"][1].ToString()
                    };

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        

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
