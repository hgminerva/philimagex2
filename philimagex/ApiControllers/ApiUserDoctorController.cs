using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace philimagex.ApiControllers
{
    public class ApiUserDoctorController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // list user doctor
        [Authorize]
        [HttpGet]
        [Route("api/userDoctor/list")]
        public List<Models.MstUserDoctor> listUserDoctor()
        {
            var userDoctors = from d in db.MstUserDoctors
                              select new Models.MstUserDoctor
                              {
                                  Id = d.Id,
                                  UserId = d.UserId,
                                  User = d.MstUser.FullName,
                                  DoctorId = d.DoctorId,
                                  Doctor = d.MstUser1.FullName,
                              };

            return userDoctors.ToList();
        }
    }
}
