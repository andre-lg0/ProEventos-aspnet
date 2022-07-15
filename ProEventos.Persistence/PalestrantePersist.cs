using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProEventosContext _context;

        public PalestrantePersist(ProEventosContext context)
        {
            this._context = context;
        }
        
       
        public async  Task<Palestrante[]> GetAllPalestrantesByNameAsync(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                    .Include(palestrante => palestrante.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(palestrante => palestrante.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(palestrante => palestrante.Id)
                .Where(palestrante => palestrante.User.PrimeiroNome == name);
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(palestrante => palestrante.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(palestrante => palestrante.Id);
            return await query.ToArrayAsync();
        }

        public async  Task<Palestrante> GetAllPalestranteByIdAsync(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(palestrante => palestrante.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking().OrderBy(palestrante => palestrante.Id)
                .Where(pe => pe.Id == palestranteId);
            return await query.FirstOrDefaultAsync();
        }
    }
}