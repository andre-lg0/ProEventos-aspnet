using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {

        //Eventos
        Task<Evento[]> GetAllEventosByTemaASync(int userId, string tema, bool includePalestrantes);
        Task<Evento> GetEventosByIdASync(int userId, int eventoId, bool includePalestrante);
        Task<Evento[]> GetAllEventosAsync( int userId, bool includePalestrantes);
        

    }
}