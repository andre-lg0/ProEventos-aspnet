using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly IUserPersist _userPersist;
        private readonly IGeralPersist _geralPersist;

        public AccountService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager,
         IUserPersist userPersist, IGeralPersist geralPersist)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _userPersist = userPersist;
            _geralPersist = geralPersist;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                                            .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());

                return await _signInManager.CheckPasswordSignInAsync(user,password,false);
            }
            catch (System.Exception ex)
            {
                
                throw new Exception($"Erro ao tentar verificar o password. Erro {ex.Message}");
            }
        }

        public  async Task<UserUpdateDto> CreateAccountAsync(UserDto userDto)
        {
            try{
            var user  = _mapper.Map<User>(userDto);
            var create = await _userManager.CreateAsync(user,userDto.Password);

            if(create.Succeeded){
                return _mapper.Map<UserUpdateDto>(user);
            }

            return null;
            }
            catch(Exception ex){
                throw new Exception($"Erro ao tentar criar um usuario. Erro {ex.Message}");
            }

        }

        public async Task<UserUpdateDto> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _userPersist.GetUserByUsernameAsync(username);

                if(user == null) return null;

                return _mapper.Map<UserUpdateDto>(user);
            }
            catch (System.Exception ex)
            {
                
                throw new Exception($"Erro ao tentar buscar um usuario pelo UserName. Erro {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccountAsync(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByUsernameAsync(userUpdateDto.UserName);

                if(user == null) return null;

                userUpdateDto.Id = user.Id;
                _mapper.Map(userUpdateDto,user);

                if(userUpdateDto.Password != null){
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await  _userManager.ResetPasswordAsync(user,token,userUpdateDto.Password);
                }

                _geralPersist.Update<User>(user);

                if(await _geralPersist.SaveChangesAsync()){
                    var userRetorno = await _userPersist.GetUserByUsernameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }
                return null;

            }
            catch (System.Exception ex)
            {
                
                throw new Exception($"Erro ao atualizar o usuario. Erro {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string usarname)
        {
           try
           {
             return await _userManager.Users.AnyAsync(user => user.UserName == usarname.ToLower());
           }
           catch (System.Exception ex)
           {
            
                throw new Exception($"Erro ao tentar verificar se o usuario existe. Erro {ex.Message}");
           }
        }
    }
}