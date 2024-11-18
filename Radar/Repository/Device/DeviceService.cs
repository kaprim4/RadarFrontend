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
    public class DeviceService : IRepositoryBase<Domain.Models.Device>
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
            return await _process.ProcessListAsync(pagable, RequestType.Put, EndPoint.Update, true, TokenManager.JwtToken);
        }

        public async Task<Domain.Models.Device> GetById(int id)
        {
            var response = await _process.ProcessAsync(id, RequestType.Put, , true, TokenManager.JwtToken);
            if (response != null)
               return response.Object;

            return null;
        }

        public Task<ResponseModel<Domain.Models.Device>> Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
