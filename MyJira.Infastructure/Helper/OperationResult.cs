using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Infastructure.Helper
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static OperationResult<T> Ok(T data)
            => new OperationResult<T> { Success = true, Data = data };

        public static OperationResult<T> Fail(string message)
            => new OperationResult<T> { Success = false, ErrorMessage = message };
    }
}
