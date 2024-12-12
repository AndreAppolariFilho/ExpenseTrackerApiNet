using Microsoft.EntityFrameworkCore;

public class InMemoryDBSetContext : DbContext
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Expense> Expenses { get; set; } = null!;
    public InMemoryDBSetContext(DbContextOptions<InMemoryDBSetContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

    }

}
