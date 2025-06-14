using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class FacturasProveedor
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public int IdLocal { get; set; }

    public int IdOrdenCompra { get; set; }

    public DateTime Fecha { get; set; }

    public int IdProveedor { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Descuentos { get; set; }

    public decimal SubtotalFinal { get; set; }

    public string? NotaInterna { get; set; }

    public virtual ICollection<FacturasProveedoresInsumo> FacturasProveedoresInsumos { get; set; } = new List<FacturasProveedoresInsumo>();

    public virtual Local IdLocalNavigation { get; set; } = null!;

    public virtual OrdenesCompra IdOrdenCompraNavigation { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;
}
