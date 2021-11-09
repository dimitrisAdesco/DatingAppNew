using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
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
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // var users = await _context.Users.ToListAsync();
            // return users;

            var users = await _userRepository.GetMembersAsync();

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










    }
}