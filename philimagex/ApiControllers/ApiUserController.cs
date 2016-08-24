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
        [Route("api/user/list")]
        public List<Models.MstUser> listUser()
        {
            var users = from d in db.MstUsers
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
    }
}
