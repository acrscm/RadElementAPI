using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Acr.Assist.Marval.Core.DTO;
using Microsoft.Extensions.Logging;
using Acr.Assist.Marval.Core.Services;
using System.Threading.Tasks;
using System;
using RadElementApi.Models;
using Serilog.Core;

namespace RadElementApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Database")]
    public class DatabaseController : Controller
    {
        // Get the CARDS modules from ASSIST database
        private readonly IModuleService moduleService;

        //Serilog instance
        protected ILogger<DatabaseController> LoggerInstance { get; set; }

        //Dbcontext for radelement db
        private radelementContext dbcontext;


        public DatabaseController(IModuleService moduleService, ILogger<DatabaseController> logger)
        {
            this.moduleService = moduleService;
            LoggerInstance = logger;
        }


        // POST: api/Database
        [HttpPost]
        public async Task Post()
        {
            try
            {
                dbcontext = new radelementContext();

                List<string> cdeAssistModules = new List<string>()
                {
                    "80785026-5023-40F2-BDA2-5BB2F17DF169" , // Accessory_Muscles_Causing_Neurovascular_Compromise
                    "BD9C136D-2008-4FDF-865A-8798C7F81017", //Acute_Appendicitis
                    "DEFF53AC-6F08-4788-90DF-029C0267AFB6" , //Cardiomegaly
                    "4C18DF65-3F6F-4AB0-9B8D-F2FED91E9D98" , //Classifying_Suspicious_Microcalcifications
                    "305E9905-1B43-4BC6-B9D6-14C1E5F8DD46" , //Colon_Polyp_Detection
                    "4494632A-6487-4A66-A6B6-5F6D5305F8C7" , //Extranodal_Extension
                    "46B4ADD9-9A32-4F70-893E-857CEFD06EB6" , //Flow_in_the_ascending_aorta
                    "68265D13-43F9-4DD2-87E8-7C18FE15872B" , //Hip_Osteolysis
                    "665B36B1-24B3-4DB0-84F3-225AEBD897B1" , //Left_Atrial_Enlargement
                    "C9CC21FE-BECF-4FBA-B274-AB760712EA65" , //Left_Ventricle_T1_Mapping_Quantification,
                    "99B5EE95-5C26-45AB-9C66-D222362B9590", //Left_Ventricle_Volume
                    "21A17B3B-397D-40AC-A299-55A7085DE34D" , //Left_Ventricle_Wall_Thickness
                    "1D66DAC4-3908-41EE-937B-4F6998D3DDB1", //Left_Ventricular_Late_Gadolinium_Enhancement_Assessment_for_MR
                    "B4E953BB-D1EF-4256-A801-374E567268F0", //Ligamentum_Teres_Injury
                    "BD9A7ED9-C3FA-4B39-993C-8063209C4955", //Midline_Shift
                    "534409C1-B825-4453-9761-EBB34682FF52",//Odontoid_Fracture
                    "C3CB6E1F-0860-4B49-A211-4C68B12AB48B", //Osteochondritis_Dissecans,
                    "3F79B9CE-6A1C-48BD-AB0F-B83275054C36" , //Pediatric_Elbow
                    "E5E15EC3-AB92-4542-BA06-87747A297DCD", // Periprosthetic_Hip_Fracture
                    "DBBB7953-73CA-4F13-B635-85D73B130BA1" , //Pneumothorax
                    "0A47E6D7-05FD-4ACD-88AE-5F56991F9773", //Radial_Head_Dislocation
                    "21095393-C591-4BDF-9EF2-2CEA65A8EDAF" , //Slipped_Capital_Femoral_Epiphysis
                    "100D6462-5AF0-47A1-B28D-42C2C5853031" , //Stener_Lesion
                    "95D60A3A-AA5A-4F56-B745-7DBC08974509" , //Tarsal_Coalition
                    "669D57B2-0941-46E2-94F3-AA29F3F0D57E", //Tarsometatarsal_Joint_Status
                    "B2E8DA74-2060-4A03-A30A-3FD368A90A7A" , //Trauma_Fracture
                    "A157E34D-961B-4896-BC58-635BEDA185C3", //A157E34D-961B-4896-BC58-635BEDA185C3
                    "F62C1110-33E3-4AB9-A49F-4BD63A5333F5",//Quantification of myocardial perfusion for CT
                    "F29EAF33-9FF2-4722-9E7A-A9EABC27A65E" , //Pulmonary Veins Mapping Preablation
                    "49BB9A8D-5547-4773-ABA7-7FD942948E14" , //Aortic Valve Analysis
                    "28EC5624-E268-4651-81C2-125F4B005D61", //Ascending Aortic Diameter
                    "1A19293F-B3BF-4B77-851B-84426400031F" , // Cardiac Output
                    "D9BEF9D7-B7D6-42F4-91B5-203B5CCD5FC2", //Cardiothoracic Ratio
                    "4CD716D3-EF8B-4802-A664-F6F0D20048B1", //Carina Angle Measurement
                    "CD222541-91C4-4FAA-B34B-50AB0F218567", //Chondral Bone Lesion Characterization
                    "472EA346-70E7-4593-A59B-4E5C1C949C3D", //Coronary Flow Reserve on PET
                    "AF415BE0-6FE2-41C2-ABDD-18169D212713" , // Flow in the pulmonary artery
                    "6003AC5A-A340-4C3E-BB97-A3CA756B2A48", // Hip Subsidence
                    "C77BC666-D1DB-416A-8D07-DE32DD8D6E55", // Left Atrial Size
                    "89EABFBC-27B7-4C94-B7F2-0D8951E65993", // Left Ventricle Myocardial Mass
                    "435B210D-5476-49DB-8000-6256010452BD", // Left Ventricle Wall Motion
                    "992C9A9A-269D-4E93-8454-69AD9AEE8546" , // Left Ventricle Wall Thickening
                    "D1038F99-9E4C-47D9-A6CC-ACA70A920E96", // Motor Cortex QSM
                    "1CC76550-CC48-4EC6-9123-66225A5A9F8F", // Periprosthetic Hip Lucency
                    "AC64649E-23C8-4048-97B3-5B0BD3A9CED0", // Pulmonary Artery Diameter
                    "DC3520CF-2F67-41C9-915F-237920FDDE2B",//Pulmonary artery to aortic diameter ratio
                    "E9BCFBF5-83C2-4AC0-A22A-2BAB8082B0F2", // Pulmonary to Systemic Flow Ratio
                    "8A6D5229-ED1A-4F37-A604-0874BC55AF07" // TAVR Aortic Root Measurements


             };


                
                foreach (var cdeassistModule in cdeAssistModules)
                {

                    //Get the module info from Assist db
                    var cdemodule = await this.moduleService.GetModule(new ModuleIdDetails() { ModuleId = cdeassistModule });
                    if (cdemodule == null)
                    {
                        Console.Write(cdemodule);
                        continue;
                    }

                    // Add all the elements, codes get the element Ids
                    List<uint> elementIds = AddElementsAndCodestoDB(cdemodule.DataElements);

                    AddElementSet(cdemodule, elementIds);
                }

                dbcontext.Dispose();


            }
            catch (Exception ex)
            {
                LoggerInstance.Log(LogLevel.Error, ex, "Failed to add record on db");
            }

        }

        private void AddElementSet(AssistModule cdemodule, List<uint> elementIds)
        {
            LoggerInstance.Log(LogLevel.Debug, cdemodule.ModuleId + ", " + cdemodule.ModuleName);
                    Elementset set = new Elementset()
            {
                Name = cdemodule.ModuleName,
                Description = cdemodule.MetaData.Info.Description,
                Url  = "https://assist.acr.org/marval/",
                ContactName = "Adam Flanders, MD", //TODO : change to contact under metadata
                Email = cdemodule.MetaData.Info.Contact.Email
            };

            // todo: Add ElementSet to  DB
            dbcontext.Elementset.Add(set);
            dbcontext.SaveChanges();


            foreach (short elementid in elementIds)
            {
                Elementsetref setref = new Elementsetref()
                {
                    ElementSetId = set.Id,
                    ElementId = elementid
                };

                dbcontext.Elementsetref.Add(setref);
                dbcontext.SaveChanges();
            }


        }

        private List<uint> AddElementsAndCodestoDB(List<DataElement> dataElements)
        {

            List<uint> elementIds = new List<uint>();

            foreach (DataElement dataElement in dataElements)
            {
                if (dataElement is GlobalValue)
                {
                    continue;
                }

                // Add the element
                uint elementId = AddElementToDB(dataElement);

                // Add code to db
               // AddCodeToDB( dataElement, elementId);

                elementIds.Add(elementId);

            }

            return elementIds;

        }

        private void AddCodeToDB( DataElement dataElement, uint elementId)
        {
           
            Code code = new Code()
            {
                AccessionDate = DateTime.Now,
                Code1 = "RID" + new Random().Next(10000, 99999).ToString(),
                Display = dataElement.Label,
                System = "RADELEMENT",
            };


            dbcontext.Code.Add(code);
            dbcontext.SaveChanges();

            short codeid = 0;

            Coderef codeRef = new Coderef()
            {
                CodeId = codeid,
                ElementId = elementId,
                ValueCode = dataElement.Label
            };
            dbcontext.Coderef.Add(codeRef);
            dbcontext.SaveChanges();

        }

        private uint AddElementToDB(DataElement data)
        {

            Element element = new Element()
            {
                Name = data.Label,
                ShortName = "",
                Definition = data.Label,
                MaxCardinality = 1,
                MinCardinality = 1,
                Source = "CAR/DS Neuro CDE",
                Status = "Active",
                StatusDate = DateTime.Now,
                Editor = "adam",
                Instructions = "",
                Question = data.Label,
                References = "",
                Synonyms = "",
                VersionDate = DateTime.Now,
                Version = "1",
                ValueSize = 0,
                Unit = ""
            };

            if (data is IntegerElement)
            {
                IntegerElement intElement = data as IntegerElement;
                element.ValueType = "integer";
                element.ValueMin = intElement.MinimumValue;
                element.ValueMax = intElement.MaximumValue;
                element.StepValue = 1;
            }


            if (data is ChoiceElement)
            {
                ChoiceElement choiceElement = data as ChoiceElement;
                element.ValueType = "valueSet";
                element.ValueSet = GetValueSet(choiceElement.Options);
            }

            if (data is MultipleChoiceElement)
            {
                MultipleChoiceElement choiceElement = data as MultipleChoiceElement;
                element.ValueType = "valueSet";
                element.ValueSet = GetValueSet(choiceElement.Options);
            }

           

            if (data is NumericElement)
            {
                NumericElement numericElement = data as NumericElement;
                float? minValue = null;
                if (numericElement.MinimumValue.HasValue)
                {
                    minValue = Convert.ToSingle(numericElement.MinimumValue.Value);
                }
                float? maxValue = null;
                if (numericElement.MaximumValue.HasValue)
                {
                    maxValue = Convert.ToSingle(numericElement.MaximumValue.Value);
                }
                element.ValueType = "float";
                element.ValueMin = minValue;
                element.ValueMax = maxValue;
                element.StepValue = 0.1f;
            }



            dbcontext.Element.Add(element);
            dbcontext.SaveChanges();


            if (data is ChoiceElement)
            {
                ChoiceElement choiceElement = data as ChoiceElement;
                AddElementValues(choiceElement.Options, element.Id);

            }

            if (data is MultipleChoiceElement)
            {
                MultipleChoiceElement choiceElement = data as MultipleChoiceElement;
                AddElementValues(choiceElement.Options, element.Id);

            }

            return element.Id;
        }

        private string GetValueSet(List<Option> options)
        {
            string valueSet = string.Empty;

            foreach (Option option in options)
            {
                valueSet += option.Label + "|";
            }

            return valueSet.TrimEnd('|');
        }


        private void AddElementValues(List<Option> options, uint elementId)
        {

            foreach (Option option in options)
            {
                Elementvalue elementvalue = new Elementvalue()
                {
                    Code = "",
                    Definition = option.Label,
                    ElementId = elementId,
                    Name = option.Label
                };

                dbcontext.Elementvalue.Add(elementvalue);
                dbcontext.SaveChanges();
            }


        }


        // PUT: api/Database/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/Database
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Database/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

    }

}