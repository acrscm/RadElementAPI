using RadElement.Core.Domain;
using RadElement.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RadElement.Service.Tests.Mocks.Data
{
    public class MockDataContext
    {
        #region Data

        public static IQueryable<Element> elementsDB = new List<Element>()
            {
                new Element { Id = 340, Name = "Tumuor bed size change", ShortName = "", Definition = "Status of the tumor bed", ValueType = "valueSet",
                              ValueSize = 0, MinCardinality = 1, MaxCardinality = 1, Unit = "", Question = "Tumuor bed size change", Instructions = "",
                              References = "", Version = "1", VersionDate = DateTime.Now, Synonyms = "", Source = "DSI TOUCH-AI", Status = "Proposed",
                              StatusDate = DateTime.Now, Editor = "" },
                new Element { Id = 338, Name = "Approximate deployment angle", ShortName = "", Definition = "Angle to optimize visualization of the 3 cusps of the aortic valve (degrees)", ValueType = "float",
                              ValueSize = 0, StepValue = 1, MinCardinality = 1, MaxCardinality = 1, Unit = "", Question = "Approximate deployment angle", Instructions = "",
                              References = "", Version = "1", VersionDate = DateTime.Now, Synonyms = "", Source = "DSI TOUCH-AI", Status = "Proposed",
                              StatusDate = DateTime.Now, Editor = "" },
                new Element { Id = 307, Name = "Myocardial wall thickening", ShortName = "", Definition = "The  percent  myocardial  thickness  increase during  end  systole  relative  to end diastole (%)", ValueType = "integer",
                              ValueSize = 0, StepValue = 1, MinCardinality = 1, MaxCardinality = 1, Unit = "", Question = "Myocardial wall thickening", Instructions = "",
                              References = "", Version = "1", VersionDate = DateTime.Now, Synonyms = "", Source = "DSI TOUCH-AI", Status = "Proposed",
                              StatusDate = DateTime.Now, Editor = "" },
                  new Element { Id = 283, Name = "Presence of Valvular Calcifications", ShortName = "", Definition = "Determine the presence of valvular calcifications", ValueType = "valueSet",
                              ValueSize = 0, MinCardinality = 1, MaxCardinality = 1, Unit = "", Question = "Presence of Valvular Calcifications", Instructions = "",
                              References = "", Version = "1", VersionDate = DateTime.Now, Synonyms = "", Source = "DSI TOUCH-AI", Status = "Proposed",
                              StatusDate = DateTime.Now, Editor = "" },
            }.AsQueryable();

        public static IQueryable<ElementSetRef> elementSetRefDb = new List<ElementSetRef>()
            {
                new ElementSetRef { Id = 351, ElementId = 283, ElementSetId = 53 },
                new ElementSetRef { Id = 375, ElementId = 307, ElementSetId = 66 },
                new ElementSetRef { Id = 406, ElementId = 338, ElementSetId = 72 },
                new ElementSetRef { Id = 408, ElementId = 340, ElementSetId = 74 }
            }.AsQueryable();

        public static IQueryable<ElementSet> elementSetDb = new List<ElementSet>()
            {
                new ElementSet { Id = 53, Name = "Pulmonary Veins Mapping Preablation", Description = "This module describes the common data elements for pulmonary veins mapping preablation use case.",
                                 ContactName = "David Wymer, MD", Status = "Proposed" },
                new ElementSet { Id = 66, Name = "Left Ventricle Wall Thickening", Description = "This module describes the common elements for left ventricle wall thickening",
                                 ContactName = "Carlo De Cecco, MD, PhD", Status = "Proposed" },
                new ElementSet { Id = 72, Name = "TAVR Aortic Root Measurements", Description = "This module describes the common data elements for TAVR Aortic Root Measurements use case.",
                                 ContactName = "Kimberly Brockenbrough, MD", Status = "Proposed" },
                new ElementSet { Id = 74, Name = "Soft Tissue Tumor Bed Size Change", Description = "This module describes the common data elements for soft tissue tumor bed size change use case.",
                                 ContactName = "Jay Patti, MD", Status = "Proposed" },
            }.AsQueryable();

        public static IQueryable<ElementValue> elementValueDb = new List<ElementValue>()
            {
                new ElementValue { Id = 1001, ElementId = 283, Value = "0", Name = "0 Unknown", Definition = "0 Unknown" },
                new ElementValue { Id = 1002, ElementId = 283, Value = "1", Name = "1 Present", Definition = "1 Present" },
                new ElementValue { Id = 1003, ElementId = 283, Value = "2", Name = "2 Absent", Definition = "2 Absent" },
                new ElementValue { Id = 1007, ElementId = 304, Value = "0", Name = "0 No change in size", Definition = "0 No change in size" },
                new ElementValue { Id = 1008, ElementId = 340, Value = "1", Name = "1 Increase in size", Definition = "1 Increase in size" },
                new ElementValue { Id = 1009, ElementId = 340, Value = "2", Name = "2 Decrease in size", Definition = "2 Decrease in size" },
        }.AsQueryable();

        public static IQueryable<Person> personDb = new List<Person>()
            {
                new Person { Id = 1, Name = "Charles E. Kahn, Jr., MD, MS" },
                new Person { Id = 2, Name = "Adam Flanders, MD, MS" },
                new Person { Id = 3, Name = "Woojin Kim, MD" },
                new Person { Id = 4, Name = "Sumit Niogi, MD" }
        }.AsQueryable();

        public static IQueryable<PersonRoleElementRef> personElementRefDb = new List<PersonRoleElementRef>()
            {
                new PersonRoleElementRef { Id = 1, PersonID = 1, ElementID = 338, Role = PersonRole.Author.ToString() },
                new PersonRoleElementRef { Id = 1, PersonID = 1, ElementID = 338, Role = PersonRole.Editor.ToString() },
                new PersonRoleElementRef { Id = 2, PersonID = 2, ElementID = 340, Role = PersonRole.Contributor.ToString() },
                new PersonRoleElementRef { Id = 3, PersonID = 3, ElementID = 307, Role = PersonRole.Editor.ToString() },
                new PersonRoleElementRef { Id = 4, PersonID = 4, ElementID = 283, Role = PersonRole.Reviewer.ToString() },
        }.AsQueryable();

        public static IQueryable<PersonRoleElementSetRef> personElementSetRefDb = new List<PersonRoleElementSetRef>()
            {
                new PersonRoleElementSetRef { Id = 1, PersonID = 1, ElementSetID = 53, Role = PersonRole.Author.ToString() },
                new PersonRoleElementSetRef { Id = 1, PersonID = 1, ElementSetID = 53, Role = PersonRole.Editor.ToString() },
                new PersonRoleElementSetRef { Id = 2, PersonID = 2, ElementSetID = 66, Role = PersonRole.Contributor.ToString() },
                new PersonRoleElementSetRef { Id = 3, PersonID = 3, ElementSetID = 72, Role = PersonRole.Editor.ToString() },
                new PersonRoleElementSetRef { Id = 4, PersonID = 4, ElementSetID = 74, Role = PersonRole.Reviewer.ToString() },
        }.AsQueryable();

        public static IQueryable<Organization> organizationDb = new List<Organization>()
            {
                new Organization { Id = 1, Name = "American College of Radiology - Data Science Institute", Abbreviation = "ACR-DSI", Url = "http://www.acrdsi.org" },
                new Organization { Id = 2, Name = "American Society of Neuroradiology", Abbreviation = "ASNR" },
                new Organization { Id = 3, Name = "American College of Radiology", Abbreviation = "ACR", Url = "http://www.acr.org" },
                new Organization { Id = 4, Name = "Radiological Society of North America", Abbreviation = "RSNA" },
        }.AsQueryable();

        public static IQueryable<OrganizationRoleElementRef> organizationElementRefDb = new List<OrganizationRoleElementRef>()
            {
                new OrganizationRoleElementRef { Id = 1, OrganizationID = 1, ElementID = 338, Role = OrganizationRole.Author.ToString() },
                new OrganizationRoleElementRef { Id = 1, OrganizationID = 1, ElementID = 338, Role = OrganizationRole.Translator.ToString() },
                new OrganizationRoleElementRef { Id = 2, OrganizationID = 2, ElementID = 340, Role = OrganizationRole.Contributor.ToString() },
                new OrganizationRoleElementRef { Id = 3, OrganizationID = 3, ElementID = 307, Role = OrganizationRole.Translator.ToString() },
                new OrganizationRoleElementRef { Id = 4, OrganizationID = 4, ElementID = 283, Role = OrganizationRole.Reviewer.ToString() },
        }.AsQueryable();

        public static IQueryable<OrganizationRoleElementSetRef> organizationElementSetRefDb = new List<OrganizationRoleElementSetRef>()
            {
                new OrganizationRoleElementSetRef { Id = 1, OrganizationID = 1, ElementSetID = 53, Role = OrganizationRole.Author.ToString() },
                new OrganizationRoleElementSetRef { Id = 1, OrganizationID = 1, ElementSetID = 53, Role = OrganizationRole.Translator.ToString() },
                new OrganizationRoleElementSetRef { Id = 2, OrganizationID = 2, ElementSetID = 66, Role = OrganizationRole.Contributor.ToString() },
                new OrganizationRoleElementSetRef { Id = 3, OrganizationID = 3, ElementSetID = 72, Role = OrganizationRole.Translator.ToString() },
                new OrganizationRoleElementSetRef { Id = 4, OrganizationID = 4, ElementSetID = 74, Role = OrganizationRole.Reviewer.ToString() },
        }.AsQueryable();

        public static IQueryable<IndexCodeSystem> indexCodeSystemDb = new List<IndexCodeSystem>()
            {
                new IndexCodeSystem { Abbrev = "DICOM", Name = "Digitial Imaging and Communications in Medicine", Oid = "1.2.840.10008",
                                      SystemURL = "http://dicom.nema.org/", CodeURL = "http://dicom.nema.org/" },
                new IndexCodeSystem { Abbrev = "LOINC", Name = "Logical Observation Identifiers Names and Codes", Oid = "2.16.840.1.113883.6.1",
                                      SystemURL = "http://bioportal.bioontology.org/ontologies/LOINC", CodeURL = "http://purl.bioontology.org/ontology/LNC/$code" },
                new IndexCodeSystem { Abbrev = "RADLEX", Name = "Radiology Lexicon", Oid = "2.16.840.1.113883.6.256",
                                      SystemURL = "http://bioportal.bioontology.org/ontologies/RADLEX", CodeURL = "http://www.radlex.org/RID/$code" },
                new IndexCodeSystem { Abbrev = "SNOMEDCT", Name = "Systematized Nomenclature of Medicine - Clinical Terms", Oid = "2.16.840.1.113883.6.96",
                                      SystemURL = "http://bioportal.bioontology.org/ontologies/SNOMEDCT", CodeURL = "http://purl.bioontology.org/ontology/SNOMEDCT/$code" },
            }.AsQueryable();

        public static IQueryable<IndexCode> indexCodeDb = new List<IndexCode>()
            {
                new IndexCode { Id= 1, Code = "RID28662", System = "RADLEX", Display = "Attenuation", AccessionDate = DateTime.UtcNow },
                new IndexCode { Id= 2,  Code = "RID11086", System = "RADLEX", Display = "Unenhanced phase", AccessionDate = DateTime.UtcNow },
                new IndexCode { Id= 3, Code = "LP35056-8", System = "LOINC", Display = "Left", AccessionDate = DateTime.UtcNow},
                new IndexCode { Id= 4, Code = "7771000", System = "SNOMEDCT", Display = "Left", AccessionDate = DateTime.UtcNow }
            }.AsQueryable();

        public static IQueryable<IndexCodeElementRef> indexCodeElementDb = new List<IndexCodeElementRef>()
            {
                new IndexCodeElementRef { Id = 1, CodeId = 1, ElementId = 340  },
                new IndexCodeElementRef { Id = 2, CodeId = 2, ElementId = 338  },
                new IndexCodeElementRef { Id = 3, CodeId = 3, ElementId = 307  },
                new IndexCodeElementRef { Id = 4, CodeId = 4, ElementId = 283  }
            }.AsQueryable();

        public static IQueryable<IndexCodeElementSetRef> indexCodeElementSetDb = new List<IndexCodeElementSetRef>()
            {
                new IndexCodeElementSetRef { Id = 1, CodeId = 1, ElementSetId = 53  },
                new IndexCodeElementSetRef { Id = 2, CodeId = 2, ElementSetId = 66  },
                new IndexCodeElementSetRef { Id = 3, CodeId = 3, ElementSetId = 62  },
                new IndexCodeElementSetRef { Id = 4, CodeId = 4, ElementSetId = 64  }
            }.AsQueryable();

        public static IQueryable<IndexCodeElementValueRef> indexCodeElementValueDb = new List<IndexCodeElementValueRef>()
            {
                new IndexCodeElementValueRef { Id = 1, CodeId = 1, ElementValueId = 1001  },
                new IndexCodeElementValueRef { Id = 2, CodeId = 2, ElementValueId = 1002  },
                new IndexCodeElementValueRef { Id = 3, CodeId = 3, ElementValueId = 1003  },
                new IndexCodeElementValueRef { Id = 4, CodeId = 4, ElementValueId = 1007  }
            }.AsQueryable();

        #endregion
    }
}
