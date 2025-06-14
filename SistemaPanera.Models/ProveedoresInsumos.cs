using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class ProveedoresInsumos
{
    public int Id { get; set; }

    public int IdProveedor { get; set; }

    public string? Codigo { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal CostoUnitario { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual ICollection<FacturasProveedoresInsumo> FacturasProveedoresInsumos { get; set; } = new List<FacturasProveedoresInsumo>();

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;

    public virtual ICollection<InsumosProveedor> InsumosProveedores { get; set; } = new List<InsumosProveedor>();

    public virtual ICollection<OrdenesComprasInsumo> OrdenesComprasInsumos { get; set; } = new List<OrdenesComprasInsumo>();
}
