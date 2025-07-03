using DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database;
using Microsoft.EntityFrameworkCore;

namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests;

[TestClass]
public class DEPLOYContextTests
{
    private DEPLOYContext? _context;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DEPLOYContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        _context = new DEPLOYContext(options);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context?.Dispose();
    }

    [TestMethod]
    public void DEPLOYContext_Should_Have_Books_DbSet()
    {
        // Assert
        Assert.IsNotNull(_context!.Books);
    }

    [TestMethod]
    public void DEPLOYContext_Should_Have_Authors_DbSet()
    {
        // Assert
        Assert.IsNotNull(_context!.Authors);
    }

    [TestMethod]
    public async Task DEPLOYContext_Should_Add_And_Retrieve_Book()
    {
        // Arrange
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Test Book",
            Author = new Author { Id = Guid.NewGuid(), Name = "Test Author" },
            Price = 19.99m,
            PublishedDate = new DateTime(2024, 1, 1),
            Genre = BookGenre.Fiction
        };

        // Act
        await _context!.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        var retrievedBook = await _context.Books.FindAsync(book.Id);

        // Assert
        Assert.IsNotNull(retrievedBook);
        Assert.AreEqual(book.Id, retrievedBook.Id);
        Assert.AreEqual(book.Title, retrievedBook.Title);
        Assert.AreEqual(book.Price, retrievedBook.Price);
        Assert.AreEqual(book.Genre, retrievedBook.Genre);
    }

    [TestMethod]
    public async Task DEPLOYContext_Should_Add_And_Retrieve_Author()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "Test Author"
        };

        // Act
        await _context!.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        var retrievedAuthor = await _context.Authors.FindAsync(author.Id);

        // Assert
        Assert.IsNotNull(retrievedAuthor);
        Assert.AreEqual(author.Id, retrievedAuthor.Id);
        Assert.AreEqual(author.Name, retrievedAuthor.Name);
    }

    [TestMethod]
    public async Task DEPLOYContext_Should_Handle_Multiple_Books()
    {
        // Arrange
        var books = new[]
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book 1",
                Author = new Author { Id = Guid.NewGuid(), Name = "Author 1" },
                Price = 15.99m,
                PublishedDate = new DateTime(2024, 1, 1),
                Genre = BookGenre.Fiction
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book 2",
                Author = new Author { Id = Guid.NewGuid(), Name = "Author 2" },
                Price = 25.99m,
                PublishedDate = new DateTime(2024, 2, 1),
                Genre = BookGenre.Fantasy
            }
        };

        // Act
        await _context!.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();

        var retrievedBooks = await _context.Books.ToListAsync();

        // Assert
        Assert.AreEqual(2, retrievedBooks.Count);
        Assert.IsTrue(retrievedBooks.Any(b => b.Title == "Book 1"));
        Assert.IsTrue(retrievedBooks.Any(b => b.Title == "Book 2"));
    }

    [TestMethod]
    public async Task DEPLOYContext_Should_Handle_Multiple_Authors()
    {
        // Arrange
        var authors = new[]
        {
            new Author { Id = Guid.NewGuid(), Name = "Author 1" },
            new Author { Id = Guid.NewGuid(), Name = "Author 2" },
            new Author { Id = Guid.NewGuid(), Name = "Author 3" }
        };

        // Act
        await _context!.Authors.AddRangeAsync(authors);
        await _context.SaveChangesAsync();

        var retrievedAuthors = await _context.Authors.ToListAsync();

        // Assert
        Assert.AreEqual(3, retrievedAuthors.Count);
        Assert.IsTrue(retrievedAuthors.Any(a => a.Name == "Author 1"));
        Assert.IsTrue(retrievedAuthors.Any(a => a.Name == "Author 2"));
        Assert.IsTrue(retrievedAuthors.Any(a => a.Name == "Author 3"));
    }
}