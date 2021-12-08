using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepo UserRepo { get; }
        IMessageRepo MessageRepo { get; }
        ILikesRepo LikesRepo { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}