using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> addEventos(EventoDto model);
        Task<bool> DeleteEvento(int eventoId);
        Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);
        
        Task<EventoDto[]> GetAllEventosAsync(bool includePalestrante = false);
        Task<EventoDto[]> GetAllEventosbyTemaAsync(string tema, bool includePalestrante = false);
        Task<EventoDto> GetEventosByIdAsync(int id, bool includePalestrante = false);
    }
}