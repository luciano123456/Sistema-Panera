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

    public virtual ICollection<InsumosProveedor> InsumosProveedores { get; set; } = new List<InsumosProveedor>();

    public virtual ICollection<ProveedoresInsumos> ProveedoresInsumos { get; set; } = new List<ProveedoresInsumos>();
}
