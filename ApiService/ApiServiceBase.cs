
using Domain.DTO;

namespace ApiService
{
    public class ApiServiceBase
    {
        public static event Action UnauthorizedAccessDetected;

        protected static void OnUnauthorizedAccessDetected()
        {
            UnauthorizedAccessDetected?.Invoke();
        }

        /// <summary>
        /// Creates an instance of IProcess for the specified type and controller.
        /// </summary>
        public IProcess<T> CreateProcess<T>(string controller)
            where T : class
        {
            var process = new IProcess<T>();
            process.SetController(controller);
            return process;
        }

        /// <summary>
        /// Retrieves paged data by making a GET request to the specified controller.
        /// </summary>
        public async Task<ResponseModel<T>> GetPagedDataAsync<T>(T request, string controller)
            where T : class
        {
            var process = CreateProcess<T>(controller);
            return await process.ProcessAsync(request, RequestType.Get, EndPoint.List);
        }

        /// <summary>
        /// Retrieves paged data by making a GET request to the specified controller (with additional endpoint flexibility).
        /// </summary>
        public async Task<ResponseModel<T>> GetPagedDataAsync<T>(T request, string controller, EndPoint customEndPoint)
            where T : class
        {
            var process = CreateProcess<T>(controller);
            return await process.ProcessAsync(request, RequestType.Get, customEndPoint);
        }
    }
}

