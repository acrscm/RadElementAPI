using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RadElement.API.Hash;
using RadElement.Core.Infrastructure;
using RadElement.Service;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RadElement.API.AuthorizationRequirements
{
    /// <summary>
    /// Filter that checks if the User specified by the UserName exists in Assist
    /// </summary>
    public class UserIdExistsRequirementHandler : AuthorizationHandler<UserIdRequirement>, IAuthorizationRequirement
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UserIdExistsRequirementHandler> logger;

        /// <summary>
        /// The authorization configuration
        /// </summary>
        private readonly AuthorizationConfig authorizationConfig;

        /// <summary>
        /// The user accounts configuration
        /// </summary>
        private readonly UserAccounts userAccountsConfig;

        /// <summary>
        /// Initializes the insatnce of the class
        /// </summary>
        /// <param name="logger">Represents the logger</param>
        /// <param name="authorizationConfig"></param>
        ///  /// <param name="userAccountsConfig"></param>
        public UserIdExistsRequirementHandler(
            ILogger<UserIdExistsRequirementHandler> logger,
            AuthorizationConfig authorizationConfig,
            UserAccounts userAccountsConfig)
        {
            this.logger = logger;
            this.authorizationConfig = authorizationConfig;
            this.userAccountsConfig = userAccountsConfig;
        }

        /// <summary>
        /// Check if  requirement has been handled
        /// </summary>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIdRequirement requirement)
        {
            try
            {
                ClaimsIdentity claimsIdentity = context.User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    var userIdClaim = claimsIdentity.FindFirst(c => c.Type == requirement.UserIdClaim &&
                                     c.Issuer == authorizationConfig.Issuer);
                    var secretClaim = claimsIdentity.FindFirst(c => c.Type == Constants.SecretClaim &&
                                 c.Issuer == authorizationConfig.Issuer);
                    var roleClaim = claimsIdentity.FindFirst(c => c.Type == Constants.RoleClaim &&
                                     c.Issuer == authorizationConfig.Issuer);

                    if (userIdClaim != null && secretClaim != null && roleClaim != null)
                    {
                        var account = userAccountsConfig.Accounts.Find(x => x.UserId == userIdClaim.Value && x.Role == roleClaim.Value);
                        if (account != null)
                        {
                            if (SecurePasswordHasher.Verify(account.Secret, secretClaim.Value))
                            {
                                context.Succeed(requirement);
                            }
                        }
                    }
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UserIdExistsRequirement::HandleRequirementAsync");
                return Task.CompletedTask;
            }
        }
    }
}
