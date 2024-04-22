using BlazorModel.Package;
using BlazorServerApp.Services;
using Grpc.Core;
using ManagementPackage;
using Microsoft.AspNetCore.Components;

namespace BlazorServerApp.Pages
{
    public partial class PackageHistory
    {
        private HisTransactions myObject;
        [Inject] private IHisTransactionBzService iHisTransactionBzService { get; set; }
        public HisRequestModel hisRequest = new HisRequestModel { FromDate=DateTime.Now,ToDate= DateTime.Now, NameCus="",NamPackage=""};

        protected override async Task OnInitializedAsync()
        {
            myObject = await iHisTransactionBzService.GetHisTransactionPackage(hisRequest);
        }
        private async Task SearchPackage()
        {
            myObject = await iHisTransactionBzService.GetHisTransactionPackage(hisRequest);
        }
    }
}
