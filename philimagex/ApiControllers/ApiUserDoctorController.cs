using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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

        // list doctor by User
        [Authorize]
        [HttpGet]
        [Route("api/userDoctor/listByUserId")]
        public List<Models.MstUserDoctor> listUserDoctorByUserId()
        {
            var userId = (from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

            var userDoctors = from d in db.MstUserDoctors
                              where d.UserId == userId
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
