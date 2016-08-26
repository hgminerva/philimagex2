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
        public String DoctorTime { get; set; }
        public String Facility { get; set; }
        public String TransactionNumber { get; set; }
        public String TransactionDateTime { get; set; }
        public String Patient { get; set; }
        public String Modality { get; set; }
        public Decimal? FacilityRate { get; set; }
        public Decimal? DoctorRate { get; set; }
        public Decimal? ImageRate { get; set; }
    }
}