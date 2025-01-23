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
    public class ProvinciaRepository : IProvinciaRepository<Provincia>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public ProvinciaRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
       
        public async Task<IQueryable<Provincia>> ObtenerTodos()
        {
            IQueryable<Provincia> query = _dbcontext.Provincias;
            return query;
        }

  


    }
}
