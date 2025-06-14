using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class Proveedor
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apodo { get; set; }

    public string? Ubicacion { get; set; }

    public string? Telefono { get; set; }

    public string? Cbu { get; set; }

    public string? Cuit { get; set; }

    public virtual ICollection<FacturasProveedor> FacturasProveedores { get; set; } = new List<FacturasProveedor>();

    public virtual ICollection<OrdenesCompra> OrdenesCompras { get; set; } = new List<OrdenesCompra>();

    public virtual ICollection<ProveedoresInsumos> ProveedoresInsumosLista { get; set; } = new List<ProveedoresInsumos>();
}
