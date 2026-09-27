using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.Domain.Enums
{
    public enum InvoiceStatus
    {
        Draft = 1,
        Issued = 2,
        Paid = 3,
        Cancelled = 4
    }
}
