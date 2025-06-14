using Microsoft.EntityFrameworkCore;
using SistemaPanera.Models;
using SistemaPanera.DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public class OrdenesCompraRepository : IOrdenesCompraRepository
    {
        private readonly SistemaPaneraContext _dbcontext;

        public OrdenesCompraRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }

        public async Task<IEnumerable<OrdenesCompra>> ObtenerTodosAsync()
        {
            return await _dbcontext.OrdenesCompras
                .Include(o => o.IdProveedorNavigation)
                .Include(o => o.IdLocalNavigation)
                .Include(o => o.IdUnidadNegocioNavigation)
                .Include(o => o.IdEstadoNavigation)
                .Include(o => o.OrdenesComprasInsumos)
                    .ThenInclude(i => i.IdInsumoNavigation)
                .ToListAsync();
        }

        public async Task<OrdenesCompra?> ObtenerPorIdAsync(int id)
        {
            return await _dbcontext.OrdenesCompras
                .Include(o => o.IdProveedorNavigation)
                .Include(o => o.IdLocalNavigation)
                .Include(o => o.IdUnidadNegocioNavigation)
                .Include(o => o.IdEstadoNavigation)
                .Include(o => o.OrdenesComprasInsumos)
                    .ThenInclude(i => i.IdInsumoNavigation)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task CrearAsync(OrdenesCompra entidad)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();

            try
            {
                if (entidad.OrdenesComprasInsumos != null)
                {
                    foreach (var i in entidad.OrdenesComprasInsumos)
                    {
                        i.Id = 0;
                        i.IdEstado = 1;
                        i.CantidadEntregada = 0;
                        i.CantidadRestante = i.CantidadPedida;
                        if (i.IdProveedorLista == 0)
                            i.IdProveedorLista = null;
                    }
                }

                _dbcontext.OrdenesCompras.Add(entidad);
                await _dbcontext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Error al crear Orden de Compra: " + ex.Message);
                throw;
            }
        }

        public async Task EditarAsync(OrdenesCompra entidad)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();

            try
            {
                var existente = await _dbcontext.OrdenesCompras
                    .Include(x => x.OrdenesComprasInsumos)
                    .FirstOrDefaultAsync(x => x.Id == entidad.Id);

                if (existente == null)
                    throw new Exception("Orden de compra no encontrada.");

                // Actualizar campos principales
                _dbcontext.Entry(existente).CurrentValues.SetValues(entidad);

                // 🔹 Obtener los IDs que vienen del frontend
                var idsNuevos = entidad.OrdenesComprasInsumos.Select(i => i.Id).ToList();

                // 🔹 Eliminar insumos quitados
                var paraEliminar = existente.OrdenesComprasInsumos
                    .Where(x => !idsNuevos.Contains(x.Id))
                    .ToList();

                _dbcontext.OrdenesComprasInsumos.RemoveRange(paraEliminar);

                // 🔹 Mapear IDs temporales si hiciera falta (no lo usamos ahora pero queda preparado)
                var idMapping = new Dictionary<int, int>();

                foreach (var nuevo in entidad.OrdenesComprasInsumos)
                {
                    var insumoExistente = existente.OrdenesComprasInsumos
                        .FirstOrDefault(x => x.Id > 0 && x.Id == nuevo.Id);

                    if (insumoExistente != null)
                    {
                        insumoExistente.IdInsumo = nuevo.IdInsumo;
                        insumoExistente.CantidadPedida = nuevo.CantidadPedida;
                        insumoExistente.CantidadRestante = nuevo.CantidadPedida;
                        insumoExistente.PrecioLista = nuevo.PrecioLista;
                        insumoExistente.Subtotal = nuevo.Subtotal;
                        insumoExistente.NotaInterna = nuevo.NotaInterna;
                        insumoExistente.IdProveedorLista = nuevo.IdProveedorLista == 0 ? null : nuevo.IdProveedorLista;
                    }
                    else
                    {
                        // Si es nuevo (Id == 0 o no está en la base), lo agregamos
                        var nuevoInsumo = new OrdenesComprasInsumo
                        {
                            IdOrdenCompra = existente.Id,
                            IdInsumo = nuevo.IdInsumo,
                            CantidadPedida = nuevo.CantidadPedida,
                            CantidadEntregada = 0,
                            CantidadRestante = nuevo.CantidadPedida,
                            PrecioLista = nuevo.PrecioLista,
                            Subtotal = nuevo.Subtotal,
                            NotaInterna = nuevo.NotaInterna,
                            IdEstado = 1,
                            IdProveedorLista = nuevo.IdProveedorLista == 0 ? null : nuevo.IdProveedorLista
                        };

                        _dbcontext.OrdenesComprasInsumos.Add(nuevoInsumo);
                    }
                }

                await _dbcontext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Error al editar Orden de Compra: " + ex.Message);
                throw;
            }
        }

        public async Task EliminarAsync(int id)
        {
            try
            {
                var orden = await _dbcontext.OrdenesCompras
                    .Include(x => x.OrdenesComprasInsumos)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (orden != null)
                {
                    _dbcontext.OrdenesComprasInsumos.RemoveRange(orden.OrdenesComprasInsumos);
                    _dbcontext.OrdenesCompras.Remove(orden);
                    await _dbcontext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar Orden de Compra: " + ex.Message);
                throw;
            }
        }
    }
}
