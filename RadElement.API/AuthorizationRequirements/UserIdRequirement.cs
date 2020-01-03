using Microsoft.AspNetCore.Authorization;

namespace RadElement.API.AuthorizationRequirements
{
    /// <summary>
    /// Represensts the requirement for user Id 
    /// </summary>
    public class UserIdRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Gets or sets the UserIdClaim
        /// </summary>
        public string UserIdClaim { get; }

        /// <summary>
        /// Initializes the isnstance of the class
        /// </summary>
        /// <param name="userIdClaim">Represents the claim</param>
        public UserIdRequirement(string userIdClaim)
        {
            UserIdClaim = userIdClaim;
        }
    }
}