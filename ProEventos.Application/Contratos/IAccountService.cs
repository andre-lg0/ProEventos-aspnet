using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{

    public interface IAccountService
    {
        Task<bool> UserExists(string usarname);

        Task<UserUpdateDto> CreateAccountAsync(UserDto userDto);
        Task<SignInResult>CheckUserPasswordAsync(UserUpdateDto userUpdate, string password);
        Task<UserUpdateDto> GetUserByUsernameAsync(string username);
        Task<UserUpdateDto> UpdateAccountAsync(UserUpdateDto userUpdateDto);

        



    }   

}