// <copyright file="LiquidParser.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid
// Created: 2022-10-16 16:14
// </copyright>

using System.Dynamic;
using System.Xml;
using DotLiquid;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using AzureLiquid.Exceptions;
using AzureLiquid.Formatting;

namespace AzureLiquid
{
    /// <summary>
    /// Parses liquid templates using a JSON string, an object or an XML document.
    /// </summary>
    public class LiquidParser
    {
        private Hash? _data;

        private Template? _template;

        /// <summary>
        /// Parses the specified template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>The current instance to use for method chaining.</returns>
        public LiquidParser Parse(string? template)
        {
            _template = Template.Parse(template);
            return this;
        }

        /// <summary>
        /// Sets content using an object. Data will be available using the 'content' variable and the serialized object as JSON.
        /// </summary>
        /// <param name="instance">The object instance.</param>
        /// <param name="forceCamelCase">if set to <c>true</c> to force camel case in serialized output.</param>
        /// <returns>
        /// The current instance to use for method chaining.
        /// </returns>
        public LiquidParser SetContent(object? instance, bool forceCamelCase = false)
        {
            var settings = new JsonSerializerSettings();
            if (forceCamelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            return SetContentJson(JsonConvert.SerializeObject(instance, settings));
        }

        /// <summary>
        /// Sets the content JSON. Data will be available using the 'content' variable.
        /// </summary>
        /// <param name="json">The JSON data.</param>
        /// <returns>
        /// The current instance to use for method chaining.
        /// </returns>
        public LiquidParser SetContentJson(string json)
        {
            var content = "{ \"content\": " + json + " }";
            dynamic? expando = JsonConvert.DeserializeObject<ExpandoObject>(content, new PreserveCaseDeserializer(),
                new ExpandoObjectConverter());
            _data = Hash.FromDictionary(new Dictionary<string, object>(expando));
            return this;
        }

        /// <summary>
        /// Sets the content JSON using XML text data as source.  Data will be available using the 'content' variable.
        /// </summary>
        /// <param name="xml">The XML data as string.</param>
        /// <returns>The current instance to use for method chaining.</returns>
        public LiquidParser SetContentXml(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            return SetContentXml(document);
        }

        /// <summary>
        /// Sets the content JSON using an XML document as source.  Data will be available using the 'content' variable.
        /// </summary>
        /// <param name="document">The XML document.</param>
        /// <returns>The current instance to use for method chaining.</returns>
        public LiquidParser SetContentXml(XmlDocument document) =>
            SetContentJson(JsonConvert.SerializeXmlNode(document));


        /// <summary>
        /// Renders this instance using the parsed template and loaded data content.
        /// </summary>
        /// <returns>The output result.</returns>
        public string Render()
        {
            GuardRender();
            return _template!.Render(_data);
        }

        /// <summary>
        /// Guards rendering for bad inputs.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LiquidParserException">
        /// No data loaded. Call the SetContent* methods before calling Render.
        /// or
        /// No template loaded. Call the Parse method before calling Render.
        /// </exception>
        private void GuardRender()
        {
            if (_data is null)
            {
                throw new LiquidParserException("No data loaded. Call the SetContent* methods before calling Render.");
            }

            if (_template is null)
            {
                throw new LiquidParserException("No template loaded. Call the Parse method before calling Render.");
            }
        }
    }
}