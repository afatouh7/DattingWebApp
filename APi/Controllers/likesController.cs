using Api.Data;
using Api.Interfaces;
using APi.Data;
using APi.Dtos;
using APi.Extensions;
using APi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APi.Controllers
{
    [Authorize]
    public class likesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILIkeRepository _likeRepository;

        public likesController(IUserRepository userRepository,ILIkeRepository likeRepository)
        {
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUSerId();
            var likesUser= await _userRepository.GetUserByUserNameAsync(username);
            var sourceUser = await _likeRepository.GetUserWithLike(sourceUserId);
            if(likesUser == null) return NotFound();
            if (sourceUser.UserName == username) return BadRequest("YOU canot like Yourself");
            var userlike = await _likeRepository.GetUserLike(sourceUserId, likesUser.Id);
           if(userlike != null) return BadRequest("You already like this user");
            userlike = new Entities.UserLike
            {
                SorceUserId = sourceUserId,
                LikeUserId = likesUser.Id
            };
            sourceUser.LikedUser.Add(userlike);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to like user"); 

        }

           [HttpGet]
           public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
        {
            var users= await _likeRepository.GetUserLikes(predicate, User.GetUSerId());
            return Ok(users);
        }

    }
}
