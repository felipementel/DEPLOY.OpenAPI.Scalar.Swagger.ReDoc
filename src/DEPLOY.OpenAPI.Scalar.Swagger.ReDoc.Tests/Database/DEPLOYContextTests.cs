using DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests.Database
{
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
        public void DEPLOYContext_Constructor_ShouldInitializeDbSets()
        {
            // Act & Assert
            Assert.NotNull(_context.Authors);
            Assert.NotNull(_context.Books);
        }

        [Fact]
        public async Task DEPLOYContext_AddAuthor_ShouldSaveToDatabase()
        {
            // Arrange
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Felipe Augusto"
            };

            // Act
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            // Assert
            var savedAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == author.Id);
            Assert.NotNull(savedAuthor);
            Assert.Equal(author.Name, savedAuthor.Name);
        }

        [Fact]
        public async Task DEPLOYContext_AddBook_ShouldSaveToDatabase()
        {
            // Arrange
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Felipe Augusto"
            };

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
            _context.Authors.Add(author);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Assert
            var savedBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            Assert.NotNull(savedBook);
            Assert.Equal(book.Title, savedBook.Title);
            Assert.Equal(book.Price, savedBook.Price);
            Assert.Equal(book.Genre, savedBook.Genre);
        }

        [Fact]
        public async Task DEPLOYContext_UpdateAuthor_ShouldModifyDatabase()
        {
            // Arrange
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Original Name"
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            // Act
            author.Name = "Updated Name";
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();

            // Assert
            var updatedAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == author.Id);
            Assert.NotNull(updatedAuthor);
            Assert.Equal("Updated Name", updatedAuthor.Name);
        }

        [Fact]
        public async Task DEPLOYContext_DeleteAuthor_ShouldRemoveFromDatabase()
        {
            // Arrange
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Felipe Augusto"
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            // Act
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            // Assert
            var deletedAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == author.Id);
            Assert.Null(deletedAuthor);
        }

        [Fact]
        public async Task DEPLOYContext_UpdateBook_ShouldModifyDatabase()
        {
            // Arrange
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Felipe Augusto"
            };

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Original Title",
                Author = author,
                Price = 29.99m,
                PublishedDate = DateTime.Now,
                Genre = BookGenre.Fiction
            };

            _context.Authors.Add(author);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Act
            book.Title = "Updated Title";
            book.Price = 39.99m;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            // Assert
            var updatedBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            Assert.NotNull(updatedBook);
            Assert.Equal("Updated Title", updatedBook.Title);
            Assert.Equal(39.99m, updatedBook.Price);
        }

        [Fact]
        public async Task DEPLOYContext_DeleteBook_ShouldRemoveFromDatabase()
        {
            // Arrange
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Felipe Augusto"
            };

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book",
                Author = author,
                Price = 29.99m,
                PublishedDate = DateTime.Now,
                Genre = BookGenre.Fiction
            };

            _context.Authors.Add(author);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Act
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            // Assert
            var deletedBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            Assert.Null(deletedBook);
        }

        [Fact]
        public async Task DEPLOYContext_GetAllAuthors_ShouldReturnAllSavedAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = Guid.NewGuid(), Name = "Author 1" },
                new Author { Id = Guid.NewGuid(), Name = "Author 2" },
                new Author { Id = Guid.NewGuid(), Name = "Author 3" }
            };

            _context.Authors.AddRange(authors);
            await _context.SaveChangesAsync();

            // Act
            var retrievedAuthors = await _context.Authors.ToListAsync();

            // Assert
            Assert.Equal(3, retrievedAuthors.Count);
            Assert.All(authors, author => 
                Assert.Contains(retrievedAuthors, a => a.Id == author.Id && a.Name == author.Name));
        }

        [Fact]
        public async Task DEPLOYContext_GetAllBooks_ShouldReturnAllSavedBooks()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "Felipe Augusto" };
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Book 1", Author = author, Price = 19.99m, PublishedDate = DateTime.Now, Genre = BookGenre.Fiction },
                new Book { Id = Guid.NewGuid(), Title = "Book 2", Author = author, Price = 29.99m, PublishedDate = DateTime.Now, Genre = BookGenre.NonFiction },
                new Book { Id = Guid.NewGuid(), Title = "Book 3", Author = author, Price = 39.99m, PublishedDate = DateTime.Now, Genre = BookGenre.ScienceFiction }
            };

            _context.Authors.Add(author);
            _context.Books.AddRange(books);
            await _context.SaveChangesAsync();

            // Act
            var retrievedBooks = await _context.Books.ToListAsync();

            // Assert
            Assert.Equal(3, retrievedBooks.Count);
            Assert.All(books, book => 
                Assert.Contains(retrievedBooks, b => b.Id == book.Id && b.Title == book.Title));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}