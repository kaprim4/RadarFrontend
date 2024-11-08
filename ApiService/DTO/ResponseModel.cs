using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.DTO
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; }

        public ResponseModel(bool _isSuccess = true)
        {
            IsSuccess = _isSuccess;
            ErrorMessage = string.Empty;
            ValidationErrors = new List<string>();
        }

        public void AddErrorMessage(string message)
        {
            ErrorMessage = message;
            if (!string.IsNullOrEmpty(message))
                IsSuccess = false;
        }


        public void AddValidationErrors(List<string> messages)
        {
            ValidationErrors.AddRange(messages);
            if (messages.Any())
                IsSuccess = false;
        }

        public List<string> GetValidationErrors(){ return ValidationErrors;}

        public string GetErrorMessage()
        {
            return ErrorMessage;
        }
    }

    public class ResponseAuthModel : ResponseModel
    {
        public string Token { get; set; }
        public bool IsAuthentificated { get; set; }
        public ResponseAuthModel(bool isSuccess) : base(isSuccess)
        {
        }
    }
}
