using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace philimagex.ApiControllers
{
    public class ApiModalityController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // modality list
        [Authorize]
        [HttpGet]
        [Route("api/modality/list")]
        public List<Models.MstModality> listModality()
        {
            var modalities = from d in db.MstModalities
                             select new Models.MstModality
                             {
                                 Id = d.Id,
                                 Modality = d.Modality
                             };

            return modalities.ToList();
        }
    }
}
