using APi.Data;
using APi.Dtos;
using APi.Entities;
using APi.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        
        private readonly ITokenService _token;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService token,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            var result = await _userManager.CreateAsync(user, rgisterDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token= await _token.CreateToken(user),
                KnownAs= user.KnownAs,
                Gender=user.Gender,
                
             
                
            };


        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user=await _userManager.Users.Include(p=>p.Photos)
                .SingleOrDefaultAsync(x=>x.UserName==loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid Username");
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(); 
            
            return new UserDto
            {
                Username=user.UserName,
                Token= await _token.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs= user.KnownAs,
                Gender=user.Gender,
            };

        }
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x=>x.UserName == username.ToLower());
        }
    }
}
