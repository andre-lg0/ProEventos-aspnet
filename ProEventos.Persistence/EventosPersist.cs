using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Helpers;

namespace ProEventos.Persistence
{
    public class EventosPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;

        public EventosPersist(ProEventosContext context)
        {
            this._context = context;
        }
        


        public async  Task<Evento> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrante = false)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking().Include(evento => evento.Lotes )
                .Include(evento => evento.RedesSocials );
            if (includePalestrante)
            {
                query = query.Include(evento => evento.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante); 
            }

            query = query.OrderBy(evento => evento.Id)
                         .Where(evento => evento.Id == eventoId &&
                                          evento.UserId == userId );
            return await query.FirstOrDefaultAsync();
        }

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes)
        {
              IQueryable<Evento> query = _context.Eventos.AsNoTracking().Include(evento => evento.Lotes )
                .Include(evento => evento.RedesSocials );
            if (includePalestrantes)
            {
                query = query.Include(evento => evento.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante); 
            }

            query = query.OrderBy(evento => evento.Id)
                         .Where(evento => evento.Tema.ToLower().Contains(pageParams.Term.ToLower()) ||
                                evento.Local.ToLower().Contains(pageParams.Term.ToLower()) &&
                                evento.UserId == userId);

            return await PageList<Evento>.CreateAsync(query,pageParams.PageNumber,pageParams.PageSize);
        }
    }
}