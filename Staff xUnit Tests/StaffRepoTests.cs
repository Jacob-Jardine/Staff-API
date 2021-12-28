using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Staff_Service.Context;
using Staff_Service.Controllers;
using Staff_Service.DomainModel;
using Staff_Service.DTOs;
using Staff_Service.Repositories;
using Staff_Service.Repositories.Models;
using System;
using System.Linq;
using Xunit;

namespace Staff_xUnit_Tests
{
    public class StaffControllerUnitTests
    {
        public StaffRepoModel staffRepoModel;
        public IMapper mapper;
        public IQueryable<StaffDomainModel> dbStaffs;
        public StaffDomainModel dbStaff;
        public Mock<DbSet<StaffDomainModel> mockStaff;
        public Mock<ProductionContext> mockDbContext;
        public FakeStaffRepository repo;
        public StaffRepoModel anonymisedStaff;


        private Mock<IStaffRepository> mockIStaffRepository;
        private FakeStaffRepository mockFakeStaffRepository;
        private StaffDomainModel staffDomainModel;
        private StaffReadDTO staffReadDTO;
        private IMapper mapper;

        private void SetFakeRepository() 
        {
            mockFakeStaffRepository = new FakeStaffRepository();
        }

        private void SetStaffDomainModel()
        {
            staffDomainModel = new StaffDomainModel()
            {
                StaffID = 1,
                StaffFirstName = "Test",
                StaffLastName = "Test",
                StaffEmailAddress = "Test-Test@ThAmCo.co.uk"
            };
        }

        private void SetStaffReadDTO() 
        {
            staffReadDTO = new StaffReadDTO()
            {
                StaffID = 1,
                StaffFirstName = "Test",
                StaffLastName = "Test",
                StaffEmailAddress = "Test-Test@ThAmCo.co.uk"
            };
        }

        [Fact]
        public void SampleTest()
        {

        }
    }
}
