using Microsoft.EntityFrameworkCore;
using UserApi.Models;

namespace UserApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasOne(account => account.User)
            .WithOne()
            .HasForeignKey<Account>(account => account.UserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(transaction => transaction.Account)
            .WithMany()
            .HasForeignKey(transaction => transaction.AccountId);

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Transaction> Transactions { get; set; }
}