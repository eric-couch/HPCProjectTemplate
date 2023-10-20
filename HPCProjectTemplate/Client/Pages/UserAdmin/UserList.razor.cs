using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace HPCProjectTemplate.Client.Pages.UserAdmin;

public partial class UserList
{
    [Inject]
    private HttpClient Http { get; set; }
    private List<UserRolesDto> Users { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Users = await Http.GetFromJsonAsync<List<UserRolesDto>>("api/admin");
    }

    private async Task UpdateAdmin(ChangeEventArgs args, string userId)
    {
        await Http.PostAsJsonAsync("api/toggle-admin-role", userId);
    }
}
