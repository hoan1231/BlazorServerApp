using Blazored.LocalStorage;
using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using BlazorModel.Package;
using BlazorServerApp.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;

namespace BlazorServerApp.Pages
{
    public partial class PackageAdd
    {
        [Inject] public ILocalStorageService _localStorageService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] private IPackageBzService packageBzService { get; set; }
         [Inject] public IToastService toastService { get; set; }
        private PackageModel packageRequest = new PackageModel();
        private string Error12 = "66666";

        async Task AddPackage()
        {
            var userLogin = await _localStorageService.GetItemAsync<string>("userName");
            packageRequest.CreatedBy = userLogin;
            packageRequest.ID = Guid.NewGuid().ToString();
            var result = await packageBzService.AddPackage(packageRequest);
            if (result.StatusCode == Enum.GetName(typeof(StatusCode), StatusCode.OK))
            {
                NavigationManager.NavigateTo("/packagedata");
                toastService.ShowSuccess(result.Message, settings => { settings.IconType = IconType.None; });
            }
            else toastService.ShowWarning(result.Message, settings => { settings.IconType = IconType.None; });

        }
    }
}
