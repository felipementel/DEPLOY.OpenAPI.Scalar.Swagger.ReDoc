using Xunit;

namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests;

public class AuthorTests
{
    [Fact]
    public void Author_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var author = new Author();

        // Assert
        Assert.Equal(Guid.Empty, author.Id);
        Assert.Equal(string.Empty, author.Name);
        Assert.Empty(author.Books);
    }

    [Fact]
    public void Author_SetProperties_ShouldSetCorrectly()
    {
        // Arrange
        var author = new Author();
        var id = Guid.NewGuid();
        var name = "Test Author";

        // Act
        author.Id = id;
        author.Name = name;

        // Assert
        Assert.Equal(id, author.Id);
        Assert.Equal(name, author.Name);
    }

    [Fact]
    public void Author_AddBook_ShouldAddBookToCollection()
    {
        // Arrange
        var author = new Author();
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Test Book",
            Price = 29.99m,
            PublishedDate = DateTime.Now,
            Genre = BookGenre.Fiction
        };

        // Act
        author.AddBook(book);

        // Assert
        Assert.Single(author.Books);
        Assert.Contains(book, author.Books);
    }

    [Fact]
    public void Author_AddMultipleBooks_ShouldAddAllBooksToCollection()
    {
        // Arrange
        var author = new Author();
        var book1 = new Book { Id = Guid.NewGuid(), Title = "Book 1", Genre = BookGenre.Fiction };
        var book2 = new Book { Id = Guid.NewGuid(), Title = "Book 2", Genre = BookGenre.Mystery };

        // Act
        author.AddBook(book1);
        author.AddBook(book2);

        // Assert
        Assert.Equal(2, author.Books.Count);
        Assert.Contains(book1, author.Books);
        Assert.Contains(book2, author.Books);
    }

    [Fact]
    public void Author_Books_ShouldBeReadOnlyCollection()
    {
        // Arrange
        var author = new Author();

        // Act & Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<Book>>(author.Books);
    }
}