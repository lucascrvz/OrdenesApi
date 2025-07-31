using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Orden> Ordenes => Set<Orden>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<OrdenProducto> OrdenesProductos => Set<OrdenProducto>();
        public DbSet<User> Users => Set<User>();

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

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Password)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "lucas", Password = "AQAAAAIAAYagAAAAEDE9Oywd5hn0dOFGuGfVHkA+DRrGiDJhOmEevDij34hRcWpAYZ9I8IulporLriXUsw==" }
            );
            
            modelBuilder.Entity<Producto>().HasData(
                new Producto { Id = 1, Nombre = "Monitor", Precio = 100 },
                new Producto { Id = 2, Nombre = "Teclado", Precio = 50 },
                new Producto { Id = 3, Nombre = "Mouse", Precio = 25 },
                new Producto { Id = 4, Nombre = "Notebook", Precio = 800 },
                new Producto { Id = 5, Nombre = "Impresora", Precio = 200 },
                new Producto { Id = 6, Nombre = "Escáner", Precio = 150 },
                new Producto { Id = 7, Nombre = "Webcam", Precio = 75 }
            );
        }
    }
}
