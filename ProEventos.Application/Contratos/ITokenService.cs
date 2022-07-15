using System.Threading.Tasks;
using ProEventos.Application.Dtos;
namespace ProEventos.Application.Contratos
{
    public interface ITokenService
    {
        public Task<string> CreateToken(UserUpdateDto userUpdateDto);
    }
}