using System.Text;

namespace BlockchainVotingApp.SmartContract.Utilities
{
    /// <summary>
    /// Use this class to generate the zokrates verifier file which must be compiled to obtain the proof.
    /// </summary>
    internal class VerifierProgramCreator
    {
        private readonly Random _random;
        private readonly List<int> _usersIds;
        private int _identation;
        
        public static VerifierProgramCreator New(IReadOnlyCollection<int> usersIds) => new VerifierProgramCreator(usersIds);

        private VerifierProgramCreator(IReadOnlyCollection<int> usersIds)
        {
            _usersIds = usersIds.ToList();
            _random = new Random();
            _identation = 0;
        }

        /// <summary>
        /// Generate the program using the given user identifiers passed to the context.
        /// </summary>
        public string Generate()
        {
            try
            {
                var builder = new StringBuilder(400);
                var usersCount = _usersIds.Count;
                var randNumber = _random.Next();

                WriteLine(builder, "def main(private field userId) {");

                // Increase identation
                _identation++;

                WriteLine(builder, $"field[{usersCount}] ids = [{string.Join(',', _usersIds)}];");

                WriteLine(builder,$"field randomSeed = {randNumber};");
                WriteLine(builder,"field mut match = randomSeed;");

                WriteLine(builder, $"for u32 i in 0..{usersCount} {{");
                WriteLine(builder, "    match = if ids[i] == userId { match + 1 } else { match }; ");
                WriteLine(builder, "}");

                WriteLine(builder, "assert(match > randomSeed);");
                WriteLine(builder, "return;");

                // Decrease identation.
                _identation--;


                WriteLine(builder, "}");


                return builder.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.Message);

                return string.Empty;
            }
        }


        private void Write(StringBuilder builder, string text)
        {
            // Add identation.
            for (int i = 0; i < _identation; i++)
            {
                builder.Append('\t');
            }

            // Add text.
            builder.Append(text);
        }

        private void WriteLine(StringBuilder builder, string text)
        {
            Write(builder,text);

            // Add newline
            builder.Append('\n');
        }
    }
}
