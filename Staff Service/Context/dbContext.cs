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

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("staff");

            builder.Entity<StaffDomainModel>()
                .HasData(
                new StaffDomainModel
                {
                    StaffID = 1,
                    StaffFirstName = "Jacob",
                    StaffLastName = "Jardine",
                    StaffEmailAddress = "Jacob-Jardine@ThAmCo.co.uk"
                },
                new StaffDomainModel
                {
                    StaffID = 2,
                    StaffFirstName = "Ben",
                    StaffLastName = "Souch",
                    StaffEmailAddress = "Ben-Souch@ThAmCo.co.uk"
                },
                new StaffDomainModel
                {
                    StaffID = 3,
                    StaffFirstName = "Joseph",
                    StaffLastName = "Stavers",
                    StaffEmailAddress = "Joseph-Stavers@ThAmCo.co.uk"
                },
                new StaffDomainModel
                {
                    StaffID = 4,
                    StaffFirstName = "Teddy",
                    StaffLastName = "Teasdale",
                    StaffEmailAddress = "Teddy-Teasdale@ThAmCo.co.uk"
                });
        }
    }
}
