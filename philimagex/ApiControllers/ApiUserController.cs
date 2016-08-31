using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace philimagex.ApiControllers
{
    public class ApiUserController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // list user
        [Authorize]
        [HttpGet]
        [Route("api/user/listByUserTypeId/{userTypeId}")]
        public List<Models.MstUser> listUserByUserTypeId(String userTypeId)
        {
            var users = from d in db.MstUsers
                        where d.UserTypeId == Convert.ToInt32(userTypeId)
                        select new Models.MstUser
                        {
                            Id = d.Id,
                            UserName = d.UserName,
                            FullName = d.FullName,
                            Address = d.Address,
                            ContactNumber = d.ContactNumber,
                            UserTypeId = d.UserTypeId,
                            UserType = d.MstUserType.UserType,
                            AspNetUserId = d.AspNetUserId
                        };

            return users.ToList();
        }

        // user by facility id
        [Authorize]
        [HttpGet]
        [Route("api/user/getByFacilityId/{facilityId}")]
        public Models.MstUser getUserByFacilityId(String facilityId)
        {
            var user = from d in db.MstUsers
                       where d.Id == Convert.ToInt32(facilityId)
                       select new Models.MstUser
                       {
                           Id = d.Id,
                           UserName = d.UserName,
                       };

            return (Models.MstUser)user.FirstOrDefault();
        }
    }
}
