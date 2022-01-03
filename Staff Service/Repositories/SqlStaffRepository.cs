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

        public async Task<StaffDomainModel> GetStaffByIDAsnyc(int ID) 
        {
            return await _dbContext.StaffTable.FirstOrDefaultAsync(x => x.StaffID == ID);
        }

        public async Task<bool> CreateStaff(StaffDomainModel staffDomainModel) 
        {
            if(staffDomainModel == null || staffDomainModel.StaffID.Equals(null) || string.IsNullOrEmpty(staffDomainModel.StaffFirstName) || string.IsNullOrEmpty(staffDomainModel.StaffLastName) ||
                string.IsNullOrEmpty(staffDomainModel.StaffEmailAddress))
            {
                return false;
            }
            try
            {
                staffDomainModel.StaffEmailAddress.ToLower();

                var emailCheck = _dbContext.StaffTable.Any(x => x.StaffEmailAddress == staffDomainModel.StaffEmailAddress);
                if (emailCheck == false)
                {
                    _dbContext.StaffTable.Add(staffDomainModel);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }


        public async Task<bool> DeleteStaff(int ID)
        {
            try
            {
                var staff = _dbContext.StaffTable.SingleOrDefault(x => x.StaffID == ID);
                if(staff != null)
                {
                    _dbContext.StaffTable.Remove(staff);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }          
        }

        public async Task<bool> UpdateStaff(StaffDomainModel staffDomainModel)
        {
            if (staffDomainModel == null || staffDomainModel.StaffID.Equals(null) || string.IsNullOrEmpty(staffDomainModel.StaffFirstName) || string.IsNullOrEmpty(staffDomainModel.StaffLastName) ||
                string.IsNullOrEmpty(staffDomainModel.StaffEmailAddress))
            {
                return false;
            }
            var staff = _dbContext.StaffTable.FirstOrDefault(x => x.StaffID == staffDomainModel.StaffID);
            if(staff == null || staff.StaffID != staffDomainModel.StaffID)
            {
                return false;
            }
            try
            {
                _dbContext.StaffTable.Update(staffDomainModel);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
