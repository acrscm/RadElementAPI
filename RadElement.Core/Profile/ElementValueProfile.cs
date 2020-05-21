using RadElement.Core.Domain;
using RadElement.Core.DTO;

namespace RadElement.Core.Profile
{
    public class ElementValueProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementValueProfile"/> class.
        /// </summary>
        public ElementValueProfile()
        {
            CreateMap<ElementValue, ElementValueAttributes>();
        }
    }
}
