using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace philimagex.Controllers
{
    public class SoftwareController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult NotFound()
        {
            return View();
        }

        [Authorize]
        public ActionResult Modality()
        {
            return View();
        }

        [Authorize]
        public ActionResult BodyParts()
        {
            return View();
        }

        [Authorize]
        public ActionResult User()
        {
            return View();
        }

        [Authorize]
        public ActionResult Rate()
        {
            return View();
        }

        [Authorize]
        public ActionResult Procedure()
        {
            return View();
        }

        [Authorize]
        public ActionResult ProcedureDetail(Int32? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Software");
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Reports()
        {
            return View();
        }
    }
}