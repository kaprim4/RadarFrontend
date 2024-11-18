using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Domain.DTO;

namespace Radar.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<ResponseModel<T>> Add(T obj);
        Task<ResponseModel<T>> Edit(T obj);
        Task<ResponseModel<T>> Remove(int id);
        Task<T> GetById(int id);
        Task<ResponseModel<T>> GetAll(PagableDTO<T> obj);
    }
}
