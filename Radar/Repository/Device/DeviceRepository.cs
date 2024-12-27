using ApiService;
using Domain.DTO;
using Domain.Models;
using Radar.Helper;
using Radar.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Repository.Device
{
    public class DeviceRepository : IRepositoryBase<Domain.Models.Device>
    {
        public IProcess<Domain.Models.Device> _process = new("devices");
        public async Task<ResponseModel<Domain.Models.Device>> Add(Domain.Models.Device obj)
        {
            return await _process.ProcessAsync(obj, RequestType.Post, EndPoint.Add, true, TokenManager.JwtToken); 
        }

        public async Task<ResponseModel<Domain.Models.Device>> Edit(Domain.Models.Device obj)
        {
            return await _process.ProcessAsync(obj, RequestType.Put, EndPoint.Update, true, TokenManager.JwtToken);
        }

        public async Task<ResponseModel<Domain.Models.Device>> GetAll(PagableDTO<Domain.Models.Device> pagable)
        {
            return await _process.ProcessListAsync(pagable, RequestType.Post, EndPoint.List, true, TokenManager.JwtToken);
        }

        public Task<ResponseModel<B>> GetAllGenerique<B>(PagableDTO<B> obj) where B : class
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Models.Device> GetById(int id)
        {

            throw new NotImplementedException();
        }

        public async Task<ResponseModel<Domain.Models.Device>> Remove(dynamic id)
        {
            return await _process.ProcessAsync(id, RequestType.Delete, EndPoint.Delete, true, TokenManager.JwtToken);
        }

    }
}
