using Blazored.LocalStorage;
using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using BlazorModel.Package;
using BlazorServerApp.Services;
using Grpc.Core;
using ManagementPackage;
using Microsoft.AspNetCore.Components;

namespace BlazorServerApp.Pages
{
    public partial class PackageRegister
    {
        [Inject] public ILocalStorageService _localStorageService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] private IHisTransactionBzService ihisTransactionBzService { get; set; }
        [Inject] public IToastService toastService { get; set; }
        private HisTransactionRequestModel hisRequest = new HisTransactionRequestModel { PackageId=""};
        private HisTransactions myObject;
        public MNGPackages lstPackage;
        private string ID;
        public string userLogin = "";
        [Inject] private IPackageBzService iPackageBzService { get; set; }
        async Task RegisterPackage()
        {
            hisRequest.CreatedBy = await _localStorageService.GetItemAsync<string>("userName");
            hisRequest.CusId = await _localStorageService.GetItemAsync<string>("userId");
            var result = await ihisTransactionBzService.AddTransactionPackage(hisRequest);
            if (result.StatusCode == Enum.GetName(typeof(StatusCode), StatusCode.OK))
            {
                toastService.ShowSuccess(result.Message, settings => { settings.IconType = IconType.None; });
                myObject = await ihisTransactionBzService.GetTransactionPackage(new HisTransactionModel { NameCus = userLogin, FromDate = "", ToDate = "", NamPackage = "" });
            }
            else toastService.ShowWarning(result.Message, settings => { settings.IconType = IconType.None; });
            // toastService.ShowInfo("Cập nhật thành công", "Thông báo");
        }
        protected override async Task OnInitializedAsync()
        {
            userLogin = await _localStorageService.GetItemAsync<string>("userName");
            lstPackage = await iPackageBzService.SearchPackage("", "");
            myObject = await ihisTransactionBzService.GetTransactionPackage(new HisTransactionModel { NameCus = userLogin, FromDate = "", ToDate = "", NamPackage = "" });
        }
        protected BlazorServerApp.Component.Confirmation CancelConfirmation { get; set; }

        public void Cancel_Click(string Id)
        {
            ID = Id;
           CancelConfirmation.Show();
        }

        protected async Task ConfirmCancle_Click(bool cancelConfirmed)
        {
            if (cancelConfirmed)
            {
                var result = await ihisTransactionBzService.DeleteTransactionPackage(new HisTransactionRequestModel { CusId = ID, PackageId = "", CreatedBy = userLogin });
                if (result.StatusCode == Enum.GetName(typeof(StatusCode), StatusCode.OK))
                {
                    toastService.ShowSuccess(result.Message, settings => { settings.IconType = IconType.None; });
                    myObject = await ihisTransactionBzService.GetTransactionPackage(new HisTransactionModel { NameCus = userLogin, FromDate = "", ToDate = "", NamPackage = "" });
                }
                else toastService.ShowWarning(result.Message, settings => { settings.IconType = IconType.None; });
            }
        }
    }
}
