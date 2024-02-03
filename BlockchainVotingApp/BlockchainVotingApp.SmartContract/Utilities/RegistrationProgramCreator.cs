using System.Text;

namespace BlockchainVotingApp.SmartContract.Utilities
{
    internal class RegistrationProgramCreator : VerifierProgramCreator
    {
        private int countyId;
        public static RegistrationProgramCreator New(int countyId) => new RegistrationProgramCreator(countyId);

        private RegistrationProgramCreator(int countyId)
        {
            this.countyId = countyId;
        }

        protected override void GenerateProgram(StringBuilder builder)
        {
            var currentYear = new DateTime().Year;
            int legalVotingAge = 18;

            WriteLine(builder, "def main(private field birthYear, private field userCounty) {");

            NewBlock(() =>
            {
                WriteLine(builder, $"field currentYear = {currentYear};");
                WriteLine(builder, $"field legalVotingAge = {legalVotingAge};");
                WriteLine(builder, $"field currentYear = {currentYear};");
                WriteLine(builder, "");

                WriteLine(builder, $"field age = currentYear - birthYear;");
                WriteLine(builder, "");
                WriteLine(builder, $"assert(age >= legalVotingAge && age > 0, \"Age is not within legal limits\");");

                if (countyId != 0)
                {
                    WriteLine(builder, "");
                    WriteLine(builder, $"field electionCounty = {countyId};");
                    WriteLine(builder, "");
                    WriteLine(builder, $"assert(userCounty == {countyId}, \"The user is not located in the county of the election.\");");
                }

                WriteLine(builder, "return;");
            });

            WriteLine(builder, "}");
        }
    }
}
