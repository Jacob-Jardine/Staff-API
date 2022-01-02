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
        private readonly StagingContext _stagingContext;
        private readonly ProductionContext _productionContext;

        public SqlStaffRepository(ProductionContext productionContext)
        {
            _productionContext = productionContext;
        }

        public SqlStaffRepository(StagingContext stagingContext, ProductionContext productionContext) 
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging") 
            {
                _stagingContext = stagingContext;
            }
            else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") 
            {
                _productionContext = productionContext;
            }
        }

        public async Task<IEnumerable<StaffDomainModel>> GetAllStaffAsync() 
        {
            return (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging") ? await _stagingContext.staging_db.ToListAsync() : await _productionContext.production_db.ToListAsync();
        }

        public async Task<StaffDomainModel> GetStaffByIDAsnyc(int? ID) 
        {
            return (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging") ? await _stagingContext.staging_db.FirstOrDefaultAsync(x => x.StaffID == ID) : await _productionContext.production_db.FirstOrDefaultAsync(x => x.StaffID == ID);
        }

        public async Task<bool> CreateStaff(StaffDomainModel staffDomainModel) 
        {
            if(staffDomainModel == null)
            {
                return false;
            }
            staffDomainModel.StaffEmailAddress.ToLower();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging")
            {
                var emailCheck = _stagingContext.staging_db.Any(x => x.StaffEmailAddress == staffDomainModel.StaffEmailAddress);
                if (emailCheck == false)
                {
                    _stagingContext.staging_db.Add(staffDomainModel);
                    return true;
                }
                return false;
            }
            else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") 
            {
                var emailCheck = _productionContext.production_db.Any(x => x.StaffEmailAddress == staffDomainModel.StaffEmailAddress);
                if (emailCheck == false)
                {
                    _productionContext.production_db.Add(staffDomainModel);
                    return true;
                }
                return false;
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
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging")
            {
                _stagingContext.staging_db.Remove(StaffDomainModel);
                return true;
            }
            else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                _productionContext.production_db.Remove(StaffDomainModel);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateStaff(StaffDomainModel staffDomainModel)
        {
            if(staffDomainModel == null)
            {
                return false;
            }
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging")
            {
                _stagingContext.staging_db.Update(staffDomainModel);
                return true;
            }
            else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                _productionContext.production_db.Update(staffDomainModel);
                return true;
            }
            return false;
        }

        public async Task SaveChangesAsync()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging")
            {
                await _stagingContext.SaveChangesAsync();
            }
            else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                await _productionContext.SaveChangesAsync();
            }     
        }
    }
}
