using HPCProjectTemplate.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace HPCProjectTemplate.Client.Pages.UserAdmin;

public partial class EditUser
{
    [Inject]
    HttpClient Http { get; set; }
    [Parameter]
    public string userId { get; set; }
    UserEditDto UserEditDto { get; set; } = new UserEditDto();


    protected override async Task OnInitializedAsync()
    {
        UserEditDto = await Http.GetFromJsonAsync<UserEditDto>($"api/admin/{userId}");
    }

    private async Task UpdateUser()
    {
        await Http.PutAsJsonAsync("api/admin", UserEditDto);
    }
}
