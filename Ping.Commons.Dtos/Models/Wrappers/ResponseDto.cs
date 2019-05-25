using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Wrappers.Response
{
    public class ResponseDto<T>
    {
        public T Dto { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
    }
}
