using Domain.DTO;
using Radar.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Repository.Data
{
    public interface IDataRepository : IRepositoryBase<Lot>
    {
        Task<List<FileCheckDTO>> ChechFiles<T>(List<string> files) where T : class;
        Task<ResponseModel<Document>> AddDocument(Document dto);
        Task<ResponseModel<object>> AddDocument(object dto);
        Task<ResponseModel<Lot>> RemoveDocument(dynamic id);
    }
}
