using Api.Dtos;
using Api.Interfaces;
using APi.Dtos;
using APi.Extensions;
using APi.Helpers;
using APi.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APi.Controllers
{
    // [Authorize]

    public class USersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public USersController( IUserRepository userRepository,IMapper mapper,IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }
       
        [HttpGet] 
        public async Task<ActionResult<IEnumerable<MemeberDto>>> GetUSers([FromQuery]UserParams userParams)
        {
          var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
          //  var user = await _userRepository.GetUserByUserNameAsync("Ahmed fatouh");
            userParams.CurrentUsername= user.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female" : "male";
            var users = await _userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TOtalCount, users.TotalPages);
       
            return Ok(users);
        }
        //[Authorize]
        
        [HttpGet("{username}",Name = "GetuserByUserName")] 
        public async Task<ActionResult<MemeberDto>> GetuserByUserName(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
            _mapper.Map(memberUpdateDto, user);
            _userRepository.Update(user);
            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Falied to update user"); 


        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file,string username)
        {

            
            var user = await _userRepository.GetUserByUserNameAsync(username);
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUri.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetuserByUserName", new {username=user.UserName},_mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("Prolem Adding Photo");
        }
        [HttpPut("set-main-photo/{PhotoId}")]
        public async Task<ActionResult> SetMainPhoto(int PhotoId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == PhotoId);
            if (photo.IsMain) return BadRequest("this is already your main photo");

            var currentmain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentmain != null) currentmain.IsMain = false;
           photo.IsMain=true;
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("failed to set main photo");
        }
        [HttpDelete("delet-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if(photo ==null) return NotFound();
            if (photo.IsMain) return BadRequest("you can delete the main photo");
            if(photo.PublicId != null)
            {
               var result= await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error !=null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if(await _userRepository.SaveAllAsync())  return Ok();
            return BadRequest("falied to deplete photo");


        }
    }
}
