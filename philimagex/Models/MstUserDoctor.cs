using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace philimagex.Models
{
    public class MstUserDoctor
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public String User { get; set; }
        public String UserFacility { get; set; }
        public Int32 DoctorId { get; set; }
        public String Doctor { get; set; }
    }
}