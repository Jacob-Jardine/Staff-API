using Microsoft.EntityFrameworkCore;
using Staff_Service.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.Repositories
{
    public class SqlStaffRepository : IStaffRepository
    {
        private readonly Context.dbContext _context;

        public SqlStaffRepository() { }

        public SqlStaffRepository(Context.dbContext context) 
        {
            _context = context;    
        }

        public async Task<IEnumerable<StaffDomainModel>> GetAllStaffAsync() => await _context._staff.ToListAsync();

        public async Task<StaffDomainModel> GetStaffByIDAsnyc(int? ID) => await _context._staff.FirstOrDefaultAsync(x => x.StaffID == ID);
        public StaffDomainModel CreateStaff(StaffDomainModel staffDomainModel) => _context._staff.Add(staffDomainModel).Entity;
        

        public void DeleteStaff(int ID)
        {
            StaffDomainModel StaffDomainModel = GetStaffByIDAsnyc(ID).Result;
            _context._staff.Remove(StaffDomainModel);
        }

        public void UpdateStaff(StaffDomainModel staffDomainModel)
        {
            _context._staff.Update(staffDomainModel);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
