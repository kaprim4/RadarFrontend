using ApiService;
using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Radar.Helper;
using Radar.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Repository.Data
{
    public class DataRepository : IDataRepository
    {
        public IProcess<Lot> _process = new("data");
        public async Task<ResponseModel<Lot>> Add(Lot obj)
        {
            return await _process.ProcessAsync(obj, RequestType.Post, EndPoint.Add, true, TokenManager.JwtToken);
        }

        public async Task<ResponseModel<Lot>> Edit(Lot obj)
        {
            return await _process.ProcessAsync(obj, RequestType.Put, EndPoint.Update, true, TokenManager.JwtToken);
        }

        public async Task<ResponseModel<Lot>> GetAll(PagableDTO<Lot> pagable)
        {
            return await _process.ProcessListAsync(pagable, RequestType.Post, EndPoint.List, true, TokenManager.JwtToken);
        }

        public async Task<ResponseModel<T>> GetAllGenerique<T>(PagableDTO<T> pagable) where T : class 
        {
            IProcess<T> _processList = new("data");
            return await _processList.ProcessListAsync(pagable, RequestType.Post, EndPoint.List, true, TokenManager.JwtToken);
        }

        public async Task<Lot> GetById(int id)
        {
            return await _process.ProcessGetByIdAsync(id, RequestType.GetById, EndPoint.GetById, true, TokenManager.JwtToken, true);
        }

        public async Task<ResponseModel<Lot>> Remove(dynamic id)
        {
            return await _process.ProcessAsync(id, RequestType.Delete, EndPoint.Delete, true, TokenManager.JwtToken);
        } 
        
        public async Task<ResponseModel<Lot>> RemoveDocument(dynamic id)
        {
            return await _process.ProcessAsync(id, RequestType.Delete, EndPoint.deletedocument, true, TokenManager.JwtToken);
        }

        public async Task<List<FileCheckDTO>> ChechFiles<T>(List<string> files) where T : class
        {
            IProcess<T> _processCheckFiles = new("data");
            return await _processCheckFiles.ProcessAsync<List<FileCheckDTO>>(files, RequestType.Post, EndPoint.checkfiles, true, TokenManager.JwtToken);
        }
        
        public async Task<ResponseModel<Document>> AddDocument(Document dto)
        {
            IProcess<Document> _processAddDocument = new("data");

            var files = dto.Jmx.VehicleDatas?.Select(x=>x.ImageFile).ToList() ?? new List<IFormFile?>();

            dto.Jmx.VehicleDatas?.ForEach(x => { x.ImageFile = null; });

            //return await _processAddDocument.ProcessAsync<ResponseModel<Document>>(new AddDocumentDTO (){ Images = files, Dto = JsonConvert.SerializeObject(dto) }, RequestType.Post, EndPoint.adddocument, true, TokenManager.JwtToken, false, true);
            return await _processAddDocument.ProcessAsync<ResponseModel<Document>>(dto, RequestType.Post, EndPoint.adddocument, true, TokenManager.JwtToken, false, true);
        }

        public async Task<ResponseModel<object>> AddDocument(object dto)
        {
            IProcess<object> _processAddDocument = new("data");
            return await _processAddDocument.ProcessAsync<ResponseModel<object>>(dto, RequestType.Post, EndPoint.adddocument, true, TokenManager.JwtToken, false, true);
        }

    }
    
    
}
