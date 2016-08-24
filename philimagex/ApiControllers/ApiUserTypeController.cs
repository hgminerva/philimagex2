using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace philimagex.ApiControllers
{
    public class ApiUserTypeController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // body user type
        [Authorize]
        [HttpGet]
        [Route("api/userType/list")]
        public List<Models.MstUserType> listUserType()
        {
            var userTypes = from d in db.MstUserTypes
                            select new Models.MstUserType
                            {
                                Id = d.Id,
                                UserType = d.UserType
                            };

            return userTypes.ToList();
        }

        // add user type
        [Authorize]
        [HttpPost]
        [Route("api/userType/add")]
        public HttpResponseMessage addUserType(Models.MstUserType userType)
        {
            try
            {
                Data.MstUserType newUserType = new Data.MstUserType();
                newUserType.UserType = userType.UserType;
                db.MstUserTypes.InsertOnSubmit(newUserType);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // update user type
        [Authorize]
        [HttpPut]
        [Route("api/userType/update/{id}")]
        public HttpResponseMessage updateUserType(String id, Models.MstUserType userType)
        {
            try
            {
                var userTypes = from d in db.MstUserTypes where d.Id == Convert.ToInt32(id) select d;
                if (userTypes.Any())
                {
                    var updateUserType = userTypes.FirstOrDefault();
                    updateUserType.UserType = userType.UserType;

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

        // delete user type
        [Authorize]
        [HttpDelete]
        [Route("api/userType/delete/{id}")]
        public HttpResponseMessage deleteUserType(String id)
        {
            try
            {
                var userTypes = from d in db.MstUserTypes where d.Id == Convert.ToInt32(id) select d;
                if (userTypes.Any())
                {
                    db.MstUserTypes.DeleteOnSubmit(userTypes.First());
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
