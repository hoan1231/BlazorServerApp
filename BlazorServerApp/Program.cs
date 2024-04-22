using Blazored.LocalStorage;
using Blazored.Toast;
using BlazorServerApp;
using BlazorServerApp.Data;
using BlazorServerApp.Hubs;
using BlazorServerApp.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using PackageSDK.Service;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddGrpcSdk();
builder.Services.AddTransient<IPackageBzService, PackageBzService>();
builder.Services.AddTransient<IHisTransactionBzService, HisTransactionBzService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<PackageHub>("/packageHub");
app.MapFallbackToPage("/_Host");

app.Run();
