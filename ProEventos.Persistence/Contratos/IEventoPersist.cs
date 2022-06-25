using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {

        //Eventos
        Task<Evento[]> GetAllEventosByTemaASync(string tema, bool includePalestrantes);
        Task<Evento> GetEventosByIdASync(int eventoId, bool includePalestrante);
        Task<Evento[]> GetAllEventosAsync( bool includePalestrantes);
        

    }
}