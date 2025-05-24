using System;
using System.Collections.Generic;
namespace SistemaPanera.Models;
public partial class Insumo
{
    public int Id { get; set; }

    public string? Sku { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdUnidadMedida { get; set; }

    public int IdCategoria { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual ICollection<ComprasDetalle> ComprasDetalles { get; set; } = new List<ComprasDetalle>();

    public virtual InsumosCategoria IdCategoriaNavigation { get; set; } = null!;

    public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

    public virtual ICollection<InsumosProveedor> InsumosProveedores { get; set; } = new List<InsumosProveedor>();

    public virtual ICollection<InsumosUnidadesNegocio> InsumosUnidadesNegocios { get; set; } = new List<InsumosUnidadesNegocio>();

    public virtual ICollection<RecetasInsumo> RecetasInsumos { get; set; } = new List<RecetasInsumo>();

    public virtual ICollection<SubrecetasInsumo> SubrecetasInsumos { get; set; } = new List<SubrecetasInsumo>();
}
