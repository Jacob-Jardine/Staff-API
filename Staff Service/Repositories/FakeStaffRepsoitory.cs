﻿using Staff_Service.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.Repositories
{
    public class FakeStaffRepsoitory : IStaffRepository
    {
        private readonly List<StaffDomainModel> _staffList;

        public FakeStaffRepsoitory() 
        {
            _staffList = new List<StaffDomainModel>()
            {
                new StaffDomainModel() {StaffID = 1, StaffFirstName = "Jacob", StaffLastName = "Jardine", StaffEmailAddress = "Jacob.Jardine@ThAmCo.co.uk" },
                new StaffDomainModel() {StaffID = 2, StaffFirstName = "Ben", StaffLastName = "Souch", StaffEmailAddress = "Ben.Souch@ThAmCo.co.uk"},
                new StaffDomainModel() {StaffID = 3, StaffFirstName = "Joseph", StaffLastName = "Stavers", StaffEmailAddress = "Joseph.Stavers@ThAmCo.co.uk"},
                new StaffDomainModel() {StaffID = 4, StaffFirstName = "Teddy", StaffLastName = "Teasdale", StaffEmailAddress = "Teddy.Teasdale@ThAmCo.co.uk"},
                new StaffDomainModel() {StaffID = 5, StaffFirstName = "Cristian", StaffLastName = "Tudor", StaffEmailAddress = "Cristian.Tudor@ThAmCo.co.uk"}
            };
        }

        public Task<IEnumerable<StaffDomainModel>> GetAllStaffAsync() => Task.FromResult(_staffList.AsEnumerable());

        public Task<StaffDomainModel> GetStaffByIDAsnyc(int? ID) => Task.FromResult(_staffList.FirstOrDefault(x => x.StaffID == ID));

        public StaffDomainModel CreateStaff(StaffDomainModel staffDomainModel)
        {
            int newStaffID = GetStaffID();
            staffDomainModel.StaffID = newStaffID;
            _staffList.Add(staffDomainModel);
            return staffDomainModel;
        }

        public void UpdateStaff(StaffDomainModel staffDomainModel) 
        {
            var oldStaffDomainModel = GetStaffByIDAsnyc(staffDomainModel.StaffID);
            _staffList.RemoveAll(x => x.StaffID == oldStaffDomainModel.Result.StaffID);    
            _staffList.Add(staffDomainModel);
        }

        public void DeleteStaff(int? ID) 
        {
            var deleteStaffDomainModel = GetStaffByIDAsnyc(ID);
            _staffList.RemoveAll(x => x.StaffID == deleteStaffDomainModel.Result.StaffID);
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        private int GetStaffID()
        {
            return (_staffList == null && _staffList.Count() == 0) ? 1 : _staffList.Max(x => x.StaffID) + 1;
        }
    }
}
