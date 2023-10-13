using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCProjectTemplate.Shared.Wrappers;

// this will wrap the response from the server (object of generic type)
// with errors if any and success status
public class Response
{
    public Response()
    {
        Errors = new Dictionary<string, string[]>();
    }

    public Response(bool success, string message)
    {
        Errors = new Dictionary<string, string[]>();
        Success = success;
        Message = message;
    }

    public Response(string message)
    {
        Errors = new Dictionary<string, string[]>();
        Success = false;
        Message = message;
    }

    public bool Success { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
}
