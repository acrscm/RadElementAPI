using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using RadElement.Core.Data;

namespace RadElement.Service
{
    /// <summary>
    /// Business service for handling the element set related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IElementSetService" />
    public class ElementSetService : IElementSetService
    {
        /// <summary>
        /// The RAD element database context
        /// </summary>
        private RadElementDbContext radElementDbContext;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSetService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public ElementSetService(
            RadElementDbContext radElementDbContext,
            IMapper mapper,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetSets()
        {
            try
            {
                var sets = radElementDbContext.ElementSet.ToList();
                return await Task.FromResult(new JsonResult(sets, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetSets()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetSet(string setId)
        {
            try
            {
                if (IsValidSetId(setId))
                {
                    int id = Convert.ToInt32(setId.Remove(0, 4));
                    var selectedSets = (from elementSet in radElementDbContext.ElementSet

                                        join eleSetIndexCodeRef in radElementDbContext.IndexCodeElementSetRef on elementSet.Id equals eleSetIndexCodeRef.ElementSetId into eleSetIndexCodeRefs
                                        from elementSetIndexCodeRef in eleSetIndexCodeRefs.DefaultIfEmpty()

                                        join eleSetIndexCode in radElementDbContext.IndexCode on elementSetIndexCodeRef.CodeId equals eleSetIndexCode.Id into eleSetIndexCodes
                                        from elementSetIndexCode in eleSetIndexCodes.DefaultIfEmpty()

                                        join eleSetSpecialtyRef in radElementDbContext.SpecialtyElementSetRef on elementSet.Id equals eleSetSpecialtyRef.ElementSetId into eleSetSpecialtyRefs
                                        from elementSetSpecialtyRef in eleSetSpecialtyRefs.DefaultIfEmpty()

                                        join eleSetSpecialty in radElementDbContext.Specialty on elementSetSpecialtyRef.SpecialtyId equals eleSetSpecialty.Id into eleSetSpecialties
                                        from elementSetSpecialty in eleSetSpecialties.DefaultIfEmpty()

                                        join eleSetReferenceRef in radElementDbContext.ReferenceRef on elementSet.Id equals eleSetReferenceRef.Reference_For_Id into eleSetReferenceRefs
                                        from elementSetReferenceRef in eleSetReferenceRefs.DefaultIfEmpty()

                                        join eleSetReference in radElementDbContext.Reference on elementSetReferenceRef.Reference_Id equals eleSetReference.Id into eleSetReferences
                                        from elementSetReference in eleSetReferences.DefaultIfEmpty()

                                        join eleSetImageRef in radElementDbContext.ImageRef on elementSet.Id equals eleSetImageRef.Image_For_Id into eleSetImageRefs
                                        from elementSetImageRef in eleSetImageRefs.DefaultIfEmpty()

                                        join eleSetImage in radElementDbContext.Image on elementSetImageRef.Image_Id equals eleSetImage.Id into eleSetImages
                                        from elementSetImage in eleSetImages.DefaultIfEmpty()

                                        join eleSetPersonRef in radElementDbContext.PersonRoleElementSetRef on elementSet.Id equals eleSetPersonRef.ElementSetID into eleSetPersonRefs
                                        from elementSetPersonRef in eleSetPersonRefs.DefaultIfEmpty()

                                        join eleSetPerson in radElementDbContext.Person on elementSetPersonRef.PersonID equals eleSetPerson.Id into eleSetPersons
                                        from elementSetPerson in eleSetPersons.DefaultIfEmpty()

                                        join eleSetOrganizationRef in radElementDbContext.OrganizationRoleElementSetRef on elementSet.Id equals eleSetOrganizationRef.ElementSetID into eleSetOrganizationRefs
                                        from elementSetOrganizationRef in eleSetOrganizationRefs.DefaultIfEmpty()

                                        join eleSetOrganization in radElementDbContext.Organization on elementSetOrganizationRef.OrganizationID equals eleSetOrganization.Id into eleSetOrganizations
                                        from elementSetOrganization in eleSetOrganizations.DefaultIfEmpty()

                                        where elementSet.Id == id

                                        select new FilteredData
                                        {
                                            ElementSet = elementSet,
                                            IndexCode = elementSetIndexCode,
                                            Specialty = elementSetSpecialty,
                                            Reference = elementSetReference,
                                            Image = elementSetImage,
                                            Person = elementSetPerson,
                                            Organization = elementSetOrganization,
                                            PersonRole = elementSetPersonRef.Role,
                                            OrganizationRole = elementSetOrganizationRef.Role
                                        }).Distinct().ToList();

                    if (selectedSets != null && selectedSets.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetElementSetDetailsDto(selectedSets, true), HttpStatusCode.OK));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetSet(string setId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the cde set.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchSets(string searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    var filteredSets = (from elementSet in radElementDbContext.ElementSet

                                        where (elementSet.Name.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase) ||
                                               ("RDES" + elementSet.Id.ToString()).Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase))

                                        select new FilteredData
                                        {
                                            ElementSet = elementSet
                                        }).Distinct().ToList();

                    if (filteredSets != null && filteredSets.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetElementSetDetailsDto(filteredSets, false), HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such set with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchSets(string searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the cde set.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateSet(CreateUpdateSet content)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (content == null)
                    {
                        return await Task.FromResult(new JsonResult("Set fileds are invalid.", HttpStatusCode.BadRequest));
                    }

                    ElementSet set = new ElementSet()
                    {
                        Name = content.Name.Trim(),
                        Description = content.Description,
                        ContactName = content.ContactName,
                        ParentId = content.ParentId,
                        Modality = content.Modality != null && content.Modality.Any() ? string.Join(",", content.Modality) : null,
                        BiologicalSex = content.BiologicalSex != null && content.BiologicalSex.Any() ? string.Join(",", content.BiologicalSex) : null,
                        AgeLowerBound = content.AgeLowerBound,
                        AgeUpperBound = content.AgeUpperBound,
                        Status = "Proposed",
                        StatusDate = DateTime.UtcNow,
                        Version = content.Version,
                        VersionDate = content.VersionDate != null ? content.VersionDate : DateTime.UtcNow
                    };

                    radElementDbContext.ElementSet.Add(set);
                    radElementDbContext.SaveChanges();

                    AddElementSetIndexCodeRefs(set.Id, content.IndexCodeReferences);
                    AddElementSetSpecialtyRefs(set.Id, content.Specialties);
                    AddElementSetReferenceRefs(set.Id, content.ReferencesRef);
                    AddElementSetImageRefs(set.Id, content.ImagesRef);
                    AddElementSetPersonRefs(set.Id, content.Persons);
                    AddElementSetOrganizationRefs(set.Id, content.Organizations);

                    transaction.Commit();

                    return await Task.FromResult(new JsonResult(new SetIdDetails() { SetId = "RDES" + set.Id.ToString() }, HttpStatusCode.Created));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'CreateSet(CreateUpdateSet content)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateSet(string setId, CreateUpdateSet content)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (IsValidSetId(setId))
                    {
                        int id = Convert.ToInt32(setId.Remove(0, 4));

                        if (content == null)
                        {
                            return await Task.FromResult(new JsonResult("Set fileds are invalid.", HttpStatusCode.BadRequest));
                        }

                        var elementSet = radElementDbContext.ElementSet.Where(x => x.Id == id).FirstOrDefault();

                        if (elementSet != null)
                        {
                            elementSet.Name = content.Name.Trim();
                            elementSet.Description = content.Description;
                            elementSet.ContactName = content.ContactName;
                            elementSet.Modality = content.Modality != null && content.Modality.Any() ? string.Join(",", content.Modality) : null;
                            elementSet.BiologicalSex = content.BiologicalSex != null && content.BiologicalSex.Any() ? string.Join(",", content.BiologicalSex) : null;
                            elementSet.AgeLowerBound = content.AgeLowerBound;
                            elementSet.AgeUpperBound = content.AgeUpperBound;
                            elementSet.Version = content.Version;
                            elementSet.VersionDate = content.VersionDate != null ? content.VersionDate : DateTime.UtcNow;

                            radElementDbContext.SaveChanges();

                            RemoveElementSetIndexCodeRefs(id);
                            RemoveElementSetSpecialtyRefs(id);
                            RemoveElementSetReferenceRefs(id);
                            RemoveElementSetImageRefs(id);
                            RemoveElementSetPersonRefs(id);
                            RemoveElementSetOrganizationRefs(id);

                            AddElementSetIndexCodeRefs(id, content.IndexCodeReferences);
                            AddElementSetSpecialtyRefs(id, content.Specialties);
                            AddElementSetReferenceRefs(id, content.ReferencesRef);
                            AddElementSetImageRefs(id, content.ImagesRef);
                            AddElementSetPersonRefs(id, content.Persons);
                            AddElementSetOrganizationRefs(id, content.Organizations);

                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Set with id '{0}' is updated.", setId), HttpStatusCode.OK));
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'UpdateSet(string setId, CreateUpdateSet content)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteSet(string setId)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (IsValidSetId(setId))
                    {
                        int id = Convert.ToInt32(setId.Remove(0, 4));
                        var elementSet = radElementDbContext.ElementSet.Where(x => x.Id == id).FirstOrDefault();

                        if (elementSet != null)
                        {
                            RemoveElementSetIndexCodeRefs(elementSet.Id);
                            RemoveElementSetSpecialtyRefs(id);
                            RemoveElementSetRefs(elementSet);
                            RemoveElementSetReferenceRefs(id);
                            RemoveElementSetImageRefs(id);
                            RemoveElementSetPersonRefs(elementSet.Id);
                            RemoveElementSetOrganizationRefs(elementSet.Id);
                            RemoveSet(elementSet.Id);

                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Set with id '{0}' is deleted.", setId), HttpStatusCode.OK));
                        }
                    }
                    return await Task.FromResult(new JsonResult(string.Format("No such set with id '{0}'.", setId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'DeleteSet(string setId)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Removes the set elements.
        /// </summary>
        /// <param name="elementSet">The element set.</param>
        private void RemoveElementSetRefs(ElementSet elementSet)
        {
            var elementSetRefs = radElementDbContext.ElementSetRef.Where(x => x.ElementSetId == elementSet.Id).ToList();
            if (elementSetRefs != null && elementSetRefs.Any())
            {
                radElementDbContext.ElementSetRef.RemoveRange(elementSetRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Adds the person references.
        /// </summary>
        /// <param name="personIds">The person ids.</param>
        private void AddElementSetPersonRefs(int setId, List<PersonDetails> personRefs)
        {
            if (personRefs != null && personRefs.Any())
            {
                foreach (var personRef in personRefs)
                {
                    var person = radElementDbContext.Person.Where(x => x.Id == personRef.PersonId).FirstOrDefault();

                    if (person != null)
                    {
                        if (personRef.Roles != null && personRef.Roles.Any())
                        {
                            foreach (var role in personRef.Roles.Distinct())
                            {
                                var setRef = new PersonRoleElementSetRef()
                                {
                                    ElementSetID = setId,
                                    PersonID = personRef.PersonId,
                                    Role = role.ToString()
                                };

                                radElementDbContext.PersonRoleElementSetRef.Add(setRef);
                                radElementDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var setRef = new PersonRoleElementSetRef()
                            {
                                ElementSetID = setId,
                                PersonID = personRef.PersonId
                            };

                            radElementDbContext.PersonRoleElementSetRef.Add(setRef);
                            radElementDbContext.SaveChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the organization references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="orgRefs">The org refs.</param>
        private void AddElementSetOrganizationRefs(int setId, List<OrganizationDetails> orgRefs)
        {
            if (orgRefs != null && orgRefs.Any())
            {
                foreach (var orgRef in orgRefs)
                {
                    var organization = radElementDbContext.Organization.Where(x => x.Id == orgRef.OrganizationId).FirstOrDefault();

                    if (organization != null)
                    {
                        if (orgRef.Roles != null && orgRef.Roles.Any())
                        {
                            foreach (var role in orgRef.Roles.Distinct())
                            {
                                var setRef = new OrganizationRoleElementSetRef()
                                {
                                    ElementSetID = setId,
                                    OrganizationID = orgRef.OrganizationId,
                                    Role = role.ToString()
                                };

                                radElementDbContext.OrganizationRoleElementSetRef.Add(setRef);
                                radElementDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var setRef = new OrganizationRoleElementSetRef()
                            {
                                ElementSetID = setId,
                                OrganizationID = orgRef.OrganizationId
                            };

                            radElementDbContext.OrganizationRoleElementSetRef.Add(setRef);
                            radElementDbContext.SaveChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the index code references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="codeReference">The code reference.</param>
        private void AddElementSetIndexCodeRefs(int setId, List<int> indexCodeReferences)
        {
            if (indexCodeReferences != null && indexCodeReferences.Any())
            {
                foreach (var indexCodeReference in indexCodeReferences)
                {
                    if (indexCodeReference != 0)
                    {
                        var setIndexCode = new IndexCodeElementSetRef
                        {
                            ElementSetId = setId,
                            CodeId = indexCodeReference
                        };

                        radElementDbContext.IndexCodeElementSetRef.Add(setIndexCode);
                        radElementDbContext.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Adds the element set specialty refs.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="specialties">The specialties.</param>
        private void AddElementSetSpecialtyRefs(int setId, List<SpecialtyValue> specialties)
        {
            if (specialties != null && specialties.Any())
            {
                foreach (var specialty in specialties)
                {
                    var spec = radElementDbContext.Specialty.Where(x => string.Equals(x.Code, specialty.Value, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (spec != null)
                    {
                        var specialtyRef = new SpecialtyElementSetRef
                        {
                            ElementSetId = setId,
                            SpecialtyId = spec.Id
                        };

                        radElementDbContext.SpecialtyElementSetRef.Add(specialtyRef);
                        radElementDbContext.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Adds the reference refs.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="references">The references.</param>
        private void AddElementSetReferenceRefs(int setId, List<int> references)
        {
            if (references != null && references.Any())
            {
                foreach (var reference in references)
                {
                    if (reference != 0)
                    {
                        var referenceRef = new ReferenceRef
                        {
                            Reference_For_Id = setId,
                            Reference_Id = reference,
                            Reference_For_Type = "set"
                        };

                        radElementDbContext.ReferenceRef.Add(referenceRef);
                        radElementDbContext.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Adds the element set image refs.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="images">The images.</param>
        private void AddElementSetImageRefs(int setId, List<int> images)
        {
            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    if (image != 0)
                    {
                        var imageRef = new ImageRef
                        {
                            Image_For_Id = setId,
                            Image_Id = image,
                            Image_For_Type = "set"
                        };

                        radElementDbContext.ImageRef.Add(imageRef);
                        radElementDbContext.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Removes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        private void RemoveSet(int setId)
        {
            var set = radElementDbContext.ElementSet.Where(x => x.Id == setId).FirstOrDefault();
            if (set != null)
            {
                radElementDbContext.ElementSet.Remove(set);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the person references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        private void RemoveElementSetPersonRefs(int setId)
        {
            var personElementSetRefs = radElementDbContext.PersonRoleElementSetRef.Where(x => x.ElementSetID == setId).ToList();
            if (personElementSetRefs != null && personElementSetRefs.Any())
            {
                radElementDbContext.PersonRoleElementSetRef.RemoveRange(personElementSetRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the organization references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        private void RemoveElementSetOrganizationRefs(int setId)
        {
            var organizationElementSetRefs = radElementDbContext.OrganizationRoleElementSetRef.Where(x => x.ElementSetID == setId).ToList();
            if (organizationElementSetRefs != null && organizationElementSetRefs.Any())
            {
                radElementDbContext.OrganizationRoleElementSetRef.RemoveRange(organizationElementSetRefs);
                radElementDbContext.SaveChanges();
            }
        }
        /// <summary>
        /// Removes the index code references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        private void RemoveElementSetIndexCodeRefs(int setId)
        {
            var indexCodeSetRefs = radElementDbContext.IndexCodeElementSetRef.Where(x => x.ElementSetId == setId).ToList();
            if (indexCodeSetRefs != null && indexCodeSetRefs.Any())
            {
                radElementDbContext.IndexCodeElementSetRef.RemoveRange(indexCodeSetRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the element set specialty refs.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        private void RemoveElementSetSpecialtyRefs(int setId)
        {
            var specialtyRefs = radElementDbContext.SpecialtyElementSetRef.Where(x => x.ElementSetId == setId).ToList();
            if (specialtyRefs != null && specialtyRefs.Any())
            {
                radElementDbContext.SpecialtyElementSetRef.RemoveRange(specialtyRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the reference refs.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        private void RemoveElementSetReferenceRefs(int setId)
        {
            var referenceRefs = radElementDbContext.ReferenceRef.Where(x => x.Reference_For_Id == setId).ToList();
            if (referenceRefs != null && referenceRefs.Any())
            {
                radElementDbContext.ReferenceRef.RemoveRange(referenceRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the element set image refs.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        private void RemoveElementSetImageRefs(int setId)
        {
            var imageRefs = radElementDbContext.ImageRef.Where(x => x.Image_For_Id == setId).ToList();
            if (imageRefs != null && imageRefs.Any())
            {
                radElementDbContext.ImageRef.RemoveRange(imageRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Determines whether [is valid set identifier] [the specified set identifier].
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is valid set identifier] [the specified set identifier]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidSetId(string setId)
        {
            if (setId.Length > 4 && string.Equals(setId.Substring(0, 4), "RDES", StringComparison.InvariantCultureIgnoreCase))
            {
                bool result = int.TryParse(setId.Remove(0, 4), out _);
                return result;
            }

            return false;
        }

        /// <summary>
        /// Gets the element set details dto.
        /// </summary>
        /// <param name="filteredSets">The filtered sets.</param>
        /// <param name="isSingleSet">if set to <c>true</c> [is single set].</param>
        /// <returns></returns>
        private object GetElementSetDetailsDto(List<FilteredData> filteredSets, bool isSingleSet)
        {
            var sets = new List<ElementSetDetails>();
            foreach (var eleSet in filteredSets)
            {
                if (!sets.Exists(x => x.Id == eleSet.ElementSet.Id))
                {
                    var set = mapper.Map<ElementSetDetails>(eleSet.ElementSet);
                    if (eleSet.IndexCode != null)
                    {
                        set.IndexCodes = new List<IndexCode>();
                        set.IndexCodes.Add(eleSet.IndexCode);
                    }
                    if (eleSet.Specialty != null)
                    {
                        set.Specialties = new List<Specialty>();
                        set.Specialties.Add(eleSet.Specialty);
                    }
                    if (eleSet.Reference != null)
                    {
                        set.References = new List<Reference>();
                        set.References.Add(eleSet.Reference);
                    }
                    if (eleSet.Image != null)
                    {
                        set.Images = new List<Image>();
                        set.Images.Add(eleSet.Image);
                    }
                    if (eleSet.Person != null)
                    {
                        var person = mapper.Map<PersonAttributes>(eleSet.Person);
                        if (!string.IsNullOrEmpty(eleSet.PersonRole))
                        {
                            person.Roles.Add(eleSet.PersonRole);
                        }

                        set.PersonInformation = new List<PersonAttributes>();
                        set.PersonInformation.Add(person);
                    }
                    if (eleSet.Organization != null)
                    {
                        var organization = mapper.Map<OrganizationAttributes>(eleSet.Organization);
                        if (!string.IsNullOrEmpty(eleSet.OrganizationRole))
                        {
                            organization.Roles.Add(eleSet.OrganizationRole);
                        }

                        set.OrganizationInformation = new List<OrganizationAttributes>();
                        set.OrganizationInformation.Add(organization);
                    }

                    sets.Add(set);
                }
                else
                {
                    var set = sets.Find(x => x.Id == eleSet.ElementSet.Id);
                    if (eleSet.IndexCode != null)
                    {
                        if (!set.IndexCodes.Exists(x => x.Id == eleSet.IndexCode.Id))
                        {
                            if (set.IndexCodes == null)
                            {
                                set.IndexCodes = new List<IndexCode>();
                            }
                            set.IndexCodes.Add(eleSet.IndexCode);
                        }
                    }
                    if (eleSet.Specialty != null)
                    {
                        if (!set.Specialties.Exists(x => x.Id == eleSet.Specialty.Id))
                        {
                            if (set.Specialties == null)
                            {
                                set.Specialties = new List<Specialty>();
                            }
                            set.Specialties.Add(eleSet.Specialty);
                        }
                    }
                    if (eleSet.Reference != null)
                    {
                        if (!set.References.Exists(x => x.Id == eleSet.Reference.Id))
                        {
                            if (set.References == null)
                            {
                                set.References = new List<Reference>();
                            }
                            set.References.Add(eleSet.Reference);
                        }
                    }
                    if (eleSet.Image != null)
                    {
                        if (!set.Images.Exists(x => x.Id == eleSet.Image.Id))
                        {
                            if (set.Images == null)
                            {
                                set.Images = new List<Image>();
                            }
                            set.Images.Add(eleSet.Image);
                        }
                    }
                    if (eleSet.Person != null)
                    {
                        var person = set.PersonInformation.Find(x => x.Id == eleSet.Person.Id);
                        if (person != null)
                        {
                            if (!string.IsNullOrEmpty(eleSet.PersonRole) && !person.Roles.Exists(x => x == eleSet.PersonRole))
                            {
                                person.Roles.Add(eleSet.PersonRole);
                            }
                        }
                        else
                        {
                            var mappedPerson = mapper.Map<PersonAttributes>(eleSet.Person);
                            if (!string.IsNullOrEmpty(eleSet.PersonRole))
                            {
                                mappedPerson.Roles.Add(eleSet.PersonRole);
                            }
                            if (set.PersonInformation == null)
                            {
                                set.PersonInformation = new List<PersonAttributes>();
                            }
                            set.PersonInformation.Add(mappedPerson);
                        }
                    }
                    if (eleSet.Organization != null)
                    {
                        var organization = set.OrganizationInformation.Find(x => x.Id == eleSet.Organization.Id);
                        if (organization != null)
                        {
                            if (!string.IsNullOrEmpty(eleSet.OrganizationRole) && !organization.Roles.Exists(x => x == eleSet.OrganizationRole))
                            {
                                organization.Roles.Add(eleSet.OrganizationRole);
                            }
                        }
                        else
                        {
                            var mappedOrganization = mapper.Map<OrganizationAttributes>(eleSet.Organization);
                            if (!string.IsNullOrEmpty(eleSet.OrganizationRole))
                            {
                                mappedOrganization.Roles.Add(eleSet.OrganizationRole);
                            }
                            if (set.OrganizationInformation == null)
                            {
                                set.OrganizationInformation = new List<OrganizationAttributes>();
                            }
                            set.OrganizationInformation.Add(mappedOrganization);
                        }
                    }
                }
            }
            if (isSingleSet)
            {
                return sets.FirstOrDefault();
            }

            return sets;
        }
    }
}
