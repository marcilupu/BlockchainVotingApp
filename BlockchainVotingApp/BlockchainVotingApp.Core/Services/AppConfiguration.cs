using BlockchainVotingApp.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainVotingApp.Core.Services
{
    internal class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfiguration configuration)
        {
            AppState = Boolean.TryParse(configuration.GetSection("AppState:IsElectoral").Value, out bool result) ? result : null;
        }

        public bool? AppState { get; init; }
    }
}
