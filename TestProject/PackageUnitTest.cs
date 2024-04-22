using BlazorModel.Package;
using BlazorServerApp.Services;
using Grpc.Core;
using ManagementPackage;
using PackageSDK.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class PackageUnitTest
    {
        IPackageBzService packageBzService;
        [TestInitialize]
        public void Initialize()
        {
            packageBzService = new PackageBzService();
        }
        [TestMethod]
        public async Task SearchPackage_Test()
        {
            var result = await packageBzService.SearchPackage("PK0021", "");
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count);
            result = await packageBzService.SearchPackage(Guid.NewGuid().ToString(), "");
            Assert.AreEqual(0, result.Items.Count);
        }
        [TestMethod]
        public async Task GetPackageById_Test()
        {
            var result = await packageBzService.GetPackageById("524dbd9b-864c-4db5-8cbf-016b2168c9ce");
            Assert.IsNotNull(result);
            Assert.AreEqual("VT30K", result.CodePackage);
        }
        [TestMethod]
        public async Task AddPackage_Test()
        {
            PackageModel obj = new PackageModel
            {
                CodePackage = "PK002",
                CreatedBy = "sys",
                ID = Guid.NewGuid().ToString(),
                NamePackage = "Test",
                PricePackage = "1.000"
            };

            var result = await packageBzService.AddPackage(obj);
            Assert.IsNotNull(result);
            Assert.AreEqual(Enum.GetName(typeof(StatusCode), StatusCode.AlreadyExists), result.StatusCode);
            obj = new PackageModel
            {
                CodePackage = DateTime.Now.ToString("yyyyMMddHHmmss"),
                CreatedBy = "sys",
                ID = DateTime.Now.ToString("yyyyMMddHHmm"),
                NamePackage = "Test",
                PricePackage = "1.000"
            };
            result = await packageBzService.AddPackage(obj);
            Assert.IsNotNull(result);
            Assert.AreEqual(Enum.GetName(typeof(StatusCode), StatusCode.OK), result.StatusCode);
        }
        [TestMethod]
        public async Task UpdatePackage_Test()
        {
            PackageModel obj = new PackageModel
            {
                CodePackage = DateTime.Now.ToString("yyyyMMddHHmmss"),
                CreatedBy = "sys",
                ID = Guid.NewGuid().ToString(),
                NamePackage = "Test",
                PricePackage = "1.000"
            };

            var result = await packageBzService.UpdatePackage(obj);
            Assert.IsNotNull(result);
            Assert.AreEqual(Enum.GetName(typeof(StatusCode), StatusCode.NotFound), result.StatusCode);
            obj = new PackageModel
            {
                CodePackage = "PK002",
                CreatedBy = "admin",
                ID = "002",
                NamePackage = "Gói cước 23",
                PricePackage = "50.000"
            };
            result = await packageBzService.UpdatePackage(obj);
            Assert.IsNotNull(result);
            Assert.AreEqual(Enum.GetName(typeof(StatusCode), StatusCode.OK), result.StatusCode);
        }
        [TestMethod]
        public async Task GetInfoCustomer_Test()
        {
            var result = await packageBzService.GetInfoCustomer("admin", "");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.UserName == "admin");
        }

        [TestMethod]
        public async Task DeletePackage_Test()
        {
            var result = await packageBzService.DeletePackage(DateTime.Now.ToString("yyyyMMddHHmm"), "sys");
            Assert.IsNotNull(result);
            Assert.AreEqual(Enum.GetName(typeof(StatusCode), StatusCode.OK), result.StatusCode);
        }

    }
}
