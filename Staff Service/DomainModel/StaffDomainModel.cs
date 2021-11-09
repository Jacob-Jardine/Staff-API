using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Staff_Service.DomainModel
{
    public class StaffDomainModel
    {
        [Key]
        public int StaffID { get; set; }
        [Required]
        public string StaffFirstName { get; set; }
        [Required]
        public string StaffLastName { get; set; }
        [Required]
        public string StaffEmailAddress { get; set; }
    }
}
