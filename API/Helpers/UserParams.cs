using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50; //the most amount of things we gonna return from our request
        public int PageNumber { get; set; } = 1;  //we gonna take this form user
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; //value that the user/client has supplied
        }
    }
}