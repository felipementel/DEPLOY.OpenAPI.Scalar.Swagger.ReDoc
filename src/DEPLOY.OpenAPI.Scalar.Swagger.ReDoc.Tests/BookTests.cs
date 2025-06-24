using Xunit;

namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests;

public class BookTests
{
    [Fact]
    public void Book_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var book = new Book();

        // Assert
        Assert.Equal(Guid.Empty, book.Id);
        Assert.Equal(string.Empty, book.Title);
        Assert.NotNull(book.Author);
        Assert.Equal(0, book.Price);
        Assert.Equal(DateTime.MinValue, book.PublishedDate);
        Assert.Equal(BookGenre.Fiction, book.Genre);
    }

    [Fact]
    public void Book_SetProperties_ShouldSetCorrectly()
    {
        // Arrange
        var book = new Book();
        var id = Guid.NewGuid();
        var title = "Test Book";
        var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
        var price = 29.99m;
        var publishedDate = new DateTime(2023, 1, 1);
        var genre = BookGenre.Fantasy;

        // Act
        book.Id = id;
        book.Title = title;
        book.Author = author;
        book.Price = price;
        book.PublishedDate = publishedDate;
        book.Genre = genre;

        // Assert
        Assert.Equal(id, book.Id);
        Assert.Equal(title, book.Title);
        Assert.Equal(author, book.Author);
        Assert.Equal(price, book.Price);
        Assert.Equal(publishedDate, book.PublishedDate);
        Assert.Equal(genre, book.Genre);
    }

    [Theory]
    [InlineData(BookGenre.Fiction)]
    [InlineData(BookGenre.NonFiction)]
    [InlineData(BookGenre.ScienceFiction)]
    [InlineData(BookGenre.Fantasy)]
    [InlineData(BookGenre.Mystery)]
    [InlineData(BookGenre.Romance)]
    [InlineData(BookGenre.Thriller)]
    [InlineData(BookGenre.Horror)]
    [InlineData(BookGenre.Biography)]
    [InlineData(BookGenre.History)]
    [InlineData(BookGenre.Poetry)]
    public void Book_SetGenre_ShouldAcceptAllValidGenres(BookGenre genre)
    {
        // Arrange
        var book = new Book();

        // Act
        book.Genre = genre;

        // Assert
        Assert.Equal(genre, book.Genre);
    }

    [Fact]
    public void Book_PriceValidation_ShouldAcceptPositiveValues()
    {
        // Arrange
        var book = new Book();
        var validPrices = new[] { 0.01m, 10.50m, 999.99m };

        foreach (var price in validPrices)
        {
            // Act
            book.Price = price;

            // Assert
            Assert.Equal(price, book.Price);
        }
    }

    [Fact]
    public void Book_PublishedDate_ShouldAcceptValidDates()
    {
        // Arrange
        var book = new Book();
        var validDates = new[]
        {
            new DateTime(1900, 1, 1),
            new DateTime(2023, 6, 15),
            DateTime.Now
        };

        foreach (var date in validDates)
        {
            // Act
            book.PublishedDate = date;

            // Assert
            Assert.Equal(date, book.PublishedDate);
        }
    }

    [Fact]
    public void Book_WithAuthor_ShouldCreateCorrectRelationship()
    {
        // Arrange
        var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Test Book",
            Author = author,
            Price = 25.00m,
            PublishedDate = DateTime.Now,
            Genre = BookGenre.Fiction
        };

        // Act
        author.AddBook(book);

        // Assert
        Assert.Equal(author, book.Author);
        Assert.Contains(book, author.Books);
    }
}