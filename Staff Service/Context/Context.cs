using Microsoft.EntityFrameworkCore;
using Staff_Service.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.Context
{
    public class Context : DbContext
    {
        public DbSet<StaffDomainModel> _staff { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }
    }
}
