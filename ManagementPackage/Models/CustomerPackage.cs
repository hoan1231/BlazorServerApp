using System;
using System.Collections.Generic;

namespace ManagementPackage.Models
{
    public partial class CustomerPackage
    {
        public string Id { get; set; } = null!;
        public string? CustomerId { get; set; }
        public string? PackageId { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public int? IsDeleted { get; set; }
    }
}
