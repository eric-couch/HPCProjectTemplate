using HPCProjectTemplate.Client;
using HPCProjectTemplate.Client.HttpRepository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("HPCProjectTemplate.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// local storage accessability (cookies)
builder.Services.AddBlazoredLocalStorage();

// Supply HttpClient instances that include access tokens when making requests to the server project
// scoped, transient, singleton
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HPCProjectTemplate.ServerAPI"));
builder.Services.AddScoped<IUserMoviesHttpRepository, UserMoviesHttpRepository>();
builder.Services.AddApiAuthorization();
builder.Services.AddSyncfusionBlazor();
await builder.Build().RunAsync();
