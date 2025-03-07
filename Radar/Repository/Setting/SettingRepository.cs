using ApiService;
using Domain.DTO;
using Radar.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Repository.Setting
{
    public class SettingRepository : ISettingRepository
    {
        public IProcess<SettingDTO> _process = new("setting");

        public async Task<ResponseModel<SettingDTO>> List()
        {
            return await _process.ProcessAsync(null, RequestType.Get, EndPoint.List, true, TokenManager.JwtToken);
        }

        public async Task<ResponseModel<SettingDTO>> Update(List<SettingDTO> settings)
        {
            return await _process.ProcessAsync(settings, RequestType.Post, EndPoint.Update, true, TokenManager.JwtToken);
        }
    }
}
