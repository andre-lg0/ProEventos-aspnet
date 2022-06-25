using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class EventosPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;

        public EventosPersist(ProEventosContext context)
        {
            this._context = context;
        }
        

        public async Task<Evento[]> GetAllEventosByTemaASync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking().Include(evento => evento.Lotes )
                .Include(evento => evento.RedesSocials );
            if (includePalestrantes)
            {
                query = query.Include(evento => evento.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante); 
            }

            query = query.OrderBy(evento => evento.Id).Where(evento => evento.Tema.ToLower() == tema.ToLower());
            return await query.ToArrayAsync();
        }

        public async  Task<Evento> GetEventosByIdASync(int eventoId, bool includePalestrante = false)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking().Include(evento => evento.Lotes )
                .Include(evento => evento.RedesSocials );
            if (includePalestrante)
            {
                query = query.Include(evento => evento.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante); 
            }

            query = query.OrderBy(evento => evento.Id).Where(evento => evento.Id == eventoId);
            return await query.FirstOrDefaultAsync();
        }

        public async  Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking().Include(evento => evento.Lotes )
                .Include(evento => evento.RedesSocials );
            if (includePalestrantes)
            {
                query = query.Include(evento => evento.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante); 
            }
            query = query.OrderBy(evento => evento.Id);
            return await query.ToArrayAsync();
        }

       
      
    }
}