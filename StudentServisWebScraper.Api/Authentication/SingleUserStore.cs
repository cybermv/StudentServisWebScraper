using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentServisWebScraper.Api.Authentication
{
    public class SingleUserStore : IUserPasswordStore<IdentityUser>
    {
        private SingleUserStoreOptions _options;

        public SingleUserStore(IOptions<SingleUserStoreOptions> options)
        {
            this._options = options.Value;
        }

        private Task<IdentityUser> GetSingleUser()
        {
            IdentityUser user = new IdentityUser
            {
                Id = _options.Id,
                UserName = _options.Username,
                Email = _options.Email
            };

            user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, _options.Password);

            return Task.FromResult(user);
        }

        public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (string.Equals(userId, _options.Id, StringComparison.OrdinalIgnoreCase))
            {
                return GetSingleUser();
            }
            else return Task.FromResult<IdentityUser>(null);
        }

        public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            if (string.Equals(normalizedUserName, _options.Id, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(normalizedUserName, _options.Username, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(normalizedUserName, _options.Email, StringComparison.OrdinalIgnoreCase))
            {
                return GetSingleUser();
            }
            else return Task.FromResult<IdentityUser>(null);
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(string.IsNullOrEmpty(user.PasswordHash));
        }

        public void Dispose()
        {
        }

        #region Unsupported

        public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        #endregion Unsupported
    }
}
