using System;
using System.Collections.Generic;

namespace ManagementPackage.Models
{
    public partial class MngHistoryTransaction
    {
        public string Id { get; set; } = null!;
        public string? CustomerId { get; set; }
        public string? PackageId { get; set; }
        public string? FullName { get; set; }
        public string? NamePackage { get; set; }
        public string? TypeTransaction { get; set; }
        public string? PricePackage { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? IsDeleted { get; set; }
        public string? Decription { get; set; }
    }
}
