using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface ILoteService
    {
        
        Task<bool> DeleteLote(int eventoId, int id);
        Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models);
        
        Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDto> GetLoteByIdAsync(int eventoId, int id);
    }
}