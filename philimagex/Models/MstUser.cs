using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace philimagex.Models
{
    public class MstUser
    {
        [Key]
        public Int32 Id { get; set; }
        public String UserName { get; set; }
        public String FullName { get; set; }
        public String Address { get; set; }
        public String ContactNumber { get; set; }
        public Int32 UserTypeId { get; set; }
        public String UserType { get; set; }
        public Int32 AspNetUserId { get; set; }
    }
}