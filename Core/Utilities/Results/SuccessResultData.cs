using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
   public class SuccessResultData<T>:DataResult<T>
    {
        public SuccessResultData(T data) : base(data,true)
        {
        }
        public SuccessResultData(T data, string message) : base(data,true, message)
        {
        }

        public SuccessResultData(string message) : base(default, true, message)
        {

        }
    }
}
