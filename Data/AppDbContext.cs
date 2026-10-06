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
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Account → User
        modelBuilder.Entity<Account>()
            .HasOne(account => account.User)
            .WithOne()
            .HasForeignKey<Account>(account => account.UserId);

        // Transaction → Account
        modelBuilder.Entity<Transaction>()
            .HasOne(transaction => transaction.Account)
            .WithMany()
            .HasForeignKey(transaction => transaction.AccountId);

        // Order → User
        modelBuilder.Entity<Order>()
            .HasOne(order => order.User)
            .WithMany()
            .HasForeignKey(order => order.UserId);

        // Order → Asset
        modelBuilder.Entity<Order>()
            .HasOne(order => order.Asset)
            .WithMany()
            .HasForeignKey(order => order.AssetId);

        // Decimal precision
        modelBuilder.Entity<Account>()
            .Property(account => account.Balance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .Property(transaction => transaction.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Asset>()
            .Property(asset => asset.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(order => order.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(order => order.TotalAmount)
            .HasPrecision(18, 2);

        base.OnModelCreating(modelBuilder);
    }
}