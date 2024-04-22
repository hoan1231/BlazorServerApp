using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorModel.Package
{
    public class PackageModel
    {
        [Required(ErrorMessage = "Không được để trống mã gói cước")]
        public string CodePackage { get; set; }
        public string ID { get; set; }
        public string NamePackage { get; set; }
        public string PricePackage { get; set; }
        public string CreatedBy { get; set; }
    }

    public class PackageRequestModel
    {
        public string CodePackage { get; set; }
        public string NamePackage { get; set; }
        public string PricePackage { get; set; }
    }
    public class PackageResponseModel
    {
        public string ID { get; set; }
        public string CodePackage { get; set; }
        public string NamePackage { get; set; }
        public string TypePackage { get; set; }
        public string PricePackage { get; set; }
        public string Decription { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
    public class PackageResponsesModel
    {
        public List<PackageResponseModel> items { get; set; }
    }
    public class PackageMessageModel
    {
        public string message { get; set; }
        public string statusCode { get; set; }
    }
}