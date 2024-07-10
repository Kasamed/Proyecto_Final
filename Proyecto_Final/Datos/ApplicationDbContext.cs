using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Datos;
using Proyecto_Final.Models;

namespace Proyecto_Final.Datos
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<Comprador> Comprador { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Venta)
                .WithMany(v => v.DetalleVenta)
                .HasForeignKey(dv => dv.Id_venta);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Comprador)
                .WithMany(c => c.DetalleVenta)
                .HasForeignKey(dv => dv.Id_comprador);
        }
    }
}
