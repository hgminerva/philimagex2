using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace philimagex.Models
{
    public class TrnProcedureResult
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 ProcedureId { get; set; }
        public String Procedure { get; set; }
        public Int32 ModalityProcedureId { get; set; }
        public String ModalityProcedure { get; set; }
        public String Result { get; set; }
        public Int32 DoctorId { get; set; }
        public String Doctor { get; set; }
        public String DoctorDateTime { get; set; }
    }
}