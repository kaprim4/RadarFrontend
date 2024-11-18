using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{

    public enum RequestType
    {
        Get,
        Post,
        Patch,
        Put,
        Delete,
    }

    public enum EndPoint
    {
        List,
        Add,
        Update,
        Delete,
        Login,
        register
    }
}
