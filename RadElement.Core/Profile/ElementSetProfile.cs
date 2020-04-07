using RadElement.Core.Domain;
using RadElement.Core.DTO;

namespace RadElement.Core.Profile
{
    public class ElementSetProfile: AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSetProfile"/> class.
        /// </summary>
        public ElementSetProfile()
        {
            CreateMap<ElementSet, ElementSetDetails>();
        }
    }
}
