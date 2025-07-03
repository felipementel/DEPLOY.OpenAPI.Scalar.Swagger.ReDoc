# DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests

This project contains MSTest unit and integration tests for the DEPLOY OpenAPI application.

## Test Structure

### Unit Tests
- **BookTests.cs**: Tests for the Book model and BookGenre enum
- **AuthorTests.cs**: Tests for the Author model and its methods  
- **DEPLOYContextTests.cs**: Tests for the Entity Framework database context

### Integration Tests
- **BasicApiTests.cs**: Basic API endpoint tests using WebApplicationFactory

## Running Tests

To run all tests:
```bash
dotnet test
```

To run tests with detailed output:
```bash
dotnet test --verbosity normal
```

To run specific test class:
```bash
dotnet test --filter "ClassName=BookTests"
```

## Test Coverage

The tests cover:
- ✅ Model validation and property setting
- ✅ Entity Framework database operations  
- ✅ Basic API endpoint functionality
- ✅ Authorization requirements
- ✅ HTTP status code validation

## Dependencies

- MSTest.TestFramework
- Microsoft.AspNetCore.Mvc.Testing
- Microsoft.EntityFrameworkCore.InMemory