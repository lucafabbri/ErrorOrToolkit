using System.Text.Json;

namespace ErrorOr;

/// <summary>
/// The error or json serializer class
/// </summary>
public static class ErrorOrJsonSerializer
{
    /// <summary>
    /// Serializes the value
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="options">The options</param>
    /// <returns>An error or of string</returns>
    public static ErrorOr<string> Serialize(object value, JsonSerializerOptions? options = null)
    {
        try
        {
            if (value is null)
            {
                return Error.Validation("InvalidValue", "The value to serialize cannot be null.");
            }
            if (value is string strValue)
            {
                return strValue; // Return the string directly without serialization
            }
            return JsonSerializer.Serialize(value, options);
        }
        catch (NotSupportedException ex)
        {
            return Error.Conflict("UnsupportedOperation", ex.Message);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message, metadata: new Dictionary<string, object>
            {
                { "ValueType", value.GetType().FullName },
                { "Value", value }
            });
        }
    }

    /// <summary>
    /// Deserializes the json
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="json">The json</param>
    /// <param name="options">The options</param>
    /// <returns>An error or of t</returns>
    public static ErrorOr<T> Deserialize<T>(string json, JsonSerializerOptions? options = null)
    {
        try
        {
            var result = JsonSerializer.Deserialize<T>(json, options);

            if (result is null)
            {
                return Error.Unexpected(description: "Deserialized object is null.");
            }

            return result;
        }
        catch (JsonException ex)
        {
            return Error.Validation("InvalidJson", "The provided JSON is invalid.", metadata: new Dictionary<string, object>
            {
                { "JsonPath", ex.Path ?? "Unknown path" },
                { "LineNumber", ex.LineNumber },
                { "BytePositionInLine", ex.BytePositionInLine },
                { "Message", ex.Message },
                { "Data", ex.Data }
            });
        }
        catch (ArgumentNullException ex)
        {
            return Error.Validation("InvalidJson", "The provided JSON is null or empty.", metadata: new Dictionary<string, object>
            {
                { ex.ParamName ?? "Unknown", "The JSON string cannot be null or empty." }
            });
        }
        catch (ArgumentException ex)
        {
            return Error.Validation("InvalidJson", ex.Message);

        }
        catch (NotSupportedException ex)
        {
            return Error.Conflict("UnsupportedOperation", ex.Message);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }
}
