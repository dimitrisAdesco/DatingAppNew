using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;
using API.Extentions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepo _userRepo;
        private readonly ILikesRepo _likesRepo;
        public LikesController(IUserRepo userRepo, ILikesRepo likesRepo)
        {
            _likesRepo = likesRepo;
            _userRepo = userRepo;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepo.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepo.GetUserWithLikes(sourceUserId);

            if (likedUser == null)
            {
                return NotFound();
            }

            if (sourceUser.Username == username)
            {
                return BadRequest("cant like urself");
            }

            var userLike = await _likesRepo.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null)
            {
                return BadRequest("u already liked this user");
            }

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike); //add the user like

            if (await _userRepo.SaveAllAsync())
                return Ok();

            return BadRequest("failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
        {
            var users = await _likesRepo.GetUserLikes(predicate, User.GetUserId());

            return Ok(users);
        }

    }
}