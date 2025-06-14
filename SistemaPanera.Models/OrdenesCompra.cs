using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class OrdenesCompra
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public int IdLocal { get; set; }

    public DateTime FechaEmision { get; set; }

    public int IdProveedor { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public decimal CostoTotal { get; set; }

    public int IdEstado { get; set; }

    public string? NotaInterna { get; set; }

    public virtual ICollection<FacturasProveedor> FacturasProveedores { get; set; } = new List<FacturasProveedor>();

    public virtual OrdenesComprasEstado IdEstadoNavigation { get; set; } = null!;

    public virtual Local IdLocalNavigation { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

    public virtual ICollection<OrdenesComprasInsumo> OrdenesComprasInsumos { get; set; } = new List<OrdenesComprasInsumo>();
}
