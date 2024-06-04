using Church.ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.ERP.Application.Interfaces
{
    public interface IJWTConfiguration
    {
         string GenerateJwtToken(ApplicationUser user);
    }
}
