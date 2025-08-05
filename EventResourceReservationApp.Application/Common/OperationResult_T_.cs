using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Common
{
    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }
        protected OperationResult(T data, string message) : base(true, message)
        {
            Data = data;
        }
        protected OperationResult(IEnumerable<string> errors, string errorCode, string message) : base(false, message, errorCode, errors)
        {
            Data = default(T);
        }
        public static OperationResult<T> Success(T data, string message = "Operación exitosa.") => new OperationResult<T>(data, message);
        public static OperationResult<T> Failure(string error, string errorCode, string message = "La operación falló.") => new OperationResult<T>(new[] { error },errorCode, message);
        public static OperationResult<T> Failure(IEnumerable<string> errors, string errorCode, string message = "La operación falló.") => new OperationResult<T>(errors, errorCode, message);
    }
}
