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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Staff_xUnit_Tests
{
    public class StaffRepositoryTests
    {
        public StaffRepoModel staffRepoModel;
        public IMapper mapper;
        public IQueryable<StaffDomainModel> dbStaffs;
        public StaffDomainModel dbStaff;
        public Mock<DbSet<StaffDomainModel>> mockStaff;
        public Mock<StaffDbContext> mockDbContext;
        public SqlStaffRepository repo;
        public StaffRepoModel anonymisedStaff;

        private void SetUpStaffRepoModel()
        {
            staffRepoModel = new StaffRepoModel
            {
                StaffID = 99,
                StaffFirstName = "ABC",
                StaffLastName = "DEF",
                StaffEmailAddress = "ABCDEF@ThAmCo.com"
            };
        }

        private void SetUpDbStaff()
        {
            dbStaff = new StaffDomainModel
            {
                StaffID = 22,
                StaffFirstName = "Jacob",
                StaffLastName = "Jardine",
                StaffEmailAddress = "JacobJardine2@ThAmCo.com"
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
            mockDbContext = new Mock<StaffDbContext>();
            mockDbContext.Setup(m => m.StaffTable).Returns(mockStaff.Object);
        }

        private void DefaultSetup()
        {
            SetUpMapper();
            SetUpStaffRepoModel();
            SetUpStaffs();
            SetUpMockStaffs();
            SetupMockDbContext();
            repo = new SqlStaffRepository(mockDbContext.Object);
        }

        [Fact]
        public async Task CreatStaff_True()
        {
            //Arrange
            DefaultSetup();
            var staffModel = mapper.Map<StaffDomainModel>(staffRepoModel);
            //Act
            var result =  await repo.CreateStaff(staffModel);

            //Assert
            Assert.True(result);
            mockDbContext.Verify(m => m.StaffTable.Add(It.IsAny<StaffDomainModel>()), Times.Once());
            mockDbContext.Verify(m => m.StaffTable.Remove(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task CreateStaff_null()
        {
            //Arrange
            DefaultSetup();

            //Act

            var result = await repo.CreateStaff(null);

            //Assert
            Assert.False(result);
            mockDbContext.Verify(m => m.StaffTable.Add(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.StaffTable.Remove(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task CreateStaff_InvalidModel()
        {
            //Arrange
            DefaultSetup();
            var staffModel = mapper.Map<StaffDomainModel>(staffRepoModel);
            staffModel.StaffEmailAddress = string.Empty;

            //Act
            var result = await repo.CreateStaff(staffModel);

            //Assert
            Assert.False(result);
            mockDbContext.Verify(m => m.StaffTable.Add(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.StaffTable.Remove(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task UpdateStaff_True()
        {
            //Arange
            DefaultSetup();
            staffRepoModel.StaffID = dbStaff.StaffID;
            var staffModel = mapper.Map<StaffDomainModel>(staffRepoModel);

            //Act
            var result = await repo.UpdateStaff(staffModel);

            //Assert
            Assert.True(result);
            Assert.Equal(dbStaff.StaffID, staffRepoModel.StaffID);
            Assert.Equal(dbStaff.StaffFirstName, staffRepoModel.StaffFirstName);
            Assert.Equal(dbStaff.StaffLastName, staffRepoModel.StaffLastName);
            Assert.Equal(dbStaff.StaffEmailAddress, staffRepoModel.StaffEmailAddress);
            mockDbContext.Verify(m => m.StaffTable.Add(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.StaffTable.Remove(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateStaff_DoesntExist_False()
        {
            //Arange
            DefaultSetup();
            var staffModel = mapper.Map<StaffDomainModel>(staffRepoModel);

            //Act
            var result = await repo.UpdateStaff(staffModel);

            //Assert
            Assert.False(result);
            Assert.NotEqual(dbStaff.StaffID, staffRepoModel.StaffID);
            Assert.NotEqual(dbStaff.StaffFirstName, staffRepoModel.StaffFirstName);
            Assert.NotEqual(dbStaff.StaffLastName, staffRepoModel.StaffLastName);
            Assert.NotEqual(dbStaff.StaffEmailAddress, staffRepoModel.StaffEmailAddress);
            mockDbContext.Verify(m => m.StaffTable.Add(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.StaffTable.Remove(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task UpdateStaff_null()
        {
            //Arrange
            DefaultSetup();

            //Act
            var result = await repo.UpdateStaff(null);

            //Assert
            Assert.False(result);
            Assert.NotEqual(dbStaff.StaffID, staffRepoModel.StaffID);
            Assert.NotEqual(dbStaff.StaffFirstName, staffRepoModel.StaffFirstName);
            Assert.NotEqual(dbStaff.StaffLastName, staffRepoModel.StaffLastName);
            Assert.NotEqual(dbStaff.StaffEmailAddress, staffRepoModel.StaffEmailAddress);
            mockDbContext.Verify(m => m.StaffTable.Add(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.StaffTable.Remove(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task DeleteStaff_True()
        {
            //Arrange
            DefaultSetup();

            //Act
            var result = await repo.DeleteStaff(dbStaff.StaffID);

            //Assert
            Assert.True(result);
            mockDbContext.Verify(m => m.StaffTable.Add(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.StaffTable.Remove(dbStaff), Times.Once());
            mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task DeleteStaff_DoesntExist_False()
        {
            //Arrange
            DefaultSetup();
            int ID = 1;
            //Act
            var result = await repo.DeleteStaff(ID);

            //Assert
            Assert.False(result);
            mockDbContext.Verify(m => m.StaffTable.Add(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.StaffTable.Remove(It.IsAny<StaffDomainModel>()), Times.Never());
            mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
