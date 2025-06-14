using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class UnidadesNegocio
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<FacturasProveedor> FacturasProveedores { get; set; } = new List<FacturasProveedor>();

    public virtual ICollection<InsumosStock> InsumosStocks { get; set; } = new List<InsumosStock>();

    public virtual ICollection<InsumosUnidadesNegocio> InsumosUnidadesNegocios { get; set; } = new List<InsumosUnidadesNegocio>();

    public virtual ICollection<Local> Locales { get; set; } = new List<Local>();

    public virtual ICollection<OrdenesCompra> OrdenesCompras { get; set; } = new List<OrdenesCompra>();

    public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();

    public virtual ICollection<Subreceta> Subreceta { get; set; } = new List<Subreceta>();
}
