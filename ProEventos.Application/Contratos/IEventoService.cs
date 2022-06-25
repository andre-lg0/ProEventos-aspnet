using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<Evento> addEventos(Evento model);
        Task<bool> DeleteEvento(int eventoId);
        Task<Evento> UpdateEvento(int eventoId, Evento model);
        
        Task<Evento[]> GetAllEventosAsync(bool includePalestrante = false);
        Task<Evento[]> GetAllEventosbyTemaAsync(string tema, bool includePalestrante = false);
        Task<Evento> GetEventosByIdAsync(int id, bool includePalestrante = false);
    }
}