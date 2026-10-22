using ApiOrnek;
using Microsoft.EntityFrameworkCore;


public class DbNot : DbContext
{
    public DbNot(DbContextOptions<DbNot> Options) : base(Options)
    {

    }
    public DbSet<Not> notlar => Set<Not>();
    public DbSet<Baslik> Basliklar => Set<Baslik>();
}