using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace philimagex.ApiControllers
{
    public class ApiFacilityController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // facility list
        [Authorize]
        [HttpGet]
        [Route("api/facility/list")]
        public List<Models.MstUserDoctor> listBodyPart()
        {
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

            if (user.FirstOrDefault().UserTypeId == 1)
            {
                var users = from d in db.MstUsers
                            where d.AspNetUserId == User.Identity.GetUserId()
                            && d.UserTypeId == 1
                            && d.Id == user.FirstOrDefault().Id
                            select new Models.MstUserDoctor
                            {
                                Id = d.Id,
                                UserId = d.Id,
                                UserFacility = d.UserName
                            };

                return users.ToList();
            }
            else
            {
                var userFacility = from d in db.MstUserDoctors
                                   where d.DoctorId == user.FirstOrDefault().Id
                                   select new Models.MstUserDoctor
                                   {
                                       Id = d.Id,
                                       UserId = d.UserId,
                                       User = d.MstUser.FullName,
                                       UserFacility = d.MstUser.UserName,
                                       DoctorId = d.DoctorId,
                                       Doctor = d.MstUser1.FullName
                                   };

                return userFacility.ToList();
            }
        }
    }
}
