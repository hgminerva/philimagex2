using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace philimagex.Models
{
    public class MstModalityProcedure
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 ModalityId { get; set; }
        public String Modality { get; set; }
        public String ModalityProcedure { get; set; }
        public String ModalityResultTemplate { get; set; }
    }
}