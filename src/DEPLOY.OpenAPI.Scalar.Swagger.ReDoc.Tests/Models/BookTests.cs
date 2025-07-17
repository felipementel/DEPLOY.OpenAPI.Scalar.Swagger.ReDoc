using Xunit;

namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests.Models
{
    public class BookTests
    {
        [Fact]
        public void Book_Constructor_ShouldInitializeProperties()
        {
            // Arrange & Act
            var book = new Book();

            // Assert
            Assert.Equal(Guid.Empty, book.Id);
            Assert.Equal(string.Empty, book.Title);
            Assert.NotNull(book.Author);
            Assert.Equal(0m, book.Price);
            Assert.Equal(DateTime.MinValue, book.PublishedDate);
            Assert.Equal(BookGenre.Fiction, book.Genre); // Default enum value
        }

        [Fact]
        public void Book_SetProperties_ShouldUpdateValues()
        {
            // Arrange
            var book = new Book();
            var expectedId = Guid.NewGuid();
            var expectedTitle = "Test Book Title";
            var expectedAuthor = new Author { Name = "Test Author" };
            var expectedPrice = 49.99m;
            var expectedDate = DateTime.Now;
            var expectedGenre = BookGenre.ScienceFiction;

            // Act
            book.Id = expectedId;
            book.Title = expectedTitle;
            book.Author = expectedAuthor;
            book.Price = expectedPrice;
            book.PublishedDate = expectedDate;
            book.Genre = expectedGenre;

            // Assert
            Assert.Equal(expectedId, book.Id);
            Assert.Equal(expectedTitle, book.Title);
            Assert.Equal(expectedAuthor, book.Author);
            Assert.Equal(expectedPrice, book.Price);
            Assert.Equal(expectedDate, book.PublishedDate);
            Assert.Equal(expectedGenre, book.Genre);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Clean Code")]
        [InlineData("The Pragmatic Programmer")]
        [InlineData("Design Patterns: Elements of Reusable Object-Oriented Software")]
        public void Book_Title_ShouldAcceptValidStrings(string title)
        {
            // Arrange
            var book = new Book();

            // Act
            book.Title = title;

            // Assert
            Assert.Equal(title, book.Title);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        [InlineData(10.50)]
        [InlineData(99.99)]
        [InlineData(1000)]
        public void Book_Price_ShouldAcceptValidDecimalValues(decimal price)
        {
            // Arrange
            var book = new Book();

            // Act
            book.Price = price;

            // Assert
            Assert.Equal(price, book.Price);
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
        public void Book_Genre_ShouldAcceptAllValidEnumValues(BookGenre genre)
        {
            // Arrange
            var book = new Book();

            // Act
            book.Genre = genre;

            // Assert
            Assert.Equal(genre, book.Genre);
        }

        [Fact]
        public void Book_PublishedDate_ShouldAcceptDateTimeValues()
        {
            // Arrange
            var book = new Book();
            var pastDate = new DateTime(2020, 1, 1);
            var futureDate = new DateTime(2025, 12, 31);
            var currentDate = DateTime.Now;

            // Act & Assert - Past date
            book.PublishedDate = pastDate;
            Assert.Equal(pastDate, book.PublishedDate);

            // Act & Assert - Future date
            book.PublishedDate = futureDate;
            Assert.Equal(futureDate, book.PublishedDate);

            // Act & Assert - Current date
            book.PublishedDate = currentDate;
            Assert.Equal(currentDate, book.PublishedDate);
        }

        [Fact]
        public void Book_WithCompleteData_ShouldRetainAllProperties()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "Felipe Augusto" };
            var bookId = Guid.NewGuid();
            var title = "Advanced .NET Programming";
            var price = 79.99m;
            var publishedDate = new DateTime(2023, 6, 15);
            var genre = BookGenre.NonFiction;

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
            Assert.Equal(bookId, book.Id);
            Assert.Equal(title, book.Title);
            Assert.Equal(author, book.Author);
            Assert.Equal(price, book.Price);
            Assert.Equal(publishedDate, book.PublishedDate);
            Assert.Equal(genre, book.Genre);
        }
    }
}