using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]     //auth user, we need to add a middleware for this
    public class UsersController : BaseApiController
    {
        private readonly IUserRepo _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepo userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams) //userParams -> query string parameters
        {
            // var users = await _context.Users.ToListAsync();
            // return users;

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            //var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users); //what we mapping to and then parse it the source obj

            return Ok(users);

        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            // var user = await _context.Users.FindAsync(id);
            // return user;

            var user = await _userRepository.GetMemberAsync(username);  // _userRepository.GetUserByUsernameAsync(username)

            return user;    //_mapper.Map<MemberDto>(user);

        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("failed to update user");


        }

    }
}