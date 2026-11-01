using Microsoft.EntityFrameworkCore;

public class Calbaza_Ioan_Lab2Context(DbContextOptions<Calbaza_Ioan_Lab2Context> options) : DbContext(options)
{
    public DbSet<Calbaza_Ioan_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Calbaza_Ioan_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    public DbSet<Calbaza_Ioan_Lab2.Models.Authors> Authors { get; set; } = default!;
}
