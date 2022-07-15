using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> addEventos(int userId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
        
        Task<EventoDto[]> GetAllEventosAsync( int userId, bool includePalestrante = false);
        Task<EventoDto[]> GetAllEventosbyTemaAsync(int userId, string tema, bool includePalestrante = false);
        Task<EventoDto> GetEventosByIdAsync(int userId, int id, bool includePalestrante = false);
    }
}