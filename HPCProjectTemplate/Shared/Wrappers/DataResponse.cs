using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCProjectTemplate.Shared.Wrappers;

public class DataResponse<T> : Response
{
    public T Data { get; set; }

    public DataResponse()
    {
        
    }

    public DataResponse(T data)
    {
        Success = true;
        Data = data;
    }
}
