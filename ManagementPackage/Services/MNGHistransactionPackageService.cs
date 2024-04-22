using BlazorModel;
using BlazorModel.Package;
using Grpc.Core;
using ManagementPackage.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using Newtonsoft.Json;
namespace ManagementPackage.Services
{
    public class MNGHistransactionPackageService: TransactionPackageProto.TransactionPackageProtoBase
    {
        private readonly ILogger<MNGHistransactionPackageService> _logger;
        public PackageDBContext dbContext;
        public MNGHistransactionPackageService(ILogger<MNGHistransactionPackageService> logger, PackageDBContext DBContext)
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
        public override Task<HisTransactions> GetTransactionPackage(HisTransactionSearchRequest request, ServerCallContext context)
        {
            HisTransactions response = new HisTransactions();
            var item =
                from a in dbContext.MngPackages
                join bpk in dbContext.CustomerPackages on a.Id equals bpk.PackageId
                join cus in dbContext.Customers on bpk.CustomerId equals cus.Id
                where a.IsDeleted == 0 && bpk.IsDeleted == 0 && cus.IsDeleted == 0 && a.NamePackage.Contains(request.NamPackage) && cus.UserName.Contains(request.NameCus)
                select new HisTransaction()
                {
                    ID = bpk.Id,
                    NameCus = cus.UserName,
                    TypePackage = a.TypePackage,
                    CreatedBy = bpk.CreatedBy,
                    NamePackage = a.NamePackage,
                    PricePackage = a.PricePackage,
                    Decription = a.Decription,
                    CreatedDate = bpk.CreatedDate
                };
            response.Items.AddRange(item.ToList());
            return Task.FromResult(response);
        }/// <summary>
        /// Tra cứu lịch sử tác động
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HisTransactions> GetHisTransactionPackage(HisTransactionSearchRequest request, ServerCallContext context)
        {
            HisTransactions response = new HisTransactions();
            var query = dbContext.MngHistoryTransactions.AsQueryable();
            if (!string.IsNullOrEmpty(request.FromDate) && DateTime.TryParseExact(request.FromDate, FormatDate.DateTime_103,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fdate))
            {
                query = query.Where(x => x.CreatedDate >= fdate);
            }
            if (!string.IsNullOrEmpty(request.ToDate) && DateTime.TryParseExact(request.ToDate, FormatDate.DateTime_103,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime tdate))
            {
                tdate = tdate.AddDays(1);
                query = query.Where(x => x.CreatedDate <= tdate);
            }
            if (!string.IsNullOrEmpty(request.NamPackage)) 
            
                query = query.Where(x => x.NamePackage.Contains(request.NamPackage));
            if (!string.IsNullOrEmpty(request.NameCus))  query = query.Where(x => x.FullName.Contains(request.NameCus));
            var item = query.Select(x => new HisTransactionResponseModel()
        {
            ID = x.Id,
            NameCus = x.FullName,
            TypePackage = x.TypeTransaction,
            CreatedBy = x.CreatedBy,
            NamePackage = x.NamePackage,
            PricePackage = x.PricePackage,
            CreatedDate = x.CreatedDate,
            Decription = string.IsNullOrEmpty(x.Decription) ? "Thành công" : "Thất bại. " + x.Decription,
        }).OrderByDescending(x => x.CreatedDate).ToList();
            var result = item.Select(x => new HisTransaction()
            {
                ID = x.ID,
                NameCus = x.NameCus,
                TypePackage = x.TypePackage,
                CreatedBy = x.CreatedBy,
                NamePackage = x.NamePackage,
                PricePackage = x.PricePackage,
                CreatedDate = x.CreatedDateStr,
                Decription=x.Decription,
            }).ToList();
            response.Items.AddRange(result);
            return Task.FromResult(response);
        }
        /// <summary>
        /// Đăng ký gói cước
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<HisTransactionResponse> AddTransactionPackage(HisTransactionRequest request, ServerCallContext context)
        {
            var log = new LogRequestModel();
            log.LogRequest = JsonConvert.SerializeObject(request);
            log.CreatedBy = request.CreatedBy;
            var item = dbContext.CustomerPackages.Where(x => x.CustomerId == request.CusId && x.PackageId == request.PackageId && x.IsDeleted == 0).FirstOrDefault();
            var his = new MngHistoryTransaction()
            {
                Id = Guid.NewGuid().ToString(),
                PackageId = request.PackageId,
                CreatedBy = request.CreatedBy,
                FullName = request.CreatedBy,
                CustomerId = request.CusId,
                IsDeleted = 0,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                UpdatedBy = request.CreatedBy,
                TypeTransaction = "Add",
            };
            var package = dbContext.MngPackages.Where(x => x.Id == request.PackageId && x.IsDeleted == 0).FirstOrDefault();
            if (package == null)
            {
                if (item != null)
                {
                    his.Decription = "Không tìm thấy gói cước đăng ký";
                    var resultLog1 = new HisTransactionResponse { Message = his.Decription, StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists) };
                    log.LogResponse = JsonConvert.SerializeObject(resultLog1);
                    log.StatusCode = resultLog1.StatusCode;
                    await SaveLogRequest(log);
                    await dbContext.MngHistoryTransactions.AddAsync(his);
                    await dbContext.SaveChangesAsync();
                    return await Task.FromResult(resultLog1);
                }
            }
            his.NamePackage = package.NamePackage;
            his.PricePackage = package.PricePackage;
            his.Decription = package.Decription;
            if (item != null)
            {
                his.Decription = "Bạn đã đăng ký gói cước này";
                var resultLog = new HisTransactionResponse { Message = his.Decription, StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists) };
                log.LogResponse = JsonConvert.SerializeObject(resultLog);
                log.StatusCode = resultLog.StatusCode;
                await SaveLogRequest(log);
                await dbContext.MngHistoryTransactions.AddAsync(his);
                await dbContext.SaveChangesAsync();
                return await Task.FromResult(resultLog);
            }
            var obj = new CustomerPackage()
            {
                Id = Guid.NewGuid().ToString(),
                PackageId = request.PackageId,
                CreatedBy = request.CreatedBy,
                CustomerId = request.CusId,
                IsDeleted = 0,
                CreatedDate = DateTime.Now.ToString(FormatDate.DateTime_ddMMyyyyHHmmss),
                UpdatedDate = DateTime.Now.ToString(FormatDate.DateTime_ddMMyyyyHHmmss),
                UpdatedBy = request.CreatedBy,

            };
            await dbContext.CustomerPackages.AddAsync(obj);
            await dbContext.MngHistoryTransactions.AddAsync(his);
            await dbContext.SaveChangesAsync();
            var result_Log = new HisTransactionResponse { Message = "Thêm mới thành công", StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.OK) };
            log.LogResponse = JsonConvert.SerializeObject(result_Log);
            log.StatusCode = result_Log.StatusCode;
            return await Task.FromResult(result_Log);
        }
        /// <summary>
        /// Hủy gói cước
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<HisTransactionResponse> DeleteTransactionPackage(HisTransactionRequest request, ServerCallContext context)
        {
            var log = new LogRequestModel();
            log.LogRequest = JsonConvert.SerializeObject(request);
            log.CreatedBy = request.CreatedBy;
            var item = dbContext.CustomerPackages.Where(x => x.Id == request.CusId && x.IsDeleted == 0).FirstOrDefault();
            var his = new MngHistoryTransaction()
            {
                Id = Guid.NewGuid().ToString(),
                PackageId = request.PackageId,
                CreatedBy = request.CreatedBy,
                FullName = request.CreatedBy,
                CustomerId = request.CusId,
                IsDeleted = 0,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                UpdatedBy = request.CreatedBy,
                NamePackage = "",
                PricePackage = "",
                Decription = "",
                TypeTransaction = "Cancel",
            };
            if (item == null)
            {
                his.Decription = "Không tìm thấy gói cước đăng ký bạn đăng ký";
                var result_Log = new HisTransactionResponse { Message = his.Decription, StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists) };
                log.LogResponse = JsonConvert.SerializeObject(result_Log);
                log.StatusCode = result_Log.StatusCode;
                await SaveLogRequest(log);
                await dbContext.MngHistoryTransactions.AddAsync(his);
                await dbContext.SaveChangesAsync();
                return await Task.FromResult(result_Log);
            }
            var package = dbContext.MngPackages.Where(x => x.Id == item.PackageId && x.IsDeleted == 0).FirstOrDefault();
            if (package == null)
            {
                his.Decription = "Không tìm thấy gói cước đăng ký";
                var resultLog1 = new HisTransactionResponse { Message = his.Decription, StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists) };
                log.LogResponse = JsonConvert.SerializeObject(resultLog1);
                log.StatusCode = resultLog1.StatusCode;
                await SaveLogRequest(log);
                await dbContext.MngHistoryTransactions.AddAsync(his);
                await dbContext.SaveChangesAsync();
                return await Task.FromResult(resultLog1);
            }
            his.NamePackage = package.NamePackage;
            his.PricePackage = package.PricePackage;
            his.Decription = package.Decription;
            item.IsDeleted = 1;
            item.UpdatedBy = request.CreatedBy;
            item.UpdatedDate = DateTime.Now.ToString(FormatDate.DateTime_ddMMyyyyHHmmss);
            await dbContext.MngHistoryTransactions.AddAsync(his);
            await dbContext.SaveChangesAsync();
            var resultLog = new HisTransactionResponse { Message = "Hủy gói cước thành công", StatusCode = Enum.GetName(typeof(StatusCode), StatusCode.OK) };
            log.LogResponse = JsonConvert.SerializeObject(resultLog);
            log.StatusCode = resultLog.StatusCode;
            await SaveLogRequest(log);
            return await Task.FromResult(resultLog);
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
