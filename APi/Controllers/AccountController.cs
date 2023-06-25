using APi.Data;
using APi.Dtos;
using APi.Entities;
using APi.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _token;
        private readonly IMapper _mapper;

        public AccountController(DataContext context,ITokenService token,IMapper mapper)
        {
            _context = context;
            _token = token;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RgisterDto  rgisterDto)
        {
        
            if (await UserExists(rgisterDto.Username)) return BadRequest("Username is taken");
            var user = _mapper.Map<AppUser>(rgisterDto);
            using var hmac = new HMACSHA512();
            user.UserName = rgisterDto.Username;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rgisterDto.Password));
                user.PasswordSalt = hmac.Key;

            
            _context.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.UserName,
                Token=_token.CreateToken(user),
                KnownAs= user.KnownAs,
                Gender=user.Gender,
             
                
            };


        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user=await _context.Users.Include(p=>p.Photos)
                .SingleOrDefaultAsync(x=>x.UserName==loginDto.Username);
            if (user == null) return Unauthorized("Invalid Username"); 
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i=0; i<computedHash.Length;i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalida password");
            }
            return new UserDto
            {
                Username=user.UserName,
                Token=_token.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs= user.KnownAs,
                Gender=user.Gender,
            };

        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x=>x.UserName == username.ToLower());
        }
    }
}
