using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AuthenticationProyect.Models
{
    public partial class PRUEBATECNICANELSONREYESContext : DbContext
    {
        public PRUEBATECNICANELSONREYESContext()
        {
        }

        public PRUEBATECNICANELSONREYESContext(DbContextOptions<PRUEBATECNICANELSONREYESContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Negocio> Negocios { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserTipo> UserTipos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(e => e.NombreCategoria)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.CategoriaId).HasColumnName("Categoria_id");

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.NegocioId).HasColumnName("Negocio_id");

                entity.Property(e => e.NombreItem)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Items__Categoria__2D27B809");

                entity.HasOne(d => d.Negocio)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.NegocioId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Items__Negocio_i__2E1BDC42");
            });

            modelBuilder.Entity<Negocio>(entity =>
            {
                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.FechaCreacion).HasColumnType("date");

                entity.Property(e => e.NombreNegocio)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioId).HasColumnName("Usuario_id");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Negocios)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_Usuario_Negocio");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Apellidos)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Contrasenia)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("contrasenia");

                entity.Property(e => e.Correo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TipoId).HasColumnName("Tipo_id");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("FK__Users__Tipo_id__2E1BDC42");
            });

            modelBuilder.Entity<UserTipo>(entity =>
            {
                entity.Property(e => e.NombreTipoUsuario)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
