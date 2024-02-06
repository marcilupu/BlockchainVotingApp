using System.Text;

namespace BlockchainVotingApp.SmartContract.Utilities
{
    internal class RegistrationProgramCreator : VerifierProgramCreator
    {
        private int? countyId;
        public static RegistrationProgramCreator New(int? countyId) => new RegistrationProgramCreator(countyId);

        private RegistrationProgramCreator(int? countyId)
        {
            this.countyId = countyId;
        }

        protected override void GenerateProgram(StringBuilder builder)
        {
            var currentYear = DateTime.Now.Year;
            int legalVotingAge = 18;

            if (countyId.HasValue)
            {
                WriteLine(builder, "def main(private field birthYear, private field userCounty) {");
            }
            else
            {
                WriteLine(builder, "def main(private field birthYear) {");
            }

            NewBlock(() =>
            {
                WriteLine(builder, $"field currentYear = {currentYear};");
                WriteLine(builder, $"field legalVotingAge = {legalVotingAge};");
                WriteLine(builder, string.Empty);

                WriteLine(builder, $"field age = currentYear - birthYear;");
                WriteLine(builder, string.Empty);
                WriteLine(builder, $"assert(age >= legalVotingAge && age > 0);");

                if (countyId.HasValue)
                {
                    WriteLine(builder, string.Empty);
                    WriteLine(builder, $"field electionCounty = {countyId.Value};");
                    WriteLine(builder, string.Empty);
                    WriteLine(builder, $"assert(userCounty == electionCounty);");
                }

                WriteLine(builder, "return;");
            });

            WriteLine(builder, "}");
        }
    }
}
