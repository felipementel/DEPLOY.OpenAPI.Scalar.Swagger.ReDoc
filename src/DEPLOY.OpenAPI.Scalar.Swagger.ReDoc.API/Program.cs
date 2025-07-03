using DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Basic services
builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Canal DEPLOY - Temporal Tables - V1", 
        Version = "v1" 
    });
    c.SwaggerDoc("v2", new OpenApiInfo 
    { 
        Title = "Canal DEPLOY - Temporal Tables - V2", 
        Version = "v2" 
    });
});

// Database
builder.Services.AddEntityFrameworkInMemoryDatabase()
    .AddDbContext<DEPLOYContext>(opt =>
    {
        opt.UseInMemoryDatabase("DEPLOY");
    });

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Canal DEPLOY - V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Canal DEPLOY - V2");
    });
}

app.UseAuthentication();
app.UseAuthorization();

// Simple Books endpoints
app.MapPost("/api/v1/books", 
    async (DEPLOYContext dbContext, Book book) =>
    {
        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/api/v1/books/{book.Id}", book);
    })
    .WithTags("Books");

app.MapGet("/api/v1/books", 
    async (DEPLOYContext dbContext) =>
    {
        var books = await dbContext.Books.ToListAsync();
        return Results.Ok(books);
    })
    .WithTags("Books");

app.MapGet("/api/v1/books/{id:guid}", 
    async (DEPLOYContext dbContext, Guid id) =>
    {
        var book = await dbContext.Books.FindAsync(id);
        return book is not null ? Results.Ok(book) : Results.NotFound();
    })
    .WithTags("Books");

// Simple Authors endpoints
app.MapPost("/api/v1/authors", 
    async (DEPLOYContext dbContext, Author author) =>
    {
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/api/v1/authors/{author.Id}", author);
    })
    .WithTags("Authors")
    .RequireAuthorization();

app.MapGet("/api/v1/authors", 
    async (DEPLOYContext dbContext) =>
    {
        var authors = await dbContext.Authors.ToListAsync();
        return Results.Ok(authors);
    })
    .WithTags("Authors")
    .RequireAuthorization();

await app.RunAsync();