using System.Text;

namespace BlockchainVotingApp.SmartContract.Utilities
{

    internal class VotingProgramCreator : VerifierProgramCreator
    {
        private readonly List<int> _usersIds;

        public static VotingProgramCreator New(IReadOnlyCollection<int> usersIds) => new VotingProgramCreator(usersIds);

        private VotingProgramCreator(IReadOnlyCollection<int> usersIds)
        {
            _usersIds = usersIds.ToList();
        }

        /// <summary>
        /// Generate the program using the given user identifiers passed to the context.
        /// </summary>
        protected override void GenerateProgram(StringBuilder builder)
        {
            var usersCount = _usersIds.Count;
            var randNumber = _random.Next();

            WriteLine(builder, "def main(private field userId) {");

            NewBlock(() =>
            {
                WriteLine(builder, $"field[{usersCount}] ids = [{string.Join(',', _usersIds)}];");

                WriteLine(builder, $"field randomSeed = {randNumber};");
                WriteLine(builder, "field mut match = randomSeed;");

                WriteLine(builder, $"for u32 i in 0..{usersCount} {{");

                NewBlock(() =>
                {
                    WriteLine(builder, "match = if ids[i] == userId { match + 1 } else { match }; ");
                });

                WriteLine(builder, "}");

                WriteLine(builder, "assert(match > randomSeed);");
                WriteLine(builder, "return;");
            });

            WriteLine(builder, "}");
        }

    }
}
