using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class Local
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<FacturasProveedor> FacturasProveedores { get; set; } = new List<FacturasProveedor>();

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

    public virtual ICollection<InsumosStock> InsumosStocks { get; set; } = new List<InsumosStock>();

    public virtual ICollection<OrdenesCompra> OrdenesCompras { get; set; } = new List<OrdenesCompra>();

    public virtual ICollection<RecetasStock> RecetasStocks { get; set; } = new List<RecetasStock>();

    public virtual ICollection<SubrecetasStock> SubrecetasStocks { get; set; } = new List<SubrecetasStock>();
}
