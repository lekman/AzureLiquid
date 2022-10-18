// <copyright file="PreserveCaseDeserializer.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid
// Created: 2022-10-18 07:46
// </copyright>

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureLiquid.Formatting
{
    /// <summary>
    /// Ensures all properties are deserialized without changing any casing.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class PreserveCaseDeserializer : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null!;
            }

            var target = new JObject();

            foreach (var property in JToken.Load(reader).Children())
            {
                target.Add(property);
            }

            return target.ToObject(objectType);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var o = (JObject)JToken.FromObject(value!);
            o.WriteTo(writer);
        }
    }
}