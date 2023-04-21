using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Models
{
    public record EmailSettings
    {
        public required string ApiKey { get; set; }
        public required string FromAddress { get; set; }
        public required string FromName { get; set; }
    }
}
