using AutoMapper;
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

        //private void SetMockReviewRepo()
        //{
        //    mockRepo = new Mock<IStaffRepository>(MockBehavior.Strict);
            
        //    mockRepo.Setup(repo => repo.GetStaffByIDAsnyc(It.IsAny<int>()))
        //       .ReturnsAsync(new StaffDomainModel()).Verifiable();
            
        //    mockRepo.Setup(repo => repo.GetReviewsByCustomerId(It.IsAny<int>(), It.IsAny<bool>()))
        //      .ReturnsAsync(new List<ReviewModel>()).Verifiable();
            
        //    mockRepo.Setup(repo => repo.GetReview(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
        //      .ReturnsAsync(reviewExists ? new ReviewModel() : null).Verifiable();
            
        //    mockRepo.Setup(repo => repo.HideReview(It.IsAny<int>(), It.IsAny<int>()))
        //      .ReturnsAsync(hideReviewSucceeds && reviewExists).Verifiable();
        //}

        //private void DefaultSetup()
        //{
        //    SetupReviewModel();
        //    SetupCustomerRepoModel();
        //    SetMapper();
        //    SetLogger();
        //}

        //private void SetupWithFakes()
        //{
        //    DefaultSetup();
        //    SetFakeRepo();
        //    controller = new StaffReviewController(logger, fakeRepo, mapper);
        //}

        //private void SetupWithMocks()
        //{
        //    DefaultSetup();
        //    SetMockReviewRepo();
        //    controller = new StaffReviewController(logger, mockRepo.Object, mapper);
        //}
    }
}
