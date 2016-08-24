using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace philimagex.Models
{
    public class MstBodyPart
    {
        [Key]
        public Int32 Id { get; set; }
        public String BodyPart { get; set; }
    }
}