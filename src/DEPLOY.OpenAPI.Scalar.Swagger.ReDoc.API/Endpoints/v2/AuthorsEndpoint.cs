using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Endpoints.v2
{
    public static class AuthorsEndpoint
    {
		public static void MapAuthorsEndpointsV2(this Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app)
		{
			const string AuthorsTag = "Authors";

			var apiVersionSetAuthors_V1 = app
				.NewApiVersionSet(AuthorsTag)
				.HasApiVersion(new Asp.Versioning.ApiVersion(2, 0))
				.ReportApiVersions()
				.Build();

			var Authors_V2 = app
				.MapGroup("/api/v{version:apiVersion}/authors")
				.WithApiVersionSet(apiVersionSetAuthors_V1)
				.RequireAuthorization();

			Authors_V2
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
		}
	}
}
