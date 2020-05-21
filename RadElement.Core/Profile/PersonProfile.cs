using RadElement.Core.Domain;
using RadElement.Core.DTO;

namespace RadElement.Core.Profile
{
    public class PersonProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonProfile"/> class.
        /// </summary>
        public PersonProfile()
        {
            CreateMap<Person, PersonAttributes>();
        }
    }
}
