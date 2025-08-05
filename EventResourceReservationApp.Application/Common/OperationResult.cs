using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Common
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string ErrorCode { get; set; }

        protected OperationResult(bool isSuccess, string message, string errorCodde = null, IEnumerable<string> errors = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            ErrorCode = errorCodde;
            Errors  = errors ?? Enumerable.Empty<string>();
        }
        public static OperationResult Success(string message = "Operación exitosa.") => new OperationResult(true, message);
        public static OperationResult Failure(string error, string errorCode, string message = "La operación falló.") => new OperationResult(false, message, errorCode, new[] {error});
        public static OperationResult Failure(IEnumerable<string> errors, string errorCode, string message = "La operación falló.") => new OperationResult(false, message, errorCode, errors);
    }
}
