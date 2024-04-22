using BlazorModel.Package;
using BlazorServerApp.Services;
using Grpc.Core;
using ManagementPackage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace BlazorServerApp.Hubs
{
    public class PackageHub : Hub
    {
     
        public async Task SendMessage1(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        /// <summary>
        /// Tìm kiếm package
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task SearchPackage(string code, string name)
        {
            IPackageBzService _packageBzService = new PackageBzService();
            var result = await _packageBzService.SearchPackage(code, name);
            await Clients.All.SendAsync("ReceivePackageResult", result);
        }
        /// <summary>
        /// Lấy gói cước theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task GetPackageById(string id)
        {
            IPackageBzService _packageBzService = new PackageBzService();
            var result = await _packageBzService.GetPackageById(id);
            await Clients.All.SendAsync("GetPackageByIdResult", result);
        }
        /// <summary>
        /// Cập nhật gói cước
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task UpdatePackage(PackageModel obj)
        {
            IPackageBzService _packageBzService = new PackageBzService();
            var result = await _packageBzService.UpdatePackage(obj);
            await Clients.All.SendAsync("UpdatePackageResult", result);
        }
        public async Task AddPackage(PackageModel obj)
        {
            IPackageBzService _packageBzService = new PackageBzService();
            var result = await _packageBzService.AddPackage(obj);
            await Clients.All.SendAsync("AddPackageResult", result);
        }
    }
}
