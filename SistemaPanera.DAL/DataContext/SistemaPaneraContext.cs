﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaPanera.Models;

namespace SistemaPanera.DAL.DataContext;

public partial class SistemaPaneraContext : DbContext
{
    public SistemaPaneraContext()
    {
    }

    public SistemaPaneraContext(DbContextOptions<SistemaPaneraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<ComprasDetalle> ComprasDetalles { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<EstadosUsuario> EstadosUsuarios { get; set; }

    public virtual DbSet<FormasdePago> FormasdePagos { get; set; }

    public virtual DbSet<Insumo> Insumos { get; set; }

    public virtual DbSet<InsumosCategoria> InsumosCategorias { get; set; }

    public virtual DbSet<InsumosStock> InsumosStocks { get; set; }

    public virtual DbSet<Local> Locales { get; set; }

    public virtual DbSet<Prefabricado> Prefabricados { get; set; }

    public virtual DbSet<PrefabricadosCategoria> PrefabricadosCategorias { get; set; }

    public virtual DbSet<PrefabricadosInsumo> PrefabricadosInsumos { get; set; }

    public virtual DbSet<PrefabricadosStock> PrefabricadosStocks { get; set; }

    public virtual DbSet<Proveedor> Proveedores { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Receta> Recetas { get; set; }

    public virtual DbSet<RecetasCategoria> RecetasCategorias { get; set; }

    public virtual DbSet<RecetasInsumo> RecetasInsumos { get; set; }

    public virtual DbSet<RecetasPrefabricado> RecetasPrefabricados { get; set; }

    public virtual DbSet<RecetasStock> RecetasStocks { get; set; }

    public virtual DbSet<RecetasTipo> RecetasTipos { get; set; }

    public virtual DbSet<Rol> Roles { get; set; }

    public virtual DbSet<UnidadesMedida> UnidadesMedida { get; set; }

    public virtual DbSet<UnidadesNegocio> UnidadesNegocios { get; set; }

    public virtual DbSet<User> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-J2J16BG\\SQLEXPRESS; Database=Sistema_Panera; Integrated Security=true; Trusted_Connection=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compra>(entity =>
        {
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.NumeroOrden)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdLocalNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compras_Locales");

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.Compras)
               .HasForeignKey(d => d.IdUnidadNegocio)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Compras_Unidades_Negocio");


        });

        modelBuilder.Entity<ComprasDetalle>(entity =>
        {
            entity.ToTable("Compras_Detalle");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.ComprasDetalles)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compras_Detalle_Compras1");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.ComprasDetalles)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compras_Detalle_Insumos");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadosUsuario>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FormasdePago>(entity =>
        {
            entity.ToTable("FormasdePago");

            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Insumo>(entity =>
        {
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.Sku)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Insumos_Insumos_Categorias");

            entity.HasOne(d => d.IdUnidadMedidaNavigation).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Insumos_Unidades_Medida");

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.IdUnidadNegocio)
                .HasConstraintName("FK_Insumos_Unidades_Negocio");
        });

        modelBuilder.Entity<InsumosCategoria>(entity =>
        {
            entity.ToTable("Insumos_Categorias");

            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InsumosStock>(entity =>
        {
            entity.ToTable("Insumos_Stock");

            entity.Property(e => e.Egreso).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Ingreso).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Local>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.Locales)
                .HasForeignKey(d => d.IdUnidadNegocio)
                .HasConstraintName("FK_Locales_Unidades_Negocio");
        });

        modelBuilder.Entity<Prefabricado>(entity =>
        {
            entity.Property(e => e.CostoTotal).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.Sku)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Prefabricados)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prefabricados_Prefabricados_Categorias");

            entity.HasOne(d => d.IdUnidadMedidaNavigation).WithMany(p => p.Prefabricados)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prefabricados_Unidades_Medida");

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.Prefabricados)
                .HasForeignKey(d => d.IdUnidadNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prefabricados_Unidades_Negocio");
        });

        modelBuilder.Entity<PrefabricadosCategoria>(entity =>
        {
            entity.ToTable("Prefabricados_Categorias");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PrefabricadosInsumo>(entity =>
        {
            entity.ToTable("Prefabricados_Insumos");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.PrefabricadosInsumos)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prefabricados_Insumos_Insumos");

            entity.HasOne(d => d.IdPrefabricadoNavigation).WithMany(p => p.PrefabricadosInsumos)
                .HasForeignKey(d => d.IdPrefabricado)
                .HasConstraintName("FK_Prefabricados_Insumos_Prefabricados");
        });

        modelBuilder.Entity<PrefabricadosStock>(entity =>
        {
            entity.ToTable("Prefabricados_Stock");

            entity.Property(e => e.Egreso).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Ingreso).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdLocalNavigation).WithMany(p => p.PrefabricadosStocks)
                .HasForeignKey(d => d.IdLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prefabricados_Stock_Prefabricados_Stock");

            entity.HasOne(d => d.IdPrefabricadoNavigation).WithMany(p => p.PrefabricadosStocks)
                .HasForeignKey(d => d.IdPrefabricado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prefabricados_Stock_Prefabricados");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.Property(e => e.Apodo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Cbu)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CBU");
            entity.Property(e => e.Cuit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CUIT");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Receta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Productos");

            entity.Property(e => e.CostoInsumos).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoPrefabricados).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoTotal).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.Sku)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Recetas_Categorias");

            entity.HasOne(d => d.IdUnidadMedidaNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Unidades_Medida");

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.IdUnidadNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Unidades_Negocio");
        });

        modelBuilder.Entity<RecetasCategoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Productos_Categorias");

            entity.ToTable("Recetas_Categorias");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RecetasInsumo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Productos_Insumos");

            entity.ToTable("Recetas_Insumos");

            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.RecetasInsumos)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Insumos_Insumos");

            entity.HasOne(d => d.IdRecetaNavigation).WithMany(p => p.RecetasInsumos)
                .HasForeignKey(d => d.IdReceta)
                .HasConstraintName("FK_Recetas_Insumos_Recetas");
        });

        modelBuilder.Entity<RecetasPrefabricado>(entity =>
        {
            entity.ToTable("Recetas_Prefabricados");

            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdPrefabricadoNavigation).WithMany(p => p.RecetasPrefabricados)
                .HasForeignKey(d => d.IdPrefabricado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Prefabricados_Prefabricados");

            entity.HasOne(d => d.IdRecetaNavigation).WithMany(p => p.RecetasPrefabricados)
                .HasForeignKey(d => d.IdReceta)
                .HasConstraintName("FK_Recetas_Prefabricados_Recetas");
        });

        modelBuilder.Entity<RecetasStock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Productos_Stock");

            entity.ToTable("Recetas_Stock");

            entity.Property(e => e.Egreso).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Ingreso).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdLocalNavigation).WithMany(p => p.RecetasStocks)
                .HasForeignKey(d => d.IdLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Stock_Locales");
        });

        modelBuilder.Entity<RecetasTipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Productos_Tipos");

            entity.ToTable("Recetas_Tipos");

            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UnidadesMedida>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProductosUnidadesDeMedida");

            entity.ToTable("Unidades_Medida");

            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UnidadesNegocio>(entity =>
        {
            entity.ToTable("Unidades_Negocio");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Usuario");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_EstadosUsuarios");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
