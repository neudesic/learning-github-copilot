using System.Text.Json.Serialization;
using eShop.Basket.API.Repositories;
using eShop.Basket.API.IntegrationEvents.EventHandling;
using eShop.Basket.API.IntegrationEvents.EventHandling.Events;

namespace eShop.Basket.API.Extensions;

public static class Extensions
{
    /// <summary>
    /// Adds application-specific services to the host application builder.
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();

        builder.AddRedisClient("redis");

        builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

        builder.AddRabbitMqEventBus("eventbus")
               .AddSubscription<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>()
               .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));
    }
}

/// <summary>
/// JSON serialization context for integration events.
/// </summary>
[JsonSerializable(typeof(OrderStartedIntegrationEvent))]
partial class IntegrationEventContext : JsonSerializerContext
{

}
