using Microsoft.EntityFrameworkCore;
namespace DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.Database
{
    public class DEPLOYContext : DbContext
    {
        public DEPLOYContext(DbContextOptions<DEPLOYContext> options)
         : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }
    }
}