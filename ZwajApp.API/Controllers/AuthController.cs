
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ZwajApp.API.Data;
using ZwajApp.API.Dtos;
using ZwajApp.API.Models;

namespace ZwajApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo ,IConfiguration config )
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
             userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
             if(await _repo.UserExists(userForRegisterDto.Username))
             return BadRequest("هذا المستخدم مسجل من قبل");
             var UserToCreate = new User{
                          Username = userForRegisterDto.Username
             };

             var CreatedUser = await _repo.Register(UserToCreate,userForRegisterDto.Password);
             return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
           throw new Exception("sdfghjkl");
               var UserFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(),userForLoginDto.Password);
               if(UserFromRepo==null)return Unauthorized();
               var claims = new[] {
                   new Claim(ClaimTypes.NameIdentifier,UserFromRepo.Id.ToString()),
                   new Claim(ClaimTypes.Name,UserFromRepo.Username)
               };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512);
                var tokenDescripror = new SecurityTokenDescriptor{
                    Subject = new ClaimsIdentity(claims),
                    Expires=DateTime .Now.AddDays(1),
                    SigningCredentials=creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescripror);
                return Ok(new{
                    token = tokenHandler.WriteToken(token)
                });
            
                

        }

    }
}