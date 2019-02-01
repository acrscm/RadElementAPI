using Acr.Assist.RadElement.Core.Data;
using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using Acr.Assist.RadElement.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acr.Assist.RadElement.Service
{
    public class ElementSetService : IElementSetService
    {
        private IRadElementDbContext radElementDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSetService"/> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        public ElementSetService(IRadElementDbContext radElementDbContext)
        {
            this.radElementDbContext = radElementDbContext;
        }

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ElementSet>> GetSets()
        {
            var cdeSets = await radElementDbContext.ElementSet.ToListAsync();
            return cdeSets;
        }

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<ElementSet> GetSet(int setId)
        {
            var cdeSets = await radElementDbContext.ElementSet.ToListAsync();
            return cdeSets.Find(x => x.Id == setId);
        }

        /// <summary>
        /// Searches the cde set.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<List<ElementSet>> SearchSet(string searchKeyword)
        {
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                var cdeSets = await radElementDbContext.ElementSet.ToListAsync();
                return cdeSets.FindAll(x => x.Name.ToLower().Contains(searchKeyword.ToLower()) || x.Description.ToLower().Contains(searchKeyword.ToLower()) ||
                                            x.ContactName.ToLower().Contains(searchKeyword.ToLower()));
            }

            return new List<ElementSet>();
        }

        /// <summary>
        /// Creates the cde set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<SetIdDetails> CreateSet(CreateUpdateSet content)
        {
            ElementSet set = new ElementSet()
            {
                Name = content.ModuleName.Replace("_", " "),
                Description = content.Description,
                ContactName = content.ContactName,
            };

            await radElementDbContext.ElementSet.AddAsync(set);
            radElementDbContext.SaveChanges();

            return new SetIdDetails() { SetId = set.Id.ToString() };
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<bool> UpdateSet(int setId, CreateUpdateSet content)
        {
            var elementSets = await radElementDbContext.ElementSet.ToListAsync();
            var elementSet = elementSets.Find(x => x.Id == setId);

            if (elementSet != null)
            {
                elementSet.Name = content.ModuleName.Replace("_", " ");
                elementSet.Description = content.Description;
                elementSet.ContactName = content.ContactName;
                return radElementDbContext.SaveChanges() > 0;
            }

            return false;
        }

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteSet(int setId)
        {
            var elementSets = await radElementDbContext.ElementSet.ToListAsync();
            var elementSet = elementSets.Find(x => x.Id == setId);

            if (elementSet != null)
            {
                var elementSetRefs = radElementDbContext.ElementSetRef.ToList().FindAll(x => x.ElementSetId == elementSet.Id);
                if (elementSetRefs != null && elementSetRefs.Any())
                {
                    foreach (var setref in elementSetRefs)
                    {
                        var elementValues = radElementDbContext.ElementValue.ToList().FindAll(x => x.ElementId == setref.ElementId);
                        var elements = radElementDbContext.Element.ToList().FindAll(x => x.Id == setref.ElementId);

                        if (elementValues != null && elementValues.Any())
                        {
                            radElementDbContext.ElementValue.RemoveRange(elementValues);
                        }

                        if (elements != null && elements.Any())
                        {
                            radElementDbContext.Element.RemoveRange(elements);
                        }
                    }

                    radElementDbContext.ElementSetRef.RemoveRange(elementSetRefs);
                }

                radElementDbContext.ElementSet.Remove(elementSet);
                return radElementDbContext.SaveChanges() > 0;
            }

            return false;
        }
    }
}
