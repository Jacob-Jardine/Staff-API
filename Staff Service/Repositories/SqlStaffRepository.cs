using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using Staff_Service.Context;
using Staff_Service.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.Repositories
{
    public class SqlStaffRepository : IStaffRepository
    {
        private readonly StaffDbContext _dbContext;
        public SqlStaffRepository(StaffDbContext dbConext) 
        {
            _dbContext = dbConext;
        }

        public async Task<IEnumerable<StaffDomainModel>> GetAllStaffAsync() 
        {
            return await _dbContext.StaffTable.ToListAsync();
        }

        public async Task<StaffDomainModel> GetStaffByIDAsnyc(int? ID) 
        {
            return await _dbContext.StaffTable.FirstOrDefaultAsync(x => x.StaffID == ID);
        }

        public async Task<bool> CreateStaff(StaffDomainModel staffDomainModel) 
        {
            if(staffDomainModel == null)
            {
                return false;
            }
            staffDomainModel.StaffEmailAddress.ToLower();

            var emailCheck = _dbContext.StaffTable.Any(x => x.StaffEmailAddress == staffDomainModel.StaffEmailAddress);
            if (emailCheck == false)
            {
                _dbContext.StaffTable.Add(staffDomainModel);
                return true;
            }

            return false;
        }


        public async Task<bool> DeleteStaff(int ID)
        {
            if(ID == null)
            {
                return false;
            }
            StaffDomainModel StaffDomainModel = GetStaffByIDAsnyc(ID).Result;
            _dbContext.StaffTable.Remove(StaffDomainModel);
            return true;
        }

        public async Task<bool> UpdateStaff(StaffDomainModel staffDomainModel)
        {
            if(staffDomainModel == null)
            {
                return false;
            }
            _dbContext.StaffTable.Update(staffDomainModel);
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
