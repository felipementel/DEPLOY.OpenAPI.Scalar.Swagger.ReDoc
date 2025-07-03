namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests;

[TestClass]
public class BookTests
{
    [TestMethod]
    public void Book_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var book = new Book();

        // Assert
        Assert.AreEqual(Guid.Empty, book.Id);
        Assert.AreEqual(string.Empty, book.Title);
        Assert.IsNotNull(book.Author);
        Assert.AreEqual(0m, book.Price);
        Assert.AreEqual(DateTime.MinValue, book.PublishedDate);
        Assert.AreEqual(BookGenre.Fiction, book.Genre);
    }

    [TestMethod]
    public void Book_Should_Set_Properties_Correctly()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var title = "Test Book";
        var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
        var price = 19.99m;
        var publishedDate = new DateTime(2024, 1, 1);
        var genre = BookGenre.ScienceFiction;

        // Act
        var book = new Book
        {
            Id = bookId,
            Title = title,
            Author = author,
            Price = price,
            PublishedDate = publishedDate,
            Genre = genre
        };

        // Assert
        Assert.AreEqual(bookId, book.Id);
        Assert.AreEqual(title, book.Title);
        Assert.AreEqual(author, book.Author);
        Assert.AreEqual(price, book.Price);
        Assert.AreEqual(publishedDate, book.PublishedDate);
        Assert.AreEqual(genre, book.Genre);
    }

    [TestMethod]
    public void BookGenre_Should_Have_All_Expected_Values()
    {
        // Assert
        var expectedGenres = new[]
        {
            BookGenre.Fiction,
            BookGenre.NonFiction,
            BookGenre.ScienceFiction,
            BookGenre.Fantasy,
            BookGenre.Mystery,
            BookGenre.Romance,
            BookGenre.Thriller,
            BookGenre.Horror,
            BookGenre.Biography,
            BookGenre.History,
            BookGenre.Poetry
        };

        var actualGenres = Enum.GetValues<BookGenre>();

        CollectionAssert.AreEquivalent(expectedGenres, actualGenres);
    }
}