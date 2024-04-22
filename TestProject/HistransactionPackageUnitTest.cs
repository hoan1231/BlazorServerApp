using BlazorModel.Package;
using BlazorServerApp.Services;
using Grpc.Core;
using ManagementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class HistransactionPackageUnitTest
    {
        IHisTransactionBzService hisTransactionBzService;
        [TestInitialize]
        public void Initialize()
        {
            hisTransactionBzService = new HisTransactionBzService();
        }
        [TestMethod]
        public async Task GetHisTransactionPackage_Test()
        {
            HisRequestModel obj = new HisRequestModel
            {
                NameCus = "hoanlv",
                NamPackage ="",
                FromDate = DateTime.Parse("2024-04-16"),
                ToDate = DateTime.Parse("2024-04-16"),
            };
            var result = await hisTransactionBzService.GetHisTransactionPackage(obj);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Items.Count);
        }
        [TestMethod]
        public async Task GetTransactionPackage_Test()
        {
            HisTransactionModel obj = new HisTransactionModel
            {
                NameCus = "hoanlv",
                NamPackage = "",
                FromDate="",
                ToDate=""
            };
            var result = await hisTransactionBzService.GetTransactionPackage(obj);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task AddTransactionPackage_Test()
        {
            var obj = new HisTransactionRequestModel
            {
                PackageId = "7536da61-a845-4909-a3af-5dd5b8da6aa6",
                CusId = "3020857E-5A27-47A1-B474-19D1B77822BD",
                CreatedBy = "sys"
            };

            var result = await hisTransactionBzService.AddTransactionPackage(obj);
            Assert.IsNotNull(result);
            Assert.AreEqual(Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists), result.StatusCode);
            obj = new HisTransactionRequestModel
            {
                PackageId = "524dbd9b-864c-4db5-8cbf-016b2168c9ce",
                CusId = "3020857E-5A27-47A1-B474-19D1B77822BD",
                CreatedBy = "sys"
            };
            result = await hisTransactionBzService.AddTransactionPackage(obj);
            Assert.IsNotNull(result);
            Assert.IsTrue(Enum.GetName(typeof(StatusCode), StatusCode.OK) == result.StatusCode || (Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists) == result.StatusCode));

        }
        [TestMethod]
        public async Task DeleteTransactionPackage_Test()
        {
            var obj = new HisTransactionRequestModel
            {
                PackageId = Guid.NewGuid().ToString(),
                CusId = "3020857E-5A27-47A1-B474-19D1B77822BD",
                CreatedBy = "sys"
            };

            var result = await hisTransactionBzService.DeleteTransactionPackage(obj);
            Assert.IsNotNull(result);
            Assert.AreEqual(Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists), result.StatusCode);
            obj = new HisTransactionRequestModel
            {
                PackageId = "524dbd9b-864c-4db5-8cbf-016b2168c9ce",
                CusId = "e2dded95-504d-4837-a2b2-f4e2f4a21f39",
                CreatedBy = "sys"
            };
            result = await hisTransactionBzService.DeleteTransactionPackage(obj);
            Assert.IsNotNull(result);
            Assert.IsTrue(Enum.GetName(typeof(StatusCode), StatusCode.OK) == result.StatusCode || (Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists) == result.StatusCode));
        }


    }
}
