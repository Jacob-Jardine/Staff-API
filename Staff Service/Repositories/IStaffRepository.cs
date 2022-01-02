using Staff_Service.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.Repositories
{
    public interface IStaffRepository
    {
        public Task<IEnumerable<StaffDomainModel>> GetAllStaffAsync();
        public Task<StaffDomainModel> GetStaffByIDAsnyc(int? ID);
        public Task<bool> CreateStaff(StaffDomainModel staffDomainModel);
        public Task<bool> UpdateStaff(StaffDomainModel staffDomainModel);
        public Task<bool> DeleteStaff(int ID);
        public Task SaveChangesAsync();
    }
}
