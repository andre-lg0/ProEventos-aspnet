using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Helpers;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {

        //Eventos
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
        Task<Evento> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrante);
        
        

    }
}