using System;
using System.Collections.Generic;

namespace ManagementPackage.Models
{
    public partial class Customer
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? TotalMoney { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public int? IsDeleted { get; set; }
    }
}
