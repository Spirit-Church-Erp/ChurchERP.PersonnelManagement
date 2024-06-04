using Church.ERP.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.ERP.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        ReadDbContext ReadContext { get; }
        ReadWriteDbContext WriteContext { get; }
        Task<int> SaveChangesAsync();
    }
}
