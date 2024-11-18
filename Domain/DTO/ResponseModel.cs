using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ResponseModel<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; }
        public PagableDTO<T> Pagable { get; set; }
        public T Object { get; set; }

        public ResponseModel(bool _isSuccess = true)
        {
            IsSuccess = _isSuccess;
            ErrorMessage = string.Empty;
        }

        public void AddErrorMessage(string message)
        {
            ErrorMessage = message;
            if (!string.IsNullOrEmpty(message))
                IsSuccess = false;
        }


        public void AddValidationErrors(List<string> messages)
        {
            ValidationErrors ??= new List<string>();
            ValidationErrors.AddRange(messages);
            if (messages.Any())
                IsSuccess = false;
        }

        public List<string> GetValidationErrors()
        {
            ValidationErrors ??= new List<string>();

            return ValidationErrors;
        }

        public string GetErrorMessage()
        {
            return ErrorMessage;
        }

        
    }

    public class ResponseAuthModel<T> : ResponseModel<T> where T : class
    {
        public string Token { get; set; }
        public bool IsAuthentificated { get; set; }
        public ResponseAuthModel(bool isSuccess) : base(isSuccess)
        {
        }
    }
}
