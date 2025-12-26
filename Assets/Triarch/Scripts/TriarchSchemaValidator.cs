using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Triarch
{
    public sealed class SchemaValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; } = new List<string>();
    }

    public static class TriarchSchemaValidator
    {
        public static SchemaValidationResult Validate(JToken data, JToken schema)
        {
            var result = new SchemaValidationResult();
            ValidateToken(data, schema, "$", result.Errors);
            return result;
        }

        private static void ValidateToken(JToken data, JToken schema, string path, List<string> errors)
        {
            if (schema == null)
            {
                return;
            }

            var schemaType = schema.Value<string>("type");
            if (!string.IsNullOrWhiteSpace(schemaType) && !MatchesType(data, schemaType))
            {
                errors.Add($"{path}: Expected {schemaType} but found {data?.Type.ToString() ?? "null"}.");
                return;
            }

            var enumValues = schema["enum"] as JArray;
            if (enumValues != null && enumValues.Count > 0)
            {
                var matches = enumValues.Any(value => JToken.DeepEquals(value, data));
                if (!matches)
                {
                    errors.Add($"{path}: Value '{data}' not in enum [{string.Join(", ", enumValues)}].");
                }
            }

            switch (schemaType)
            {
                case "object":
                    ValidateObject(data as JObject, schema as JObject, path, errors);
                    break;
                case "array":
                    ValidateArray(data as JArray, schema as JObject, path, errors);
                    break;
            }
        }

        private static void ValidateObject(JObject data, JObject schema, string path, List<string> errors)
        {
            if (data == null || schema == null)
            {
                return;
            }

            var required = schema["required"] as JArray;
            if (required != null)
            {
                foreach (var requiredToken in required)
                {
                    var requiredName = requiredToken.Value<string>();
                    if (!data.ContainsKey(requiredName))
                    {
                        errors.Add($"{path}: Missing required property '{requiredName}'.");
                    }
                }
            }

            var properties = schema["properties"] as JObject;
            if (properties == null)
            {
                return;
            }

            foreach (var property in properties)
            {
                if (data.TryGetValue(property.Key, out var value))
                {
                    ValidateToken(value, property.Value, $"{path}.{property.Key}", errors);
                }
            }
        }

        private static void ValidateArray(JArray data, JObject schema, string path, List<string> errors)
        {
            if (data == null || schema == null)
            {
                return;
            }

            var itemsSchema = schema["items"];
            if (itemsSchema == null)
            {
                return;
            }

            for (var index = 0; index < data.Count; index++)
            {
                ValidateToken(data[index], itemsSchema, $"{path}[{index}]", errors);
            }
        }

        private static bool MatchesType(JToken data, string schemaType)
        {
            if (data == null)
            {
                return false;
            }

            switch (schemaType)
            {
                case "object":
                    return data.Type == JTokenType.Object;
                case "array":
                    return data.Type == JTokenType.Array;
                case "string":
                    return data.Type == JTokenType.String;
                case "number":
                    return data.Type == JTokenType.Float || data.Type == JTokenType.Integer;
                case "integer":
                    return data.Type == JTokenType.Integer;
                case "boolean":
                    return data.Type == JTokenType.Boolean;
                default:
                    return true;
            }
        }
    }
}
