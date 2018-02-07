using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace philimagex.ApiControllers
{
    public class ApiProcedureExternalController : ApiController
    {
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        [HttpPost]
        [Route("api/procedureExternal/add")]
        public String addProcedure(ProcedureData procedure)
        {
            try
            {
                var procedures = from d in db.TrnProcedures where d.UserId == procedure.UserId && d.TransactionNumber == procedure.TransactionNumber select d;

                if (procedures.Any())
                {
                    string newDicomFile = procedure.DICOMFileName.Replace("\\", "\\\\");

                    Data.TrnProcedure updateProcedure = procedures.FirstOrDefault();

                    if (updateProcedure.DICOMFileName.IndexOf(newDicomFile) == -1)
                    {
                        updateProcedure.DICOMFileName = updateProcedure.DICOMFileName + ";" + newDicomFile;

                        db.SubmitChanges();

                        return "Success";
                    }
                    else
                    {
                        return "Duplicate found.";
                    }
                }
                else
                {
                    Data.TrnProcedure newProcedure = new Data.TrnProcedure();

                    newProcedure.TransactionNumber = procedure.TransactionNumber == "" || procedure.TransactionNumber == null ? "NA" : procedure.TransactionNumber;

                    TimeZoneInfo timeZoneInfo;
                    DateTime dateTime;
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                    dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                    newProcedure.TransactionDateTime = dateTime;

                    newProcedure.DICOMFileName = procedure.DICOMFileName == "" || procedure.DICOMFileName == null ? "NA" : procedure.DICOMFileName.Replace("\\","\\\\");

                    newProcedure.PatientName = procedure.PatientName == "" || procedure.PatientName == null ? "NA" : procedure.PatientName;

                    newProcedure.Gender = procedure.Gender == "" || procedure.Gender == null ? "M" : procedure.Gender;

                    string[] dateFormat = { "yyyyMMdd" };
                    DateTime dateOfBirth;
                    if (DateTime.TryParseExact(procedure.DateOfBirth,
                                               dateFormat,
                                               System.Globalization.CultureInfo.InvariantCulture,
                                               System.Globalization.DateTimeStyles.None,
                                               out dateOfBirth))
                    {
                        newProcedure.DateOfBirth = dateOfBirth;
                        newProcedure.Age = dateTime.Year - dateOfBirth.Year;
                    }
                    else
                    {
                        newProcedure.DateOfBirth = dateTime;
                        newProcedure.Age = 0;
                    }

                    newProcedure.Particulars = procedure.Particulars == "" || procedure.Particulars == null ? "NA" : procedure.Particulars;

                    newProcedure.ModalityId = (from d in db.MstModalities select d.Id).FirstOrDefault(); // X-Ray

                    var bodyParts = from d in db.MstBodyParts where d.BodyPart == procedure.BodyPart select d;
                    if (bodyParts.Any())
                    {
                        newProcedure.BodyPartId = bodyParts.FirstOrDefault().Id;
                    }
                    else
                    {
                        newProcedure.BodyPartId = (from d in db.MstBodyParts where d.Id == 3 select d.Id).FirstOrDefault(); // Chest
                    }

                    newProcedure.UserId = procedure.UserId;

                    newProcedure.PatientAddress = procedure.PatientAddress == "" || procedure.PatientAddress == null ? "NA" : procedure.PatientAddress;

                    newProcedure.ReferringPhysician = procedure.ReferringPhysician == "" || procedure.ReferringPhysician == null ? "NA" : procedure.ReferringPhysician;

                    newProcedure.StudyDate = procedure.StudyDate == "" || procedure.StudyDate == null ? "NA" : procedure.StudyDate;

                    newProcedure.HospitalNumber = procedure.HospitalNumber == "" || procedure.HospitalNumber == null ? "NA" : procedure.HospitalNumber;

                    newProcedure.HospitalWardNumber = procedure.HospitalWardNumber == "" || procedure.HospitalWardNumber == null ? "NA" : procedure.HospitalWardNumber;

                    newProcedure.StudyInstanceId = procedure.StudyInstanceId == "" || procedure.StudyInstanceId == null ? "NA" : procedure.StudyInstanceId;

                    if (procedure.TransactionNumber != "NA")
                    {
                        if (db.TrnProcedures.Where(d => d.UserId == procedure.UserId && d.TransactionNumber == procedure.TransactionNumber).Any())
                        {
                            return "Duplicate found.";
                        }
                        else
                        {
                            db.TrnProcedures.InsertOnSubmit(newProcedure);
                            db.SubmitChanges();
                            return "Success";
                        }
                    }
                    else
                    {
                        return "Invalid transaction.";
                    }
                }

            }
            catch(Exception e) 
            {
                return e.ToString();
            }
        }
    }

    public class ProcedureData
    {
        public string TransactionNumber;

        public string DICOMFileName;

        public string PatientName;
        public string PatientAddress;
        public string Gender;
        public string DateOfBirth;
        public string Age;

        public string Particulars;
        public string BodyPart;

        public string StudyDate;
        public string ReferringPhysician;
        public string HospitalNumber;
        public string HospitalWardNumber;
        public string StudyInstanceId;

        public int UserId;
    }
}