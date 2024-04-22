using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlazorModel.Package
{
    public class HisTransactionModel
    {
        public string NameCus { get; set; }
        public string NamPackage { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class HisTransactionRequestModel
    {
        public string CusId { get; set; }
        public string PackageId { get; set; }
        public string CreatedBy { get; set; }
    }
    public class HisRequestModel
    {
        public string NameCus { get; set; }
        public string NamPackage { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
       public class HisTransactionResponseModel
    {
        public string ID { get; set; }
        public string NameCus { get; set; }
        public string NamePackage { get; set; }
        public string TypePackage { get; set; }
        public string PricePackage { get; set; }
        public string Decription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ? CreatedDate { get; set; }
        public string CreatedDateStr
        {
            get
            {
                if (CreatedDate.HasValue) return CreatedDate.Value.ToString(FormatDate.DateTime_ddMMyyyyHHmmss);
                else return "";
            }
        }


    }
    public class LogRequestModel
    {
        public string LogRequest { get; set; }
        public string LogResponse { get; set; }
        public string StatusCode { get; set; }
        public string CreatedBy { get; set; }
    }
}
