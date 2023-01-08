using Microsoft.EntityFrameworkCore;

namespace KingsFarms.Core.Api.Data.memory;

public class ContactsContext : DbContext
{
    public ContactsContext(DbContextOptions<ContactsContext> options) : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().HasData(
            new Contact { Id = 1, First = "Steve", Last = "Mich" }
            ,
            new Contact { Id = 2, First = "Bill", Last = "Gat" },
            new Contact { Id = 3, First = "Sat", Last = "Nad" }
        );
    }
}

public class Contact
{
    public string Last { get; set; }
    public string First { get; set; }
    public int Id { get; set; }
}