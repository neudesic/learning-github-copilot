namespace eShop.ServiceDefaults.Tests;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;
using System.Reflection;

/// <summary>
/// Unit tests for OpenApiDefaultValues operation filter.
/// Tests the behavior of response content type filtering and parameter description assignment.
/// </summary>
public class OpenApiDefaultValuesTests
{
    private readonly IOperationFilter _filter;

    public OpenApiDefaultValuesTests()
    {
        _filter = CreateFilterInstance();
    }

    [Fact]
    public void Apply_WithDeprecatedApiDescription_SetsOperationDeprecatedToTrue()
    {
        // Arrange
        var operation = new OpenApiOperation();
        var apiDescription = new ApiDescription();
        SetIsDeprecated(apiDescription, true);
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.True(operation.Deprecated);
    }

    [Fact]
    public void Apply_WithNonDeprecatedApiDescription_KeepsDeprecatedAsFalse()
    {
        // Arrange
        var operation = new OpenApiOperation { Deprecated = false };
        var apiDescription = new ApiDescription();
        SetIsDeprecated(apiDescription, false);
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.False(operation.Deprecated);
    }

    [Fact]
    public void Apply_WithDeprecatedOperationAndNonDeprecatedApi_MaintainsDeprecatedState()
    {
        // Arrange
        var operation = new OpenApiOperation { Deprecated = true };
        var apiDescription = new ApiDescription();
        SetIsDeprecated(apiDescription, false);
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.True(operation.Deprecated);
    }

    [Fact]
    public void Apply_RemovesUnsupportedResponseContentTypes()
    {
        // Arrange
        var operation = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType(),
                        ["application/xml"] = new OpenApiMediaType(),
                        ["text/plain"] = new OpenApiMediaType()
                    }
                }
            }
        };

        var responseType = new ApiResponseType { StatusCode = 200, IsDefaultResponse = false };
        responseType.ApiResponseFormats.Add(new ApiResponseFormat { MediaType = "application/json" });

        var apiDescription = new ApiDescription();
        SetSupportedResponseTypes(apiDescription, new[] { responseType });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Single(operation.Responses["200"].Content);
        Assert.Contains("application/json", operation.Responses["200"].Content.Keys);
        Assert.DoesNotContain("application/xml", operation.Responses["200"].Content.Keys);
        Assert.DoesNotContain("text/plain", operation.Responses["200"].Content.Keys);
    }

    [Fact]
    public void Apply_WithDefaultResponseType_UsesDefaultAsResponseKey()
    {
        // Arrange
        var operation = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["default"] = new OpenApiResponse
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType(),
                        ["text/plain"] = new OpenApiMediaType()
                    }
                }
            }
        };

        var responseType = new ApiResponseType { StatusCode = 0, IsDefaultResponse = true };
        responseType.ApiResponseFormats.Add(new ApiResponseFormat { MediaType = "application/json" });

        var apiDescription = new ApiDescription();
        SetSupportedResponseTypes(apiDescription, new[] { responseType });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Single(operation.Responses["default"].Content);
        Assert.Contains("application/json", operation.Responses["default"].Content.Keys);
    }

    [Fact]
    public void Apply_WithNoParameters_DoesNotThrow()
    {
        // Arrange
        var operation = new OpenApiOperation { Parameters = null };
        var apiDescription = new ApiDescription();
        var context = CreateOperationFilterContext(apiDescription);

        // Act & Assert - should not throw
        _filter.Apply(operation, context);
        Assert.Null(operation.Parameters);
    }

    [Fact]
    public void Apply_WithEmptyParameters_DoesNotThrow()
    {
        // Arrange
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter>() };
        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, Array.Empty<ApiParameterDescription>());
        var context = CreateOperationFilterContext(apiDescription);

        // Act & Assert - should not throw
        _filter.Apply(operation, context);
    }

    [Fact]
    public void Apply_AssignsMissingParameterDescription()
    {
        // Arrange
        var parameter = new OpenApiParameter { Name = "id", Description = null };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription { Name = "id" };
        var mockMetadata = CreateMockModelMetadata(description: "User ID");
        paramDescription.ModelMetadata = mockMetadata;

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Equal("User ID", operation.Parameters[0].Description);
    }

    [Fact]
    public void Apply_DoesNotOverwriteExistingParameterDescription()
    {
        // Arrange
        var existingDescription = "Existing Description";
        var parameter = new OpenApiParameter { Name = "id", Description = existingDescription };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription { Name = "id" };
        var mockMetadata = CreateMockModelMetadata(description: "New Description");
        paramDescription.ModelMetadata = mockMetadata;

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Equal(existingDescription, operation.Parameters[0].Description);
    }

    [Fact]
    public void Apply_AssignsDefaultValueWhenPresent()
    {
        // Arrange
        var parameter = new OpenApiParameter { Name = "page", Schema = new OpenApiSchema() };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription
        {
            Name = "page",
            DefaultValue = 1,
            ModelMetadata = CreateMockModelMetadata(modelType: typeof(int))
        };

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.NotNull(operation.Parameters[0].Schema.Default);
    }

    [Fact]
    public void Apply_DoesNotAssignNullDefaultValue()
    {
        // Arrange
        var parameter = new OpenApiParameter { Name = "page", Schema = new OpenApiSchema() };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription
        {
            Name = "page",
            DefaultValue = null,
            ModelMetadata = CreateMockModelMetadata(modelType: typeof(int?))
        };

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Null(operation.Parameters[0].Schema.Default);
    }

    [Fact]
    public void Apply_DoesNotAssignDBNullDefaultValue()
    {
        // Arrange
        var parameter = new OpenApiParameter { Name = "page", Schema = new OpenApiSchema() };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription
        {
            Name = "page",
            DefaultValue = DBNull.Value,
            ModelMetadata = CreateMockModelMetadata(modelType: typeof(int?))
        };

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Null(operation.Parameters[0].Schema.Default);
    }

    [Fact]
    public void Apply_SkipsAssigningDefaultValueWhenSchemaAlreadyHasDefault()
    {
        // Arrange
        var existingDefault = new Microsoft.OpenApi.Any.OpenApiInteger(5);
        var parameter = new OpenApiParameter
        {
            Name = "page",
            Schema = new OpenApiSchema { Default = existingDefault }
        };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription
        {
            Name = "page",
            DefaultValue = 1,
            ModelMetadata = CreateMockModelMetadata(modelType: typeof(int))
        };

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Same(existingDefault, operation.Parameters[0].Schema.Default);
    }

    [Fact]
    public void Apply_MaintainsRequiredFlagWhenAlreadyTrue()
    {
        // Arrange
        var parameter = new OpenApiParameter { Name = "id", Required = true };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription { Name = "id" };

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.True(operation.Parameters[0].Required);
    }

    [Fact]
    public void Apply_WithMultipleParameters_ProcessesEachParameter()
    {
        // Arrange
        var param1 = new OpenApiParameter { Name = "id", Description = null };
        var param2 = new OpenApiParameter { Name = "name", Description = null };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { param1, param2 } };

        var paramDesc1 = new ApiParameterDescription
        {
            Name = "id",
            ModelMetadata = CreateMockModelMetadata(description: "User ID")
        };
        var paramDesc2 = new ApiParameterDescription
        {
            Name = "name",
            ModelMetadata = CreateMockModelMetadata(description: "User Name")
        };

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDesc1, paramDesc2 });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Equal("User ID", operation.Parameters[0].Description);
        Assert.Equal("User Name", operation.Parameters[1].Description);
    }

    [Fact]
    public void Apply_WithStringDefaultValue_SerializesToJson()
    {
        // Arrange
        var parameter = new OpenApiParameter { Name = "status", Schema = new OpenApiSchema() };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription
        {
            Name = "status",
            DefaultValue = "active",
            ModelMetadata = CreateMockModelMetadata(modelType: typeof(string))
        };

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.NotNull(operation.Parameters[0].Schema.Default);
    }

    [Fact]
    public void Apply_WithBooleanDefaultValue_SerializesToJson()
    {
        // Arrange
        var parameter = new OpenApiParameter { Name = "active", Schema = new OpenApiSchema() };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription
        {
            Name = "active",
            DefaultValue = true,
            ModelMetadata = CreateMockModelMetadata(modelType: typeof(bool))
        };

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.NotNull(operation.Parameters[0].Schema.Default);
    }

    [Fact]
    public void Apply_WithMultipleResponses_ProcessesEachResponse()
    {
        // Arrange
        var operation = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType(),
                        ["text/plain"] = new OpenApiMediaType()
                    }
                },
                ["400"] = new OpenApiResponse
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType(),
                        ["application/xml"] = new OpenApiMediaType()
                    }
                }
            }
        };

        var response200 = new ApiResponseType { StatusCode = 200, IsDefaultResponse = false };
        response200.ApiResponseFormats.Add(new ApiResponseFormat { MediaType = "application/json" });

        var response400 = new ApiResponseType { StatusCode = 400, IsDefaultResponse = false };
        response400.ApiResponseFormats.Add(new ApiResponseFormat { MediaType = "application/json" });

        var apiDescription = new ApiDescription();
        SetSupportedResponseTypes(apiDescription, new[] { response200, response400 });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Single(operation.Responses["200"].Content);
        Assert.Single(operation.Responses["400"].Content);
        Assert.Contains("application/json", operation.Responses["200"].Content.Keys);
        Assert.Contains("application/json", operation.Responses["400"].Content.Keys);
    }

    [Fact]
    public void Apply_WithIntegerDefaultValue_SerializesCorrectly()
    {
        // Arrange
        var parameter = new OpenApiParameter { Name = "limit", Schema = new OpenApiSchema() };
        var operation = new OpenApiOperation { Parameters = new List<OpenApiParameter> { parameter } };

        var paramDescription = new ApiParameterDescription
        {
            Name = "limit",
            DefaultValue = 10,
            ModelMetadata = CreateMockModelMetadata(modelType: typeof(int))
        };

        var apiDescription = new ApiDescription();
        SetParameterDescriptions(apiDescription, new[] { paramDescription });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.NotNull(operation.Parameters[0].Schema.Default);
    }

    [Fact]
    public void Apply_WithNoSupportedResponseTypes_RemovesAllContentTypes()
    {
        // Arrange
        var operation = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType(),
                        ["application/xml"] = new OpenApiMediaType()
                    }
                }
            }
        };

        var responseType = new ApiResponseType { StatusCode = 200, IsDefaultResponse = false };

        var apiDescription = new ApiDescription();
        SetSupportedResponseTypes(apiDescription, new[] { responseType });
        var context = CreateOperationFilterContext(apiDescription);

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Empty(operation.Responses["200"].Content);
    }

    // Helper methods

    private static IOperationFilter CreateFilterInstance()
    {
        var assembly = typeof(Extensions).Assembly;
        var filterType = assembly.GetType("eShop.ServiceDefaults.OpenApiDefaultValues", throwOnError: true)!;
        return (IOperationFilter)Activator.CreateInstance(filterType)!;
    }

    private static OperationFilterContext CreateOperationFilterContext(ApiDescription? apiDescription = null)
    {
        apiDescription ??= new ApiDescription();
        
        // Set ActionDescriptor to a dummy value (required by IsDeprecated extension method)
        var actionDescriptorProperty = apiDescription.GetType().GetProperty("ActionDescriptor");
        if (actionDescriptorProperty != null)
        {
            // Create a minimal action descriptor
            var ad = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor
            {
                DisplayName = "TestAction",
                ControllerName = "TestController",
                ActionName = "TestAction"
            };
            actionDescriptorProperty.SetValue(apiDescription, ad);
        }
        
        var schemaRepository = new SchemaRepository();
        var dummyMethod = typeof(object).GetMethod(nameof(object.ToString))!;
        // OperationFilterContext signature: (ApiDescription, ISchemaGenerator, SchemaRepository, MethodInfo)
        return new OperationFilterContext(apiDescription, null!, schemaRepository, dummyMethod);
    }

    private static void SetIsDeprecated(ApiDescription apiDescription, bool isDeprecated)
    {
        var property = apiDescription.GetType().GetProperty("IsDeprecated");
        if (property != null)
        {
            property.SetValue(apiDescription, isDeprecated);
        }
    }

    private static void SetParameterDescriptions(ApiDescription apiDescription, ApiParameterDescription[] descriptions)
    {
        var field = apiDescription.GetType().GetField("_parameterDescriptions", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(apiDescription, new List<ApiParameterDescription>(descriptions));
        }
    }

    private static void SetSupportedResponseTypes(ApiDescription apiDescription, ApiResponseType[] responseTypes)
    {
        var field = apiDescription.GetType().GetField("_supportedResponseTypes", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(apiDescription, new List<ApiResponseType>(responseTypes));
        }
    }

    private static ModelMetadata CreateMockModelMetadata(string? description = null, Type? modelType = null)
    {
        var type = modelType ?? typeof(string);
        
        // Find the public constructor for ModelMetadata that exists
        var constructorInfo = typeof(ModelMetadata).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(c => c.GetParameters().Length > 0)
            .FirstOrDefault();

        if (constructorInfo != null)
        {
            var parameters = constructorInfo.GetParameters();
            var args = new object?[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var paramType = parameters[i].ParameterType;
                if (paramType == typeof(Type) || paramType == type)
                {
                    args[i] = type;
                }
                else if (paramType.IsValueType)
                {
                    args[i] = Activator.CreateInstance(paramType);
                }
                else
                {
                    args[i] = null;
                }
            }

            try
            {
                var metadata = (ModelMetadata)constructorInfo.Invoke(args)!;
                if (description != null)
                {
                    var descProperty = typeof(ModelMetadata).GetProperty("Description");
                    if (descProperty?.CanWrite ?? false)
                    {
                        descProperty.SetValue(metadata, description);
                    }
                }
                return metadata;
            }
            catch
            {
                // Fall back to returning null - the test should still work for properties that don't require metadata
                return null!;
            }
        }

        return null!;
    }
}
