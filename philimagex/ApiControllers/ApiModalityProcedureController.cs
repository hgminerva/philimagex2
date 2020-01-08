using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace philimagex.ApiControllers
{
    public class ApiModalityProcedureController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // modality procedure list
        [Authorize]
        [HttpGet]
        [Route("api/modalityProcedure/list")]
        public List<Models.MstModalityProcedure> listModalityProcedure()
        {
            var modalityProcedures = from d in db.MstModalityProcedures
                                     select new Models.MstModalityProcedure
                                     {
                                         Id = d.Id,
                                         ModalityId = d.ModalityId,
                                         Modality = d.MstModality.Modality,
                                         ModalityProcedure = d.ModalityProcedure,
                                         ModalityResultTemplate = d.ModalityResultTemplate
                                     };

            return modalityProcedures.ToList();
        }

        // modality procedure list filtered by username
        [Authorize]
        [HttpGet]
        [Route("api/modalityProcedure/listByUserName/{UserName}")]
        public List<Models.MstModalityProcedure> listModalityProcedureByDoctor(String UserName)
        {
            var users = from d in db.MstUsers where d.UserName == UserName select d;
            if (users.Any())
            {
                var modalityProcedures = from d in db.MstModalityProcedures
                                         where d.DoctorId == users.First().Id
                                         orderby d.ModalityProcedure ascending
                                         select new Models.MstModalityProcedure
                                         {
                                            Id = d.Id,
                                            ModalityId = d.ModalityId,
                                            Modality = d.MstModality.Modality,
                                            ModalityProcedure = d.ModalityProcedure + " (" + d.MstModality.Modality + ") " + d.ModalityResultTemplate.Substring(0, 30) + " ...",
                                            ModalityResultTemplate = d.ModalityResultTemplate
                                         };
                return modalityProcedures.ToList();
            }
            else
            {
                return new List<Models.MstModalityProcedure>();
            }
        }

        // modality procedure get by Id
        [Authorize]
        [HttpGet]
        [Route("api/modalityProcedure/getById/{id}")]
        public Models.MstModalityProcedure getModalityProcedureById(String id)
        {
            var modalityProcedures = from d in db.MstModalityProcedures
                                     where d.Id == Convert.ToInt32(id)
                                     select new Models.MstModalityProcedure
                                     {
                                         Id = d.Id,
                                         ModalityId = d.ModalityId,
                                         Modality = d.MstModality.Modality,
                                         ModalityProcedure = d.ModalityProcedure,
                                         ModalityResultTemplate = d.ModalityResultTemplate
                                     };

            return (Models.MstModalityProcedure)modalityProcedures.FirstOrDefault();
        }


        // add modality procedure
        [Authorize]
        [HttpPost]
        [Route("api/modalityProcedure/add")]
        public HttpResponseMessage addModalityProcedure(Models.MstModalityProcedure modalityProcedure)
        {
            try
            {
                Data.MstModalityProcedure newModalityProcedure = new Data.MstModalityProcedure();

                newModalityProcedure.ModalityId = modalityProcedure.ModalityId;
                newModalityProcedure.ModalityProcedure = modalityProcedure.ModalityProcedure;
                newModalityProcedure.ModalityResultTemplate = modalityProcedure.ModalityResultTemplate;
               
                db.MstModalityProcedures.InsertOnSubmit(newModalityProcedure);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // update modality procedure
        [Authorize]
        [HttpPut]
        [Route("api/modalityProcedure/update/{id}")]
        public HttpResponseMessage updatedModalityProcedure(String id, Models.MstModalityProcedure modalityProcedure)
        {
            try
            {
                var modalityProcedures = from d in db.MstModalityProcedures where d.Id == Convert.ToInt32(id) select d;
                if (modalityProcedures.Any())
                {
                    var updateModalityProcedure = modalityProcedures.FirstOrDefault();

                    updateModalityProcedure.ModalityId = modalityProcedure.ModalityId;
                    updateModalityProcedure.ModalityProcedure = modalityProcedure.ModalityProcedure;
                    updateModalityProcedure.ModalityResultTemplate = modalityProcedure.ModalityResultTemplate;

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

        // delete modality procedure
        [Authorize]
        [HttpDelete]
        [Route("api/modalityProcedure/delete/{id}")]
        public HttpResponseMessage deleteModalityProcedure(String id)
        {
            try
            {
                var modalityProcedures = from d in db.MstModalityProcedures where d.Id == Convert.ToInt32(id) select d;
                if (modalityProcedures.Any())
                {
                    db.MstModalityProcedures.DeleteOnSubmit(modalityProcedures.First());
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
