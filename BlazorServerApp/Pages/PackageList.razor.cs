using Blazored.LocalStorage;
using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using BlazorModel.Package;
using BlazorServerApp.Services;
using Grpc.Core;
using ManagementPackage;
using Microsoft.AspNetCore.Components;
//using PackageSDK.Service;

namespace BlazorServerApp.Pages
{
    public partial class PackageList
    {
        private MNGPackages myObject;
        [Inject] public ILocalStorageService _localStorageService { get; set; }
        [Inject] public IToastService toastService { get; set; }
        [Inject] private IPackageBzService iPackageBzService { get; set; }
     //   [Inject] private IPackageGrpcService iPackageGrpcService { get; set; }
        
        public PackageModel packageRequest = new PackageModel { CodePackage="",ID="", NamePackage="", PricePackage=""};
        private string ID;
        public string userLogin = "";

        protected override async Task OnInitializedAsync()
        {
            // var aa = await iPackageGrpcService.GetAllAsync(packageRequest);
         
            myObject = await iPackageBzService.SearchPackage(packageRequest.CodePackage, packageRequest.NamePackage);
        }
        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                userLogin = await _localStorageService.GetItemAsync<string>("userName");
            }
            base.OnAfterRender(firstRender);
        }
        private async Task SearchPackage()
        {
            myObject = await iPackageBzService.SearchPackage(packageRequest.CodePackage, packageRequest.NamePackage);
        }
        public BlazorServerApp.Component.Confirmation DeleteConfirmation { get; set; }

        public void Delete_Click(string Id)
        {
            ID = Id;
            DeleteConfirmation.Show();
        }

        public async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                var result = await iPackageBzService.DeletePackage(ID,userLogin);
                if (result.StatusCode == Enum.GetName(typeof(StatusCode), StatusCode.OK))
                {
                    toastService.ShowSuccess("Hủy gói cước thành công", settings => { settings.IconType = IconType.None; });
                    await SearchPackage();
                }
                else
                {
                    toastService.ShowWarning(result.Message, settings => {settings.IconType = IconType.None; });
                }
            }
        }
    }
}
