namespace Webhooks.API.Tests;

using Microsoft.EntityFrameworkCore;
using Webhooks.API.Infrastructure;
using Webhooks.API.Model;

public class WebhooksRetrieverTests : IDisposable
{
    private readonly WebhooksContext _dbContext;
    private readonly WebhooksRetriever _retriever;

    public WebhooksRetrieverTests()
    {
        var options = new DbContextOptionsBuilder<WebhooksContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new WebhooksContext(options);
        _retriever = new WebhooksRetriever(_dbContext);
    }

    [Fact]
    public async Task GetSubscriptionsOfType_WithCatalogItemPriceChangeType_ReturnsMatchingSubscriptions()
    {
        // Arrange
        var userId = "user-1";
        var sub1 = new WebhookSubscription
        {
            Type = WebhookType.CatalogItemPriceChange,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook1",
            Token = "token1",
            UserId = userId
        };
        var sub2 = new WebhookSubscription
        {
            Type = WebhookType.OrderShipped,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook2",
            Token = "token2",
            UserId = userId
        };
        var sub3 = new WebhookSubscription
        {
            Type = WebhookType.CatalogItemPriceChange,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook3",
            Token = "token3",
            UserId = "user-2"
        };

        _dbContext.Subscriptions.AddRange(sub1, sub2, sub3);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _retriever.GetSubscriptionsOfType(WebhookType.CatalogItemPriceChange);

        // Assert
        var subscriptions = result.ToList();
        Assert.Equal(2, subscriptions.Count);
        Assert.All(subscriptions, s => Assert.Equal(WebhookType.CatalogItemPriceChange, s.Type));
        Assert.Contains(sub1, subscriptions);
        Assert.Contains(sub3, subscriptions);
    }

    [Fact]
    public async Task GetSubscriptionsOfType_WithOrderShippedType_ReturnsMatchingSubscriptions()
    {
        // Arrange
        var sub1 = new WebhookSubscription
        {
            Type = WebhookType.OrderShipped,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook1",
            Token = "token1",
            UserId = "user-1"
        };
        var sub2 = new WebhookSubscription
        {
            Type = WebhookType.OrderPaid,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook2",
            Token = "token2",
            UserId = "user-1"
        };
        var sub3 = new WebhookSubscription
        {
            Type = WebhookType.OrderShipped,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook3",
            Token = "token3",
            UserId = "user-2"
        };

        _dbContext.Subscriptions.AddRange(sub1, sub2, sub3);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _retriever.GetSubscriptionsOfType(WebhookType.OrderShipped);

        // Assert
        var subscriptions = result.ToList();
        Assert.Equal(2, subscriptions.Count);
        Assert.All(subscriptions, s => Assert.Equal(WebhookType.OrderShipped, s.Type));
        Assert.Contains(sub1, subscriptions);
        Assert.Contains(sub3, subscriptions);
    }

    [Fact]
    public async Task GetSubscriptionsOfType_WithOrderPaidType_ReturnsMatchingSubscriptions()
    {
        // Arrange
        var sub1 = new WebhookSubscription
        {
            Type = WebhookType.OrderPaid,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook1",
            Token = "token1",
            UserId = "user-1"
        };
        var sub2 = new WebhookSubscription
        {
            Type = WebhookType.CatalogItemPriceChange,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook2",
            Token = "token2",
            UserId = "user-2"
        };

        _dbContext.Subscriptions.AddRange(sub1, sub2);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _retriever.GetSubscriptionsOfType(WebhookType.OrderPaid);

        // Assert
        var subscriptions = result.ToList();
        Assert.Single(subscriptions);
        Assert.Equal(WebhookType.OrderPaid, subscriptions[0].Type);
        Assert.Contains(sub1, subscriptions);
    }

    [Fact]
    public async Task GetSubscriptionsOfType_WithNoMatchingSubscriptions_ReturnsEmptyList()
    {
        // Arrange
        var sub1 = new WebhookSubscription
        {
            Type = WebhookType.OrderShipped,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook1",
            Token = "token1",
            UserId = "user-1"
        };

        _dbContext.Subscriptions.Add(sub1);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _retriever.GetSubscriptionsOfType(WebhookType.CatalogItemPriceChange);

        // Assert
        var subscriptions = result.ToList();
        Assert.Empty(subscriptions);
    }

    [Fact]
    public async Task GetSubscriptionsOfType_WithEmptyDatabase_ReturnsEmptyList()
    {
        // Act
        var result = await _retriever.GetSubscriptionsOfType(WebhookType.OrderPaid);

        // Assert
        var subscriptions = result.ToList();
        Assert.Empty(subscriptions);
    }

    [Fact]
    public async Task GetSubscriptionsOfType_WithMultipleSubscriptionsOfSameType_ReturnsAllMatching()
    {
        // Arrange
        var subscriptions = new List<WebhookSubscription>
        {
            new()
            {
                Type = WebhookType.OrderPaid,
                Date = DateTime.UtcNow,
                DestUrl = "https://example.com/webhook1",
                Token = "token1",
                UserId = "user-1"
            },
            new()
            {
                Type = WebhookType.OrderPaid,
                Date = DateTime.UtcNow,
                DestUrl = "https://example.com/webhook2",
                Token = "token2",
                UserId = "user-2"
            },
            new()
            {
                Type = WebhookType.OrderPaid,
                Date = DateTime.UtcNow,
                DestUrl = "https://example.com/webhook3",
                Token = "token3",
                UserId = "user-3"
            }
        };

        _dbContext.Subscriptions.AddRange(subscriptions);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _retriever.GetSubscriptionsOfType(WebhookType.OrderPaid);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);
        Assert.All(resultList, s => Assert.Equal(WebhookType.OrderPaid, s.Type));
    }

    [Fact]
    public async Task GetSubscriptionsOfType_PreservesSubscriptionProperties()
    {
        // Arrange
        var userId = "test-user-123";
        var destUrl = "https://example.com/webhook";
        var token = "test-token-abc";
        var date = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);

        var subscription = new WebhookSubscription
        {
            Type = WebhookType.CatalogItemPriceChange,
            Date = date,
            DestUrl = destUrl,
            Token = token,
            UserId = userId
        };

        _dbContext.Subscriptions.Add(subscription);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _retriever.GetSubscriptionsOfType(WebhookType.CatalogItemPriceChange);

        // Assert
        var retrieved = result.First();
        Assert.Equal(userId, retrieved.UserId);
        Assert.Equal(destUrl, retrieved.DestUrl);
        Assert.Equal(token, retrieved.Token);
        Assert.Equal(date, retrieved.Date);
    }

    [Fact]
    public async Task GetSubscriptionsOfType_ReturnsNewEnumerableInstanceEachCall()
    {
        // Arrange
        var sub = new WebhookSubscription
        {
            Type = WebhookType.OrderShipped,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook",
            Token = "token",
            UserId = "user-1"
        };

        _dbContext.Subscriptions.Add(sub);
        await _dbContext.SaveChangesAsync();

        // Act
        var result1 = await _retriever.GetSubscriptionsOfType(WebhookType.OrderShipped);
        var result2 = await _retriever.GetSubscriptionsOfType(WebhookType.OrderShipped);

        // Assert
        Assert.NotSame(result1, result2);
        Assert.Equal(result1, result2);
    }

    [Fact]
    public async Task GetSubscriptionsOfType_WithAllWebhookTypes_ReturnCorrectResults()
    {
        // Arrange
        var catalogSub = new WebhookSubscription
        {
            Type = WebhookType.CatalogItemPriceChange,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook1",
            Token = "token1",
            UserId = "user-1"
        };
        var shippedSub = new WebhookSubscription
        {
            Type = WebhookType.OrderShipped,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook2",
            Token = "token2",
            UserId = "user-2"
        };
        var paidSub = new WebhookSubscription
        {
            Type = WebhookType.OrderPaid,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook3",
            Token = "token3",
            UserId = "user-3"
        };

        _dbContext.Subscriptions.AddRange(catalogSub, shippedSub, paidSub);
        await _dbContext.SaveChangesAsync();

        // Act & Assert
        var catalogResult = (await _retriever.GetSubscriptionsOfType(WebhookType.CatalogItemPriceChange)).ToList();
        Assert.Single(catalogResult);
        Assert.Equal(WebhookType.CatalogItemPriceChange, catalogResult[0].Type);

        var shippedResult = (await _retriever.GetSubscriptionsOfType(WebhookType.OrderShipped)).ToList();
        Assert.Single(shippedResult);
        Assert.Equal(WebhookType.OrderShipped, shippedResult[0].Type);

        var paidResult = (await _retriever.GetSubscriptionsOfType(WebhookType.OrderPaid)).ToList();
        Assert.Single(paidResult);
        Assert.Equal(WebhookType.OrderPaid, paidResult[0].Type);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}
