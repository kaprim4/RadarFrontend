using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Repository.Setting
{
    public interface ISettingRepository
    {
        Task<ResponseModel<SettingDTO>> List();
        Task<ResponseModel<SettingDTO>> Update(List<SettingDTO> settings);
    }
}
