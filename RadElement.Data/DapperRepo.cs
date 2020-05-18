using Dapper;
using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.DTO;
using RadElement.Core.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RadElement.Data
{
    public class DapperRepo: BaseRepository, IDapperRepo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperRepo"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        public DapperRepo(IConfigurationManager configurationManager): base(configurationManager) { }

        /// <summary>
        /// Searches the elements.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<List<Element>> SearchElementsDetails(string searchKeyword)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                const string searchElementsQuery = @"SELECT el.id, el.Name, el.shortName, el.definition, el.valueType, el.valueSize, el.valueMin, el.valueMax, 
                                                     el.stepValue, el.minCardinality, el.maxCardinality, el.unit, el.question, el.instructions,
                                                     el.references, el.version, el.versionDate, el.synonyms, el.source, el.status, el.statusDate,
                                                     el.editor, el.modality, el.biologicalSex, el.ageUpperBound, el.ageLowerBound
                                                     FROM element el WHERE el.name LIKE @searchKeyword";
                return (await connection.QueryAsync<Element>(searchElementsQuery, new { searchKeyword = '%' + searchKeyword + '%' })).AsList();
            }
        }

        /// <summary>
        /// Searches the elements.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<List<BasicElementDetails>> SearchBasicElementsDetails(string searchKeyword)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                const string searchElementsQuery = @"SELECT el.id, el.Name FROM element el WHERE el.name LIKE @searchKeyword";
                return (await connection.QueryAsync<BasicElementDetails>(searchElementsQuery, new { searchKeyword = '%' + searchKeyword + '%' })).AsList();
            }
        }

        /// <summary>
        /// Gets the element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<List<ElementSet>> GetSetsByElementId(int elementId)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                const string getSetQuery = @"SELECT eleSet.id, eleSet.name FROM elementset eleSet INNER JOIN elementsetref ref ON
                                             eleSet.id = ref.elementSetID WHERE ref.elementID = @elementId";
                return (await connection.QueryAsync<ElementSet>(getSetQuery, new { elementId })).AsList();
            }
        }

        /// <summary>
        /// Gets the element values by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<List<ElementValue>> GetElementValuesByElementId(int elementId)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                const string getElementValuesQuery = @"SELECT id, elementID, value, name, definition, images FROM elementvalue WHERE elementID = @elementId";
                return (await connection.QueryAsync<ElementValue>(getElementValuesQuery, new { elementId })).AsList();
            }
        }

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        public async Task<Person> GetPerson(int personId)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                const string getPersonQuery = @"SELECT id, name, orcid, url, twitterHandle FROM person WHERE id = @personId";
                return await connection.QueryFirstOrDefaultAsync<Person>(getPersonQuery, new { personId });
            }
        }

        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public async Task<Organization> GetOrganization(int organizationId)
            {
                using (var connection = GetConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    const string getOrganizationQuery = @"SELECT id, name, abbreviation, url, comment, twitterHandle, email FROM organization WHERE 
                                                      id = @organizationId";
                    return await connection.QueryFirstOrDefaultAsync<Organization>(getOrganizationQuery, new { organizationId });
                }
            }

        /// <summary>
        /// Gets the person roles by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<List<PersonRoleElementRef>> GetPersonRolesByElementId(int elementId)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                const string getPersonRolesQuery = @"SELECT id, personID, elementID, role FROM personroleelementref WHERE elementID = @elementId";
                return (await connection.QueryAsync<PersonRoleElementRef>(getPersonRolesQuery, new { elementId })).AsList();
            }
        }

        /// <summary>
        /// Gets the organization roles by element identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<List<OrganizationRoleElementRef>> GetOrganizationRolesByElementId(int elementId)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                const string getOrganizationRolesQuery = @"SELECT id, organizationID, elementID, role FROM organizationroleelementref
                                                                  WHERE elementID = @elementId";
                return (await connection.QueryAsync<OrganizationRoleElementRef>(getOrganizationRolesQuery, new { elementId })).AsList();
            }
        }
    }
}
