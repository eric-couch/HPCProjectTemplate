using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCProjectTemplate.Shared;

public class UserRolesDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string[] Roles { get; set; }

}
