using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractedRabbitMQ
{
    public class SubResult<T>
    {
        public SubResult()
        {
            IsSuccess = true;
        }
        public bool IsSuccess { get;private set; }
        public string? ReasonPhrase { get; private set; }
        public void AddError(string errorMessage)
        {
            IsSuccess = false;
            ReasonPhrase += errorMessage + "\n";
        }
        public T? Value { get; set; }
    }
}
