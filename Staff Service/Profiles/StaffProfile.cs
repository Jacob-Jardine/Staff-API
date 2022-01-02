using AutoMapper;
using Staff_Service.DomainModel;
using Staff_Service.DTOs;
using Staff_Service.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.Profiles
{
    public class StaffProfile : Profile
    {
        public StaffProfile()
        {
            CreateMap<StaffDomainModel, StaffReadDTO>();
            CreateMap<StaffCreateDTO, StaffDomainModel>();
            CreateMap<StaffDomainModel, StaffUpdateDTO>();
            CreateMap<StaffUpdateDTO, StaffDomainModel>();
            CreateMap<StaffDeleteDTO, StaffDomainModel>();
            CreateMap<StaffRepoModel, StaffDomainModel>();
        }
    }
}
