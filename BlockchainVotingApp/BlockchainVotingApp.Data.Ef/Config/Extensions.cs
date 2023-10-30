using BlockchainVotingApp.Data.Ef.Context;
using BlockchainVotingApp.Data.Ef.Repositories;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlockchainVotingApp.Data.Ef.Config
{
    public static class Extensions
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add repos to DI container.
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICountyRepository, CountyRepository>();
            services.AddScoped<IElectionRepository, ElectionRepository>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();   

            services.AddIdentity<DbUser, DbUserRole>().AddEntityFrameworkStores<VDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login/Index";
            });

            return services;
        }
    }
}
