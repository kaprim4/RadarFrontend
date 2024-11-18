using Domain.DTO;
using Domain.Models;
using Radar.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Repository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<ResponseAuthModel<object>> AuthenticateUser(NetworkCredential credential);
        User GetByUsername(string username);
    }
}
