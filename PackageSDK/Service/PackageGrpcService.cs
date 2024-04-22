using BlazorModel.Package;
using Grpc.Core;
using Grpc.Net.Client;
using ManagementPackage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PackageSDK.Service
{
    public interface IPackageGrpcService
    {
        Task<MNGPackages> GetAllAsync(PackageModel request);
        /// <summary>
        /// Tìm kiếm gói cước
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<MNGPackages> SearchPackage(string code, string name);
        /// <summary>
        /// Lấy gói cước theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MNG_Package> GetPackageById(string id);
        /// <summary>
        /// Xóa gói
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MNGPackagesResponse> DeletePackage(string id);
        /// <summary>
        /// Cập nhật gói cước
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<MNGPackagesResponse> UpdatePackage(PackageModel obj);

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<MNGPackagesResponse> AddPackage(PackageModel obj);
        Task<MNG_InfoCustomerResonse> GetInfoCustomer(string userName, string passWord);
    }
    public class PackageGrpcService : IPackageGrpcService
    {
        private readonly PackageProto.PackageProtoClient grpcClient;

        private readonly ILogger<PackageGrpcService> _logger;
        public PackageGrpcService(ILogger<PackageGrpcService> logger, PackageProto.PackageProtoClient grpcClient)
        {
            this.grpcClient = grpcClient;
            _logger = logger;
        }
        public PackageGrpcService()
        {
        }
        public async Task<MNGPackages> GetAllAsync(PackageModel request)
        {
            try
            {
                var obj = new MngPacketRequest { Name = request.NamePackage, Code = request.CodePackage };
               var result = await grpcClient.GetAllAsync(obj, cancellationToken: new CancellationToken());
                return result;
            }
            catch (RpcException ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Tìm kiếm gói
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<MNGPackages> SearchPackage(string code, string name)
        {
            try
            {
                var result = await grpcClient.GetAllAsync(new MngPacketRequest { Name = name, Code = code });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Err-SearchPackage", ex);
                return null;
            }
        }
        /// <summary>
        /// Lấy thông tin theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MNG_Package> GetPackageById(string id)
        {
            try
            {
                return await grpcClient.GetByIdAsync(new MngPacketRequest { ID = id });
            }
            catch (Exception ex)
            {
                _logger.LogError("Err-GetPackageById", ex);
                return null;
            }
        }
        /// <summary>
        /// update gói
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<MNGPackagesResponse> UpdatePackage(PackageModel obj)
        {
            return await grpcClient.UpdatePackageAsync(new MNG_Package { ID = obj.ID, CodePackage = obj.CodePackage, PricePackage = obj.PricePackage, NamePackage = obj.NamePackage, UpdatedBy = "admin" });
        }
        /// <summary>
        /// Thêm mới gói cước
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<MNGPackagesResponse> AddPackage(PackageModel obj)
        {
            return await grpcClient.AddPackageAsync(new MNG_Package { ID = Guid.NewGuid().ToString(), CodePackage = obj.CodePackage, PricePackage = obj.PricePackage, NamePackage = obj.NamePackage, CreatedBy = "admin" });
        }
        /// <summary>
        /// Xóa gói
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MNGPackagesResponse> DeletePackage(string id)
        {
            return await grpcClient.DeletePackageAsync(new MngPacketRequest { ID = id, Name = "admin" });
        }
        /// <summary>
        /// Lấy thông tin user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public async Task<MNG_InfoCustomerResonse> GetInfoCustomer(string userName, string passWord)
        {
            return await grpcClient.GetInfoCustomerAsync(new MNG_InfoCustomerRequest { UserName = userName, PassWord = passWord });
        }

    }
}
