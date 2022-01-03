using AutoMapper;
using Microsoft.AspNetCore.Http;
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
                StaffID = 1,
                StaffFirstName = "Jacob",
                StaffLastName = "Jardine",
                StaffEmailAddress = "JacobJardine@ThAmCo.com"
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
            SetUpController(controller);
        }

        private void SetupWithFakes()
        {
            DefaultSetup();
            SetFakeRepo();
            controller = new StaffController(fakeRepo, mapper, logger, _memoryCache); 
        }

        private void SetupWithMocks()
        {
            DefaultSetup();
            SetMockReviewRepo();
            controller = new StaffController(mockRepo.Object, mapper, logger, _memoryCache);
        }
    }
}
