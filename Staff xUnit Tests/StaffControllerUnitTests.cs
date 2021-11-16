using AutoMapper;
using Moq;
using Staff_Service.Controllers;
using Staff_Service.DomainModel;
using Staff_Service.DTOs;
using Staff_Service.Repositories;
using System;
using Xunit;

namespace Staff_xUnit_Tests
{
    public class StaffControllerUnitTests
    {
        private StaffController staffController;
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
