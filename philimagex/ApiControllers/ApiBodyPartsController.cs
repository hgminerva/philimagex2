using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace philimagex.ApiControllers
{
    public class ApiBodyPartsController : ApiController
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        // body part list
        [Authorize]
        [HttpGet]
        [Route("api/bodyPart/list")]
        public List<Models.MstBodyPart> listBodyPart()
        {
            var bodyParts = from d in db.MstBodyParts
                            select new Models.MstBodyPart
                            {
                                Id = d.Id,
                                BodyPart = d.BodyPart
                            };

            return bodyParts.ToList();
        }

        // add body part
        [Authorize]
        [HttpPost]
        [Route("api/bodyPart/add")]
        public HttpResponseMessage addBodyPart(Models.MstBodyPart bodyPart)
        {
            try
            {
                Data.MstBodyPart newBodyPart = new Data.MstBodyPart();
                newBodyPart.BodyPart = bodyPart.BodyPart;
                db.MstBodyParts.InsertOnSubmit(newBodyPart);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // update body part
        [Authorize]
        [HttpPut]
        [Route("api/bodyPart/update/{id}")]
        public HttpResponseMessage updateBodyPart(String id, Models.MstBodyPart bodyPart)
        {
            try
            {
                var bodyParts = from d in db.MstBodyParts where d.Id == Convert.ToInt32(id) select d;
                if (bodyParts.Any())
                {
                    var updateBodyPart = bodyParts.FirstOrDefault();
                    updateBodyPart.BodyPart = bodyPart.BodyPart;
                  
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

        // delete body part
        [Authorize]
        [HttpDelete]
        [Route("api/bodyPart/delete/{id}")]
        public HttpResponseMessage deleteBodyPart(String id)
        {
            try
            {
                var bodyParts = from d in db.MstBodyParts where d.Id == Convert.ToInt32(id) select d;
                if (bodyParts.Any())
                {
                    db.MstBodyParts.DeleteOnSubmit(bodyParts.First());
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
