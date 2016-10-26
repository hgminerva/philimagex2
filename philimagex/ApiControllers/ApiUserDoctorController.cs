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
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

            if (user.Any())
            {
                if (user.FirstOrDefault().UserTypeId == 2) {
                    var userDoctors1 = from d in db.MstUsers
                                       where d.Id == user.FirstOrDefault().Id
                                       select new Models.MstUserDoctor
                                       {
                                           Id = d.Id,
                                           UserId = d.Id,
                                           User = d.FullName,
                                           DoctorId = d.Id,
                                           Doctor = d.FullName,
                                       };
                    return userDoctors1.ToList();
                } else {
                    var userDoctors2 = from d in db.MstUserDoctors
                                      where d.UserId == user.FirstOrDefault().Id
                                      select new Models.MstUserDoctor
                                      {
                                            Id = d.Id,
                                            UserId = d.UserId,
                                            User = d.MstUser.FullName,
                                            DoctorId = d.DoctorId,
                                            Doctor = d.MstUser1.FullName,
                                      };
                    return userDoctors2.ToList();
                }
            } else {
                return new List<Models.MstUserDoctor>();
            }
        }
    }
}
