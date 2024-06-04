using Church.ERP.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.ERP.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReadDbContext _readContext;
        private readonly ReadWriteDbContext _writeContext;

        public UnitOfWork(ReadDbContext readContext, ReadWriteDbContext writeContext)
        {
            _readContext = readContext;
            _writeContext = writeContext;
        }

        public ReadDbContext ReadContext => _readContext;
        public ReadWriteDbContext WriteContext => _writeContext;

        public async Task<int> SaveChangesAsync()
        {
            return await _writeContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _readContext?.Dispose();
            _writeContext?.Dispose();
        }
    }
}
