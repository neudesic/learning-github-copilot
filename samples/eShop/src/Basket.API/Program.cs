var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Adds basic service defaults to the builder.
/// </summary>
builder.AddBasicServiceDefaults();

/// <summary>
/// Adds application-specific services to the builder.
/// </summary>
builder.AddApplicationServices();

/// <summary>
/// Adds gRPC services to the builder.
/// </summary>
builder.Services.AddGrpc();

var app = builder.Build();

/// <summary>
/// Maps the default endpoints for the application.
/// </summary>
app.MapDefaultEndpoints();

/// <summary>
/// Maps the gRPC service for the basket.
/// </summary>
app.MapGrpcService<BasketService>();

/// <summary>
/// Runs the application.
/// </summary>
app.Run();
