using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Models
{
    public record Email
    {
        public required string To { get; set; }
        public required string Subject { get; set; }
        public string Body { get; set; } = string.Empty;
    }
}
