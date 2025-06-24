using Microsoft.EntityFrameworkCore;
using DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests.Database;
using Xunit;

namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests;

public class DEPLOYContextTests : IDisposable
{
    private readonly DEPLOYContext _context;

    public DEPLOYContextTests()
    {
        var options = new DbContextOptionsBuilder<DEPLOYContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DEPLOYContext(options);
    }

    [Fact]
    public async Task Context_AddAuthor_ShouldSaveSuccessfully()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "Test Author"
        };

        // Act
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        // Assert
        var savedAuthor = await _context.Authors.FindAsync(author.Id);
        Assert.NotNull(savedAuthor);
        Assert.Equal(author.Name, savedAuthor.Name);
    }

    [Fact]
    public async Task Context_AddBook_ShouldSaveSuccessfully()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "Test Author"
        };

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Test Book",
            Author = author,
            Price = 29.99m,
            PublishedDate = new DateTime(2023, 1, 1),
            Genre = BookGenre.Fiction
        };

        // Act
        await _context.Authors.AddAsync(author);
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        // Assert
        var savedBook = await _context.Books.FindAsync(book.Id);
        Assert.NotNull(savedBook);
        Assert.Equal(book.Title, savedBook.Title);
        Assert.Equal(book.Price, savedBook.Price);
    }

    [Fact]
    public async Task Context_GetAllAuthors_ShouldReturnAllAuthors()
    {
        // Arrange
        var authors = new[]
        {
            new Author { Id = Guid.NewGuid(), Name = "Author 1" },
            new Author { Id = Guid.NewGuid(), Name = "Author 2" },
            new Author { Id = Guid.NewGuid(), Name = "Author 3" }
        };

        await _context.Authors.AddRangeAsync(authors);
        await _context.SaveChangesAsync();

        // Act
        var allAuthors = await _context.Authors.ToListAsync();

        // Assert
        Assert.Equal(3, allAuthors.Count);
        Assert.Contains(allAuthors, a => a.Name == "Author 1");
        Assert.Contains(allAuthors, a => a.Name == "Author 2");
        Assert.Contains(allAuthors, a => a.Name == "Author 3");
    }

    [Fact]
    public async Task Context_GetAllBooks_ShouldReturnAllBooks()
    {
        // Arrange
        var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
        var books = new[]
        {
            new Book { Id = Guid.NewGuid(), Title = "Book 1", Author = author, Genre = BookGenre.Fiction, Price = 10.99m },
            new Book { Id = Guid.NewGuid(), Title = "Book 2", Author = author, Genre = BookGenre.Mystery, Price = 15.99m }
        };

        await _context.Authors.AddAsync(author);
        await _context.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();

        // Act
        var allBooks = await _context.Books.ToListAsync();

        // Assert
        Assert.Equal(2, allBooks.Count);
        Assert.Contains(allBooks, b => b.Title == "Book 1");
        Assert.Contains(allBooks, b => b.Title == "Book 2");
    }

    [Fact]
    public async Task Context_UpdateAuthor_ShouldUpdateSuccessfully()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "Original Name"
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        // Act
        author.Name = "Updated Name";
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();

        // Assert
        var updatedAuthor = await _context.Authors.FindAsync(author.Id);
        Assert.NotNull(updatedAuthor);
        Assert.Equal("Updated Name", updatedAuthor.Name);
    }

    [Fact]
    public async Task Context_DeleteAuthor_ShouldDeleteSuccessfully()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "Test Author"
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        // Act
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        // Assert
        var deletedAuthor = await _context.Authors.FindAsync(author.Id);
        Assert.Null(deletedAuthor);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}