using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)   //Task<ActionResult<AppUser>> ---> Task<ActionResult<UserDto>>
        {
            if (await UserExists(registerDto.Username))
            {
                return BadRequest("Username is taken");   //BadRequest mporoume na to xrisimopoiisoume apo ActionResult
            }

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();   //as soon as we are finished with HMACSHA512 its gonna dispose it (me to using)

            user.Username = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users.Add(user);   //we are not actually adding to the Database, we are tracking
            await _context.SaveChangesAsync();   //now we save it

            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs

            };
        }

        //ENDPOINT for Login

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //1st we need the user from the Database
            var user = await _context.Users.SingleOrDefaultAsync(p => p.Username == loginDto.Username);

            if (user == null)
            {
                return Unauthorized("Invalid User");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };

        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(p => p.Username == username.ToLower());
        }

    }
}
