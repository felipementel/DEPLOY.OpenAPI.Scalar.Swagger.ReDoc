namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests;

[TestClass]
public class AuthorTests
{
    [TestMethod]
    public void Author_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var author = new Author();

        // Assert
        Assert.AreEqual(Guid.Empty, author.Id);
        Assert.AreEqual(string.Empty, author.Name);
        Assert.IsNotNull(author.Books);
        Assert.AreEqual(0, author.Books.Count);
    }

    [TestMethod]
    public void Author_Should_Set_Properties_Correctly()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var name = "Test Author";

        // Act
        var author = new Author
        {
            Id = authorId,
            Name = name
        };

        // Assert
        Assert.AreEqual(authorId, author.Id);
        Assert.AreEqual(name, author.Name);
        Assert.AreEqual(0, author.Books.Count);
    }

    [TestMethod]
    public void Author_AddBook_Should_Add_Book_To_Collection()
    {
        // Arrange
        var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
        var book = new Book 
        { 
            Id = Guid.NewGuid(), 
            Title = "Test Book", 
            Author = author,
            Price = 19.99m,
            PublishedDate = DateTime.Now,
            Genre = BookGenre.Fiction
        };

        // Act
        author.AddBook(book);

        // Assert
        Assert.AreEqual(1, author.Books.Count);
        Assert.IsTrue(author.Books.Contains(book));
    }

    [TestMethod]
    public void Author_AddBook_Should_Add_Multiple_Books()
    {
        // Arrange
        var author = new Author { Id = Guid.NewGuid(), Name = "Test Author" };
        var book1 = new Book { Id = Guid.NewGuid(), Title = "Book 1" };
        var book2 = new Book { Id = Guid.NewGuid(), Title = "Book 2" };

        // Act
        author.AddBook(book1);
        author.AddBook(book2);

        // Assert
        Assert.AreEqual(2, author.Books.Count);
        Assert.IsTrue(author.Books.Contains(book1));
        Assert.IsTrue(author.Books.Contains(book2));
    }

    [TestMethod]
    public void Author_Books_Should_Be_ReadOnly()
    {
        // Arrange
        var author = new Author();

        // Act & Assert
        Assert.IsInstanceOfType(author.Books, typeof(System.Collections.ObjectModel.ReadOnlyCollection<Book>));
    }
}