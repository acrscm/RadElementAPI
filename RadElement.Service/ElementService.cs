using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using System.Net;
using Serilog;
using AutoMapper;
using RadElement.Core.Data;

namespace RadElement.Service
{
    /// <summary>
    /// Business service for handling the element related operations
    /// </summary>
    /// <seealso cref="RadElement.Core.Services.IElementService" />
    public class ElementService : IElementService
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
        /// Initializes a new instance of the <see cref="ElementService" /> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public ElementService(
            RadElementDbContext radElementDbContext,
            IMapper mapper,
            ILogger logger)
        {
            this.radElementDbContext = radElementDbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetElements()
        {
            try
            {
                var elements = (from element in radElementDbContext.Element
                                join eleValue in radElementDbContext.ElementValue on element.Id equals eleValue.ElementId into eleValues
                                from elementValue in eleValues.DefaultIfEmpty()

                                select new FilteredData
                                {
                                    Element = element,
                                    ElementValue = elementValue
                                }).Distinct().ToList();
                return await Task.FromResult(new JsonResult(GetElementDetailsDto(elements, false), HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetElements()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetElement(string elementId)
        {
            try
            {
                if (IsValidElementId(elementId))
                {
                    int elementInternalId = Convert.ToInt32(elementId.Remove(0, 3));
                    var selectedElements = (from element in radElementDbContext.Element
                                            join eleSetRef in radElementDbContext.ElementSetRef on (int)element.Id equals eleSetRef.ElementId into eleSetRefs
                                            from elementSetRef in eleSetRefs.DefaultIfEmpty()

                                            join eleSet in radElementDbContext.ElementSet on elementSetRef.ElementSetId equals eleSet.Id into eleSets
                                            from elementSet in eleSets.DefaultIfEmpty()

                                            join eleValue in radElementDbContext.ElementValue on element.Id equals eleValue.ElementId into eleValues
                                            from elementValue in eleValues.DefaultIfEmpty()

                                            join eleIndexCodeRef in radElementDbContext.IndexCodeElementRef on element.Id equals eleIndexCodeRef.ElementId into eleIndexCodeRefs
                                            from elementIndexCodeRef in eleIndexCodeRefs.DefaultIfEmpty()

                                            join eleIndexCode in radElementDbContext.IndexCode on elementIndexCodeRef.CodeId equals eleIndexCode.Id into eleIndexCodes
                                            from elementIndexCode in eleIndexCodes.DefaultIfEmpty()

                                            join eleIndexCodeValueRef in radElementDbContext.IndexCodeElementValueRef on elementValue.Id equals eleIndexCodeValueRef.ElementValueId into eleIndexCodeValueRefs
                                            from indexCodeValueRef in eleIndexCodeValueRefs.DefaultIfEmpty()

                                            join eleValueIndexCode in radElementDbContext.IndexCode on indexCodeValueRef.CodeId equals eleValueIndexCode.Id into eleValueIndexCodes
                                            from elementValueIndexCode in eleValueIndexCodes.DefaultIfEmpty()

                                            join elePersonRef in radElementDbContext.PersonRoleElementRef on (int)element.Id equals elePersonRef.ElementID into elePersonRefs
                                            from elementPersonRef in elePersonRefs.DefaultIfEmpty()

                                            join elePerson in radElementDbContext.Person on elementPersonRef.PersonID equals elePerson.Id into elePersons
                                            from elementPerson in elePersons.DefaultIfEmpty()

                                            join eleOrganizationRef in radElementDbContext.OrganizationRoleElementRef on (int)element.Id equals eleOrganizationRef.ElementID into eleOrganizationRefs
                                            from elementOrganizaionRef in eleOrganizationRefs.DefaultIfEmpty()

                                            join eleOrganization in radElementDbContext.Organization on elementOrganizaionRef.OrganizationID equals eleOrganization.Id into eleOrganizations
                                            from elementOrganization in eleOrganizations.DefaultIfEmpty()

                                            where element.Id == elementInternalId

                                            select new FilteredData
                                            {
                                                Element = element,
                                                ElementSet = elementSet,
                                                ElementValue = elementValue,
                                                IndexCode = elementIndexCode,
                                                ElementValueIndexCode = elementValueIndexCode,
                                                Person = elementPerson,
                                                Organization = elementOrganization,
                                                PersonRole = elementPersonRef.Role,
                                                OrganizationRole = elementOrganizaionRef.Role
                                            }).Distinct().ToList();

                    if (selectedElements != null && selectedElements.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetElementDetailsDto(selectedElements, true), HttpStatusCode.OK));
                    }
                }

                return await Task.FromResult(new JsonResult(string.Format("No such element with id '{0}'.", elementId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetElement(string elementId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Gets the elements by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> GetElementsBySetId(string setId)
        {
            try
            {
                if (IsValidSetId(setId))
                {
                    int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                    var selectedElements = (from element in radElementDbContext.Element
                                            join eleSetRef in radElementDbContext.ElementSetRef on (int)element.Id equals eleSetRef.ElementId into eleSetRefs
                                            from elementSetRef in eleSetRefs.DefaultIfEmpty()

                                            join eleSet in radElementDbContext.ElementSet on elementSetRef.ElementSetId equals eleSet.Id into eleSets
                                            from elementSet in eleSets.DefaultIfEmpty()

                                            join eleValue in radElementDbContext.ElementValue on element.Id equals eleValue.ElementId into eleValues
                                            from elementValue in eleValues.DefaultIfEmpty()

                                            join eleIndexCodeRef in radElementDbContext.IndexCodeElementRef on element.Id equals eleIndexCodeRef.ElementId into eleIndexCodeRefs
                                            from elementIndexCodeRef in eleIndexCodeRefs.DefaultIfEmpty()

                                            join eleIndexCode in radElementDbContext.IndexCode on elementIndexCodeRef.CodeId equals eleIndexCode.Id into eleIndexCodes
                                            from elementIndexCode in eleIndexCodes.DefaultIfEmpty()

                                            join eleIndexCodeValueRef in radElementDbContext.IndexCodeElementValueRef on elementValue.Id equals eleIndexCodeValueRef.ElementValueId into eleIndexCodeValueRefs
                                            from indexCodeValueRef in eleIndexCodeValueRefs.DefaultIfEmpty()

                                            join eleValueIndexCode in radElementDbContext.IndexCode on indexCodeValueRef.CodeId equals eleValueIndexCode.Id into eleValueIndexCodes
                                            from elementValueIndexCode in eleValueIndexCodes.DefaultIfEmpty()

                                            join elePersonRef in radElementDbContext.PersonRoleElementRef on (int)element.Id equals elePersonRef.ElementID into elePersonRefs
                                            from elementPersonRef in elePersonRefs.DefaultIfEmpty()

                                            join elePerson in radElementDbContext.Person on elementPersonRef.PersonID equals elePerson.Id into elePersons
                                            from elementPerson in elePersons.DefaultIfEmpty()

                                            join eleOrganizationRef in radElementDbContext.OrganizationRoleElementRef on (int)element.Id equals eleOrganizationRef.ElementID into eleOrganizationRefs
                                            from elementOrganizaionRef in eleOrganizationRefs.DefaultIfEmpty()

                                            join eleOrganization in radElementDbContext.Organization on elementOrganizaionRef.OrganizationID equals eleOrganization.Id into eleOrganizations
                                            from elementOrganization in eleOrganizations.DefaultIfEmpty()

                                            where elementSet.Id == setInternalId

                                            select new FilteredData
                                            {
                                                Element = element,
                                                ElementSet = elementSet,
                                                ElementValue = elementValue,
                                                IndexCode = elementIndexCode,
                                                ElementValueIndexCode = elementValueIndexCode,
                                                Person = elementPerson,
                                                Organization = elementOrganization,
                                                PersonRole = elementPersonRef.Role,
                                                OrganizationRole = elementOrganizaionRef.Role
                                            }).Distinct().ToList();

                    if (selectedElements != null && selectedElements.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetElementDetailsDto(selectedElements, false), HttpStatusCode.OK));
                    }
                }
                return await Task.FromResult(new JsonResult(string.Format("No such elements with set id '{0}'.", setId), HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'GetElementsBySetId(string setId)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Searches the element.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<JsonResult> SearchElements(string searchKeyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    if (searchKeyword.Length < 3)
                    {
                        return await Task.FromResult(new JsonResult("The Keyword field must be a string with a minimum length of '3'.", HttpStatusCode.BadRequest));
                    }

                    var filteredElements = (from element in radElementDbContext.Element
                                            join eleSetRef in radElementDbContext.ElementSetRef on (int)element.Id equals eleSetRef.ElementId into eleSetRefs
                                            from elementSetRef in eleSetRefs.DefaultIfEmpty()

                                            join eleSet in radElementDbContext.ElementSet on elementSetRef.ElementSetId equals eleSet.Id into eleSets
                                            from elementSet in eleSets.DefaultIfEmpty()

                                            join eleValue in radElementDbContext.ElementValue on element.Id equals eleValue.ElementId into eleValues
                                            from elementValue in eleValues.DefaultIfEmpty()

                                            where element.Name.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase)

                                            select new FilteredData
                                            {
                                                Element = element,
                                                ElementSet = elementSet,
                                                ElementValue = elementValue
                                            }).Distinct().ToList();

                    if (filteredElements != null && filteredElements.Any())
                    {
                        return await Task.FromResult(new JsonResult(GetElementDetailsDto(filteredElements, false), HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such element with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchElements(SearchKeyword searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Deeps the search elements.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeepSearchElements(string searchKeyword, string operation)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    if (searchKeyword.Length < 3)
                    {
                        return await Task.FromResult(new JsonResult("The Keyword field must be a string with a minimum length of '3'.", HttpStatusCode.BadRequest));
                    }

                    var filteredData = new List<FilteredData>();
                    var deepSearchResponse = new DeepSearchResponse();
                    var dbExecutionWatch = new System.Diagnostics.Stopwatch();
                    dbExecutionWatch.Start();

                    if (operation == "set")
                    {
                        filteredData = (from element in radElementDbContext.Element
                                        join eleSetRef in radElementDbContext.ElementSetRef on (int)element.Id equals eleSetRef.ElementId into eleSetRefs
                                        from elementSetRef in eleSetRefs.DefaultIfEmpty()

                                        join eleSet in radElementDbContext.ElementSet on elementSetRef.ElementSetId equals eleSet.Id into eleSets
                                        from elementSet in eleSets.DefaultIfEmpty()

                                        where element.Name.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase)

                                        select new FilteredData
                                        {
                                            Element = element,
                                            ElementSet = elementSet
                                        }).Distinct().ToList();
                    }
                    else if (operation == "values")
                    {
                        filteredData = (from element in radElementDbContext.Element
                                        join eleSetRef in radElementDbContext.ElementSetRef on (int)element.Id equals eleSetRef.ElementId into eleSetRefs
                                        from elementSetRef in eleSetRefs.DefaultIfEmpty()

                                        join eleSet in radElementDbContext.ElementSet on elementSetRef.ElementSetId equals eleSet.Id into eleSets
                                        from elementSet in eleSets.DefaultIfEmpty()

                                        join eleValue in radElementDbContext.ElementValue on element.Id equals eleValue.ElementId into eleValues
                                        from elementValue in eleValues.DefaultIfEmpty()

                                        where element.Name.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase)

                                        select new FilteredData
                                        {
                                            Element = element,
                                            ElementSet = elementSet,
                                            ElementValue = elementValue
                                        }).Distinct().ToList();
                    }
                    else if (operation == "persorg")
                    {
                        filteredData = (from element in radElementDbContext.Element
                                        join eleSetRef in radElementDbContext.ElementSetRef on (int)element.Id equals eleSetRef.ElementId into eleSetRefs
                                        from elementSetRef in eleSetRefs.DefaultIfEmpty()

                                        join eleSet in radElementDbContext.ElementSet on elementSetRef.ElementSetId equals eleSet.Id into eleSets
                                        from elementSet in eleSets.DefaultIfEmpty()

                                        join eleValue in radElementDbContext.ElementValue on element.Id equals eleValue.ElementId into eleValues
                                        from elementValue in eleValues.DefaultIfEmpty()

                                        join eleIndexCodeRef in radElementDbContext.IndexCodeElementRef on element.Id equals eleIndexCodeRef.ElementId into eleIndexCodeRefs
                                        from elementIndexCodeRef in eleIndexCodeRefs.DefaultIfEmpty()

                                        join eleIndexCode in radElementDbContext.IndexCode on elementIndexCodeRef.CodeId equals eleIndexCode.Id into eleIndexCodes
                                        from elementIndexCode in eleIndexCodes.DefaultIfEmpty()

                                        join eleIndexCodeValueRef in radElementDbContext.IndexCodeElementValueRef on elementValue.Id equals eleIndexCodeValueRef.ElementValueId into eleIndexCodeValueRefs
                                        from indexCodeValueRef in eleIndexCodeValueRefs.DefaultIfEmpty()

                                        join eleValueIndexCode in radElementDbContext.IndexCode on indexCodeValueRef.CodeId equals eleValueIndexCode.Id into eleValueIndexCodes
                                        from elementValueIndexCode in eleValueIndexCodes.DefaultIfEmpty()

                                        join elePersonRef in radElementDbContext.PersonRoleElementRef on (int)element.Id equals elePersonRef.ElementID into elePersonRefs
                                        from elementPersonRef in elePersonRefs.DefaultIfEmpty()

                                        join elePerson in radElementDbContext.Person on elementPersonRef.PersonID equals elePerson.Id into elePersons
                                        from elementPerson in elePersons.DefaultIfEmpty()

                                        join eleOrganizationRef in radElementDbContext.OrganizationRoleElementRef on (int)element.Id equals eleOrganizationRef.ElementID into eleOrganizationRefs
                                        from elementOrganizaionRef in eleOrganizationRefs.DefaultIfEmpty()

                                        join eleOrganization in radElementDbContext.Organization on elementOrganizaionRef.OrganizationID equals eleOrganization.Id into eleOrganizations
                                        from elementOrganization in eleOrganizations.DefaultIfEmpty()

                                        where element.Name.Contains(searchKeyword, StringComparison.InvariantCultureIgnoreCase)

                                        select new FilteredData
                                        {
                                            Element = element,
                                            ElementSet = elementSet,
                                            ElementValue = elementValue,
                                            IndexCode = elementIndexCode,
                                            ElementValueIndexCode = elementValueIndexCode,
                                            Person = elementPerson == null ? null : new PersonAttributes
                                            {
                                                Id = elementPerson.Id,
                                                Name = elementPerson.Name,
                                                Orcid = elementPerson.Orcid,
                                                TwitterHandle = elementPerson.TwitterHandle,
                                                Url = elementPerson.Url,
                                                Roles = !string.IsNullOrEmpty(elementPersonRef.Role) ? new List<string> { elementPersonRef.Role } : new List<string>()
                                            },
                                            Organization = elementOrganization == null ? null : new OrganizationAttributes
                                            {
                                                Id = elementOrganization.Id,
                                                Name = elementOrganization.Name,
                                                Abbreviation = elementOrganization.Abbreviation,
                                                TwitterHandle = elementOrganization.TwitterHandle,
                                                Comment = elementOrganization.Comment,
                                                Email = elementOrganization.Email,
                                                Url = elementOrganization.Url,
                                                Roles = !string.IsNullOrEmpty(elementOrganizaionRef.Role) ? new List<string> { elementOrganizaionRef.Role } : new List<string>()
                                            }
                                        }).Distinct().ToList();
                    }

                    dbExecutionWatch.Stop();
                    deepSearchResponse.DBExecutionTime = string.Format("Execution Time: {0} ms", dbExecutionWatch.ElapsedMilliseconds);

                    if (filteredData != null && filteredData.Any())
                    {
                        var loopExecutionWatch = new System.Diagnostics.Stopwatch();
                        loopExecutionWatch.Start();
                        var elements = GetElementDetailsDto(filteredData, false);
                        loopExecutionWatch.Stop();
                        deepSearchResponse.LoopExecutionTime = string.Format("Execution Time: {0} ms", loopExecutionWatch.ElapsedMilliseconds);
                        deepSearchResponse.Elements = elements;

                        return await Task.FromResult(new JsonResult(deepSearchResponse, HttpStatusCode.OK));
                    }
                    else
                    {
                        return await Task.FromResult(new JsonResult(string.Format("No such element with keyword '{0}'.", searchKeyword), HttpStatusCode.NotFound));
                    }
                }

                return await Task.FromResult(new JsonResult("Keyword given is invalid.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in method 'SearchElements(SearchKeyword searchKeyword)'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        public async Task<JsonResult> CreateElement(string setId, CreateElement dataElement)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (IsValidSetId(setId))
                    {
                        int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                        if (dataElement == null)
                        {
                            return await Task.FromResult(new JsonResult("Element fields are invalid.", HttpStatusCode.BadRequest));
                        }

                        if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
                        {
                            if (dataElement.Options == null || dataElement.Options.Count == 0)
                            {
                                return await Task.FromResult(new JsonResult("'Options' field are missing for Choice type elements.", HttpStatusCode.BadRequest));
                            }
                        }

                        var elementSet = radElementDbContext.ElementSet.Where(x => x.Id == setInternalId).FirstOrDefault();

                        if (elementSet != null)
                        {
                            if (string.IsNullOrEmpty(dataElement.ElementId))
                            {
                                Element element = new Element()
                                {
                                    Name = dataElement.Name,
                                    ShortName = dataElement.ShortName ?? string.Empty,
                                    Definition = dataElement.Definition ?? string.Empty,
                                    ValueType = GetElementValueType(dataElement.ValueType),
                                    MinCardinality = 1,
                                    MaxCardinality = (dataElement.ValueType == DataElementType.MultiChoice) ? (uint)dataElement.Options.Count : 1,
                                    Unit = dataElement.Unit ?? string.Empty,
                                    Question = dataElement.Question ?? dataElement.Name,
                                    Instructions = dataElement.Instructions ?? string.Empty,
                                    References = dataElement.References ?? string.Empty,
                                    Version = dataElement.Version ?? "",
                                    VersionDate = dataElement.VersionDate ?? DateTime.Now,
                                    Synonyms = dataElement.Synonyms ?? string.Empty,
                                    Source = dataElement.Source ?? string.Empty,
                                    Status = "Proposed",
                                    StatusDate = DateTime.Now,
                                    Editor = dataElement.Editor ?? string.Empty,
                                    Modality = dataElement.Modality != null && dataElement.Modality.Any() ? string.Join(",", dataElement.Modality) : null,
                                    BiologicalSex = dataElement.BiologicalSex != null && dataElement.BiologicalSex.Any() ? string.Join(",", dataElement.BiologicalSex) : null,
                                    AgeLowerBound = dataElement.AgeLowerBound,
                                    AgeUpperBound = dataElement.AgeUpperBound,
                                    ValueSize = 0
                                };

                                if (dataElement.ValueType == DataElementType.Integer)
                                {
                                    element.ValueMin = dataElement.ValueMin;
                                    element.ValueMax = dataElement.ValueMax;
                                    element.StepValue = 1;
                                }

                                if (dataElement.ValueType == DataElementType.Numeric)
                                {
                                    float? minValue = null;
                                    float? maxValue = null;

                                    if (dataElement.ValueMin.HasValue)
                                    {
                                        minValue = Convert.ToSingle(dataElement.ValueMin.Value);
                                    }

                                    if (dataElement.ValueMax.HasValue)
                                    {
                                        maxValue = Convert.ToSingle(dataElement.ValueMax.Value);
                                    }

                                    element.ValueMin = minValue;
                                    element.ValueMax = maxValue;
                                    element.StepValue = 0.1f;
                                }

                                radElementDbContext.Element.Add(element);
                                radElementDbContext.SaveChanges();

                                AddElementIndexCodeReferences(element.Id, dataElement.IndexCodeReferences);
                                AddElementValues(dataElement.Options, element.Id);
                                AddElementSetReferences(setInternalId, (int)element.Id);
                                AddPersonReferences((int)element.Id, dataElement.Persons);
                                AddOrganizationReferences((int)element.Id, dataElement.Organizations);

                                radElementDbContext.SaveChanges();
                                transaction.Commit();

                                return await Task.FromResult(new JsonResult(new ElementIdDetails() { ElementId = "RDE" + element.Id.ToString() }, HttpStatusCode.Created));
                            }
                            else
                            {
                                if (IsValidElementId(dataElement.ElementId))
                                {
                                    int elementInternalId = Convert.ToInt32(dataElement.ElementId.Remove(0, 3));
                                    var element = radElementDbContext.Element.Where(x => x.Id == elementInternalId).FirstOrDefault();

                                    if (element != null)
                                    {
                                        var elementsetRefs = radElementDbContext.ElementSetRef.ToList();
                                        if (!elementsetRefs.Exists(x => x.ElementId == elementInternalId && x.ElementSetId == setInternalId))
                                        {
                                            AddElementSetReferences(setInternalId, elementInternalId);
                                            radElementDbContext.SaveChanges();
                                            transaction.Commit();
                                        }

                                        return await Task.FromResult(new JsonResult(new ElementIdDetails() { ElementId = dataElement.ElementId }, HttpStatusCode.Created));
                                    }
                                }

                                return await Task.FromResult(new JsonResult(string.Format("No such element with element id '{0}'.", dataElement.ElementId), HttpStatusCode.NotFound));
                            }
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such set with set id {0}.", setId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'CreateElement(string setId, CreateUpdateElement dataElement)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateElement(string setId, string elementId, UpdateElement dataElement)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (IsValidSetId(setId) && IsValidElementId(elementId))
                    {
                        int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                        int elementInternalId = Convert.ToInt32(elementId.Remove(0, 3));

                        if (dataElement == null)
                        {
                            return await Task.FromResult(new JsonResult("Element fields are invalid.", HttpStatusCode.BadRequest));
                        }

                        if (dataElement.ValueType == DataElementType.Choice || dataElement.ValueType == DataElementType.MultiChoice)
                        {
                            if (dataElement.Options == null || dataElement.Options.Count == 0)
                            {
                                return await Task.FromResult(new JsonResult("'Options' field are missing for Choice type elements.", HttpStatusCode.BadRequest));
                            }
                        }

                        var elementSet = radElementDbContext.ElementSet.Where(x => x.Id == setInternalId).FirstOrDefault();
                        var setElementRefs = radElementDbContext.ElementSetRef.Where(x => x.ElementId == elementInternalId && x.ElementSetId == setInternalId).FirstOrDefault();

                        if (elementSet != null && setElementRefs != null)
                        {
                            var element = radElementDbContext.Element.Where(x => x.Id == elementInternalId).FirstOrDefault();
                            if (element != null)
                            {
                                element.Name = dataElement.Name;
                                element.ShortName = dataElement.ShortName ?? string.Empty;
                                element.Definition = dataElement.Definition ?? string.Empty;
                                element.ValueType = GetElementValueType(dataElement.ValueType);
                                element.MinCardinality = 1;
                                element.MaxCardinality = (dataElement.ValueType == DataElementType.MultiChoice) ? (uint)dataElement.Options.Count : (uint)1;
                                element.Unit = dataElement.Unit ?? string.Empty;
                                element.Question = dataElement.Question ?? dataElement.Name;
                                element.Instructions = dataElement.Instructions ?? string.Empty;
                                element.References = dataElement.References ?? string.Empty;
                                element.Version = dataElement.Version ?? "";
                                element.VersionDate = DateTime.Now;
                                element.Synonyms = dataElement.Synonyms ?? string.Empty;
                                element.Source = dataElement.Source ?? string.Empty;
                                element.Status = "Proposed";
                                element.StatusDate = DateTime.Now;
                                element.Editor = dataElement.Editor ?? string.Empty;
                                element.Modality = dataElement.Modality != null && dataElement.Modality.Any() ? string.Join(",", dataElement.Modality) : null;
                                element.BiologicalSex = dataElement.BiologicalSex != null && dataElement.BiologicalSex.Any() ? string.Join(",", dataElement.BiologicalSex) : null;
                                element.AgeLowerBound = dataElement.AgeLowerBound;
                                element.AgeUpperBound = dataElement.AgeUpperBound;
                                element.ValueSize = 0;

                                if (dataElement.ValueType == DataElementType.Integer)
                                {
                                    element.ValueMin = dataElement.ValueMin;
                                    element.ValueMax = dataElement.ValueMax;
                                    element.StepValue = 1;
                                }

                                if (dataElement.ValueType == DataElementType.Numeric)
                                {
                                    float? minValue = null;
                                    float? maxValue = null;

                                    if (dataElement.ValueMin.HasValue)
                                    {
                                        minValue = Convert.ToSingle(dataElement.ValueMin.Value);
                                    }

                                    if (dataElement.ValueMax.HasValue)
                                    {
                                        maxValue = Convert.ToSingle(dataElement.ValueMax.Value);
                                    }

                                    element.ValueMin = minValue;
                                    element.ValueMax = maxValue;
                                    element.StepValue = 0.1f;
                                }

                                radElementDbContext.SaveChanges();

                                RemoveElementValuesIndexCodeReferences(element.Id);
                                RemoveElementValues((int)element.Id);
                                RemoveElementIndexCodeReferences(element.Id);
                                RemovePersonReferences(element.Id);
                                RemoveOrganizationReferences(element.Id);

                                AddElementValues(dataElement.Options, element.Id);
                                AddElementIndexCodeReferences(element.Id, dataElement.IndexCodeReferences);
                                AddPersonReferences((int)element.Id, dataElement.Persons);
                                AddOrganizationReferences((int)element.Id, dataElement.Organizations);

                                transaction.Commit();

                                return await Task.FromResult(new JsonResult(string.Format("Element with set id '{0}' and element id '{1}' is updated.", setId, elementId), HttpStatusCode.OK));
                            }
                        }
                    }

                    return await Task.FromResult(new JsonResult(string.Format("No such element with set id '{0}' and element id '{1}'.", setId, elementId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'UpdateElement(string setId, string elementId, CreateUpdateElement dataElement)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Deletes the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteElement(string setId, string elementId)
        {
            using (var transaction = radElementDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (IsValidSetId(setId) && IsValidElementId(elementId))
                    {
                        int setInternalId = Convert.ToInt32(setId.Remove(0, 4));
                        int elementInternalId = Convert.ToInt32(elementId.Remove(0, 3));
                        var elementSetRef = radElementDbContext.ElementSetRef.Where(x => x.ElementSetId == setInternalId && x.ElementId == elementInternalId).FirstOrDefault();

                        if (elementSetRef != null)
                        {
                            RemoveElementValuesIndexCodeReferences((uint)elementInternalId);
                            RemoveElementValues(elementInternalId);
                            RemoveElementIndexCodeReferences((uint)elementInternalId);
                            RemoveElementSetReferences(elementSetRef);
                            RemovePersonReferences((uint)elementInternalId);
                            RemoveOrganizationReferences((uint)elementInternalId);
                            RemoveElement(elementInternalId);

                            radElementDbContext.SaveChanges();
                            transaction.Commit();

                            return await Task.FromResult(new JsonResult(string.Format("Element with set id '{0}' and element id '{1}' is deleted.", setId, elementId), HttpStatusCode.OK));
                        }
                    }
                    return await Task.FromResult(new JsonResult(string.Format("No such element with set id '{0}' and element id '{1}'.", setId, elementId), HttpStatusCode.NotFound));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(ex, "Exception in method 'DeleteElement(string setId, string elementId)'");
                    var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return await Task.FromResult(new JsonResult(exMessage, HttpStatusCode.InternalServerError));
                }
            }
        }

        /// <summary>
        /// Adds the element set references.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        private void AddElementSetReferences(int setId, int elementId)
        {
            ElementSetRef setRef = new ElementSetRef()
            {
                ElementSetId = setId,
                ElementId = elementId
            };

            radElementDbContext.ElementSetRef.Add(setRef);
            radElementDbContext.SaveChanges();
        }

        /// <summary>
        /// Adds the element values.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="elementId">The element identifier.</param>
        private void AddElementValues(List<Option> options, uint elementId)
        {
            if (options != null && options.Any())
            {
                foreach (Option option in options)
                {
                    ElementValue elementvalue = new ElementValue()
                    {
                        ElementId = elementId,
                        Value = option.Value ?? "",
                        Name = option.Name ?? "",
                        Definition = option.Definition ?? "",
                        Images = option.Images ?? ""
                    };

                    radElementDbContext.ElementValue.Add(elementvalue);
                    radElementDbContext.SaveChanges();

                    AddElementValuesIndexCodeReferences(elementvalue.Id, option.IndexCodeReferences);
                }
            }
        }

        /// <summary>
        /// Adds the person references.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="personRefs">The person refs.</param>
        private void AddPersonReferences(int elementId, List<PersonDetails> personRefs)
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
                                var setRef = new PersonRoleElementRef()
                                {
                                    ElementID = elementId,
                                    PersonID = personRef.PersonId,
                                    Role = role.ToString()
                                };

                                radElementDbContext.PersonRoleElementRef.Add(setRef);
                                radElementDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var setRef = new PersonRoleElementRef()
                            {
                                ElementID = elementId,
                                PersonID = personRef.PersonId
                            };

                            radElementDbContext.PersonRoleElementRef.Add(setRef);
                            radElementDbContext.SaveChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the organization references.
        /// </summary>
        /// <param name="elementId">The set identifier.</param>
        /// <param name="orgRefs">The org refs.</param>
        private void AddOrganizationReferences(int elementId, List<OrganizationDetails> orgRefs)
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
                                var setRef = new OrganizationRoleElementRef()
                                {
                                    ElementID = elementId,
                                    OrganizationID = orgRef.OrganizationId,
                                    Role = role.ToString()
                                };

                                radElementDbContext.OrganizationRoleElementRef.Add(setRef);
                                radElementDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var setRef = new OrganizationRoleElementRef()
                            {
                                ElementID = elementId,
                                OrganizationID = orgRef.OrganizationId
                            };

                            radElementDbContext.OrganizationRoleElementRef.Add(setRef);
                            radElementDbContext.SaveChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the element index code references.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="codeReferences">The code references.</param>
        private void AddElementIndexCodeReferences(uint elementId, List<IndexCodeReference> codeReferences)
        {
            if (codeReferences != null && codeReferences.Any())
            {
                foreach (var codeReference in codeReferences)
                {
                    int codeId = 0;
                    var indexCodeSystem = radElementDbContext.IndexCodeSystem.Where(x => string.Equals(x.Abbrev, codeReference.System, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (indexCodeSystem != null)
                    {
                        var indexCode = radElementDbContext.IndexCode.Where(x => string.Equals(x.System, indexCodeSystem.Abbrev, StringComparison.InvariantCultureIgnoreCase) &&
                                                                                 string.Equals(x.Code, codeReference.Code, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (indexCode != null)
                        {
                            codeId = indexCode.Id;
                        }
                        else
                        {
                            var indexCodeSys = new IndexCode
                            {
                                Code = codeReference.Code,
                                System = indexCodeSystem.Abbrev,
                                Display = codeReference.Display,
                                AccessionDate = DateTime.UtcNow
                            };
                            radElementDbContext.IndexCode.Add(indexCodeSys);
                            radElementDbContext.SaveChanges();

                            codeId = indexCodeSys.Id;
                        }

                        var elementIndexCode = new IndexCodeElementRef
                        {
                            ElementId = elementId,
                            CodeId = codeId
                        };

                        radElementDbContext.IndexCodeElementRef.Add(elementIndexCode);
                        radElementDbContext.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Adds the element values index code references.
        /// </summary>
        /// <param name="elementValueId">The element value identifier.</param>
        /// <param name="codeReferences">The code references.</param>
        private void AddElementValuesIndexCodeReferences(int elementValueId, List<IndexCodeReference> codeReferences)
        {
            if (codeReferences != null && codeReferences.Any())
            {
                foreach (var codeReference in codeReferences)
                {
                    int codeId = 0;
                    var indexCodeSystem = radElementDbContext.IndexCodeSystem.Where(x => string.Equals(x.Abbrev, codeReference.System, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (indexCodeSystem != null)
                    {
                        var indexCode = radElementDbContext.IndexCode.Where(x => string.Equals(x.System, indexCodeSystem.Abbrev, StringComparison.InvariantCultureIgnoreCase) &&
                                                                                 string.Equals(x.Code, codeReference.Code, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (indexCode != null)
                        {
                            codeId = indexCode.Id;
                        }
                        else
                        {
                            var indexCodeSys = new IndexCode
                            {
                                Code = codeReference.Code,
                                System = indexCodeSystem.Abbrev,
                                Display = codeReference.Display,
                                AccessionDate = DateTime.UtcNow
                            };

                            radElementDbContext.IndexCode.Add(indexCodeSys);
                            radElementDbContext.SaveChanges();

                            codeId = indexCodeSys.Id;
                        }

                        var elementValueIndexCode = new IndexCodeElementValueRef
                        {
                            ElementValueId = elementValueId,
                            CodeId = codeId
                        };

                        radElementDbContext.IndexCodeElementValueRef.Add(elementValueIndexCode);
                        radElementDbContext.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Removes the element.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        private void RemoveElement(int elementId)
        {
            var element = radElementDbContext.Element.Where(x => x.Id == elementId).FirstOrDefault();
            if (element != null)
            {
                radElementDbContext.Element.Remove(element);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the element values.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        private void RemoveElementValues(int elementId)
        {
            var elementValues = radElementDbContext.ElementValue.Where(x => x.ElementId == elementId).ToList();
            if (elementValues != null && elementValues.Any())
            {
                radElementDbContext.ElementValue.RemoveRange(elementValues);
                radElementDbContext.SaveChanges();
            };
        }

        /// <summary>
        /// Removes the element set references.
        /// </summary>
        /// <param name="setRef">The set reference.</param>
        private void RemoveElementSetReferences(ElementSetRef setRef)
        {
            radElementDbContext.ElementSetRef.Remove(setRef);
            radElementDbContext.SaveChanges();
        }

        /// <summary>
        /// Removes the person elements references.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        private void RemovePersonReferences(uint elementId)
        {
            var personElementsRefs = radElementDbContext.PersonRoleElementRef.Where(x => x.ElementID == elementId).ToList();
            if (personElementsRefs != null && personElementsRefs.Any())
            {
                radElementDbContext.PersonRoleElementRef.RemoveRange(personElementsRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the organization element references.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        private void RemoveOrganizationReferences(uint elementId)
        {
            var organizationElementsRefs = radElementDbContext.OrganizationRoleElementRef.Where(x => x.ElementID == elementId).ToList();
            if (organizationElementsRefs != null && organizationElementsRefs.Any())
            {
                radElementDbContext.OrganizationRoleElementRef.RemoveRange(organizationElementsRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the element index code references.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        private void RemoveElementIndexCodeReferences(uint elementId)
        {
            var indexCodeElementRefs = radElementDbContext.IndexCodeElementRef.Where(x => x.ElementId == elementId).ToList();
            if (indexCodeElementRefs != null && indexCodeElementRefs.Any())
            {
                radElementDbContext.IndexCodeElementRef.RemoveRange(indexCodeElementRefs);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the element values index code references.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        private void RemoveElementValuesIndexCodeReferences(uint elementId)
        {
            var elementValueIndexCodes = (from elementValue in radElementDbContext.ElementValue
                                          join elementIndexCode in radElementDbContext.IndexCodeElementValueRef on elementValue.Id equals (int)elementIndexCode.ElementValueId
                                          where elementValue.ElementId == (uint)elementId
                                          select elementIndexCode).Distinct().ToList();
            if (elementValueIndexCodes != null && elementValueIndexCodes.Any())
            {
                radElementDbContext.IndexCodeElementValueRef.RemoveRange(elementValueIndexCodes);
                radElementDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Determines whether [is valid element identifier] [the specified element identifier].
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is valid element identifier] [the specified element identifier]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidElementId(string elementId)
        {
            if (elementId.Length > 3 && string.Equals(elementId.Substring(0, 3), "RDE", StringComparison.InvariantCultureIgnoreCase))
            {
                bool result = int.TryParse(elementId.Remove(0, 3), out _);
                return result;
            }

            return false;
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
        /// Gets the element details dto1.
        /// </summary>
        /// <param name="filteredElements">The filtered elements.</param>
        /// <param name="isSingleElement">if set to <c>true</c> [is single element].</param>
        /// <returns></returns>
        private object GetElementDetailsDto(List<FilteredData> filteredElements, bool isSingleElement)
        {
            var elements = new List<ElementDetails>();
            foreach (var elem in filteredElements)
            {
                if (!elements.Exists(x => x.Id == elem.Element.Id))
                {
                    var element = mapper.Map<ElementDetails>(elem.Element);
                    if (elem.ElementSet != null)
                    {
                        element.SetInformation = new List<SetBasicAttributes>();
                        var elementSet = mapper.Map<ElementSetDetails>(elem.ElementSet);
                        var setAttributes = new SetBasicAttributes { SetId = elementSet.SetId, SetName = elementSet.Name };
                        element.SetInformation.Add(setAttributes);
                    }
                    if (elem.ElementValue != null && element.ValueType == "valueSet")
                    {
                        var elementValue = mapper.Map<ElementValueAttributes>(elem.ElementValue);
                        if (elem.ElementValueIndexCode != null)
                        {
                            elementValue.IndexCodes = new List<IndexCode>();
                            elementValue.IndexCodes.Add(elem.ElementValueIndexCode);
                        }

                        element.ElementValues = new List<ElementValueAttributes>();
                        element.ElementValues.Add(elementValue);
                    }
                    if (elem.IndexCode != null)
                    {
                        element.IndexCodes = new List<IndexCode>();
                        element.IndexCodes.Add(elem.IndexCode);
                    }
                    if (elem.Person != null)
                    {
                        var person = mapper.Map<PersonAttributes>(elem.Person);
                        if (!string.IsNullOrEmpty(elem.PersonRole))
                        {
                            person.Roles.Add(elem.PersonRole);
                        }

                        element.PersonInformation = new List<PersonAttributes>();
                        element.PersonInformation.Add(person);
                    }
                    if (elem.Organization != null)
                    {
                        var organization = mapper.Map<OrganizationAttributes>(elem.Organization);
                        if (!string.IsNullOrEmpty(elem.OrganizationRole))
                        {
                            organization.Roles.Add(elem.OrganizationRole);
                        }

                        element.OrganizationInformation = new List<OrganizationAttributes>();
                        element.OrganizationInformation.Add(organization);
                    }

                    elements.Add(element);
                }
                else
                {
                    var element = elements.Find(x => x.Id == elem.Element.Id);
                    if (elem.ElementSet != null)
                    {
                        var elementSet = mapper.Map<ElementSetDetails>(elem.ElementSet);
                        if (!element.SetInformation.Exists(x => x.SetId == elementSet.SetId))
                        {
                            if (element.SetInformation == null)
                            {
                                element.SetInformation = new List<SetBasicAttributes>();
                            }
                            var setAttributes = new SetBasicAttributes { SetId = elementSet.SetId, SetName = elementSet.Name };
                            element.SetInformation.Add(setAttributes);
                        }
                    }
                    if (elem.ElementValue != null && element.ValueType == "valueSet")
                    {
                        var elementValue = element.ElementValues.Find(x => x.Id == elem.ElementValue.Id);
                        if (elementValue != null)
                        {
                            if (elem.ElementValueIndexCode != null && !elementValue.IndexCodes.Exists(x => x.Id == elem.ElementValueIndexCode.Id))
                            {
                                if (elementValue.IndexCodes == null)
                                {
                                    elementValue.IndexCodes = new List<IndexCode>();
                                }
                                elementValue.IndexCodes.Add(elem.ElementValueIndexCode);
                            }
                        }
                        else
                        {
                            if (element.ElementValues == null)
                            {
                                element.ElementValues = new List<ElementValueAttributes>();
                            }

                            var mappedElementValue = mapper.Map<ElementValueAttributes>(elem.ElementValue);
                            mappedElementValue.IndexCodes = new List<IndexCode>();
                            if (elem.ElementValueIndexCode != null)
                            {
                                mappedElementValue.IndexCodes.Add(elem.ElementValueIndexCode);
                            }

                            element.ElementValues.Add(mappedElementValue);
                        }
                    }
                    if (elem.IndexCode != null)
                    {
                        if (!element.IndexCodes.Exists(x => x.Id == elem.IndexCode.Id))
                        {
                            if (element.IndexCodes == null)
                            {
                                element.IndexCodes = new List<IndexCode>();
                            }
                            element.IndexCodes.Add(elem.IndexCode);
                        }
                    }
                    if (elem.Person != null)
                    {
                        var person = element.PersonInformation.Find(x => x.Id == elem.Person.Id);
                        if (person != null)
                        {
                            if (!string.IsNullOrEmpty(elem.PersonRole) && !person.Roles.Exists(x => x == elem.PersonRole))
                            {
                                person.Roles.Add(elem.PersonRole);
                            }
                        }
                        else
                        {
                            var mappedPerson = mapper.Map<PersonAttributes>(elem.Person);
                            if (!string.IsNullOrEmpty(elem.PersonRole))
                            {
                                mappedPerson.Roles.Add(elem.PersonRole);
                            }
                            if (element.PersonInformation == null)
                            {
                                element.PersonInformation = new List<PersonAttributes>();
                            }
                            element.PersonInformation.Add(mappedPerson);
                        }
                    }
                    if (elem.Organization != null)
                    {
                        var organization = element.OrganizationInformation.Find(x => x.Id == elem.Organization.Id);
                        if (organization != null)
                        {
                            if (!string.IsNullOrEmpty(elem.OrganizationRole) && !organization.Roles.Exists(x => x == elem.OrganizationRole))
                            {
                                organization.Roles.Add(elem.OrganizationRole);
                            }
                        }
                        else
                        {
                            var mappedOrganization = mapper.Map<OrganizationAttributes>(elem.Organization);
                            if (!string.IsNullOrEmpty(elem.OrganizationRole))
                            {
                                mappedOrganization.Roles.Add(elem.OrganizationRole);
                            }
                            if (element.OrganizationInformation == null)
                            {
                                element.OrganizationInformation = new List<OrganizationAttributes>();
                            }
                            element.OrganizationInformation.Add(mappedOrganization);
                        }
                    }
                }
            }
            if (isSingleElement)
            {
                return elements.FirstOrDefault();
            }

            return elements;
        }

        /// <summary>
        /// Gets the type of the element value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private string GetElementValueType(DataElementType? type)
        {
            var valueType = string.Empty;

            switch (type)
            {
                case DataElementType.Choice:
                case DataElementType.MultiChoice:
                    valueType = "valueSet";
                    break;

                case DataElementType.Integer:
                    valueType = "integer";
                    break;

                case DataElementType.Numeric:
                    valueType = "float";
                    break;

                case DataElementType.DateTime:
                    valueType = "date";
                    break;

                case DataElementType.String:
                    valueType = "string";
                    break;
            }

            return valueType;
        }
    }
}
