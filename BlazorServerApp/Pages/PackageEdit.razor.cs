using Blazored.LocalStorage;
using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using BlazorModel.Package;
using BlazorServerApp.Services;
using Grpc.Core;
using ManagementPackage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorServerApp.Pages
{
    public partial class PackageEdit
    {
        [Inject] public ILocalStorageService _localStorageService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] private IPackageBzService packageBzService { get; set; }
        [Inject] public IToastService toastService { get; set; }
        private MNG_Package packageRequest;
        [Parameter]
        public string Id { get; set; }
        protected override async Task OnInitializedAsync()
        {
            packageRequest = await packageBzService.GetPackageById(Id);
        }
        async Task UpdatePackage()
        {
            var userLogin = await _localStorageService.GetItemAsync<string>("userName");
            PackageModel obj = new PackageModel { CodePackage = packageRequest.CodePackage, ID = Id,CreatedBy= userLogin, NamePackage = packageRequest.NamePackage, PricePackage = packageRequest.PricePackage };
            var result = await packageBzService.UpdatePackage(obj);
            if(result.StatusCode == Enum.GetName(typeof(StatusCode), StatusCode.OK))
            {
                NavigationManager.NavigateTo("/packagedata");
                toastService.ShowSuccess(result.Message, settings => { settings.IconType = IconType.None; });
            }
            else toastService.ShowWarning(result.Message, settings => { settings.IconType = IconType.None; });

        }
    }
}
