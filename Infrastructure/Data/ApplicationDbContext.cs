using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Domain.Entities;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Orden> Ordenes => Set<Orden>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<OrdenProducto> OrdenesProductos => Set<OrdenProducto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrdenProducto>()
            .HasOne(op => op.Orden)
            .WithMany(o => o.Productos)
            .HasForeignKey(op => op.OrdenId);

        modelBuilder.Entity<OrdenProducto>()
            .HasOne(op => op.Producto)
            .WithMany(p => p.Ordenes)
            .HasForeignKey(op => op.ProductoId);

        modelBuilder.Entity<Orden>()
            .Property(o => o.Total)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasPrecision(18, 2);
    }
}
