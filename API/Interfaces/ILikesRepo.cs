using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;

namespace API.Interfaces
{
    public interface ILikesRepo
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);

        //return a type of <LikeDto>
        Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
    }
}