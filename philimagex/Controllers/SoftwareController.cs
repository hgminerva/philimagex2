using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace philimagex.Controllers
{
    public class SoftwareController : Controller
    {
        // data
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        [Authorize]
        public ActionResult Index(Int32? facilityId)
        {
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;
            if (facilityId != null)
            {
                if (user.FirstOrDefault().UserTypeId == 1)
                {
                    if (user.FirstOrDefault().Id == facilityId)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else if (user.FirstOrDefault().UserTypeId == 4)
                {
                    return RedirectToAction("Index", "Manage");
                }
                else
                {
                    var userFacility = from d in db.MstUserDoctors where d.DoctorId == user.FirstOrDefault().Id && d.UserId == facilityId select d;
                    if (userFacility.Any())
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Software");
            }
        }

        [Authorize]
        public ActionResult NotFound()
        {
            return View();
        }

        [Authorize]
        public ActionResult Modality(Int32? facilityId)
        {
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;
            if (facilityId != null)
            {
                if (user.FirstOrDefault().UserTypeId == 1)
                {
                    if (user.FirstOrDefault().Id == facilityId)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else if (user.FirstOrDefault().UserTypeId == 4)
                {
                    return RedirectToAction("Index", "Manage");
                }
                else
                {
                    var userFacility = from d in db.MstUserDoctors where d.DoctorId == user.FirstOrDefault().Id && d.UserId == facilityId select d;
                    if (userFacility.Any())
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Software");
            }
        }

        [Authorize]
        public ActionResult BodyParts(Int32? facilityId)
        {
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;
            if (facilityId != null)
            {
                if (user.FirstOrDefault().UserTypeId == 1)
                {
                    if (user.FirstOrDefault().Id == facilityId)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else if (user.FirstOrDefault().UserTypeId == 4)
                {
                    return RedirectToAction("Index", "Manage");
                }
                else
                {
                    var userFacility = from d in db.MstUserDoctors where d.DoctorId == user.FirstOrDefault().Id && d.UserId == facilityId select d;
                    if (userFacility.Any())
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Software");
            }
        }

        [Authorize]
        public ActionResult Users(Int32? facilityId)
        {
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;
            if (facilityId != null)
            {
                if (user.FirstOrDefault().UserTypeId == 1)
                {
                    if (user.FirstOrDefault().Id == facilityId)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else if (user.FirstOrDefault().UserTypeId == 4)
                {
                    return RedirectToAction("Index", "Manage");
                }
                else
                {
                    var userFacility = from d in db.MstUserDoctors where d.DoctorId == user.FirstOrDefault().Id && d.UserId == facilityId select d;
                    if (userFacility.Any())
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Software");
            }
        }

        [Authorize]
        public ActionResult Rate(Int32? facilityId)
        {
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;
            if (facilityId != null)
            {
                if (user.FirstOrDefault().UserTypeId == 1)
                {
                    if (user.FirstOrDefault().Id == facilityId)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else if (user.FirstOrDefault().UserTypeId == 4)
                {
                    return RedirectToAction("Index", "Manage");
                }
                else
                {
                    var userFacility = from d in db.MstUserDoctors where d.DoctorId == user.FirstOrDefault().Id && d.UserId == facilityId select d;
                    if (userFacility.Any())
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Software");
            }
        }

        [Authorize]
        public ActionResult Procedure(Int32? facilityId)
        {
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;
            if (facilityId != null)
            {
                if (user.FirstOrDefault().UserTypeId == 1)
                {
                    if (user.FirstOrDefault().Id == facilityId)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else if (user.FirstOrDefault().UserTypeId == 4)
                {
                    return RedirectToAction("Index", "Manage");
                }
                else
                {
                    var userFacility = from d in db.MstUserDoctors where d.DoctorId == user.FirstOrDefault().Id && d.UserId == facilityId select d;
                    if (userFacility.Any())
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Software");
            }
        }

        [Authorize]
        public ActionResult ProcedureDetail(Int32? facilityId, Int32? id)
        {
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;
            if (facilityId != null)
            {
                if (user.FirstOrDefault().UserTypeId == 1)
                {
                    if (id == null)
                    {
                        return RedirectToAction("NotFound", "Software");
                    }
                    else
                    {
                        if (user.FirstOrDefault().Id == facilityId)
                        {
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Index", "Manage");
                        }
                    }
                }
                else if (user.FirstOrDefault().UserTypeId == 4)
                {
                    return RedirectToAction("Index", "Manage");
                }
                else
                {
                    var userFacility = from d in db.MstUserDoctors where d.DoctorId == user.FirstOrDefault().Id && d.UserId == facilityId select d;
                    if (userFacility.Any())
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Software");
            }
        }

        [Authorize]
        public ActionResult Reports(Int32? facilityId)
        {
            var user = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;
            if (facilityId != null)
            {
                if (user.FirstOrDefault().UserTypeId == 1)
                {
                    if (user.FirstOrDefault().Id == facilityId)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else if (user.FirstOrDefault().UserTypeId == 4)
                {
                    return RedirectToAction("Index", "Manage");
                }
                else
                {
                    var userFacility = from d in db.MstUserDoctors where d.DoctorId == user.FirstOrDefault().Id && d.UserId == facilityId select d;
                    if (userFacility.Any())
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Software");
            }
        }
    }
}