using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersist
    {
      
        
        //Palestrantes
        Task<Palestrante[]> GetAllPalestrantesByNameAsync(string name, bool includeEventos);
        Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos);
        Task<Palestrante > GetAllPalestranteByIdAsync(int PalestrnateId, bool includeEventos);

    }
}