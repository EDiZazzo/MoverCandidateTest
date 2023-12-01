using Microsoft.EntityFrameworkCore;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.EntityFramework;

public class EfInventoryItemContext : DbContext
{
    public DbSet<InventoryItem> Inventory { get; set; }

    public EfInventoryItemContext(DbContextOptions<EfInventoryItemContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.Sku);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.Quantity)
                .IsRequired()
                .HasDefaultValue(0);
        });
    }
}