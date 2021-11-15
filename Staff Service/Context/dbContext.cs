using Microsoft.EntityFrameworkCore;
using Staff_Service.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.Context
{
    public class dbContext : DbContext
    {
        public DbSet<StaffDomainModel> _staff { get; set; }

        public dbContext(DbContextOptions<dbContext> options) : base(options) { }
    }
}
