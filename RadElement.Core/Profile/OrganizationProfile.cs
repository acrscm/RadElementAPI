using RadElement.Core.Domain;
using RadElement.Core.DTO;

namespace RadElement.Core.Profile
{
    public class OrganizationProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationProfile" /> class.
        /// </summary>
        public OrganizationProfile()
        {
            CreateMap<Organization, OrganizationDetails>();
        }
    }
}
