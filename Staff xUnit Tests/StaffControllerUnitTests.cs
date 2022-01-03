using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Staff_Service.Controllers;
using Staff_Service.DomainModel;
using Staff_Service.DTOs;
using Staff_Service.Profiles;
using Staff_Service.Repositories;
using System;
using System.Collections.Generic;
using Xunit;

namespace Staff_xUnit_Tests
{
    public class StaffControllerUnitTests
    {
        private StaffDomainModel staffModel;
        private List<StaffDomainModel> listStaffModel;
        private FakeStaffRepository fakeRepo;
        private Mock<IStaffRepository> mockRepo;
        private IMapper mapper;
        private ILogger<StaffController> logger;
        private StaffController controller;
        private readonly IMemoryCache _memoryCache;

        private void SetUpController(StaffController controller)
        {
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        public void SetUpStaffModel()
        {
            staffModel = new StaffDomainModel
            {
                StaffID = 3,
                StaffFirstName = "ABC",
                StaffLastName = "DEF",
                StaffEmailAddress = "ABCDEF@ThAmCo.com"
            };
        }

        public void SetUpFakeStaffList()
        {
            listStaffModel = new List<StaffDomainModel>()
            {
                new StaffDomainModel() {StaffID = 1, StaffFirstName = "Jacob", StaffLastName = "Jardine", StaffEmailAddress = "Jacob.Jardine@ThAmCo.co.uk" },
                new StaffDomainModel() {StaffID = 2, StaffFirstName = "Ben", StaffLastName = "Souch", StaffEmailAddress = "Ben.Souch@ThAmCo.co.uk"}
            };
        }

        private void SetFakeRepo()
        {
            fakeRepo = new FakeStaffRepository
            {
                _staffList = listStaffModel 
            };
        }

        private void SetMapper()
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StaffProfile());
            }).CreateMapper();
        }

        private void SetLogger()
        {
            logger = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider()
                .GetService<ILoggerFactory>()
                .CreateLogger<StaffController>();
        }

        private void SetMockReviewRepo()
        {
            mockRepo = new Mock<IStaffRepository>(MockBehavior.Strict);
            
            mockRepo.Setup(repo => repo.GetAllStaffAsync())
              .ReturnsAsync(new List<StaffDomainModel>()).Verifiable();

            mockRepo.Setup(repo => repo.GetStaffByIDAsnyc(It.IsAny<int>()))
               .ReturnsAsync(new StaffDomainModel()).Verifiable();
            mockRepo.Setup(repo => repo.CreateStaff(It.IsAny<StaffDomainModel>())).Verifiable();
            
            mockRepo.Setup(repo => repo.UpdateStaff(It.IsAny<StaffDomainModel>())).Verifiable();
            
            mockRepo.Setup(repo => repo.DeleteStaff(It.IsAny<int>())).Verifiable();
        }

        private void DefaultSetup()
        {
            SetUpStaffModel();
            SetUpFakeStaffList();
            SetMapper();
        }

        private void SetupWithFakes()
        {
            DefaultSetup();
            SetFakeRepo();
            controller = new StaffController(fakeRepo, mapper, logger, _memoryCache);
            SetUpController(controller);
        }

        private void SetupWithMocks()
        {
            DefaultSetup();
            SetMockReviewRepo();
            controller = new StaffController(mockRepo.Object, mapper, logger, _memoryCache);
            SetUpController(controller);
        }

        #region Testing With Fakes
        [Fact]
        public async void GetAllStaff_True()
        {
            //Arrange
            SetupWithFakes();
            int StaffId = 1;

            //Act
            var result = await controller.GetStaffByID(StaffId);

            //Assert
            Assert.NotNull(result);
            var objResult = result as OkObjectResult;
            Assert.NotNull(objResult);
            var staff = objResult.Value;
            var staffModel = mapper.Map<StaffDomainModel>(staff);
            Assert.NotNull(staff);
            Assert.True(staffModel.StaffID == staff.StaffID);
            Assert.True(staffModel.StaffFirstName == staff.StaffFirstName);
            Assert.True(staffModel.StaffLastName == staff.StaffLastName);
            Assert.True(staffModel.StaffEmailAddress == staff.StaffEmailAddress);
        }

        [Fact]
        public async void GetStaffById_True()
        {
            //Arrange
            SetupWithFakes();
            int StaffId = 1;

            //Act
            var result = await controller.GetStaffByID(StaffId);
            
            //Assert
            Assert.NotNull(result);
            var objResult = result as OkObjectResult;
            Assert.NotNull(objResult);
            var staff = objResult.Value;
            var staffModel = mapper.Map<StaffDomainModel>(staff);
            Assert.NotNull(staff);
            Assert.True(staffModel.StaffID == staff);
            Assert.True(staffModel.StaffFirstName == staff.StaffFirstName);
            Assert.True(staffModel.StaffLastName == staff.StaffLastName);
            Assert.True(staffModel.StaffEmailAddress == staff.StaffEmailAddress);
        }

        [Fact]
        public async void GetStaffById0_False()
        {
            //Arrange
            SetupWithFakes();
            int StaffId = 0;

            //Act
            var result = await controller.GetStaffByID(StaffId);

            //Assert
            Assert.NotNull(result);
            var objResult = result as NotFoundObjectResult;
            Assert.NotNull(objResult);
        }

        [Fact]
        public async void GetStaffByIncorrectId_False()
        {
            //Arrange
            SetupWithFakes();
            int StaffId = 100;

            //Act
            var result = await controller.GetStaffByID(StaffId);

            //Assert
            Assert.NotNull(result);
            var objResult = result as NotFoundObjectResult;
            Assert.NotNull(objResult);
        }

        [Fact]
        public async void CreateStaff_True()
        {
            //Arrange
            SetupWithFakes();
            var staff = staffModel;
            var staffMember = mapper.Map<StaffCreateDTO>(staff);
            //Act
            var result = await controller.CreateStaffMember(staffMember);

            //Assert
            Assert.NotNull(result);
            var objResult = result as BadRequestObjectResult;
            Assert.Null(objResult);
            var checkStaff = new StaffDomainModel();
            foreach(var item in fakeRepo._staffList)
            {
                if(item.StaffID == staffMember.StaffID)
                {
                    checkStaff = item;
                }
            }
            Assert.True(staffMember.StaffID == checkStaff.StaffID);
            Assert.True(staffMember.StaffFirstName == checkStaff.StaffFirstName);
            Assert.True(staffMember.StaffLastName == checkStaff.StaffLastName);
            Assert.True(staffMember.StaffEmailAddress == checkStaff.StaffEmailAddress);
        }

        [Fact]
        public async void CreateStaff_Null()
        {
            //Arrange
            SetupWithFakes();

            //Act
            var result = await controller.CreateStaffMember(null);

            //Assert
            Assert.NotNull(result);
            var objResult = result as BadRequestObjectResult;
            Assert.Null(objResult);
        }

        [Fact]
        public async void UpdateStaff_True()
        {
            //Arrange
            SetupWithFakes();
            var staff = listStaffModel[0];
            staff.StaffFirstName = "Testing";
            var staffMember = mapper.Map<StaffUpdateDTO>(staff);

            //Act
            var result = await controller.UpdateStaffMemeber(staffMember, 1);

            //Assert
            Assert.NotNull(result);
            var objResult = result as OkObjectResult;
            Assert.NotNull(objResult);            
            var staffModel = mapper.Map<StaffDomainModel>(staff);
            Assert.NotNull(staff);
            Assert.True(staffModel.StaffID == staff.StaffID);
            Assert.True(staffModel.StaffFirstName == staff.StaffFirstName);
            Assert.True(staffModel.StaffLastName == staff.StaffLastName);
            Assert.True(staffModel.StaffEmailAddress == staff.StaffEmailAddress);

        }

        [Fact]
        public async void UpdateStaff0_False()
        {
            //Arrange
            SetupWithFakes();
            var staff = staffModel;
            var staffMember = mapper.Map<StaffUpdateDTO>(staff);

            //Act
            var result = await controller.UpdateStaffMemeber(staffMember, 0);

            //Assert
            Assert.NotNull(result);
            var objResult = result as BadRequestObjectResult;
            Assert.Null(objResult);
        }

        [Fact]
        public async void UpdateStaffNonExistId_False()
        {
            //Arrange
            SetupWithFakes();
            var staff = staffModel;
            var staffMember = mapper.Map<StaffUpdateDTO>(staff);

            //Act
            var result = await controller.UpdateStaffMemeber(staffMember, 100);

            //Assert
            Assert.NotNull(result);
            var objResult = result as BadRequestObjectResult;
            Assert.Null(objResult);
        }
        #endregion
    }
}
