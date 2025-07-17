using Xunit;

namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests.Models
{
    public class AuthorTests
    {
        [Fact]
        public void Author_Constructor_ShouldInitializeProperties()
        {
            // Arrange & Act
            var author = new Author();

            // Assert
            Assert.Equal(Guid.Empty, author.Id);
            Assert.Equal(string.Empty, author.Name);
            Assert.NotNull(author.Books);
            Assert.Empty(author.Books);
        }

        [Fact]
        public void Author_SetProperties_ShouldUpdateValues()
        {
            // Arrange
            var author = new Author();
            var expectedId = Guid.NewGuid();
            var expectedName = "Felipe Augusto";

            // Act
            author.Id = expectedId;
            author.Name = expectedName;

            // Assert
            Assert.Equal(expectedId, author.Id);
            Assert.Equal(expectedName, author.Name);
        }

        [Fact]
        public void Author_AddBook_ShouldAddBookToCollection()
        {
            // Arrange
            var author = new Author { Name = "Felipe Augusto" };
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book",
                Author = author,
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
            var author = new Author { Name = "Felipe Augusto" };
            var book1 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book 1",
                Author = author,
                Price = 29.99m,
                PublishedDate = DateTime.Now,
                Genre = BookGenre.Fiction
            };
            var book2 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book 2",
                Author = author,
                Price = 39.99m,
                PublishedDate = DateTime.Now.AddDays(-30),
                Genre = BookGenre.NonFiction
            };

            // Act
            author.AddBook(book1);
            author.AddBook(book2);

            // Assert
            Assert.Equal(2, author.Books.Count);
            Assert.Contains(book1, author.Books);
            Assert.Contains(book2, author.Books);
        }

        [Fact]
        public void Author_BooksProperty_ShouldBeReadOnly()
        {
            // Arrange
            var author = new Author();
            var books = author.Books;

            // Act & Assert
            Assert.IsAssignableFrom<IReadOnlyCollection<Book>>(books);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("John Doe")]
        [InlineData("Maria Silva")]
        [InlineData("Felipe Augusto, MVP")]
        public void Author_Name_ShouldAcceptValidStrings(string name)
        {
            // Arrange
            var author = new Author();

            // Act
            author.Name = name;

            // Assert
            Assert.Equal(name, author.Name);
        }
    }
}