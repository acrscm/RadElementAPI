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
                   "1113ADBF-9F91-4756-8AD8-46805D348E65", //	Inflammatory-Sinus
                   "207780AA-B713-4E8C-BF9C-A6618100B651",	//Brain MS     
                    "910A514E-A029-484E-A473-D73AFDEF64FD", //	Epidural Spinal CC
                    "A2679F0F-DBF7-458F-820C-FA4CC51C6442", //	CT Stroke
                    "E5E37E05-6292-43AD-BCFF-7F0D3BAF2B2E"  ,//PituitaryMicroadenoma
                    "FA3EBD1C-948E-4E42-AE0D-BFB6AD71CE21"	//Lymphnodes
                };

                foreach (string cdeassistModule in cdeAssistModules)
                {

                    //Get the module info from Assist db
                    var cdemodule = await this.moduleService.GetModule(new ModuleIdDetails() { ModuleId = cdeassistModule });

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
            Elementset set = new Elementset()
            {
                Name = cdemodule.ModuleName,
                Description = cdemodule.MetaData.Info.Description,
                Url  = "https://assist.acr.org/marval2/",
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
                AddCodeToDB( dataElement, elementId);

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

                if (data is IntegerElement)
                {
                    IntegerElement intElement = data as IntegerElement;
                    element.ValueType = "float";
                    element.ValueMin = intElement.MinimumValue;
                    element.ValueMax = intElement.MaximumValue;
                    element.StepValue = 0.1f;
                }
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