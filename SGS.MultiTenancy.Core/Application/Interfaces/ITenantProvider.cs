using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    public interface ITenantProvider
    {
        Guid TenantId { get; }
    }
}
