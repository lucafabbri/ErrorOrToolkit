using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace ErrorOrToolkit.Tests;

/// <summary>
/// The dumb class
/// </summary>
public class DumbClass
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the value of the age
    /// </summary>
    public int Age { get; set; }
}

/// <summary>
/// The error or json serializer tests class
/// </summary>
public class ErrorOrJsonSerializerTests
{
    /// <summary>
    /// Tests that serialize test
    /// </summary>
    [Fact()]
    public void SerializeTest()
    {
        // Arrange
        var value = new DumbClass { Name = "Test", Age = 30 };
        var options = new System.Text.Json.JsonSerializerOptions();
        // Act
        var result = ErrorOrJsonSerializer.Serialize(value, options);
        // Assert
        Assert.True(result.IsSuccess());
        Assert.IsType<string>(result.Value);
    }

    /// <summary>
    /// Tests that deserialize test
    /// </summary>
    [Fact()]
    public void DeserializeTest()
    {
        // Arrange
        var json = "{\"Name\":\"Test\",\"Age\":30}";
        var options = new System.Text.Json.JsonSerializerOptions();
        // Act
        var result = ErrorOrJsonSerializer.Deserialize<DumbClass>(json, options);
        // Assert
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Value);
        Assert.Equal("Test", result.Value.Name);
        Assert.Equal(30, result.Value.Age);
    }

    /// <summary>   
    /// Tests that deserialize with error test
    /// </summary>
    /// <param name="json">The json string to deserialize</param>
    /// <param name="options">The json serializer options</param>
    [Fact()]
    public void DeserializeWithErrorTest()
    {
        // Arrange
        var json = "Invalid JSON";
        var options = new System.Text.Json.JsonSerializerOptions();
        // Act
        var result = ErrorOrJsonSerializer.Deserialize<dynamic>(json, options);
        // Assert
        Assert.True(result.IsError);
        Assert.NotNull(result.Errors);
        Assert.Contains(result.Errors, e => e.Description.Contains("JSON"));
    }
    
    /// <summary>
    /// Tests that serialize with error test
    /// </summary>
    /// <param name="value">The value to serialize</param>
    /// <param name="options">The json serializer options</param>
    [Fact()]
    public void SerializeWithErrorTest()
    {
        // Arrange
        var value = new { Name = "Test", Age = 30 };
        var options = new System.Text.Json.JsonSerializerOptions();
        options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()); // Example of adding a converter
        // Act
        var result = ErrorOrJsonSerializer.Serialize(value, options);
        // Assert
        Assert.True(result.IsSuccess());
        Assert.IsType<string>(result.Value);
    }
}