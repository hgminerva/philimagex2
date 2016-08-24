using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace philimagex.Models
{
    public class MstUserType
    {
        [Key]
        public Int32 Id { get; set; }
        public String UserType { get; set; }
    }
}