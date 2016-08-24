using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace philimagex.ApiControllers
{
    public class ApiProcedureResultController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // body procedure result
        [Authorize]
        [HttpGet]
        [Route("api/procedureResult/list")]
        public List<Models.TrnProcedureResult> listProcedureResult()
        {
            var procedureResults = from d in db.TrnProcedureResults
                                   select new Models.TrnProcedureResult
                                    {
                                        Id = d.Id,
                                        ProcedureId = d.ProcedureId,
                                        ModalityProcedureId = d.ModalityProcedureId,
                                        ModalityProcedure = d.MstModalityProcedure.ModalityProcedure,
                                        Result = d.Result,
                                        DoctorId = d.DoctorId,
                                        Doctor = d.MstUser.FullName,
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
                newProcedureResult.DoctorDateTime = Convert.ToDateTime(procedureResult.DoctorDateTime);

                db.TrnProcedureResults.InsertOnSubmit(newProcedureResult);
                db.SubmitChanges();

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
                    updaterPocedureResult.DoctorDateTime = Convert.ToDateTime(procedureResult.DoctorDateTime);

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
    }
}
