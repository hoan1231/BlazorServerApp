using BlazorModel.Package;
using Grpc.Core;
using Grpc.Net.Client;
using ManagementPackage;

namespace BlazorServerApp.Services
{
    public interface IPackageBzService
    {
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
        Task<MNGPackagesResponse> DeletePackage(string id, string userLogin);
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
    public class PackageBzService : IPackageBzService
    {
        private readonly ILogger<PackageBzService> _logger;
        public PackageBzService(ILogger<PackageBzService> logger)
        {
            _logger = logger;
        }

        public PackageBzService()
        {
        }
        public string address= new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Application_Url")["Url"];
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<MNGPackages> SearchPackage(string code, string name)
        {
            try
            {
                if (code == null) code = string.Empty;
                if (name == null) name = string.Empty;
                using var channel = GrpcChannel.ForAddress(address);
                var client1 = new PackageProto.PackageProtoClient(channel);
                var result = await client1.GetAllAsync(new MngPacketRequest { Name = name,Code=code });
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError("SearchPackage", ex);
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
                using var channel = GrpcChannel.ForAddress(address);
                var client1 = new PackageProto.PackageProtoClient(channel);
                return await client1.GetByIdAsync(new MngPacketRequest { ID=id });
            }
            catch (Exception ex)
            {
                _logger.LogError("GetPackageById "+id,ex);
                return null;
            }
        }
        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<MNGPackagesResponse> UpdatePackage(PackageModel obj)
        {
                using var channel = GrpcChannel.ForAddress(address);
                var client1 = new PackageProto.PackageProtoClient(channel);
                return await client1.UpdatePackageAsync(new MNG_Package { ID = obj.ID, CodePackage = obj.CodePackage,PricePackage=obj.PricePackage, NamePackage = obj.NamePackage,UpdatedBy=obj.CreatedBy });
        }
        public async Task<MNGPackagesResponse> AddPackage(PackageModel obj)
        {
            using var channel = GrpcChannel.ForAddress(address);
            var client1 = new PackageProto.PackageProtoClient(channel);
            return await client1.AddPackageAsync(new MNG_Package { ID = obj.ID, CodePackage = obj.CodePackage, PricePackage = obj.PricePackage, NamePackage = obj.NamePackage, CreatedBy = obj.CreatedBy });
        }
        public async Task<MNGPackagesResponse> DeletePackage(string id,string userLogin)
        {
            using var channel = GrpcChannel.ForAddress(address);
            var client1 = new PackageProto.PackageProtoClient(channel);
            return await client1.DeletePackageAsync(new MngPacketRequest { ID = id,Name= userLogin });
        }
        public async Task<MNG_InfoCustomerResonse> GetInfoCustomer(string userName,string passWord)
        {

            using var channel = GrpcChannel.ForAddress(address);
            var client1 = new PackageProto.PackageProtoClient(channel);
            return await client1.GetInfoCustomerAsync(new MNG_InfoCustomerRequest { UserName = userName, PassWord = passWord });
        }
    }
}
