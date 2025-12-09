using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using eShop.EventBus.Abstractions;

namespace EventBus.Tests;

public class EventBusSubscriptionInfoTests
{
    [Fact]
    public void Constructor_ShouldInitializeEventTypesAsEmptyDictionary()
    {
        // Arrange & Act
        var subscriptionInfo = new EventBusSubscriptionInfo();

        // Assert
        Assert.NotNull(subscriptionInfo.EventTypes);
        Assert.IsType<Dictionary<string, Type>>(subscriptionInfo.EventTypes);
        Assert.Empty(subscriptionInfo.EventTypes);
    }

    [Fact]
    public void EventTypes_ShouldBeMutable()
    {
        // Arrange
        var subscriptionInfo = new EventBusSubscriptionInfo();
        var testType = typeof(string);
        var eventName = "TestEvent";

        // Act
        subscriptionInfo.EventTypes.Add(eventName, testType);

        // Assert
        Assert.Single(subscriptionInfo.EventTypes);
        Assert.Contains(eventName, subscriptionInfo.EventTypes.Keys);
        Assert.Equal(testType, subscriptionInfo.EventTypes[eventName]);
    }

    [Fact]
    public void EventTypes_ShouldAllowMultipleEntries()
    {
        // Arrange
        var subscriptionInfo = new EventBusSubscriptionInfo();
        var type1 = typeof(string);
        var type2 = typeof(int);
        var type3 = typeof(DateTime);

        // Act
        subscriptionInfo.EventTypes.Add("Event1", type1);
        subscriptionInfo.EventTypes.Add("Event2", type2);
        subscriptionInfo.EventTypes.Add("Event3", type3);

        // Assert
        Assert.Equal(3, subscriptionInfo.EventTypes.Count);
        Assert.Equal(type1, subscriptionInfo.EventTypes["Event1"]);
        Assert.Equal(type2, subscriptionInfo.EventTypes["Event2"]);
        Assert.Equal(type3, subscriptionInfo.EventTypes["Event3"]);
    }

    [Fact]
    public void JsonSerializerOptions_ShouldNotBeNull()
    {
        // Arrange & Act
        var subscriptionInfo = new EventBusSubscriptionInfo();

        // Assert
        Assert.NotNull(subscriptionInfo.JsonSerializerOptions);
        Assert.IsType<JsonSerializerOptions>(subscriptionInfo.JsonSerializerOptions);
    }

    [Fact]
    public void JsonSerializerOptions_ShouldHaveTypeInfoResolver()
    {
        // Arrange & Act
        var subscriptionInfo = new EventBusSubscriptionInfo();

        // Assert
        Assert.NotNull(subscriptionInfo.JsonSerializerOptions.TypeInfoResolver);
    }

    [Fact]
    public void MultipleInstances_ShouldHaveIndependentEventTypeDictionaries()
    {
        // Arrange
        var subscriptionInfo1 = new EventBusSubscriptionInfo();
        var subscriptionInfo2 = new EventBusSubscriptionInfo();

        // Act
        subscriptionInfo1.EventTypes.Add("Event1", typeof(string));
        subscriptionInfo2.EventTypes.Add("Event2", typeof(int));

        // Assert
        Assert.Single(subscriptionInfo1.EventTypes);
        Assert.Single(subscriptionInfo2.EventTypes);
        Assert.NotEqual(subscriptionInfo1.EventTypes, subscriptionInfo2.EventTypes);
    }

    [Fact]
    public void JsonSerializerOptions_ShouldBeConsistentAcrossInstances()
    {
        // Arrange & Act
        var subscriptionInfo1 = new EventBusSubscriptionInfo();
        var subscriptionInfo2 = new EventBusSubscriptionInfo();

        // Assert
        Assert.NotNull(subscriptionInfo1.JsonSerializerOptions);
        Assert.NotNull(subscriptionInfo2.JsonSerializerOptions);
        Assert.NotNull(subscriptionInfo1.JsonSerializerOptions.TypeInfoResolver);
        Assert.NotNull(subscriptionInfo2.JsonSerializerOptions.TypeInfoResolver);
    }

    [Fact]
    public void EventTypes_ShouldPersistAcrossMultipleCalls()
    {
        // Arrange
        var subscriptionInfo = new EventBusSubscriptionInfo();
        var type1 = typeof(string);
        var type2 = typeof(int);

        // Act
        subscriptionInfo.EventTypes.Add("Event1", type1);
        var count1 = subscriptionInfo.EventTypes.Count;
        subscriptionInfo.EventTypes.Add("Event2", type2);
        var count2 = subscriptionInfo.EventTypes.Count;

        // Assert
        Assert.Equal(1, count1);
        Assert.Equal(2, count2);
        Assert.Equal(type1, subscriptionInfo.EventTypes["Event1"]);
        Assert.Equal(type2, subscriptionInfo.EventTypes["Event2"]);
    }

    [Fact]
    public void EventTypes_ShouldSupportKeyUpdateWithDifferentType()
    {
        // Arrange
        var subscriptionInfo = new EventBusSubscriptionInfo();
        var initialType = typeof(string);
        var updatedType = typeof(int);

        // Act
        subscriptionInfo.EventTypes.Add("Event1", initialType);
        subscriptionInfo.EventTypes["Event1"] = updatedType;

        // Assert
        Assert.Equal(updatedType, subscriptionInfo.EventTypes["Event1"]);
    }

    [Fact]
    public void JsonSerializerOptions_ShouldBeConfigurable()
    {
        // Arrange
        var subscriptionInfo = new EventBusSubscriptionInfo();
        var originalPropertyNamingPolicy = subscriptionInfo.JsonSerializerOptions.PropertyNamingPolicy;

        // Act
        subscriptionInfo.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

        // Assert
        Assert.Equal(JsonNamingPolicy.CamelCase, subscriptionInfo.JsonSerializerOptions.PropertyNamingPolicy);
        Assert.NotEqual(originalPropertyNamingPolicy, subscriptionInfo.JsonSerializerOptions.PropertyNamingPolicy);
    }

    [Theory]
    [InlineData("SimpleEvent", typeof(object))]
    [InlineData("ComplexEvent", typeof(Dictionary<string, object>))]
    [InlineData("CollectionEvent", typeof(List<string>))]
    public void EventTypes_ShouldAcceptVariousTypeValues(string eventName, Type eventType)
    {
        // Arrange
        var subscriptionInfo = new EventBusSubscriptionInfo();

        // Act
        subscriptionInfo.EventTypes.Add(eventName, eventType);

        // Assert
        Assert.Contains(eventName, subscriptionInfo.EventTypes.Keys);
        Assert.Equal(eventType, subscriptionInfo.EventTypes[eventName]);
    }
}
