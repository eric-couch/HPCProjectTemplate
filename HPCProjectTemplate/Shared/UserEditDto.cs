using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCProjectTemplate.Shared;

public class UserEditDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsAdmin { get; set; }
    [MinLength(3, ErrorMessage = "First Name must be at least 3 characters long.")]
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
