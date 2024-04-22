using System;
using System.Collections.Generic;

namespace ManagementPackage.Models
{
    public partial class LogRequest
    {
        public Guid Id { get; set; }
        public string? ModelRequest { get; set; }
        public string? ModelResponse { get; set; }
        public string? StatusCode { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
