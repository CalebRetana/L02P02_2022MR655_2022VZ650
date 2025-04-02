using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace L02P02_2022MR655_2022VZ650.Models;

public partial class LibreriaContext : DbContext
{
    public LibreriaContext()
    {
    }

    public LibreriaContext(DbContextOptions<LibreriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autore> Autores { get; set; }
    public virtual DbSet<Categoria> Categorias { get; set; }
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<ComentariosLibro> ComentariosLibros { get; set; }
    public virtual DbSet<Libro> Libros { get; set; }
    public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }
    public virtual DbSet<PedidoEncabezado> PedidoEncabezados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-2NRPP4K;Database=LibreriaEntityDb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__autores__3213E83F8911CC3E");
            entity.ToTable("autores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Autor)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("autor");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__categori__3213E83FF57B2B14");
            entity.ToTable("categorias");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("categoria");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__clientes__3213E83F93F299FF");
            entity.ToTable("clientes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(255)
                .HasColumnName("apellido");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<ComentariosLibro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__comentar__3213E83FE844A9F2");
            entity.ToTable("comentarios_libros");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comentarios)
                .IsUnicode(false)
                .HasColumnName("comentarios");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdLibro).HasColumnName("id_libro");
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario");

            entity.HasOne(d => d.IdLibroNavigation).WithMany(p => p.ComentariosLibros)
                .HasForeignKey(d => d.IdLibro)
                .HasConstraintName("FK__comentari__id_li__4BAC3F29");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__libros__3213E83F097CE66D");
            entity.ToTable("libros");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.IdAutor).HasColumnName("id_autor");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.UrlImagen)
                .HasMaxLength(255)
                .HasColumnName("url_imagen");

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.Libros)
                .HasForeignKey(d => d.IdAutor)
                .HasConstraintName("FK__libros__id_autor__3E52440B");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Libros)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK__libros__id_categ__3F466844");
        });

        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pedido_d__3213E83F17A1D77B");
            entity.ToTable("pedido_detalle");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdLibro).HasColumnName("id_libro");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");

            entity.HasOne(d => d.IdLibroNavigation).WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.IdLibro)
                .HasConstraintName("FK__pedido_de__id_li__47DBAE45");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__pedido_de__id_pe__46E78A0C");
        });

        modelBuilder.Entity<PedidoEncabezado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pedido_e__3213E83F1B362EBF");
            entity.ToTable("pedido_encabezado");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadLibros).HasColumnName("cantidad_libros");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente")
                .HasColumnName("estado");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.PedidoEncabezados)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__pedido_en__id_cl__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
