using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace philimagex.Models
{
    public class TrnProcedure
    {
        [Key]
        public Int32 Id { get; set; }
        public String TransactionNumber { get; set; }
        public String TransactionDateTime { get; set; }
        public String DICOMFileName { get; set; }
        public String PatientName { get; set; }
        public String Gender { get; set; }
        public String DateOfBirth { get; set; }
        public String Age { get; set; }
        public String Particulars { get; set; }
        public Int32 ModalityId { get; set; }
        public String Modality { get; set; }
        public Int32 BodyPartId { get; set; }
        public String BodyPart { get; set; }
        public Int32 UserId { get; set; }
        public String UserId { get; set; }
    }
}