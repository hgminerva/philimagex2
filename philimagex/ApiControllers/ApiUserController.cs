﻿using System;
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

        // user by username
        [Authorize]
        [HttpGet]
        [Route("api/user/getUserByUserName/{userName}")]
        public Models.MstUser getUserByUserName(String userName)
        {
            var user = from d in db.MstUsers
                       where d.UserName == userName
                       select new Models.MstUser
                       {
                           Id = d.Id,
                           UserName = d.UserName,
                           UserTypeId = d.UserTypeId,
                           UserType = d.MstUserType.UserType
                       };

            return (Models.MstUser)user.FirstOrDefault();
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

        // home page report
        [HttpGet]
        [Route("api/user/listHomePageStatus")]
        public List<String> listHomePageStatus()
        {
            List<String> status = new List<String>();

            var doctors = from d in db.MstUsers where d.UserTypeId == 2 select d;
            if (doctors.Any())
            {
                long numberOfDoctors = doctors.Count();
                status.Add("Total number of Physicians: " + numberOfDoctors.ToString("N0"));
            }

            var facilities = from d in db.MstUsers where d.UserTypeId == 1 select d;
            if (facilities.Any())
            {
                long numberOfFacilities = facilities.Count();
                status.Add("Total number of Hospitals / Healthcare Facilities: " + numberOfFacilities.ToString("N0"));
            }

            var studies = from d in db.TrnProcedures select d;
            if (studies.Any())
            {
                long numberOfStudies = studies.Count();
                status.Add("Total number of Studies / Procedures performed: " + numberOfStudies.ToString("N0"));
            }

            var results = from d in db.TrnProcedureResults select d;
            if (results.Any())
            {
                long numberOfResults= results.Count();
                status.Add("Total number of Results given: " + numberOfResults.ToString("N0"));
            }

            return status;
        }
    }
}
