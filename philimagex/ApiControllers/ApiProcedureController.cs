using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace philimagex.ApiControllers
{
    public class ApiProcedureController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // list procedure
        [Authorize]
        [HttpGet]
        [Route("api/procedure/list")]
        public List<Models.TrnProcedure> listProcedure()
        {
            var procedures = from d in db.TrnProcedures
                             select new Models.TrnProcedure
                            {
                                Id = d.Id,
                                TransactionNumber = d.TransactionNumber,
                                TransactionDateTime = d.TransactionDateTime.ToShortDateString(),
                                TransactionTime = d.TransactionDateTime.ToShortTimeString(),
                                DICOMFileName = d.DICOMFileName,
                                PatientName = d.PatientName,
                                Gender = d.Gender,
                                DateOfBirth = d.DateOfBirth.ToShortDateString(),
                                Age = d.Age,
                                Particulars = d.Particulars,
                                ModalityId = d.ModalityId,
                                Modality = d.MstModality.Modality,
                                BodyPartId = d.BodyPartId,
                                BodyPart = d.MstBodyPart.BodyPart,
                                UserId = d.UserId,
                                User = d.MstUser.FullName
                            };

            return procedures.ToList();
        }

        // get procedure by Id
        [Authorize]
        [HttpGet]
        [Route("api/procedure/getById/{id}")]
        public Models.TrnProcedure getProcedureById(String id)
        {
            var procedures = from d in db.TrnProcedures
                             where d.Id == Convert.ToInt32(id)
                             select new Models.TrnProcedure
                             {
                                 Id = d.Id,
                                 TransactionNumber = d.TransactionNumber,
                                 TransactionDateTime = d.TransactionDateTime.ToShortDateString(),
                                 TransactionTime = d.TransactionDateTime.ToShortTimeString(),
                                 DICOMFileName = d.DICOMFileName,
                                 PatientName = d.PatientName,
                                 Gender = d.Gender,
                                 DateOfBirth = d.DateOfBirth.ToShortDateString(),
                                 Age = d.Age,
                                 Particulars = d.Particulars,
                                 ModalityId = d.ModalityId,
                                 Modality = d.MstModality.Modality,
                                 BodyPartId = d.BodyPartId,
                                 BodyPart = d.MstBodyPart.BodyPart,
                                 UserId = d.UserId,
                                 User = d.MstUser.FullName
                             };

            return (Models.TrnProcedure)procedures.FirstOrDefault();
        }

        // add procedure
        [Authorize]
        [HttpPost]
        [Route("api/procedure/add")]
        public Int32 addProcedure(Models.TrnProcedure procedure)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.TrnProcedure newProcedure = new Data.TrnProcedure();
                newProcedure.TransactionNumber = "NA";
                newProcedure.TransactionDateTime = DateTime.Now;
                newProcedure.DICOMFileName = "NA";
                newProcedure.PatientName = "NA";
                newProcedure.Gender = "M";
                newProcedure.DateOfBirth = DateTime.Now;
                newProcedure.Age = 0;
                newProcedure.Particulars = "NA";
                newProcedure.ModalityId = (from d in db.MstModalities select d.Id).FirstOrDefault();
                newProcedure.BodyPartId = (from d in db.MstBodyParts select d.Id).FirstOrDefault();
                newProcedure.UserId = userId;

                db.TrnProcedures.InsertOnSubmit(newProcedure);
                db.SubmitChanges();

                return newProcedure.Id;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // update procedure
        [Authorize]
        [HttpPut]
        [Route("api/procedure/update/{id}")]
        public HttpResponseMessage updateProcedure(String id, Models.TrnProcedure procedure)
        {
            try
            {
                var procedures = from d in db.TrnProcedures where d.Id == Convert.ToInt32(id) select d;
                if (procedures.Any())
                {
                    var userId = (from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();
                    var updateProcedure = procedures.FirstOrDefault();

                    updateProcedure.TransactionNumber = procedure.TransactionNumber;
                    updateProcedure.DICOMFileName = procedure.DICOMFileName;
                    updateProcedure.PatientName = procedure.PatientName;
                    updateProcedure.Gender = procedure.Gender;
                    updateProcedure.DateOfBirth = Convert.ToDateTime(procedure.DateOfBirth);
                    updateProcedure.Age = procedure.Age;
                    updateProcedure.Particulars = procedure.Particulars;
                    updateProcedure.ModalityId = procedure.ModalityId;
                    updateProcedure.BodyPartId = procedure.BodyPartId;
                    updateProcedure.UserId = userId;

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

        // delete procedure
        [Authorize]
        [HttpDelete]
        [Route("api/procedure/delete/{id}")]
        public HttpResponseMessage deleteProcedure(String id)
        {
            try
            {
                var procedures = from d in db.TrnProcedures where d.Id == Convert.ToInt32(id) select d;
                if (procedures.Any())
                {
                    db.TrnProcedures.DeleteOnSubmit(procedures.First());
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


        // list procedure by dates
        [Authorize]
        [HttpGet]
        [Route("api/procedure/listByDateRange/{startDate}/{endDate}")]
        public List<Models.TrnProcedure> listProcedureByDateRange(String startDate, String endDate)
        {
            var procedures = from d in db.TrnProcedures
                             where d.TransactionDateTime >= Convert.ToDateTime(startDate)
                             && d.TransactionDateTime <= Convert.ToDateTime(endDate)
                             select new Models.TrnProcedure
                             {
                                 Id = d.Id,
                                 TransactionNumber = d.TransactionNumber,
                                 TransactionDateTime = d.TransactionDateTime.ToShortDateString(),
                                 TransactionTime = d.TransactionDateTime.ToShortTimeString(),
                                 DICOMFileName = d.DICOMFileName,
                                 PatientName = d.PatientName,
                                 Gender = d.Gender,
                                 DateOfBirth = d.DateOfBirth.ToShortDateString(),
                                 Age = d.Age,
                                 Particulars = d.Particulars,
                                 ModalityId = d.ModalityId,
                                 Modality = d.MstModality.Modality,
                                 BodyPartId = d.BodyPartId,
                                 BodyPart = d.MstBodyPart.BodyPart,
                                 UserId = d.UserId,
                                 User = d.MstUser.FullName,
                                 Facility = d.MstUser.MstUserType.UserType
                             };

            return procedures.ToList();
        }

    }
}
