
using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using ApiService;
using Radar.Helper;
using Radar.Repositories;

namespace Radar.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IProcess<User> _process = new("users");
        
        public void Add(User userModel)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseAuthModel<object>> AuthenticateUser(NetworkCredential credential)
        {
            var Process = _process.SetController("auth");
            var response = await Process.AuthAsync(new LoginDTO
            {
                UserName = credential.UserName,
                Password = credential.Password
            });

            
            return response;
        }


        public async Task<ResponseModel<User>> GetAll(PagableDTO<User> pagable)
        {
            var response = await _processList.ProcessAsync(pagable, RequestType.Post, Domain.DTO.EndPoint.List, true, TokenManager.JwtToken);
            if (response.IsSuccess)
                return response;
            
            return null; 
        }

        public async Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }
        

        public async Task<ResponseModel<User>> Remove(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResponseModel<User>> IRepositoryBase<User>.Add(User userModel)
        {
            throw new NotImplementedException();
        }

        Task<ResponseModel<User>> IRepositoryBase<User>.Edit(User userModel)
        {
            throw new NotImplementedException();
        }

        User IUserRepository.GetByUsername(string username)
        {
            return new User()
            {
                UserName = username,
                FullName = "ayyyyyyyyyyyyy"
            };
        }
    }
}
