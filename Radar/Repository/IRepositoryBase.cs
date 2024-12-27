using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Domain.DTO;
using NLog.Filters;

namespace Radar.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<ResponseModel<T>> Add(T obj);
        Task<ResponseModel<T>> Edit(T obj);
        Task<ResponseModel<T>> Remove(dynamic id);
        Task<T> GetById(int id);
        Task<ResponseModel<T>> GetAll(PagableDTO<T> obj);
        Task<ResponseModel<B>> GetAllGenerique<B>(PagableDTO<B> obj) where B : class;
    }
}
