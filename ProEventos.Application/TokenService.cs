using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ProEventos.Application.Dtos;
using ProEventos.Application.Contratos;
using ProEventos.Domain.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ProEventos.Application
{

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config, IMapper mapper, UserManager<User> userManager)
        {
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public async Task<string> CreateToken(UserUpdateDto userUpdateDto)
        {
            var user = _mapper.Map<User>(userUpdateDto);

            var claims = new  List<Claim>{
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role,role)));

            var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescrip = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescrip);

            return tokenHandler.WriteToken(token);

        }
    }


}