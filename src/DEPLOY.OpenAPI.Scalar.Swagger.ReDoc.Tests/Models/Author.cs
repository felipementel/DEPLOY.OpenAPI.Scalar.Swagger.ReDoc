public class Author
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    private readonly List<Book> _books = new List<Book>();
    public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

    public void AddBook(Book book)
    {
        _books.Add(book);
    }
}