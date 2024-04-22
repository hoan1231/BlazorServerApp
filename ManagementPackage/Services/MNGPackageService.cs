using BlazorModel;
using BlazorModel.Package;
using Grpc.Core;
using ManagementPackage;
using ManagementPackage.Models;
using Newtonsoft.Json;
namespace ManagementPackage.Services
{
    public class MNGPackageService : PackageProto.PackageProtoBase
    {
        private readonly ILogger<MNGPackageService> _logger;
        public PackageDBContext dbContext;
        public MNGPackageService(ILogger<MNGPackageService> logger, PackageDBContext DBContext)
        {
            _logger = logger;
            dbContext = DBContext;
        }
        /// <summary>
        /// Lấy tất cả gói cước
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<MNGPackages> GetAll(MngPacketRequest request, ServerCallContext context)
        {

            MNGPackages response = new MNGPackages();
            var item = from pk in dbContext.MngPackages
                       where pk.IsDeleted == 0 && pk.NamePackage.Contains(request.Name) && pk.CodePackage.Contains(request.Code)
                       select new MNG_Package()
                       {
                           ID = pk.Id,
                           CodePackage = pk.CodePackage,
                           CreatedBy = pk.CreatedBy,
                           NamePackage = pk.NamePackage,
                           PricePackage = pk.PricePackage,
                           Decription = pk.Decription,
                           CreatedDate = pk.CreatedDate,
                           UpdatedBy = pk.UpdatedBy,
                           UpdatedDate = pk.UpdatedDate
                       };
            response.Items.AddRange(item.ToArray());
            return Task.FromResult(response);
        }
        /// <summary>
        /// Lấy gói cước theo ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<MNG_Package> GetById(MngPacketRequest request, ServerCallContext context)
        {
            var item = dbContext.MngPackages.Where(x => x.Id == request.ID && x.IsDeleted == 0).FirstOrDefault();
            if (item == null) return null;
            var searchedPackage = new MNG_Package()
            {
                ID = item.Id,
                CodePackage = item.CodePackage,
                CreatedBy = item.CreatedBy,
                NamePackage = item.NamePackage,
                PricePackage = item.PricePackage,
                Decription = item.Decription,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate
            };
            return Task.FromResult(searchedPackage);
        }
        /// <summary>
        /// Thêm mới gói cước
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<MNGPackagesResponse> AddPackage(MNG_Package request, ServerCallContext context)
        {
            var log = new LogRequestModel();
            log.LogRequest = JsonConvert.SerializeObject(request);
            log.CreatedBy = request.CreatedBy;
            var item = dbContext.MngPackages.Where(x => x.CodePackage == request.CodePackage && x.IsDeleted == 0).FirstOrDefault();
            if (item != null)
            {
                var resultLog = new MNGPackagesResponse { Message = "Mã gói cước đã tồn tại", StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists) };
                log.LogResponse = JsonConvert.SerializeObject(resultLog);
                log.StatusCode = resultLog.StatusCode;
                await SaveLogRequest(log);
                return await Task.FromResult(resultLog);
            }
            var obj = new MngPackage()
            {
                Id = request.ID,
                CodePackage = request.CodePackage,
                CreatedBy = request.CreatedBy,
                NamePackage = request.NamePackage,
                PricePackage = request.PricePackage,
                Decription = request.Decription,
                IsDeleted = 0,
                TypePackage = "",
                CreatedDate = DateTime.Now.ToString(FormatDate.DateTime_ddMMyyyyHHmmss),
                UpdatedDate = DateTime.Now.ToString(FormatDate.DateTime_ddMMyyyyHHmmss),
                UpdatedBy = request.CreatedBy,

            };
            dbContext.MngPackages.Add(obj);
            dbContext.SaveChanges();
            var result = new MNGPackagesResponse { Message = "Thêm mới thành công", StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.OK) };
            log.LogResponse = JsonConvert.SerializeObject(result);
            log.StatusCode = result.StatusCode;
            await SaveLogRequest(log);
            return await Task.FromResult(result);
        }
        /// <summary>
        /// Cập nhật gói cước
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<MNGPackagesResponse> UpdatePackage(MNG_Package request, ServerCallContext context)
        {
            var log = new LogRequestModel();
            log.LogRequest = JsonConvert.SerializeObject(request);
            log.CreatedBy = request.UpdatedBy;
            var item = dbContext.MngPackages.Where(x => x.Id == request.ID && x.IsDeleted == 0).FirstOrDefault();
            if (item == null)
            {
                var resultLog = new MNGPackagesResponse { Message = "Không tìm thấy gói cước", StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.NotFound) };
                log.LogResponse = JsonConvert.SerializeObject(resultLog);
                log.StatusCode = resultLog.StatusCode;
                await SaveLogRequest(log);
                return await Task.FromResult(resultLog);
            }
            item.CodePackage = request.CodePackage;
            item.UpdatedBy = request.UpdatedBy;
            item.NamePackage = request.NamePackage;
            item.PricePackage = request.PricePackage;
            item.Decription = request.Decription;
            item.IsDeleted = 0;
            item.UpdatedDate = DateTime.Now.ToString(FormatDate.DateTime_ddMMyyyyHHmmss);
            dbContext.SaveChanges();
            var result = new MNGPackagesResponse { Message = "Cập nhật thành công", StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.OK) };
            log.LogResponse = JsonConvert.SerializeObject(result);
            log.StatusCode = result.StatusCode;
            await SaveLogRequest(log);
            return await Task.FromResult(result);
        }
        /// <summary>
        /// Xóa gói cước
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async  Task<MNGPackagesResponse> DeletePackage(MngPacketRequest request, ServerCallContext context)
        {
            var log = new LogRequestModel();
            log.LogRequest = JsonConvert.SerializeObject(request);
            log.CreatedBy = request.Name;
            var item = dbContext.MngPackages.Where(x => x.Id == request.ID && x.IsDeleted == 0).FirstOrDefault();
            if (item == null)
            {
                var resultLog = new MNGPackagesResponse { Message = "Không tìm thấy gói cước", StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists) };
                log.LogResponse = JsonConvert.SerializeObject(resultLog);
                log.StatusCode = resultLog.StatusCode;
                await SaveLogRequest(log);
                return await Task.FromResult(resultLog);
            }
            item.UpdatedBy = request.Name;
            item.UpdatedDate = item.UpdatedDate;
            item.IsDeleted = 1;
            dbContext.SaveChanges();
            var result = new MNGPackagesResponse { Message = "Xóa thành công", StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.OK) };
            log.LogResponse = JsonConvert.SerializeObject(result);
            log.StatusCode = result.StatusCode;
            await SaveLogRequest(log);
            return await Task.FromResult(result);
        }
        public override Task<MNG_InfoCustomerResonse> GetInfoCustomer(MNG_InfoCustomerRequest request, ServerCallContext context)
        {
            var item = dbContext.Customers.Where(x => x.UserName == request.UserName && x.IsDeleted == 0).FirstOrDefault();
            if (item == null)
            {
                return Task.FromResult(new MNG_InfoCustomerResonse { });
            }
            return Task.FromResult(new MNG_InfoCustomerResonse { UserName = item.UserName, FullName = item.FullName, Amount = item.TotalMoney, UserId = item.Id });
        }
        /// <summary>
        /// Lưu log request
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task SaveLogRequest(LogRequestModel obj)
        {
            await dbContext.LogRequests.AddAsync(new LogRequest
            {
                Id = Guid.NewGuid(),
                ModelRequest = obj.LogRequest,
                ModelResponse = obj.LogResponse,
                StatusCode = obj.StatusCode,
                CreateBy = obj.CreatedBy,
                CreatedDate = DateTime.Now,
            });
            await dbContext.SaveChangesAsync();
        }
    }
}
