using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    class ErrorResultData<T> : DataResult<T>
    {
        public ErrorResultData(T data) : base(data, true)
        {
        }
        public ErrorResultData(T data, string message) : base(data, true, message)
        {
        }

        public ErrorResultData(string message) : base(default, true, message)
        {
        }
    }
}