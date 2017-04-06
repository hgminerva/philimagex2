using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace philimagex.ApiControllers
{
    public class ApiProcedureResultController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // pdf report
        private Controllers.PDFController pdf = new Controllers.PDFController();

        // body procedure result
        [Authorize]
        [HttpGet]
        [Route("api/procedureResult/listByProcedureId/{procedureId}")]
        public List<Models.TrnProcedureResult> listProcedureResultByProcedureId(String procedureId)
        {
            var procedureResults = from d in db.TrnProcedureResults
                                   where d.ProcedureId == Convert.ToInt32(procedureId)
                                   select new Models.TrnProcedureResult
                                    {
                                        Id = d.Id,
                                        ProcedureId = d.ProcedureId,
                                        ModalityProcedureId = d.ModalityProcedureId,
                                        ModalityProcedure = d.MstModalityProcedure.ModalityProcedure,
                                        Result = d.Result,
                                        DoctorId = d.DoctorId,
                                        Doctor = d.MstUser.UserName,
                                        DoctorDateTime = d.DoctorDateTime.ToShortDateString(),
                                        DoctorTime = d.DoctorDateTime.ToShortTimeString(),
                                    };

            return procedureResults.ToList();
        }

        // add procedure result
        [Authorize]
        [HttpPost]
        [Route("api/procedureResult/add")]
        public HttpResponseMessage addProcedureResult(Models.TrnProcedureResult procedureResult)
        {
            try
            {
                Data.TrnProcedureResult newProcedureResult = new Data.TrnProcedureResult();
                newProcedureResult.ProcedureId = procedureResult.ProcedureId;
                newProcedureResult.ModalityProcedureId = procedureResult.ModalityProcedureId;
                newProcedureResult.Result = procedureResult.Result;
                newProcedureResult.DoctorId = procedureResult.DoctorId;
                newProcedureResult.DoctorDateTime = DateTime.Now;

                db.TrnProcedureResults.InsertOnSubmit(newProcedureResult);
                db.SubmitChanges();

                if (newProcedureResult.TrnProcedure.UserId == 24) sendPDFEmail(newProcedureResult.Id); 

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // update procedure result
        [Authorize]
        [HttpPut]
        [Route("api/procedureResult/update/{id}")]
        public HttpResponseMessage updateProcedureResult(String id, Models.TrnProcedureResult procedureResult)
        {
            try
            {
                var procedureResults = from d in db.TrnProcedureResults where d.Id == Convert.ToInt32(id) select d;
                if (procedureResults.Any())
                {
                    var updaterPocedureResult = procedureResults.FirstOrDefault();
                    updaterPocedureResult.ProcedureId = procedureResult.ProcedureId;
                    updaterPocedureResult.ModalityProcedureId = procedureResult.ModalityProcedureId;
                    updaterPocedureResult.Result = procedureResult.Result;
                    updaterPocedureResult.DoctorId = procedureResult.DoctorId;
                    updaterPocedureResult.DoctorDateTime = DateTime.Now;

                    db.SubmitChanges();

                    if (updaterPocedureResult.TrnProcedure.UserId==24) sendPDFEmail(updaterPocedureResult.Id);

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

        // delete procedure result
        [Authorize]
        [HttpDelete]
        [Route("api/procedureResult/delete/{id}")]
        public HttpResponseMessage deleteProcedureResult(String id)
        {
            try
            {
                var procedureResults = from d in db.TrnProcedureResults where d.Id == Convert.ToInt32(id) select d;
                if (procedureResults.Any())
                {
                    db.TrnProcedureResults.DeleteOnSubmit(procedureResults.First());
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


        // body procedure result
        [Authorize]
        [HttpGet]
        [Route("api/procedureResult/listByDateRange/{startDate}/{endDate}/{facilityId}")]
        public List<Models.TrnProcedureResult> listProcedureResultByDateRange(String startDate, String endDate, String facilityId)
        {
            try
            {
                var procedureResults = from d in db.TrnProcedureResults
                                       where d.TrnProcedure.TransactionDateTime >= Convert.ToDateTime(startDate)
                                       && d.TrnProcedure.TransactionDateTime <= Convert.ToDateTime(endDate)
                                       && d.TrnProcedure.UserId == Convert.ToInt32(facilityId)
                                       select new Models.TrnProcedureResult
                                       {
                                           Id = d.Id,
                                           ProcedureId = d.ProcedureId,
                                           ModalityProcedureId = d.ModalityProcedureId,
                                           ModalityProcedure = d.MstModalityProcedure.ModalityProcedure,
                                           Result = d.Result,
                                           DoctorId = d.DoctorId,
                                           Doctor = d.MstUser.UserName,
                                           DoctorDateTime = d.DoctorDateTime.ToShortDateString(),
                                           DoctorTime = d.DoctorDateTime.ToShortTimeString(),
                                           Facility = d.TrnProcedure.MstUser.UserName,
                                           TransactionNumber = d.TrnProcedure.TransactionNumber,
                                           TransactionDateTime = d.TrnProcedure.TransactionDateTime.ToShortDateString(),
                                           Patient = d.TrnProcedure.PatientName,
                                           Modality = d.TrnProcedure.MstModality.Modality,
                                           FacilityRate = d.TrnProcedure.MstUser.MstUserRates.FirstOrDefault().FacilityRate,
                                           DoctorRate = d.MstUser.MstUserRates.FirstOrDefault().DoctorRate,
                                           ImageRate = d.MstModalityProcedure.MstUserRates.FirstOrDefault().ImageRate,
                                       };

                return procedureResults.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        private void sendPDFEmail(Int32 procedureResultId)
        {
            var pocedureResults = from d in db.TrnProcedureResults where d.Id == procedureResultId select d;

            if (pocedureResults.Any())
            {
                var procedureResult = pocedureResults.FirstOrDefault();

                MemoryStream attachmentPDF = pdf.ProcedureResult(procedureResult.Id);

                string sender = "dmtipacs.margosatubig@gmail.com";
                string receiver = "margosatubig.bsi@gmail.com";

                MailMessage mm = new MailMessage(sender, receiver);

                //string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + procedureResult.TrnProcedure.TransactionNumber + ".pdf";
                //string fileName = procedureResult.TrnProcedure.PatientName.Replace(" ", "^") + "_" +
                //                  procedureResult.TrnProcedure.StudyDate == null ? "" : procedureResult.TrnProcedure.StudyDate + "_" + 
                //                  procedureResult.TrnProcedure.StudyInstanceId == null ? "" : procedureResult.TrnProcedure.StudyInstanceId + "_" + 
                //                  DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                string patientName = procedureResult.TrnProcedure.PatientName == null ? "NA" : procedureResult.TrnProcedure.PatientName.Replace(" ","^");
                string studyUId = procedureResult.TrnProcedure.StudyInstanceId == null ? "NA" : procedureResult.TrnProcedure.StudyInstanceId;
                string fileName = patientName + "_" + studyUId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                mm.Subject = fileName;
                mm.Body = fileName;

                mm.Attachments.Add(new Attachment(attachmentPDF, fileName));

                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = "dmtipacs.margosatubig@gmail.com";
                NetworkCred.Password = "@dmtipacs1";

                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

    }
}
