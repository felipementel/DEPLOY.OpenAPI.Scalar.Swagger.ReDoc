using Xunit;

namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.Tests.Models
{
    public class BookGenreTests
    {
        [Fact]
        public void BookGenre_ShouldHaveExpectedValues()
        {
            // Assert - Verify all expected enum values exist
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.Fiction));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.NonFiction));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.ScienceFiction));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.Fantasy));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.Mystery));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.Romance));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.Thriller));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.Horror));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.Biography));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.History));
            Assert.True(Enum.IsDefined(typeof(BookGenre), BookGenre.Poetry));
        }

        [Fact]
        public void BookGenre_ShouldHaveCorrectCount()
        {
            // Assert - Verify we have exactly 11 genres
            var genreValues = Enum.GetValues(typeof(BookGenre));
            Assert.Equal(11, genreValues.Length);
        }

        [Fact]
        public void BookGenre_DefaultValue_ShouldBeFiction()
        {
            // Act
            var defaultGenre = default(BookGenre);

            // Assert
            Assert.Equal(BookGenre.Fiction, defaultGenre);
        }

        [Theory]
        [InlineData(BookGenre.Fiction, "Fiction")]
        [InlineData(BookGenre.NonFiction, "NonFiction")]
        [InlineData(BookGenre.ScienceFiction, "ScienceFiction")]
        [InlineData(BookGenre.Fantasy, "Fantasy")]
        [InlineData(BookGenre.Mystery, "Mystery")]
        [InlineData(BookGenre.Romance, "Romance")]
        [InlineData(BookGenre.Thriller, "Thriller")]
        [InlineData(BookGenre.Horror, "Horror")]
        [InlineData(BookGenre.Biography, "Biography")]
        [InlineData(BookGenre.History, "History")]
        [InlineData(BookGenre.Poetry, "Poetry")]
        public void BookGenre_ToString_ShouldReturnCorrectName(BookGenre genre, string expectedName)
        {
            // Act
            var result = genre.ToString();

            // Assert
            Assert.Equal(expectedName, result);
        }
    }
}