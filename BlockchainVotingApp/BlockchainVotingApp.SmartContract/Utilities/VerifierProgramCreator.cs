using System.Text;

namespace BlockchainVotingApp.SmartContract.Utilities
{
    /// <summary>
    /// Use this class to generate the zokrates verifier file which must be compiled to obtain the proof.
    /// </summary>
    internal abstract class VerifierProgramCreator
    {
        private int _identation;
        protected readonly Random _random;

        protected VerifierProgramCreator()
        {
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

                GenerateProgram(builder);

                return builder.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.Message);

                return string.Empty;
            }
        }

        protected abstract void GenerateProgram(StringBuilder builder);

        protected void Write(StringBuilder builder, string text)
        {
            // Add identation.
            for (int i = 0; i < _identation; i++)
            {
                builder.Append('\t');
            }

            // Add text.
            builder.Append(text);
        }

        protected void WriteLine(StringBuilder builder, string text)
        {
            Write(builder,text);

            // Add newline
            builder.Append('\n');
        }

        protected void NewBlock(Action writer)
        {
            // Increase identation
            _identation++;

            writer();

            _identation--;
        }
    }
}
