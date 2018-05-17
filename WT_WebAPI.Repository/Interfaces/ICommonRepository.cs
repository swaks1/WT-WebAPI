using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WT_WebAPI.Entities;
using WT_WebAPI.Entities.DTO;

namespace WT_WebAPI.Repository.Interfaces
{
    public interface ICommonRepository
    {
        Task<WTUser> GetUserByUsername(string username);

        Task<WTUser> GetUserByUsernameFullInfo(string username);

        Task<bool> UpdateUser(WTUserDTO user);

        Task<bool> DeleteUser(string username);

        Task<bool> UserExists(string username);


    }
}
