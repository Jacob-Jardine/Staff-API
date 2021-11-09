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
        public StaffDomainModel CreateStaff(StaffDomainModel staffDomainModel);
        public void UpdateStaff(StaffDomainModel staffDomainModel);
        public void DeleteStaff(int? ID);
        public Task SaveChangesAsync();
    }
}
