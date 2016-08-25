using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace philimagex.Models
{
    public class MstUserRate
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public String UserFullName { get; set; }
        public Int32 ModalityProcedureId { get; set; }
        public String ModalityProcedure { get; set; }
        public String ModalityProcedureCode { get; set; }
        public Decimal FacilityRate { get; set; }
        public Decimal DoctorRate { get; set; }
        public Decimal ImageRate { get; set; }
        public String Remarks { get; set; }
    }
}