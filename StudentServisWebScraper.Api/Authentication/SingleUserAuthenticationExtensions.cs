using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StudentServisWebScraper.Api.Authentication
{
    public static class SingleUserAuthenticationExtensions
    {
        public static IdentityBuilder AddSingleUserStores(this IdentityBuilder builder, IConfiguration config)
        {
            builder.AddUserStore<SingleUserStore>();
            builder.AddRoleStore<SingleRoleStore>();

            builder.Services.Configure<SingleUserStoreOptions>(config);

            return builder;
        }
    }
}
