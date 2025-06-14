﻿using Microsoft.EntityFrameworkCore;
using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using SistemaPanera.DAL.DataContext;

namespace SistemaPanera.DAL.Repository
{
    public class OrdenesComprasInsumosEstadoRepository : IOrdenesComprasInsumoEstadoRepository<OrdenesComprasInsumosEstado>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public OrdenesComprasInsumosEstadoRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(OrdenesComprasInsumosEstado model)
        {
            _dbcontext.OrdenesComprasInsumosEstados.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            OrdenesComprasInsumosEstado model = _dbcontext.OrdenesComprasInsumosEstados.First(c => c.Id == id);
            _dbcontext.OrdenesComprasInsumosEstados.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(OrdenesComprasInsumosEstado model)
        {
            _dbcontext.OrdenesComprasInsumosEstados.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<OrdenesComprasInsumosEstado> Obtener(int id)
        {
            OrdenesComprasInsumosEstado model = await _dbcontext.OrdenesComprasInsumosEstados.FindAsync(id);
            return model;
        }
        public async Task<IQueryable<OrdenesComprasInsumosEstado>> ObtenerTodos()
        {
            IQueryable<OrdenesComprasInsumosEstado> query = _dbcontext.OrdenesComprasInsumosEstados;
            return await Task.FromResult(query);
        }




    }
}
