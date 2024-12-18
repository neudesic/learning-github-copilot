#nullable enable

namespace eShop.Basket.API.Extensions;

internal static class ServerCallContextIdentityExtensions
{
    /// <summary>
    /// Gets the user identity from the server call context.
    /// </summary>
    /// <param name="context">The server call context.</param>
    /// <returns>The user identity.</returns>
    public static string? GetUserIdentity(this ServerCallContext context) => context.GetHttpContext().User.FindFirst("sub")?.Value;

    /// <summary>
    /// Gets the user name from the server call context.
    /// </summary>
    /// <param name="context">The server call context.</param>
    /// <returns>The user name.</returns>
    public static string? GetUserName(this ServerCallContext context) => context.GetHttpContext().User.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
}
