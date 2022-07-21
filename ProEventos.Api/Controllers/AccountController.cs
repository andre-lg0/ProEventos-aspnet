using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Api.Extensions;
using ProEventos.Application.Contratos;

namespace ProEventos.Api.Controllers
{   [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService,
                                 ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }
        

        [HttpGet("GetUser/")]
        public async Task<IActionResult> GetUser(){
                try
                {
                    var userName  = User.GetUserName();
                    var user = await _accountService.GetUserByUsernameAsync(userName);
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    
                    return this.StatusCode(StatusCodes.Status500InternalServerError,
                        $"Erro ao tentar recuperar o Usuário. Erro:{ex.Message}");
                }
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto userDto){
                try
                {
                    if(await _accountService.UserExists(userDto.UserName)){
                        return BadRequest("Usuário ja existe, tente outro");
                    }

                    var user = await _accountService.CreateAccountAsync(userDto);
                    if(user != null)
                        return Ok(new {
                            userName = user.UserName,
                            PrimeiroNome = user.PrimeiroNome,
                            token = _tokenService.CreateToken(user).Result
                        });
                    
                    return BadRequest("Conta nao foi criada, tente mais tarde");

                }
                catch (Exception ex)
                {
                    
                    return this.StatusCode(StatusCodes.Status500InternalServerError,
                        $"Erro ao criar a conta do Usuário. Erro:{ex.Message}");
                }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto userDto){
                try
                {
                    var user = await _accountService.GetUserByUsernameAsync(userDto.Username);
                    if(user is null) return Unauthorized("Usuario ou senha estão incorretas");


                    var login = await _accountService.CheckUserPasswordAsync(user,userDto.Password);
                    if(!login.Succeeded) return Unauthorized();

                    return Ok(new {
                        userName = user.UserName,
                        PrimeiroNome = user.PrimeiroNome,
                        token = _tokenService.CreateToken(user).Result
                    });


                }
                catch (Exception ex)
                {
                    
                    return this.StatusCode(StatusCodes.Status500InternalServerError,
                        $"Erro ao logar no Usuario. Erro:{ex.Message}");
                }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userDto){
                try
                {
                    var user = await _accountService.GetUserByUsernameAsync(User.GetUserName());
                    if(user is null) return Unauthorized("Usuario Invalido");


                    var updateUser = await  _accountService.UpdateAccountAsync(userDto);
                    if(updateUser is null ) return NoContent();

                    return Ok(updateUser);
                    


                }
                catch (Exception ex)
                {
                    
                    return this.StatusCode(StatusCodes.Status500InternalServerError,
                        $"Erro ao atualizar  a conta do Usuário. Erro:{ex.Message}");
                }
        }

    }


}