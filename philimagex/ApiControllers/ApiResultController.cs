using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net.Http.Headers;

namespace philimagex.ApiControllers
{
    public class ApiResultController : ApiController
    {
        Controllers.PDFController pdf = new Controllers.PDFController();
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        [HttpGet]
        [Route("api/pdf")]
        public HttpResponseMessage GetPDF()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string Report = nvc["Report"].ToString();
            Int32 Id = Convert.ToInt32(nvc["Id"]);
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            switch (Report)
            {
                case "ProcedureResult":
                    response.Content = new StreamContent(pdf.ProcedureResult(Id));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    break;
            }

            return response;
        }

        [HttpGet]
        [Route("api/pdf/download")]
        public HttpResponseMessage GetPDFDownload()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string Report = nvc["Report"].ToString();
            string TransactionNumber = nvc["TransactionNumber"].ToString();
            Int32 UserId = Convert.ToInt32(nvc["UserId"]);
            Int32 ReportNumber = Convert.ToInt32(nvc["ReportNumber"]);
            Int32 counter = 1;

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            switch (Report)
            {
                case "ProcedureResult":
                    var procedure = from d in db.TrnProcedures where d.TransactionNumber == TransactionNumber && d.UserId == UserId select d;

                    if (procedure.Any())
                    {
                        foreach (var result in procedure.FirstOrDefault().TrnProcedureResults)
                        {
                            if (counter == ReportNumber)
                            {
                                response.Content = new StreamContent(pdf.ProcedureResult(result.Id));
                                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                {
                                    FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + TransactionNumber + ".pdf"
                                };
                            }
                            counter++;
                        }
                    }
                    break;
            }

            return response;
        }
    }
}