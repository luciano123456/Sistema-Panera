using System;
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

    public virtual DbSet<EstadosUsuario> EstadosUsuarios { get; set; }

    public virtual DbSet<FacturasProveedor> FacturasProveedores { get; set; }

    public virtual DbSet<FacturasProveedoresInsumo> FacturasProveedoresInsumos { get; set; }

    public virtual DbSet<Insumo> Insumos { get; set; }

    public virtual DbSet<InsumosCategoria> InsumosCategorias { get; set; }

    public virtual DbSet<InsumosProveedor> InsumosProveedores { get; set; }

    public virtual DbSet<InsumosStock> InsumosStocks { get; set; }

    public virtual DbSet<InsumosUnidadesNegocio> InsumosUnidadesNegocios { get; set; }

    public virtual DbSet<Local> Locales { get; set; }

    public virtual DbSet<OrdenesCompra> OrdenesCompras { get; set; }

    public virtual DbSet<OrdenesComprasEstado> OrdenesComprasEstados { get; set; }

    public virtual DbSet<OrdenesComprasInsumo> OrdenesComprasInsumos { get; set; }

    public virtual DbSet<OrdenesComprasInsumosEstado> OrdenesComprasInsumosEstados { get; set; }

    public virtual DbSet<Proveedor> Proveedores { get; set; }

    public virtual DbSet<ProveedoresInsumos> ProveedoresInsumos { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Receta> Recetas { get; set; }

    public virtual DbSet<RecetasCategoria> RecetasCategorias { get; set; }

    public virtual DbSet<RecetasInsumo> RecetasInsumos { get; set; }

    public virtual DbSet<RecetasStock> RecetasStocks { get; set; }

    public virtual DbSet<RecetasSubreceta> RecetasSubrecetas { get; set; }

    public virtual DbSet<Rol> Roles { get; set; }

    public virtual DbSet<Subreceta> Subrecetas { get; set; }

    public virtual DbSet<SubrecetasCategoria> SubrecetasCategorias { get; set; }

    public virtual DbSet<SubrecetasInsumo> SubrecetasInsumos { get; set; }

    public virtual DbSet<SubrecetasStock> SubrecetasStocks { get; set; }

    public virtual DbSet<SubrecetasSubreceta> SubrecetasSubrecetas { get; set; }

    public virtual DbSet<UnidadesMedida> UnidadesMedida { get; set; }

    public virtual DbSet<UnidadesNegocio> UnidadesNegocios { get; set; }

    public virtual DbSet<User> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-3MT5F5F; Database=Sistema_Panera; Integrated Security=true; Trusted_Connection=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compra>(entity =>
        {
            entity.Property(e => e.Costo).HasColumnType("decimal(20, 2)");
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

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.ComprasDetalles)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compras_Detalle_Insumos");
        });

        modelBuilder.Entity<EstadosUsuario>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FacturasProveedor>(entity =>
        {
            entity.Property(e => e.Descuentos).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.NotaInterna)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Subtotal).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.SubtotalFinal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdLocalNavigation).WithMany(p => p.FacturasProveedores)
                .HasForeignKey(d => d.IdLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturasProveedores_Locales");

            entity.HasOne(d => d.IdOrdenCompraNavigation).WithMany(p => p.FacturasProveedores)
                .HasForeignKey(d => d.IdOrdenCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturasProveedores_OrdenesCompras");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.FacturasProveedores)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturasProveedores_Proveedores");

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.FacturasProveedores)
                .HasForeignKey(d => d.IdUnidadNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturasProveedores_Unidades_Negocio");
        });

        modelBuilder.Entity<FacturasProveedoresInsumo>(entity =>
        {
            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.DescuentoTotal).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.DescuentoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Diferencia).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.PorcDescuento).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.PrecioFactura).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.PrecioFinal).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.PrecioLista).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.SubtotalConDescuento).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.SubtotalFinal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdFacturaProveedorNavigation).WithMany(p => p.FacturasProveedoresInsumos)
                .HasForeignKey(d => d.IdFacturaProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturasProveedoresInsumos_FacturasProveedores");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.FacturasProveedoresInsumos)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturasProveedoresInsumos_Insumos");

            entity.HasOne(d => d.IdInsumoStockNavigation).WithMany(p => p.FacturasProveedoresInsumos)
                .HasForeignKey(d => d.IdInsumoStock)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturasProveedoresInsumos_Insumos_Stock");

            entity.HasOne(d => d.IdProveedorListaNavigation).WithMany(p => p.FacturasProveedoresInsumos)
                .HasForeignKey(d => d.IdProveedorLista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturasProveedoresInsumos_Proveedores_Insumos_Listas");
        });

        modelBuilder.Entity<Insumo>(entity =>
        {
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
        });

        modelBuilder.Entity<InsumosCategoria>(entity =>
        {
            entity.ToTable("Insumos_Categorias");

            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InsumosProveedor>(entity =>
        {
            entity.ToTable("Insumos_Proveedores");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.InsumosProveedores)
                .HasForeignKey(d => d.IdInsumo)
                .HasConstraintName("FK_Insumos_Proveedores_Insumos");

            entity.HasOne(d => d.IdListaProveedorNavigation).WithMany(p => p.InsumosProveedores)
                .HasForeignKey(d => d.IdListaProveedor)
                .HasConstraintName("FK_Insumos_Proveedores_Proveedores_Insumos_Listas");
        });

        modelBuilder.Entity<InsumosStock>(entity =>
        {
            entity.ToTable("Insumos_Stock");

            entity.Property(e => e.Concepto)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Entrada).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Salida).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(70)
                .IsUnicode(false);

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.InsumosStocks)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Insumos_Stock_Insumos");

            entity.HasOne(d => d.IdLocalNavigation).WithMany(p => p.InsumosStocks)
                .HasForeignKey(d => d.IdLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Insumos_Stock_Locales");

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.InsumosStocks)
                .HasForeignKey(d => d.IdUnidadNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Insumos_Stock_Unidades_Negocio");
        });

        modelBuilder.Entity<InsumosUnidadesNegocio>(entity =>
        {
            entity.ToTable("Insumos_UnidadesNegocio");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.InsumosUnidadesNegocios)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Insumos_UnidadesNegocio_Insumos_UnidadesNegocio");

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.InsumosUnidadesNegocios)
                .HasForeignKey(d => d.IdUnidadNegocio)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Insumos_UnidadesNegocio_Unidades_Negocio");
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

        modelBuilder.Entity<OrdenesCompra>(entity =>
        {
            entity.Property(e => e.CostoTotal).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.FechaEmision).HasColumnType("datetime");
            entity.Property(e => e.FechaEntrega).HasColumnType("datetime");
            entity.Property(e => e.NotaInterna)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.OrdenesCompras)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenesCompras_OrdenesComprasEstados");

            entity.HasOne(d => d.IdLocalNavigation).WithMany(p => p.OrdenesCompras)
                .HasForeignKey(d => d.IdLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenesCompras_Locales");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.OrdenesCompras)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenesCompras_Proveedores");

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.OrdenesCompras)
                .HasForeignKey(d => d.IdUnidadNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenesCompras_Unidades_Negocio");
        });

        modelBuilder.Entity<OrdenesComprasEstado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Estados");

            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrdenesComprasInsumo>(entity =>
        {
            entity.Property(e => e.CantidadEntregada).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CantidadPedida).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CantidadRestante).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.NotaInterna)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PrecioLista).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.OrdenesComprasInsumos)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenesComprasInsumos_OrdenesComprasInsumosEstados");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.OrdenesComprasInsumos)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenesComprasInsumos_Insumos");

            entity.HasOne(d => d.IdOrdenCompraNavigation).WithMany(p => p.OrdenesComprasInsumos)
                .HasForeignKey(d => d.IdOrdenCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenesComprasInsumos_OrdenesCompras");

            entity.HasOne(d => d.IdProveedorListaNavigation).WithMany(p => p.OrdenesComprasInsumos)
                .HasForeignKey(d => d.IdProveedorLista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenesComprasInsumos_Proveedores_Insumos_Listas");
        });

        modelBuilder.Entity<OrdenesComprasInsumosEstado>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
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

        modelBuilder.Entity<ProveedoresInsumos>(entity =>
        {
            entity.ToTable("Proveedores_Insumos_Listas");

            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.ProveedoresInsumosLista)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK_Proveedores_Insumos_Listas_Proveedores");
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
            entity.Property(e => e.CostoPorcion).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoSubRecetas).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.Rendimiento).HasColumnType("decimal(20, 2)");
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

            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 2)");
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

        modelBuilder.Entity<RecetasSubreceta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Recetas_Prefabricados");

            entity.ToTable("Recetas_Subrecetas");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdRecetaNavigation).WithMany(p => p.RecetasSubreceta)
                .HasForeignKey(d => d.IdReceta)
                .HasConstraintName("FK_Recetas_Prefabricados_Recetas");

            entity.HasOne(d => d.IdSubRecetaNavigation).WithMany(p => p.RecetasSubreceta)
                .HasForeignKey(d => d.IdSubReceta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Subrecetas_Subrecetas");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Subreceta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Prefabricados");

            entity.Property(e => e.CostoInsumos).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoPorcion).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoSubRecetas).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.Rendimiento).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Subreceta)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subrecetas_Subrecetas_Categorias");

            entity.HasOne(d => d.IdUnidadMedidaNavigation).WithMany(p => p.Subreceta)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subrecetas_Unidades_Medida");

            entity.HasOne(d => d.IdUnidadNegocioNavigation).WithMany(p => p.Subreceta)
                .HasForeignKey(d => d.IdUnidadNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subrecetas_Unidades_Negocio");
        });

        modelBuilder.Entity<SubrecetasCategoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Prefabricados_Categorias");

            entity.ToTable("Subrecetas_Categorias");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SubrecetasInsumo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Prefabricados_Insumos");

            entity.ToTable("Subrecetas_Insumos");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.SubrecetasInsumos)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prefabricados_Insumos_Insumos");

            entity.HasOne(d => d.IdSubrecetaNavigation).WithMany(p => p.SubrecetasInsumos)
                .HasForeignKey(d => d.IdSubreceta)
                .HasConstraintName("FK_Subrecetas_Insumos_Subrecetas");
        });

        modelBuilder.Entity<SubrecetasStock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Prefabricados_Stock");

            entity.ToTable("Subrecetas_Stock");

            entity.Property(e => e.Egreso).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Ingreso).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdLocalNavigation).WithMany(p => p.SubrecetasStocks)
                .HasForeignKey(d => d.IdLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prefabricados_Stock_Prefabricados_Stock");

            entity.HasOne(d => d.IdSubrecetaNavigation).WithMany(p => p.SubrecetasStocks)
                .HasForeignKey(d => d.IdSubreceta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subrecetas_Stock_Subrecetas");
        });

        modelBuilder.Entity<SubrecetasSubreceta>(entity =>
        {
            entity.ToTable("Subrecetas_Subrecetas");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.IdSubRecetaHijaNavigation).WithMany(p => p.SubrecetasSubrecetaIdSubRecetaHijaNavigations)
                .HasForeignKey(d => d.IdSubRecetaHija)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subrecetas_Subrecetas_Subrecetas");

            entity.HasOne(d => d.IdSubRecetaPadreNavigation).WithMany(p => p.SubrecetasSubrecetaIdSubRecetaPadreNavigations)
                .HasForeignKey(d => d.IdSubRecetaPadre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subrecetas_Subrecetas_Subrecetas1");
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
