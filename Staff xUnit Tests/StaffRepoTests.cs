using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Staff_Service.Context;
using Staff_Service.Controllers;
using Staff_Service.DomainModel;
using Staff_Service.DTOs;
using Staff_Service.Profiles;
using Staff_Service.Repositories;
using Staff_Service.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Staff_xUnit_Tests
{
    public class StaffRepoTests
    {
        public StaffRepoModel staffRepoModel;
        public IMapper mapper;
        public IQueryable<StaffDomainModel> dbStaffs;
        public StaffDomainModel dbStaff;
        public Mock<DbSet<StaffDomainModel>> mockStaff;
        public Mock<ProductionContext> mockDbContext;
        public FakeStaffRepository repo;
        public StaffRepoModel anonymisedStaff;

        private void SetUpStaffRepoModel()
        {
            staffRepoModel = new StaffRepoModel
            {
                StaffID = 1,
                StaffFirstName = "Jacob",
                StaffLastName = "Jardine",
                StaffEmailAddress = "JacobJardine@ThAmCo.com"
            };
        }

        private void SetUpDbStaff()
        {
            dbStaff = new StaffDomainModel
            {
                StaffID = 1,
                StaffFirstName = "Jacob",
                StaffLastName = "Jardine",
                StaffEmailAddress = "JacobJardine@ThAmCo.com"
            };
        }

        private void SetUpMapper()
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StaffProfile());
            }).CreateMapper();
        }

        private void SetUpStaffs()
        {
            SetUpDbStaff();
            dbStaffs = new List<StaffDomainModel>
            {
                dbStaff
            }.AsQueryable();
        }

        private void SetUpMockStaffs()
        {
            mockStaff = new Mock<DbSet<StaffDomainModel>>();
            mockStaff.As<IQueryable<StaffDomainModel>>().Setup(m => m.Provider).Returns(dbStaffs.Provider);
            mockStaff.As<IQueryable<StaffDomainModel>>().Setup(m => m.Expression).Returns(dbStaffs.Expression);
            mockStaff.As<IQueryable<StaffDomainModel>>().Setup(m => m.ElementType).Returns(dbStaffs.ElementType);
            mockStaff.As<IQueryable<StaffDomainModel>>().Setup(m => m.GetEnumerator()).Returns(dbStaffs.GetEnumerator());
        }

        private void SetupMockDbContext()
        {
            mockDbContext = new Mock<ProductionContext>();
            mockDbContext.Setup(m => m.production_db).Returns(mockStaff.Object);
        }

        private void DefaultSetup()
        {
            SetUpMapper();
            SetUpStaffRepoModel();
            SetUpStaffs();
            SetUpMockStaffs();
            SetupMockDbContext();
            repo = new SqlStaffRepository(mockDbContext.Object, mapper);
        }
    }
}
