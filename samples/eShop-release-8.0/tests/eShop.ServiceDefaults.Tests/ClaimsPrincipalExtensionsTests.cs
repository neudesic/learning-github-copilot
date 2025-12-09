namespace eShop.ServiceDefaults.Tests;

using System.Security.Claims;
using Xunit;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void GetUserId_WithValidSubClaim_ReturnsUserId()
    {
        // Arrange
        var userId = "user-123";
        var claims = new List<Claim>
        {
            new Claim("sub", userId)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserId();

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public void GetUserId_WithoutSubClaim_ReturnsNull()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "John Doe")
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserId();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUserId_WithEmptySubClaim_ReturnsEmptyString()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim("sub", "")
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserId();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void GetUserId_WithNullPrincipal_ThrowsNullReferenceException()
    {
        // Arrange
        ClaimsPrincipal principal = null!;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => principal.GetUserId());
    }

    [Fact]
    public void GetUserId_WithMultipleSubClaims_ReturnsFirstSubClaim()
    {
        // Arrange
        var userId1 = "user-1";
        var userId2 = "user-2";
        var claims = new List<Claim>
        {
            new Claim("sub", userId1),
            new Claim("sub", userId2)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserId();

        // Assert
        Assert.Equal(userId1, result);
    }

    [Fact]
    public void GetUserId_WithSpecialCharactersInSubClaim_ReturnsUserId()
    {
        // Arrange
        var userId = "user-123@domain.com";
        var claims = new List<Claim>
        {
            new Claim("sub", userId)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserId();

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public void GetUserId_WithGuidAsSubClaim_ReturnsGuid()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim("sub", userId)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserId();

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public void GetUserName_WithValidNameClaim_ReturnsUserName()
    {
        // Arrange
        var userName = "John Doe";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserName();

        // Assert
        Assert.Equal(userName, result);
    }

    [Fact]
    public void GetUserName_WithoutNameClaim_ReturnsNull()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim("sub", "user-123")
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserName();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUserName_WithEmptyNameClaim_ReturnsEmptyString()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "")
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserName();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void GetUserName_WithNullPrincipal_ThrowsNullReferenceException()
    {
        // Arrange
        ClaimsPrincipal principal = null!;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => principal.GetUserName());
    }

    [Fact]
    public void GetUserName_WithMultipleNameClaims_ReturnsFirstNameClaim()
    {
        // Arrange
        var userName1 = "John Doe";
        var userName2 = "Jane Smith";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName1),
            new Claim(ClaimTypes.Name, userName2)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserName();

        // Assert
        Assert.Equal(userName1, result);
    }

    [Fact]
    public void GetUserName_WithSpecialCharactersInNameClaim_ReturnsUserName()
    {
        // Arrange
        var userName = "John O'Reilly-Smith";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserName();

        // Assert
        Assert.Equal(userName, result);
    }

    [Fact]
    public void GetUserName_WithWhitespaceOnlyNameClaim_ReturnsWhitespace()
    {
        // Arrange
        var userName = "   ";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserName();

        // Assert
        Assert.Equal(userName, result);
    }

    [Fact]
    public void GetUserName_WithLongNameClaim_ReturnsFullName()
    {
        // Arrange
        var userName = "Christopher Alexander Montgomery-Smith III";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserName();

        // Assert
        Assert.Equal(userName, result);
    }

    [Fact]
    public void GetUserId_AndGetUserName_BothWorkWithCompleteClaims()
    {
        // Arrange
        var userId = "user-123";
        var userName = "John Doe";
        var claims = new List<Claim>
        {
            new Claim("sub", userId),
            new Claim(ClaimTypes.Name, userName)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var resultId = principal.GetUserId();
        var resultName = principal.GetUserName();

        // Assert
        Assert.Equal(userId, resultId);
        Assert.Equal(userName, resultName);
    }

    [Fact]
    public void GetUserId_WithMultipleIdentities_ReturnsSubFromFirstIdentity()
    {
        // Arrange
        var userId = "user-123";
        var claims1 = new List<Claim>
        {
            new Claim("sub", userId)
        };
        var claims2 = new List<Claim>
        {
            new Claim("sub", "user-456")
        };
        var identity1 = new ClaimsIdentity(claims1);
        var identity2 = new ClaimsIdentity(claims2);
        var principal = new ClaimsPrincipal(new[] { identity1, identity2 });

        // Act
        var result = principal.GetUserId();

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public void GetUserName_WithMultipleIdentities_ReturnsNameFromFirstIdentity()
    {
        // Arrange
        var userName = "John Doe";
        var claims1 = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName)
        };
        var claims2 = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Jane Smith")
        };
        var identity1 = new ClaimsIdentity(claims1);
        var identity2 = new ClaimsIdentity(claims2);
        var principal = new ClaimsPrincipal(new[] { identity1, identity2 });

        // Act
        var result = principal.GetUserName();

        // Assert
        Assert.Equal(userName, result);
    }

    [Fact]
    public void GetUserId_WithEmptyPrincipal_ReturnsNull()
    {
        // Arrange
        var principal = new ClaimsPrincipal();

        // Act
        var result = principal.GetUserId();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUserName_WithEmptyPrincipal_ReturnsNull()
    {
        // Arrange
        var principal = new ClaimsPrincipal();

        // Act
        var result = principal.GetUserName();

        // Assert
        Assert.Null(result);
    }
}
