using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.Api.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
;

// Register CategoryService
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductAttributeService>();
// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed the database with sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Only seed if the database is empty
    if (!context.Categories.Any())
    {
        var categories = new[]
        {
            new Category { Name = "Electronics", Description = "Electronic gadgets and devices" },
            new Category { Name = "Laptops", Description = "Portable computers", ParentCategoryID = 1 },
            new Category { Name = "Smartphones", Description = "Mobile phones and accessories", ParentCategoryID = 1 },
            new Category { Name = "Accessories", Description = "Electronics accessories", ParentCategoryID = 1 }
        };
        
        context.Categories.AddRange(categories);
        context.SaveChanges();
        
        var products = new[]
        {
            new Product { Name = "UltraBook X1", Description = "Lightweight business laptop", SKU = "SKU-UBX1", CategoryID = 2, Brand = "TechBrand" },
            new Product { Name = "Gaming Beast Z9", Description = "High-end gaming laptop", SKU = "SKU-GBZ9", CategoryID = 2, Brand = "GamePro" },
            new Product { Name = "Galaxy X10", Description = "Latest smartphone with AI camera", SKU = "SKU-GX10", CategoryID = 3, Brand = "SmartTech" }
        };
        
        context.Products.AddRange(products);
        context.SaveChanges();
    }
}

app.Run();
