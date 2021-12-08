using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepo
    {
        void Update(AppUser user);
        //Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string userename);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams); //Task<IEnumerable<MemberDto>> GetMembersAsync();

        Task<MemberDto> GetMemberAsync(string username);
        Task<string> GetUserGender(string username);
    }
}