using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Acr.Assist.RadElement.Core.Data;
using Acr.Assist.RadElement.Core.Domain;
using Acr.Assist.RadElement.Core.DTO;
using Acr.Assist.RadElement.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Acr.Assist.RadElement.Service
{
    public class ElementService : IElementService
    {
        private IRadElementDbContext radElementDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementService"/> class.
        /// </summary>
        /// <param name="radElementDbContext">The RAD element database context.</param>
        public ElementService(IRadElementDbContext radElementDbContext)
        {
            this.radElementDbContext = radElementDbContext;
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Element>> GetElements()
        {
            var elements = await radElementDbContext.Element.ToListAsync();
            return elements;
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<Element> GetElement(int elementId)
        {
            var elements = await radElementDbContext.Element.ToListAsync();
            return elements.Find(x => x.Id == elementId);
        }

        /// <summary>
        /// Gets the elements by set identifier.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <returns></returns>
        public async Task<List<Element>> GetElementsBySetId(int setId)
        {
            var setRefs = await radElementDbContext.ElementSetRef.ToListAsync();
            var elementIds = setRefs.FindAll(x => x.ElementSetId == setId);
            var elements = await radElementDbContext.Element.ToListAsync();

            var selectedElements = from elemetId in elementIds
                                   join element in elements on elemetId.Id equals (int)element.Id
                                   select element;

            return selectedElements.ToList();
        }

        /// <summary>
        /// Searches the element.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        public async Task<List<Element>> SearchElement(string searchKeyword)
        {
            if(!string.IsNullOrEmpty(searchKeyword))
            {
                var elements = await radElementDbContext.Element.ToListAsync();
                return elements.FindAll(x => x.Definition.ToLower().Contains(searchKeyword.ToLower()) || x.Editor.ToLower().Contains(searchKeyword.ToLower()) ||
                                             x.Instructions.ToLower().Contains(searchKeyword.ToLower()) || x.Name.ToLower().Contains(searchKeyword.ToLower()) ||
                                             x.Question.ToLower().Contains(searchKeyword.ToLower()) || x.References.ToLower().Contains(searchKeyword.ToLower()) ||
                                             x.ShortName.ToLower().Contains(searchKeyword.ToLower()) || x.Source.ToLower().Contains(searchKeyword.ToLower()) ||
                                             x.Synonyms.ToLower().Contains(searchKeyword.ToLower()));
            }

            return new List<Element>();
        }

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<ElementIdDetails> CreateElement(int setId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            int elementId = 0;
            var elementSets = await radElementDbContext.ElementSet.ToListAsync();
            var elementSet = elementSets.Find(x => x.Id == setId);

            if (elementSet != null)
            {
                if (elementType != DataElementType.Global)
                {
                    Element element = new Element()
                    {
                        Name = dataElement.Label,
                        ShortName = "",
                        Definition = dataElement.Definition ?? "",
                        MaxCardinality = 1,
                        MinCardinality = 1,
                        Source = "DSI TOUCH-AI",
                        Status = "Proposed",
                        StatusDate = DateTime.Now,
                        Editor = "",
                        Instructions = "",
                        Question = dataElement.Label ?? "",
                        References = "",
                        Synonyms = "",
                        VersionDate = DateTime.Now,
                        Version = "1",
                        ValueSize = 0,
                        Unit = ""
                    };

                    if (elementType == DataElementType.Integer)
                    {
                        element.ValueType = "integer";
                        element.ValueMin = dataElement.ValueMin;
                        element.ValueMax = dataElement.ValueMax;
                        element.StepValue = 1;
                    }

                    if (elementType == DataElementType.Choice)
                    {
                        element.ValueType = "valueSet";
                    }

                    if (elementType == DataElementType.MultiChoice)
                    {
                        element.ValueType = "valueSet";
                        element.MaxCardinality = (short)dataElement.Options.Count;
                    }

                    if (elementType == DataElementType.Numeric)
                    {
                        float? minValue = null;
                        if (dataElement.ValueMin.HasValue)
                        {
                            minValue = Convert.ToSingle(dataElement.ValueMin.Value);
                        }
                        float? maxValue = null;
                        if (dataElement.ValueMax.HasValue)
                        {
                            maxValue = Convert.ToSingle(dataElement.ValueMax.Value);
                        }
                        element.ValueType = "float";
                        element.ValueMin = minValue;
                        element.ValueMax = maxValue;
                        element.StepValue = 0.1f;
                    }

                    radElementDbContext.Element.Add(element);
                    radElementDbContext.SaveChanges();

                    elementId = (int)element.Id;

                    if (elementType == DataElementType.MultiChoice || elementType == DataElementType.Choice)
                    {
                        AddElementValues(dataElement.Options, element.Id);
                    }
                }

                ElementSetRef setRef = new ElementSetRef()
                {
                    ElementSetId = setId,
                    ElementId = (short)elementId
                };

                radElementDbContext.ElementSetRef.Add(setRef);
                radElementDbContext.SaveChanges();
            }

            return new ElementIdDetails() { ElementId = elementId == 0 ? string.Empty : elementId.ToString() };
        }

        /// <summary>
        /// Updates the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="dataElement">The data element.</param>
        /// <returns></returns>
        public async Task<bool> UpdateElement(int setId, int elementId, DataElementType elementType, CreateUpdateElement dataElement)
        {
            var elementSets = await radElementDbContext.ElementSet.ToListAsync();
            var elementSet = elementSets.Find(x => x.Id == setId);

            if (elementSet != null)
            {
                var element = radElementDbContext.Element.ToList().Find(x => x.Id == elementId);
                if (element != null)
                {
                    var elementValues = radElementDbContext.ElementValue.ToList().FindAll(x => x.ElementId == element.Id);
                    if (elementValues != null && elementValues.Any())
                    {
                        radElementDbContext.ElementValue.RemoveRange(elementValues);
                        AddElementValues(dataElement.Options, element.Id);
                    }

                    element.Name = dataElement.Label;
                    element.ShortName = "";
                    element.Definition = dataElement.Definition ?? "";
                    element.MaxCardinality = 1;
                    element.MinCardinality = 1;
                    element.Source = "DSI TOUCH-AI";
                    element.Status = "Proposed";
                    element.StatusDate = DateTime.Now;
                    element.Editor = "";
                    element.Instructions = "";
                    element.Question = dataElement.Label ?? "";
                    element.References = "";
                    element.Synonyms = "";
                    element.VersionDate = DateTime.Now;
                    element.Version = "1";
                    element.ValueSize = 0;
                    element.Unit = "";

                    if (elementType == DataElementType.Integer)
                    {
                        element.ValueType = "integer";
                        element.ValueMin = dataElement.ValueMin;
                        element.ValueMax = dataElement.ValueMax;
                        element.StepValue = 1;
                    }

                    if (elementType == DataElementType.Choice)
                    {
                        element.ValueType = "valueSet";
                    }

                    if (elementType == DataElementType.MultiChoice)
                    {
                        element.ValueType = "valueSet";
                        element.MaxCardinality = (short)dataElement.Options.Count;
                    }

                    if (elementType == DataElementType.Numeric)
                    {
                        float? minValue = null;
                        if (dataElement.ValueMin.HasValue)
                        {
                            minValue = Convert.ToSingle(dataElement.ValueMin.Value);
                        }
                        float? maxValue = null;
                        if (dataElement.ValueMax.HasValue)
                        {
                            maxValue = Convert.ToSingle(dataElement.ValueMax.Value);
                        }
                        element.ValueType = "float";
                        element.ValueMin = minValue;
                        element.ValueMax = maxValue;
                        element.StepValue = 0.1f;
                    }

                    return radElementDbContext.SaveChanges() > 0;
                }
            }

            return false;
        }

        /// <summary>
        /// Deletes the set.
        /// </summary>
        /// <param name="setId">The set identifier.</param>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteElement(int setId, int elementId)
        {
            var elementSetRefs = await radElementDbContext.ElementSetRef.ToListAsync();
            var elementSetRef = elementSetRefs.Find(x => x.ElementSetId == setId && x.ElementId == elementId);

            if (elementSetRef != null)
            {
                var elementValues = radElementDbContext.ElementValue.ToList().FindAll(x => x.ElementId == elementSetRef.ElementId);
                var element = radElementDbContext.Element.ToList().Find(x => x.Id == elementSetRef.ElementId);

                if (elementValues != null && elementValues.Any())
                {
                    radElementDbContext.ElementValue.RemoveRange(elementValues);
                }

                if (element != null)
                {
                    radElementDbContext.Element.Remove(element);
                }

                radElementDbContext.ElementSetRef.Remove(elementSetRef);
                return radElementDbContext.SaveChanges() > 0;
            }

            return false;
        }     
             
        private void AddElementValues(List<Core.DTO.Option> options, uint elementId)
        {
            foreach (Core.DTO.Option option in options)
            {
                ElementValue elementvalue = new ElementValue()
                {
                    Name = option.Label,
                    Value = option.Value,
                    Definition = option.Label,
                    ElementId = elementId,
                };

                radElementDbContext.ElementValue.Add(elementvalue);
                radElementDbContext.SaveChanges();
            }
        }
    }
}
