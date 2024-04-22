using BlazorModel;
using BlazorModel.Package;
using Grpc.Core;
using Grpc.Net.Client;
using ManagementPackage;

namespace BlazorServerApp.Services
{
    public interface IHisTransactionBzService
    {
        Task<HisTransactions> GetHisTransactionPackage(HisRequestModel request);
        Task<HisTransactions> GetTransactionPackage(HisTransactionModel request);
        Task<HisTransactionResponse> AddTransactionPackage(HisTransactionRequestModel obj);
        Task<HisTransactionResponse> DeleteTransactionPackage(HisTransactionRequestModel obj);
        
    }
    public class HisTransactionBzService : IHisTransactionBzService
    {
        private readonly ILogger<HisTransactionBzService> _logger;
        public HisTransactionBzService(ILogger<HisTransactionBzService> logger)
        {
            _logger = logger;
        }

        public HisTransactionBzService()
        {
        }
        private string address = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Application_Url")["Url"];
        public async Task<HisTransactions> GetHisTransactionPackage(HisRequestModel request)
        {
        using var channel = GrpcChannel.ForAddress(address);
        var client1 = new TransactionPackageProto.TransactionPackageProtoClient(channel);
        var result = await client1.GetHisTransactionPackageAsync(new HisTransactionSearchRequest { NameCus=request.NameCus, NamPackage=request.NamPackage,FromDate=request.FromDate.ToString(FormatDate.DateTime_103),ToDate=request.ToDate.ToString(FormatDate.DateTime_103) });
            return result;
        }
        public async Task<HisTransactions> GetTransactionPackage(HisTransactionModel request)
        {
            using var channel = GrpcChannel.ForAddress(address);
            var client1 = new TransactionPackageProto.TransactionPackageProtoClient(channel);
            var result = await client1.GetTransactionPackageAsync(new HisTransactionSearchRequest { NameCus = request.NameCus, NamPackage = request.NamPackage, FromDate = request.FromDate, ToDate = request.ToDate });
            return result;
        }
        public async Task<HisTransactionResponse> AddTransactionPackage(HisTransactionRequestModel obj)
        {
            using var channel = GrpcChannel.ForAddress(address);
            var client1 = new TransactionPackageProto.TransactionPackageProtoClient(channel);
            return await client1.AddTransactionPackageAsync(new HisTransactionRequest {  CusId=obj.CusId, PackageId=obj.PackageId, CreatedBy=obj.CreatedBy});
        }
        public async Task<HisTransactionResponse> DeleteTransactionPackage(HisTransactionRequestModel obj)
        {
            using var channel = GrpcChannel.ForAddress(address);
            var client1 = new TransactionPackageProto.TransactionPackageProtoClient(channel);
            return await client1.DeleteTransactionPackageAsync(new HisTransactionRequest { CusId = obj.CusId, PackageId = obj.PackageId, CreatedBy = obj.CreatedBy });
        }
    }

}
