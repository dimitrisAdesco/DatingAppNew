using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.Username == username)                                                                                                              // .Where(x => x.Username == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //ConfigurationProvider the config we provided in our automapperprofiles                    // .Select(user => new MemberDto
                .SingleOrDefaultAsync();                                                                                                                        // {
        }                                                                                                                                                        //     Id = user.Id,             //manually mapping the properties
                                                                                                                                                                 //     Username = user.Username
                                                                                                                                                             // }).SingleOrDefaultAsync(); // ---> this is where we exec our query

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users                                          // return await _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)            //.ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //project to gia olous tous users
                .AsNoTracking();                                                //.ToListAsync();
            return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(p => p.Username == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)   //gia n paroume to Photos k na mn einai null
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}