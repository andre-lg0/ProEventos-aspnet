using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Helpers;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> addEventos(int userId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
        
        Task<PageList<EventoDto>> GetAllEventosAsync( int userId,PageParams pageParams, bool includePalestrante = false);
       
        Task<EventoDto> GetEventosByIdAsync(int userId, int id, bool includePalestrante = false);
    }
}