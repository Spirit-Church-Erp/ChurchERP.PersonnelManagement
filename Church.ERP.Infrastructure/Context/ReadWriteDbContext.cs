using Church.ERP.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.ERP.Infrastructure.Context
{
    public class ReadWriteDbContext :  IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ReadWriteDbContext(DbContextOptions<ReadWriteDbContext> options)
             : base(options)
        {
        }

        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ignore the PhoneNumbers collection and PhoneNumberConfirmed property
            builder.Entity<ApplicationUser>()
                .Ignore(u => u.PhoneNumber)
                .Ignore(u => u.PhoneNumberConfirmed);
        }
    }
}
