using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Staff_Service.DomainModel;
using Staff_Service.DTOs;
using Staff_Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _staffRepository;
        private IMapper _mapper;

        public StaffController(IStaffRepository staffRepository, IMapper mapper) 
        {
            _staffRepository = staffRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAllStaff")]
        public async Task<ActionResult<IEnumerable<StaffReadDTO>>> GetAllStaff()
        {
            try 
            {
                var getAllStaff = await _staffRepository.GetAllStaffAsync();
                return Ok(_mapper.Map<IEnumerable<StaffReadDTO>>(getAllStaff));
            }
            catch 
            {
                return NotFound();
            }
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<StaffReadDTO>> GetStaffByID(int ID)
        {
            try 
            {
                var getStaffByID = await _staffRepository.GetStaffByIDAsnyc(ID);
                if (getStaffByID != null) 
                {
                    return Ok(_mapper.Map<StaffReadDTO>(getStaffByID));
                }
                else 
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost("CreateStaff")]
        public async Task<ActionResult> CreateStaffMember([FromBody] StaffCreateDTO staffCreateDTO) 
        {
            try 
            {
                var staffModel = _mapper.Map<StaffDomainModel>(staffCreateDTO);
                StaffDomainModel newStaffDomainModel = _staffRepository.CreateStaff(staffModel);
                await _staffRepository.SaveChangesAsync();
                return CreatedAtAction(nameof(GetStaffByID), new { ID = newStaffDomainModel.StaffID }, newStaffDomainModel);
            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpPut("update/{ID}")]
        public async Task<ActionResult> UpdateStaffMemeber([FromBody] StaffUpdateDTO staffUpdateDTO, int ID) 
        {
            try 
            {
                var staffModel = await _staffRepository.GetStaffByIDAsnyc(ID);
                if (staffModel == null) 
                {
                    return NotFound();
                }
                else
                {
                    var updateStaff = _mapper.Map<StaffUpdateDTO>(staffUpdateDTO);
                    updateStaff.StaffID = staffModel.StaffID;

                    if (!TryValidateModel(updateStaff)) 
                    {
                        return ValidationProblem(ModelState);
                    }
                    else
                    {
                        _mapper.Map(updateStaff, staffModel);
                        _staffRepository.UpdateStaff(staffModel);
                        await _staffRepository.SaveChangesAsync();
                        return Ok();
                    }
                }
            }
            catch 
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteStaffByID(int ID) 
        {
            try
            {
                var staffModel = await _staffRepository.GetStaffByIDAsnyc(ID);
                if (staffModel == null)
                {
                    return NotFound();
                }
                else
                {
                    _staffRepository.DeleteStaff(staffModel.StaffID);
                    await _staffRepository.SaveChangesAsync();
                    return Ok();
                }
            }
            catch
            {
                return BadRequest();
            }        
        }
    }
}
