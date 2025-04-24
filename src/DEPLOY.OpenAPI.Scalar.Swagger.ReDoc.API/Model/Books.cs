public class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public Author Author { get; set; } = new Author();

    public decimal Price { get; set; }

    public DateTime PublishedDate { get; set; }

    public BookGenre Genre { get; set; }
}

public enum BookGenre
{
    Fiction,
    NonFiction,
    ScienceFiction,
    Fantasy,
    Mystery,
    Romance,
    Thriller,
    Horror,
    Biography,
    History,
    Poetry
}