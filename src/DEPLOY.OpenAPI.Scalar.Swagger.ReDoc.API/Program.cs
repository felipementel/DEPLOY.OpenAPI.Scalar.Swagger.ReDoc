using DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Endpoints.v2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi(); //first step to add OpenAPI

builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});

builder.Services.AddOpenApi("v1", options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Felipe Augusto, MVP",
            Url = new Uri("https://www.youtube.com/@D.E.P.L.O.Y"),
        };

        document.Info.License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };

        document.ExternalDocs = new Microsoft.OpenApi.Models.OpenApiExternalDocs
        {
            Description = "Canal DEPLOY - OpenAPISpec Tips",
            Url = new Uri("https://www.youtube.com/@D.E.P.L.O.Y")
        };

        // Inline enum schemas
        options.CreateSchemaReferenceId = (type) => type.Type.IsEnum ? null : OpenApiOptions.CreateDefaultSchemaReferenceId(type);

        return Task.CompletedTask;
    });

    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
        {
            operation.Security = new List<Microsoft.OpenApi.Models.OpenApiSecurityRequirement>
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    [new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.Schema,
                            Id = "Bearer"
                        }
                    }] = new string[] { }
                }
            };
        }

        options.AddDocumentTransformer<OpenApiSecuritySchemeTransformer>();

        return Task.CompletedTask;
    });

    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        if (context.JsonTypeInfo.Type == typeof(decimal))
        {
            // default schema for decimal is just type: number.  Add format: decimal
            //https://spec.openapis.org/oas/v3.1.1.html#data-type-format
            schema.Format = "decimal";
        }
        return Task.CompletedTask;
    });
});

builder.Services.AddOpenApi("v2", options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Felipe Augusto, MVP",
            Url = new Uri("https://www.youtube.com/@D.E.P.L.O.Y"),
        };
        document.Info.License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };
        return Task.CompletedTask;
    });
});

builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new Asp.Versioning.ApiVersion(2, 0);
        options.ApiVersionReader = new Asp.Versioning.UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'VVV";

            options.SubstituteApiVersionInUrl = true;
        })
    .EnableApiVersionBinding();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Canal DEPLOY - Temporal Tables - V1",
            Version = "v1"
        });
    options.SwaggerDoc("v2",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Canal DEPLOY - Temporal Tables - V2",
            Version = "v2"
        });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Auth Canal DEPLOY",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddEntityFrameworkInMemoryDatabase()
    .AddDbContext<DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext>(opt =>
    {
        opt.UseInMemoryDatabase("DEPLOY");
    });

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();

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
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); //second step to add OpenAPI

    //scalar
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Canal DEPLOY - OpenAPISpec Tips");
        options.WithTheme(ScalarTheme.BluePlanet);
        options.WithSidebar(true);
        options.WithTestRequestButton(true);
        options.WithLayout(ScalarLayout.Modern);

        options.Authentication =
        new ScalarAuthenticationOptions
        {
            PreferredSecurityScheme = "Bearer"
        };
    });

    //swagger
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "canal-deploy-swagger";
        options.SwaggerEndpoint("/openapi/v1.json", "Canal DEPLOY - OpenAPISpec Tips - V1");
        options.SwaggerEndpoint("/openapi/v2.json", "Canal DEPLOY - OpenAPISpec Tips - V2");
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        options.DisplayRequestDuration();
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "img")),
    RequestPath = "/img"

});

//redoc
app.UseReDoc(options =>
{
    options.DocumentTitle = "REDOC API Documentation";
    options.SpecUrl("/openapi/v1.json");
    options.RoutePrefix = "redocv1";
    options.HeadContent = @"<img src='/img/logo-deploy.png' width=130px height=50px alt='Canal DEPLOY' />";
});

app.UseReDoc(options =>
{
    options.DocumentTitle = "REDOC API Documentation";
    options.SpecUrl("/openapi/v2.json");
    options.RoutePrefix = "redocv2";
    options.HeadContent = @"<img src='/img/logo-deploy.png' width=130px height=50px alt='Canal DEPLOY' />";
});

// ====
const string AuthorsTag = "Authors";

var apiVersionSetAuthors_V1 = app
    .NewApiVersionSet(AuthorsTag)
    .HasApiVersion(new Asp.Versioning.ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

var Authors_V1 = app
    .MapGroup("/api/v{version:apiVersion}/authors")
    .WithApiVersionSet(apiVersionSetAuthors_V1)
    .RequireAuthorization();

//Post Author
Authors_V1
    .MapPost("/author", async
    (DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext,
    [System.ComponentModel.Description("New Author")] Author author) =>
    {
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();

        return Results.Created($"/api/v1/deploy/author/{author.Id}", author);
    })
    .Produces<Author>(201)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "post-authors-v1",
        Summary = "post authors v1",
        Description = "post authors v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = AuthorsTag }
        }
    })
    .WithSummary("Create a new author");

// get authors
Authors_V1
    .MapGet("/author", async
    (DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext) =>
    {
        var itens = await dbContext.Authors.ToListAsync();

        if (itens == null)
        {
            return Results.NotFound();
        }

        return TypedResults.Ok(itens);
    })
    .Produces<string>(200)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "get-authors-v1",
        Summary = "get authors v1",
        Description = "get authors v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = AuthorsTag }
        }
    }).WithSummary("Get all authors");

// get author by id
Authors_V1
    .MapGet("/author/{id:guid}", async
    (DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext,
    [System.ComponentModel.Description("Author Id")] int id) =>
    {
        var author = await dbContext.Authors.FindAsync(id);

        if (author == null)
        {
            return Results.NotFound();
        }

        return TypedResults.Ok(author);
    })
    .Produces<Author>(200)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "get-authors-by-id-v1",
        Summary = "get authors by id v1",
        Description = "get authors by id v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = AuthorsTag }
        }
    })
    .WithSummary("Get author by id");

// update author
Authors_V1
    .MapPut("/author/{id:guid}", async
    (DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext,
    [System.ComponentModel.Description("Author Id")] int id,
    [System.ComponentModel.Description("New Author")] Author author) =>
    {
        var authorToUpdate = await dbContext.Authors.FindAsync(id);

        if (authorToUpdate == null)
        {
            return Results.NotFound();
        }

        authorToUpdate.Name = author.Name;
        dbContext.Authors.Update(authorToUpdate);

        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    })
    .Produces(204)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "put-authors-by-id-v1",
        Summary = "put authors by id v1",
        Description = "put authors by id v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = AuthorsTag }
        }
    })
    .WithSummary("Update author by id");

//Delete Author by Id
Authors_V1
    .MapDelete("/author/{id:guid}", async
    (DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext,
    [System.ComponentModel.Description("Author Id")] int id) =>
    {
        var author = await dbContext.Authors.FindAsync(id);

        if (author == null)
        {
            return Results.NotFound();
        }

        dbContext.Authors.Remove(author);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    })
    .Produces(204)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "delete-authors-by-id-v1",
        Summary = "delete authors by id v1",
        Description = "delete authors by id v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = AuthorsTag }
        }
    })
    .WithSummary("Delete author by id");


//=========================

const string BooksTag = "Books";

var apiVersionSetBooks_V1 = app
    .NewApiVersionSet("Books")
    .HasApiVersion(new Asp.Versioning.ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

var Books_V1 = app
    .MapGroup("/api/v{version:apiVersion}/books")
    .WithApiVersionSet(apiVersionSetBooks_V1);

//post books
Books_V1
    .MapPost("/books", async (
        DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext,
        [System.ComponentModel.Description("New Book")] Book book) =>
    {
        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        return Results.Created($"/api/v1/books/{book.Id}", book);
    })
    .Produces<Book>(201)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "post-books-v1",
        Summary = "post books v1",
        Description = "post books v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = BooksTag }
        }
    })
    .WithSummary("Create a new book");

//get books
Books_V1
    .MapGet("/books", async
    (DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext) =>
    {
        var itens = await dbContext.Books.ToListAsync();

        if (itens == null)
        {
            return Results.NotFound();
        }

        return TypedResults.Ok(itens);
    })
    .Produces<string>(200)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "get-books-v1",
        Summary = "get books v1",
        Description = "get books v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = BooksTag }
        }
    })
    .WithSummary("Get all books");

//get book by id
Books_V1
    .MapGet("/books/{id:guid}", async
    (DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext,
    [System.ComponentModel.Description("Book Id")] int id) =>
    {
        var book = await dbContext.Books.FindAsync(id);

        if (book == null)
        {
            return Results.NotFound();
        }

        return TypedResults.Ok(book);
    })
    .Produces<Book>(200)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "get-books-by-id-v1",
        Summary = "get books by id v1",
        Description = "get books by id v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = BooksTag }
        }
    })
    .WithSummary("Get book by id");

// update book
Books_V1
    .MapPut("/books/{id:guid}", async
    (DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext,
    [System.ComponentModel.Description("Book Id")] int id,
    Book book) =>
    {
        var bookToUpdate = await dbContext.Books.FindAsync(id);

        if (bookToUpdate == null)
        {
            return Results.NotFound();
        }

        bookToUpdate.Title = book.Title;
        dbContext.Books.Update(bookToUpdate);

        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    })
    .Produces(204)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "put-books-by-id-v1",
        Summary = "put books by id v1",
        Description = "put books by id v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = BooksTag }
        }
    })
    .WithSummary("Update book by id");

// delete book by id
Books_V1
    .MapDelete("/books/{id:guid}", async
    (DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database.DEPLOYContext dbContext,
    [System.ComponentModel.Description("Book Id")] int id) =>
    {
        var book = await dbContext.Books.FindAsync(id);

        if (book == null)
        {
            return Results.NotFound();
        }

        dbContext.Books.Remove(book);

        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    })
    .Produces(204)
    .WithOpenApi(operation => new(operation)
    {
        OperationId = "delete-books-by-id-v1",
        Summary = "delete books by id v1",
        Description = "delete books by id v1",
        Tags = new List<Microsoft.OpenApi.Models.OpenApiTag>
        {
            new() { Name = BooksTag }
        }
    })
    .WithSummary("Delete book by id");

app.MapAuthorsEndpointsV2();

await app.RunAsync();


public class OpenApiSecuritySchemeTransformer
    : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var securitySchema =
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            };

        var securityRequirement =
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.Header
                        }
                    },
                    []
                }
            };

        document.SecurityRequirements.Add(securityRequirement);
        document.Components = new OpenApiComponents()
        {
            SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>()
            {
                { "Bearer", securitySchema }
            }
        };
        return Task.CompletedTask;
    }
}