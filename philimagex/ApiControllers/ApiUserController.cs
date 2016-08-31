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

        [Authorize]
        [HttpGet]
        [Route("api/user/list")]
        public List<Models.MstUser> listUser()
        {
            var users = from d in db.MstUsers
                        where d.UserTypeId != Convert.ToInt32(3)
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

        [Authorize]
        [HttpPost]
        [Route("api/user/add")]
        public HttpResponseMessage addUser(Models.MstUser user)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstUser newUser = new Data.MstUser();

                newUser.UserName = user.UserName;
                newUser.FullName = user.FullName;
                newUser.Address = user.Address;
                newUser.ContactNumber = user.ContactNumber;
                newUser.UserTypeId = user.UserTypeId;
                newUser.AspNetUserId = user.AspNetUserId;

                db.MstUsers.InsertOnSubmit(newUser);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // update user rate
        [Authorize]
        [HttpPut]
        [Route("api/user/update/{id}")]
        public HttpResponseMessage updateUser(String id, Models.MstUser user)
        {
            try
            {
                var users = from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d;
                if (users.Any())
                {
                    var updateUser = users.FirstOrDefault();

                    //updateUser.UserName = user.UserName;
                    updateUser.FullName = user.FullName;
                    //updateUser.Address = user.Address;
                    //updateUser.ContactNumber = user.ContactNumber;
                    updateUser.UserTypeId = user.UserTypeId;
                    //updateUser.AspNetUserId = user.AspNetUserId;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // delete user rate
        [Authorize]
        [HttpDelete]
        [Route("api/user/delete/{id}")]
        public HttpResponseMessage deleteUser(String id)
        {
            try
            {
                var users = from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d;
                if (users.Any())
                {
                    db.MstUsers.DeleteOnSubmit(users.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
