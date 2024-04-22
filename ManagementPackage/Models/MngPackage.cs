using System;
using System.Collections.Generic;

namespace ManagementPackage.Models
{
    public partial class MngPackage
    {
        public string Id { get; set; } = null!;
        public string? CodePackage { get; set; }
        public string? NamePackage { get; set; }
        public string? TypePackage { get; set; }
        public string? PricePackage { get; set; }
        public string? Decription { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public int? IsDeleted { get; set; }
    }
}
