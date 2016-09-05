using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace philimagex.ApiControllers
{
    public class ApiUserRateController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // body user rate
        [Authorize]
        [HttpGet]
        [Route("api/userRate/list/{facilityId}")]
        public List<Models.MstUserRate> listUserRate(String facilityId)
        {
            var userRates = from d in db.MstUserRates
                            where d.UserId == Convert.ToInt32(facilityId)
                            select new Models.MstUserRate
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                Facility = d.MstUser.FullName,
                                ModalityProcedureId = d.ModalityProcedureId,
                                ModalityProcedure = d.MstModalityProcedure.ModalityProcedure,
                                ModalityProcedureCode = d.ModalityProcedureCode,
                                FacilityRate = d.FacilityRate,
                                DoctorRate = d.DoctorRate,
                                ImageRate = d.ImageRate,
                                Remarks = d.Remarks
                            };

            return userRates.ToList();
        }

        // add user rate
        [Authorize]
        [HttpPost]
        [Route("api/userRate/add")]
        public HttpResponseMessage addUserRate(Models.MstUserRate userRate)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstUserRate newUserRate = new Data.MstUserRate();
                //newUserRate.UserId = userId;
                //newUserRate.ModalityProcedureId = (from d in db.MstModalityProcedures select d.Id).FirstOrDefault();
                newUserRate.UserId = userRate.UserId;
                newUserRate.ModalityProcedureId = userRate.ModalityProcedureId;
                newUserRate.ModalityProcedureCode = userRate.ModalityProcedureCode;
                newUserRate.FacilityRate = userRate.FacilityRate;
                newUserRate.DoctorRate = userRate.DoctorRate;
                newUserRate.ImageRate = userRate.ImageRate;
                newUserRate.Remarks = userRate.Remarks;

                db.MstUserRates.InsertOnSubmit(newUserRate);
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
        [Route("api/userRate/update/{id}")]
        public HttpResponseMessage updateUserRate(String id, Models.MstUserRate userRate)
        {
            try
            {
                var userRates = from d in db.MstUserRates where d.Id == Convert.ToInt32(id) select d;
                if (userRates.Any())
                {
                    var updateUserRate = userRates.FirstOrDefault();
                    updateUserRate.UserId = userRate.UserId;
                    updateUserRate.ModalityProcedureId = userRate.ModalityProcedureId;
                    updateUserRate.ModalityProcedureCode = userRate.ModalityProcedureCode;
                    updateUserRate.FacilityRate = userRate.FacilityRate;
                    updateUserRate.DoctorRate = userRate.DoctorRate;
                    updateUserRate.ImageRate = userRate.ImageRate;
                    updateUserRate.Remarks = userRate.Remarks;

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
        [Route("api/userRate/delete/{id}")]
        public HttpResponseMessage deleteUserRate(String id)
        {
            try
            {
                var userRates = from d in db.MstUserRates where d.Id == Convert.ToInt32(id) select d;
                if (userRates.Any())
                {
                    db.MstUserRates.DeleteOnSubmit(userRates.First());
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
