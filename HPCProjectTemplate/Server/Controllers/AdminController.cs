using HPCProjectTemplate.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using HPCProjectTemplate.Server.Models;
using HPCProjectTemplate.Shared;

namespace HPCProjectTemplate.Server.Controllers;

public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public AdminController( UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    [HttpGet("api/admin")]
    public async Task<List<UserRolesDto>> Get()
    {
        var users = _userManager.Users.Select(u => new UserRolesDto()
        {
            Id = u.Id,
            UserName = u.UserName,
            Roles = _userManager.GetRolesAsync(u).Result.ToArray()
        }).ToList();

        return users;
    }

    [HttpGet("api/admin/{id}")]
    public async Task<ActionResult<UserEditDto>> Get(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var userEditDto = new UserEditDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            IsAdmin = userRoles.Contains("Admin"),
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        return Ok(userEditDto);
    }

    [HttpPost("api/toggle-admin-role")]
    public async Task<IActionResult> ToggleAdminRole([FromBody] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (isAdmin)
        {
            await _userManager.RemoveFromRoleAsync(user, "Admin");
        } else
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        return Ok();
    }

    [HttpPut("api/admin")]
    public async Task<IActionResult> Put([FromBody] UserEditDto userEdit)
    {
        var user = await _userManager.FindByIdAsync(userEdit.Id);
        if (user == null)
        {
            return NotFound();
        }

        user.UserName = userEdit.UserName;
        user.Email = userEdit.Email;
        user.EmailConfirmed = userEdit.EmailConfirmed;
        user.FirstName = userEdit.FirstName;
        user.LastName = userEdit.LastName;

        var userRoles = await _userManager.GetRolesAsync(user);
        var isAdmin = userRoles.Contains("Admin");
        if (isAdmin && !userEdit.IsAdmin)
        {
            await _userManager.RemoveFromRoleAsync(user, "Admin");
        } else if (!isAdmin && userEdit.IsAdmin)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        await _userManager.UpdateAsync(user);

        return Ok();

    }
}
