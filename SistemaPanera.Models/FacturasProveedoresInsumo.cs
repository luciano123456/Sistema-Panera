using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class FacturasProveedoresInsumo
{
    public int Id { get; set; }

    public int IdFacturaProveedor { get; set; }

    public int IdInsumo { get; set; }

    public int IdProveedorLista { get; set; }

    public decimal Cantidad { get; set; }

    public decimal PrecioLista { get; set; }

    public decimal PrecioFactura { get; set; }

    public decimal Diferencia { get; set; }

    public decimal? PorcDescuento { get; set; }

    public decimal? DescuentoUnitario { get; set; }

    public decimal PrecioFinal { get; set; }

    public decimal? DescuentoTotal { get; set; }

    public decimal SubtotalConDescuento { get; set; }

    public decimal SubtotalFinal { get; set; }

    public int IdInsumoStock { get; set; }

    public virtual FacturasProveedor IdFacturaProveedorNavigation { get; set; } = null!;

    public virtual Insumo IdInsumoNavigation { get; set; } = null!;

    public virtual InsumosStock IdInsumoStockNavigation { get; set; } = null!;

    public virtual ProveedoresInsumos IdProveedorListaNavigation { get; set; } = null!;
}
