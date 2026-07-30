using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Producto>()
                .HasOne(producto => producto.Categoria)
                .WithMany(categoria => categoria.Productos)
                .HasForeignKey(producto => producto.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Producto>().HasIndex(p => p.Codigo).IsUnique();

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasIndex(f => f.NumeroFactura).IsUnique();
                entity.HasOne(f => f.Cliente).WithMany(c => c.Facturas).HasForeignKey(f => f.ClienteId).OnDelete(DeleteBehavior.Restrict);// Relación uno a muchos entre Factura y Cliente
                entity.HasMany(f => f.Detalles).WithOne(d => d.Factura).HasForeignKey(d => d.FacturaId).OnDelete(DeleteBehavior.Cascade);// Relación uno a muchos entre Factura y FacturaDetalle
                entity.HasOne(f => f.FacturaOrigen).WithMany(f => f.NotasCredito).HasForeignKey(f => f.FacturaOrigenId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(d => d.Producto).WithMany(p => p.FacturaDetalles).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.Restrict);// Relación uno a muchos entre FacturaDetalle y Producto

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Producto> Producto { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalle { get; set; }

    }
}
