using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.DTOs
{
    public class StaffReadDTO
    {
        public int StaffID { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string StaffEmailAddress { get; set; }
    }
}
