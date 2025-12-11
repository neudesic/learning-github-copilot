using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using eShop.Identity.API.Data;
using eShop.Identity.API.Models;

namespace Identity.API.Tests;

public class ApplicationDbContextModelSnapshotTests
{
    [Fact]
    public void DbContext_ShouldHaveModel()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        Assert.NotNull(model);
    }

    [Fact]
    public void DbContext_ShouldConfigureIdentityRole()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestIdentityRole")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var roleEntity = model.FindEntityType("Microsoft.AspNetCore.Identity.IdentityRole");
        Assert.NotNull(roleEntity);
        
        var idProperty = roleEntity.FindProperty("Id");
        Assert.NotNull(idProperty);
        
        var nameProperty = roleEntity.FindProperty("Name");
        Assert.NotNull(nameProperty);
        Assert.Equal(256, nameProperty.GetMaxLength());
    }

    [Fact]
    public void DbContext_ShouldConfigureApplicationUser()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestApplicationUser")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userEntity = model.FindEntityType(typeof(ApplicationUser));
        Assert.NotNull(userEntity);
        
        var cardNumberProperty = userEntity.FindProperty("CardNumber");
        Assert.NotNull(cardNumberProperty);
        
        var cityProperty = userEntity.FindProperty("City");
        Assert.NotNull(cityProperty);
        
        var countryProperty = userEntity.FindProperty("Country");
        Assert.NotNull(countryProperty);
    }

    [Fact]
    public void DbContext_ShouldConfigureIdentityRoleClaim()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestRoleClaim")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var roleClaimEntity = model.FindEntityType("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>");
        Assert.NotNull(roleClaimEntity);
        
        var idProperty = roleClaimEntity.FindProperty("Id");
        Assert.NotNull(idProperty);
        Assert.True(idProperty.ValueGenerated == ValueGenerated.OnAdd);
    }

    [Fact]
    public void DbContext_ShouldConfigureIdentityUserClaim()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserClaim")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userClaimEntity = model.FindEntityType("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>");
        Assert.NotNull(userClaimEntity);
        
        var userIdProperty = userClaimEntity.FindProperty("UserId");
        Assert.NotNull(userIdProperty);
        Assert.False(userIdProperty.IsNullable);
    }

    [Fact]
    public void DbContext_ShouldConfigureIdentityUserLogin()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserLogin")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userLoginEntity = model.FindEntityType("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>");
        Assert.NotNull(userLoginEntity);
        
        var key = userLoginEntity.FindPrimaryKey();
        Assert.NotNull(key);
        Assert.Equal(2, key.Properties.Count);
    }

    [Fact]
    public void DbContext_ShouldConfigureIdentityUserRole()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserRole")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userRoleEntity = model.FindEntityType("Microsoft.AspNetCore.Identity.IdentityUserRole<string>");
        Assert.NotNull(userRoleEntity);
        
        var key = userRoleEntity.FindPrimaryKey();
        Assert.NotNull(key);
        Assert.Equal(2, key.Properties.Count);
    }

    [Fact]
    public void DbContext_ShouldConfigureIdentityUserToken()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserToken")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userTokenEntity = model.FindEntityType("Microsoft.AspNetCore.Identity.IdentityUserToken<string>");
        Assert.NotNull(userTokenEntity);
        
        var key = userTokenEntity.FindPrimaryKey();
        Assert.NotNull(key);
        Assert.Equal(3, key.Properties.Count);
    }

    [Fact]
    public void DbContext_ShouldConfigureApplicationUserProperties()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserProperties")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userEntity = model.FindEntityType(typeof(ApplicationUser));
        Assert.NotNull(userEntity);
        
        var cardHolderNameProperty = userEntity.FindProperty("CardHolderName");
        Assert.NotNull(cardHolderNameProperty);
        Assert.False(cardHolderNameProperty.IsNullable);
        
        var cardTypeProperty = userEntity.FindProperty("CardType");
        Assert.NotNull(cardTypeProperty);
        
        var expirationProperty = userEntity.FindProperty("Expiration");
        Assert.NotNull(expirationProperty);
        
        var securityNumberProperty = userEntity.FindProperty("SecurityNumber");
        Assert.NotNull(securityNumberProperty);
    }

    [Fact]
    public void DbContext_ShouldConfigureUserIndexes()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserIndexes")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userEntity = model.FindEntityType(typeof(ApplicationUser));
        Assert.NotNull(userEntity);
        
        var indexes = userEntity.GetIndexes().ToList();
        Assert.NotEmpty(indexes);
        
        var emailIndex = indexes.FirstOrDefault(i => i.Properties.Any(p => p.Name == "NormalizedEmail"));
        Assert.NotNull(emailIndex);
        
        var userNameIndex = indexes.FirstOrDefault(i => i.Properties.Any(p => p.Name == "NormalizedUserName"));
        Assert.NotNull(userNameIndex);
        Assert.True(userNameIndex.IsUnique);
    }

    [Fact]
    public void DbContext_ShouldConfigureRoleIndexes()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestRoleIndexes")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var roleEntity = model.FindEntityType("Microsoft.AspNetCore.Identity.IdentityRole");
        Assert.NotNull(roleEntity);
        
        var indexes = roleEntity.GetIndexes().ToList();
        Assert.NotEmpty(indexes);
        
        var roleNameIndex = indexes.FirstOrDefault(i => i.Properties.Any(p => p.Name == "NormalizedName"));
        Assert.NotNull(roleNameIndex);
        Assert.True(roleNameIndex.IsUnique);
    }

    [Fact]
    public void DbContext_ShouldConfigureForeignKeys()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestForeignKeys")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userClaimEntity = model.FindEntityType("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>");
        Assert.NotNull(userClaimEntity);
        
        var foreignKeys = userClaimEntity.GetForeignKeys().ToList();
        Assert.NotEmpty(foreignKeys);
        
        var userForeignKey = foreignKeys.First();
        Assert.Equal(DeleteBehavior.Cascade, userForeignKey.DeleteBehavior);
    }

    [Fact]
    public void DbContext_ShouldConfigureConcurrencyTokens()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestConcurrency")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userEntity = model.FindEntityType(typeof(ApplicationUser));
        Assert.NotNull(userEntity);
        
        var concurrencyStampProperty = userEntity.FindProperty("ConcurrencyStamp");
        Assert.NotNull(concurrencyStampProperty);
        Assert.True(concurrencyStampProperty.IsConcurrencyToken);
    }

    [Fact]
    public void DbContext_ShouldConfigureTableNames()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestTableNames")
            .Options;

        using var context = new ApplicationDbContext(options);
        var model = context.Model;

        var userEntity = model.FindEntityType(typeof(ApplicationUser));
        Assert.NotNull(userEntity);
        Assert.Equal("AspNetUsers", userEntity.GetTableName());
        
        var roleEntity = model.FindEntityType("Microsoft.AspNetCore.Identity.IdentityRole");
        Assert.NotNull(roleEntity);
        Assert.Equal("AspNetRoles", roleEntity.GetTableName());
    }
}
