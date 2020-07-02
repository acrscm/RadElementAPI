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
                new ElementSetRef { Id = 352, ElementId = 283, ElementSetId = 66 },
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
                new ElementValue { Id = 1007, ElementId = 340, Value = "0", Name = "0 No change in size", Definition = "0 No change in size" },
                new ElementValue { Id = 1008, ElementId = 340, Value = "1", Name = "1 Increase in size", Definition = "1 Increase in size" },
                new ElementValue { Id = 1009, ElementId = 340, Value = "2", Name = "2 Decrease in size", Definition = "2 Decrease in size" },
                new ElementValue { Id = 1010, ElementId = 338, Value = "2", Name = "2 Decrease in size", Definition = "2 Decrease in size" },
                new ElementValue { Id = 1011, ElementId = 307, Value = "2", Name = "2 Decrease in size", Definition = "2 Decrease in size" }
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
                new PersonRoleElementRef { Id = 2, PersonID = 1, ElementID = 338, Role = PersonRole.Editor.ToString() },
                new PersonRoleElementRef { Id = 3, PersonID = 2, ElementID = 338, Role = PersonRole.Editor.ToString() },
                new PersonRoleElementRef { Id = 4, PersonID = 2, ElementID = 340, Role = PersonRole.Contributor.ToString() },
                new PersonRoleElementRef { Id = 5, PersonID = 3, ElementID = 307, Role = PersonRole.Editor.ToString() },
                new PersonRoleElementRef { Id = 6, PersonID = 4, ElementID = 283, Role = PersonRole.Reviewer.ToString() },
        }.AsQueryable();

        public static IQueryable<PersonRoleElementSetRef> personElementSetRefDb = new List<PersonRoleElementSetRef>()
            {
                new PersonRoleElementSetRef { Id = 1, PersonID = 1, ElementSetID = 53, Role = "" },
                new PersonRoleElementSetRef { Id = 2, PersonID = 1, ElementSetID = 53, Role = PersonRole.Author.ToString() },
                new PersonRoleElementSetRef { Id = 3, PersonID = 1, ElementSetID = 53, Role = PersonRole.Editor.ToString() },
                new PersonRoleElementSetRef { Id = 4, PersonID = 2, ElementSetID = 53, Role = PersonRole.Contributor.ToString() },
                new PersonRoleElementSetRef { Id = 4, PersonID = 2, ElementSetID = 66, Role = PersonRole.Contributor.ToString() },
                new PersonRoleElementSetRef { Id = 5, PersonID = 3, ElementSetID = 72, Role = PersonRole.Editor.ToString() },
                new PersonRoleElementSetRef { Id = 6, PersonID = 4, ElementSetID = 74, Role = PersonRole.Reviewer.ToString() },
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
                new OrganizationRoleElementRef { Id = 1, OrganizationID = 2, ElementID = 338, Role = OrganizationRole.Contributor.ToString() },
                new OrganizationRoleElementRef { Id = 2, OrganizationID = 2, ElementID = 340, Role = OrganizationRole.Contributor.ToString() },
                new OrganizationRoleElementRef { Id = 3, OrganizationID = 3, ElementID = 307, Role = OrganizationRole.Translator.ToString() },
                new OrganizationRoleElementRef { Id = 4, OrganizationID = 4, ElementID = 283, Role = OrganizationRole.Reviewer.ToString() },
        }.AsQueryable();

        public static IQueryable<OrganizationRoleElementSetRef> organizationElementSetRefDb = new List<OrganizationRoleElementSetRef>()
            {
                new OrganizationRoleElementSetRef { Id = 1, OrganizationID = 1, ElementSetID = 53, Role = OrganizationRole.Author.ToString() },
                new OrganizationRoleElementSetRef { Id = 1, OrganizationID = 1, ElementSetID = 53, Role = OrganizationRole.Translator.ToString() },
                new OrganizationRoleElementSetRef { Id = 1, OrganizationID = 2, ElementSetID = 53, Role = OrganizationRole.Author.ToString() },
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
                new IndexCode { Id= 4, Code = "7771000", System = "SNOMEDCT", Display = "Left", AccessionDate = DateTime.UtcNow },
                new IndexCode { Id= 5,  Code = "RID110861", System = "RADLEX", Display = "Unenhanced phase", AccessionDate = DateTime.UtcNow },
                new IndexCode { Id= 6, Code = "LP35056-81", System = "LOINC", Display = "Left", AccessionDate = DateTime.UtcNow},
                new IndexCode { Id= 7, Code = "77710001", System = "SNOMEDCT", Display = "Left", AccessionDate = DateTime.UtcNow }
            }.AsQueryable();

        public static IQueryable<IndexCodeElementRef> indexCodeElementDb = new List<IndexCodeElementRef>()
            {
                new IndexCodeElementRef { Id = 1, CodeId = 1, ElementId = 340  },
                new IndexCodeElementRef { Id = 2, CodeId = 2, ElementId = 340  },
                new IndexCodeElementRef { Id = 3, CodeId = 3, ElementId = 340  },
                new IndexCodeElementRef { Id = 4, CodeId = 1, ElementId = 338  },
                new IndexCodeElementRef { Id = 5, CodeId = 2, ElementId = 338  },
                new IndexCodeElementRef { Id = 6, CodeId = 1, ElementId = 307  },
                new IndexCodeElementRef { Id = 7, CodeId = 3, ElementId = 307  },
                new IndexCodeElementRef { Id = 8, CodeId = 4, ElementId = 283  }
            }.AsQueryable();

        public static IQueryable<IndexCodeElementSetRef> indexCodeElementSetDb = new List<IndexCodeElementSetRef>()
            {
                new IndexCodeElementSetRef { Id = 1, CodeId = 1, ElementSetId = 53  },
                new IndexCodeElementSetRef { Id = 2, CodeId = 2, ElementSetId = 53  },
                new IndexCodeElementSetRef { Id = 3, CodeId = 2, ElementSetId = 66  },
                new IndexCodeElementSetRef { Id = 4, CodeId = 3, ElementSetId = 72  },
                new IndexCodeElementSetRef { Id = 5, CodeId = 4, ElementSetId = 74  }
            }.AsQueryable();

        public static IQueryable<IndexCodeElementValueRef> indexCodeElementValueDb = new List<IndexCodeElementValueRef>()
            {
                new IndexCodeElementValueRef { Id = 1, CodeId = 1, ElementValueId = 1001  },
                new IndexCodeElementValueRef { Id = 2, CodeId = 2, ElementValueId = 1002  },
                new IndexCodeElementValueRef { Id = 3, CodeId = 3, ElementValueId = 1003  },
                new IndexCodeElementValueRef { Id = 4, CodeId = 4, ElementValueId = 1004  },
                new IndexCodeElementValueRef { Id = 5, CodeId = 5, ElementValueId = 1007  },
                new IndexCodeElementValueRef { Id = 6, CodeId = 6, ElementValueId = 1008  },
                new IndexCodeElementValueRef { Id = 7, CodeId = 1, ElementValueId = 1008  },
                new IndexCodeElementValueRef { Id = 8, CodeId = 7, ElementValueId = 1009  },
                new IndexCodeElementValueRef { Id = 9, CodeId = 1, ElementValueId = 1010  },
                new IndexCodeElementValueRef { Id = 10, CodeId = 2, ElementValueId = 1010  },
                new IndexCodeElementValueRef { Id = 11, CodeId = 3, ElementValueId = 1010  },
                new IndexCodeElementValueRef { Id = 12, CodeId = 7, ElementValueId = 1011  }
            }.AsQueryable();

        public static IQueryable<Reference> referenceDb = new List<Reference>()
            {
                new Reference { Id = 1, Citation = "citation 1"  },
                new Reference { Id = 2, Citation = "citation 1"  },
                new Reference { Id = 3, Citation = "citation 3"  },
                new Reference { Id = 4, Citation = "citation 4"  }
            }.AsQueryable();

        public static IQueryable<ReferenceRef> referenceRefDb = new List<ReferenceRef>()
            {
                new ReferenceRef { Id = 1, Reference_Id = 1, Reference_For_Id = 53, Reference_For_Type = "set" },
                new ReferenceRef { Id = 2, Reference_Id = 2, Reference_For_Id = 53, Reference_For_Type = "set" },
                new ReferenceRef { Id = 3, Reference_Id = 2, Reference_For_Id = 66, Reference_For_Type = "set"  },
                new ReferenceRef { Id = 4, Reference_Id = 3, Reference_For_Id = 72, Reference_For_Type = "set"  },
                new ReferenceRef { Id = 5, Reference_Id = 4, Reference_For_Id = 74, Reference_For_Type = "set"  },
                new ReferenceRef { Id = 6, Reference_Id = 1, Reference_For_Id = 340, Reference_For_Type = "element" },
                new ReferenceRef { Id = 7, Reference_Id = 2, Reference_For_Id = 340, Reference_For_Type = "element" },
                new ReferenceRef { Id = 8, Reference_Id = 2, Reference_For_Id = 338, Reference_For_Type = "element"  },
                new ReferenceRef { Id = 9, Reference_Id = 3, Reference_For_Id = 307, Reference_For_Type = "element"  },
                new ReferenceRef { Id = 10, Reference_Id = 4, Reference_For_Id = 283, Reference_For_Type = "element"  },
                new ReferenceRef { Id = 11, Reference_Id = 1, Reference_For_Id = 1001, Reference_For_Type = "element_value" },
                new ReferenceRef { Id = 12, Reference_Id = 2, Reference_For_Id = 1002, Reference_For_Type = "element_value"  },
                new ReferenceRef { Id = 13, Reference_Id = 3, Reference_For_Id = 1003, Reference_For_Type = "element_value"  },
                new ReferenceRef { Id = 14, Reference_Id = 4, Reference_For_Id = 1007, Reference_For_Type = "element_value"  },
                new ReferenceRef { Id = 15, Reference_Id = 1, Reference_For_Id = 1008, Reference_For_Type = "element_value" },
                new ReferenceRef { Id = 16, Reference_Id = 2, Reference_For_Id = 1008, Reference_For_Type = "element_value" },
                new ReferenceRef { Id = 17, Reference_Id = 2, Reference_For_Id = 1009, Reference_For_Type = "element_value"  },
                new ReferenceRef { Id = 18, Reference_Id = 3, Reference_For_Id = 1010, Reference_For_Type = "element_value"  },
                new ReferenceRef { Id = 19, Reference_Id = 4, Reference_For_Id = 1011, Reference_For_Type = "element_value"  },
            }.AsQueryable();

        public static IQueryable<Specialty> specialtyDb = new List<Specialty>()
            {
                new Specialty { Id = 1, Code = "BR", Short_Name = "Breast", Name = "Breast Imaging" },
                new Specialty { Id = 2, Code = "CA", Short_Name = "Cardiac", Name = "Cardiac Radiology" },
                new Specialty { Id = 3, Code = "CH", Short_Name = "Chest", Name = "Chest Radiology" },
                new Specialty { Id = 4, Code = "CT", Short_Name = "CT", Name = "Computed Tomography" }
            }.AsQueryable();

        public static IQueryable<SpecialtyElementSetRef> specialtyElementSetDb = new List<SpecialtyElementSetRef>()
            {
                new SpecialtyElementSetRef { Id = 1, SpecialtyId = 1, ElementSetId = 53  },
                new SpecialtyElementSetRef { Id = 2, SpecialtyId = 2, ElementSetId = 53  },
                new SpecialtyElementSetRef { Id = 3, SpecialtyId = 2, ElementSetId = 66  },
                new SpecialtyElementSetRef { Id = 4, SpecialtyId = 3, ElementSetId = 72  },
                new SpecialtyElementSetRef { Id = 5, SpecialtyId = 4, ElementSetId = 74  }
            }.AsQueryable();

        public static IQueryable<SpecialtyElementRef> specialtyElementDb = new List<SpecialtyElementRef>()
            {
                new SpecialtyElementRef { Id = 1, SpecialtyId = 1, ElementId = 340  },
                new SpecialtyElementRef { Id = 2, SpecialtyId = 2, ElementId = 340  },
                new SpecialtyElementRef { Id = 3, SpecialtyId = 3, ElementId = 340  },
                new SpecialtyElementRef { Id = 4, SpecialtyId = 1, ElementId = 338  },
                new SpecialtyElementRef { Id = 5, SpecialtyId = 2, ElementId = 338  },
                new SpecialtyElementRef { Id = 6, SpecialtyId = 1, ElementId = 307  },
                new SpecialtyElementRef { Id = 7, SpecialtyId = 3, ElementId = 307  },
                new SpecialtyElementRef { Id = 8, SpecialtyId = 4, ElementId = 283  }
            }.AsQueryable();

        public static IQueryable<Image> imagesDb = new List<Image>()
            {
                new Image { Id = 1, LocalUrl = "https://assist.acr.org", SourceUrl = "https://assist.acr.org", Caption = "Image 1",
                            Height = 0, Width = 0, Rights = "none"  },
                new Image { Id = 2, LocalUrl = "https://assist.acr.org", SourceUrl = "https://assist.acr.org", Caption = "Image 2",
                            Height = 0, Width = 0, Rights = "none"  },
                new Image { Id = 3, LocalUrl = "https://assist.acr.org", SourceUrl = "https://assist.acr.org", Caption = "Image 3",
                            Height = 0, Width = 0, Rights = "none"  },
                new Image { Id = 4, LocalUrl = "https://assist.acr.org", SourceUrl = "https://assist.acr.org", Caption = "Image 4",
                            Height = 0, Width = 0, Rights = "none"  }
            }.AsQueryable();

        public static IQueryable<ImageRef> imageRefDb = new List<ImageRef>()
            {
                new ImageRef { Id = 1, Image_Id = 1, Image_For_Id = 53, Image_For_Type = "set" },
                new ImageRef { Id = 2, Image_Id = 2, Image_For_Id = 53, Image_For_Type = "set" },
                new ImageRef { Id = 3, Image_Id = 2, Image_For_Id = 66, Image_For_Type = "set"  },
                new ImageRef { Id = 4, Image_Id = 3, Image_For_Id = 72, Image_For_Type = "set"  },
                new ImageRef { Id = 5, Image_Id = 4, Image_For_Id = 74, Image_For_Type = "set"  },
                new ImageRef { Id = 6, Image_Id = 1, Image_For_Id = 340, Image_For_Type = "element" },
                new ImageRef { Id = 7, Image_Id = 2, Image_For_Id = 340, Image_For_Type = "element" },
                new ImageRef { Id = 8, Image_Id = 2, Image_For_Id = 338, Image_For_Type = "element"  },
                new ImageRef { Id = 9, Image_Id = 3, Image_For_Id = 307, Image_For_Type = "element"  },
                new ImageRef { Id = 10, Image_Id = 4, Image_For_Id = 283, Image_For_Type = "element"  },
                new ImageRef { Id = 11, Image_Id = 1, Image_For_Id = 1001, Image_For_Type = "element_value" },
                new ImageRef { Id = 12, Image_Id = 2, Image_For_Id = 1002, Image_For_Type = "element_value"  },
                new ImageRef { Id = 13, Image_Id = 3, Image_For_Id = 1003, Image_For_Type = "element_value"  },
                new ImageRef { Id = 14, Image_Id = 4, Image_For_Id = 1007, Image_For_Type = "element_value"  },
                new ImageRef { Id = 15, Image_Id = 1, Image_For_Id = 1008, Image_For_Type = "element_value" },
                new ImageRef { Id = 16, Image_Id = 2, Image_For_Id = 1008, Image_For_Type = "element_value" },
                new ImageRef { Id = 17, Image_Id = 2, Image_For_Id = 1009, Image_For_Type = "element_value"  },
                new ImageRef { Id = 18, Image_Id = 3, Image_For_Id = 1010, Image_For_Type = "element_value"  },
                new ImageRef { Id = 19, Image_Id = 4, Image_For_Id = 1011, Image_For_Type = "element_value"  },
            }.AsQueryable();

        #endregion
    }
}
