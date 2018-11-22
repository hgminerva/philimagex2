using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace philimagex.Controllers
{
    public class PDFController
    {
        private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

        //public HttpResponseMessage Get()
        //{
        //    NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
        //    string Report = nvc["Report"].ToString();
        //    Int32 Id = Convert.ToInt32(nvc["Id"]);
        //    var response = new HttpResponseMessage(HttpStatusCode.OK);

        //    switch (Report)
        //    {
        //        case "ProcedureResult":
        //            response.Content = new StreamContent(this.ProcedureResult(Id));
        //            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //            break;
        //    }

        //    return response;
        //}

        public PdfPCell CreateRightAlignedCell(string text, Font font, int span)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT };
            cell.Colspan = span;
            return cell;
        }
        public PdfPCell CreateLeftAlignedCell(string text, Font font, int span)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT };
            cell.Colspan = span;
            return cell;
        }
        public PdfPCell CreateCenterAlignedCell(string text, Font font, int span)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font)) { HorizontalAlignment = PdfPCell.ALIGN_CENTER };
            cell.Colspan = span;
            return cell;
        }

        public MemoryStream ProcedureResult(Int32 Id)
        {
            var doc = new Document(PageSize.LETTER, 50, 50, 25, 25);
            var stream = new MemoryStream();

            try
            {
                BaseFont BaseFontTimesRoman = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font TitleFont = new Font(BaseFontTimesRoman, 18, Font.BOLD);
                Font SubTitleFont = new Font(BaseFontTimesRoman, 14, Font.BOLD);
                Font TableHeaderFont = new Font(BaseFontTimesRoman, 12, Font.BOLD);
                Font BodyFont = new Font(BaseFontTimesRoman, 12, Font.NORMAL);
                Font SubHeaderFont = new Font(BaseFontTimesRoman, 10, Font.NORMAL);
                Font EndingMessageFont = new Font(BaseFontTimesRoman, 12, Font.ITALIC);
                Font fontArial12Bold = new Font(BaseFontTimesRoman, 12, Font.BOLD);
                Font fontArial12 = new Font(BaseFontTimesRoman, 12);

                // line
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 3)));

                PdfWriter writer = PdfWriter.GetInstance(doc, stream);

                var ProcedureResults = from d in db.TrnProcedureResults where d.Id == Id select d;

                // ============
                // Margosatubig
                // ============
                if (ProcedureResults.First().TrnProcedure.MstUser.UserName == "margosatubig")
                {
                    writer.PageEvent = new PDFHeaderFooterMargosatubig(ProcedureResults.First().TrnProcedure.MstUser.FullName, Id);

                    doc.Open();

                    // Header
                    // doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.FullName, TableHeaderFont));
                    // doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.Address, SubHeaderFont));
                    // doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.ContactNumber, SubHeaderFont));
                    // Detail - Header
                    //var HeaderTable = new PdfPTable(2);
                    //HeaderTable.HorizontalAlignment = 0;
                    //HeaderTable.SpacingBefore = 20;
                    //HeaderTable.SpacingAfter = 40;
                    //HeaderTable.DefaultCell.Border = 0;
                    //HeaderTable.SetWidths(new int[] { 2, 6 });
                    //HeaderTable.AddCell(new Phrase("Transaction No.:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.TransactionNumber, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Name:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.PatientName, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Age:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.Age.ToString(), BodyFont));
                    //HeaderTable.AddCell(new Phrase("Sex:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.Gender, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Procedure:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().MstModalityProcedure.ModalityProcedure, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Transaction Date:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.TransactionDateTime.ToShortDateString(), BodyFont));
                    //doc.Add(HeaderTable);

                    doc.Add(line);
                    // Detail - Header
                    PdfPTable HeaderTable = new PdfPTable(4);
                    float[] HeaderTableWithCells = new float[] { 50f, 10f, 10f, 30f };
                    HeaderTable.SetWidths(HeaderTableWithCells);
                    HeaderTable.WidthPercentage = 100;
                    var nameLabelData = new Phrase();
                    nameLabelData.Add(new Chunk("NAME: \n", fontArial12Bold));
                    nameLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.PatientName, fontArial12));
                    var ageLabelData = new Phrase();
                    ageLabelData.Add(new Chunk("AGE: \n", fontArial12Bold));
                    ageLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Age.ToString(), fontArial12));
                    var sexLabelData = new Phrase();
                    sexLabelData.Add(new Chunk("SEX: \n", fontArial12Bold));
                    sexLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Gender, fontArial12));
                    var accessionLabelData = new Phrase();
                    accessionLabelData.Add(new Chunk("ACCESSION NO.: \n", fontArial12Bold));
                    accessionLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.TransactionNumber, fontArial12));
                    var addressLabelData = new Phrase();
                    addressLabelData.Add(new Chunk("ADDRESS: \n", fontArial12Bold));
                    addressLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.PatientAddress, fontArial12));
                    var hospitalNoLabelData = new Phrase();
                    hospitalNoLabelData.Add(new Chunk("HOSPITAL NO.: \n", fontArial12Bold));
                    hospitalNoLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.HospitalNumber, fontArial12));
                    var wardRoomLabelData = new Phrase();
                    wardRoomLabelData.Add(new Chunk("WARD/ROOM: \n", fontArial12Bold));
                    wardRoomLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.HospitalWardNumber, fontArial12));
                    var referredByLabelData = new Phrase();
                    referredByLabelData.Add(new Chunk("REFERRED BY: \n", fontArial12Bold));
                    referredByLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.ReferringPhysician, fontArial12));
                    var procedureLabelData = new Phrase();
                    procedureLabelData.Add(new Chunk("PROCEDURE: \n", fontArial12Bold));
                    procedureLabelData.Add(new Chunk(ProcedureResults.First().MstModalityProcedure.ModalityProcedure, fontArial12));
                    var dateOfExamLabelData = new Phrase();
                    dateOfExamLabelData.Add(new Chunk("DATE OF EXAM: \n", fontArial12Bold));
                    dateOfExamLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.StudyDate, fontArial12));
                    var clinicalInformationLabelData = new Phrase();
                    clinicalInformationLabelData.Add(new Chunk("CLINICAL INFORMATION: \n", fontArial12Bold));
                    clinicalInformationLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Particulars, fontArial12));
                    HeaderTable.AddCell(new PdfPCell(new Phrase(nameLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(ageLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(sexLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(accessionLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(addressLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(hospitalNoLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 2 });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(wardRoomLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(referredByLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(procedureLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 2 });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(dateOfExamLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(clinicalInformationLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 4 });
                    doc.Add(HeaderTable);

                    //PdfContentByte cb = writer.DirectContent;
                    //cb.MoveTo(0, 515);
                    //cb.LineTo(doc.PageSize.Width, 515);
                    //cb.Stroke();

                    // Detail - Result
                    var DetailTable = new PdfPTable(1);
                    DetailTable.HorizontalAlignment = 0;
                    DetailTable.SpacingAfter = 20;
                    DetailTable.DefaultCell.Border = 0;
                    DetailTable.SetWidths(new int[] { 6 });
                    DetailTable.WidthPercentage = 100;

                    PdfPCell detailCell1 = new PdfPCell(new Phrase(ProcedureResults.First().TrnProcedure.MstModality.Modality + " Result", TitleFont)) { HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_BOTTOM, PaddingTop = 20f };
                    detailCell1.Border = 0;

                    PdfPCell detailCell2 = new PdfPCell(new Phrase(ProcedureResults.First().Result, BodyFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    detailCell2.Border = 0;

                    DetailTable.AddCell(detailCell1);
                    DetailTable.AddCell(detailCell2);

                    doc.Add(DetailTable);

                    // Signature
                    Image SignatureImage = Image.GetInstance(HttpContext.Current.Server.MapPath("/Images/Signature/" + ProcedureResults.First().MstUser.UserName + ".png"));
                    SignatureImage.ScaleToFit(150f, 58f);

                    //doc.Add(SignatureImage);
                    //var SignatureTable = new PdfPTable(1);
                    //SignatureTable.HorizontalAlignment = 0;
                    //SignatureTable.SpacingBefore = 0;
                    //SignatureTable.SpacingAfter = 10;
                    //SignatureTable.SetWidths(new int[] { 6 });
                    //SignatureTable.WidthPercentage = 100;
                    //SignatureTable.DefaultCell.Border = 0;
                    ////PdfPCell cell1 = new PdfPCell(new Phrase("Radiologist:", TableHeaderFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    ////cell1.Border = 0;
                    //PdfPCell cell2 = new PdfPCell(new Phrase(ProcedureResults.First().MstUser.FullName, TableHeaderFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    ////cell2.FixedHeight = 50f;
                    //cell2.Border = 0;
                    //PdfPCell cell3 = new PdfPCell(new Phrase(ProcedureResults.First().MstUser.ContactNumber, BodyFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    //cell3.Border = 0;
                    ////SignatureTable.AddCell(cell1);
                    //SignatureTable.AddCell(cell2);
                    //SignatureTable.AddCell(cell3);
                    //doc.Add(SignatureTable);
                    //// Release
                    //var ReleaseTable = new PdfPTable(1);
                    //ReleaseTable.HorizontalAlignment = 0;
                    //ReleaseTable.SpacingBefore = 10;
                    //ReleaseTable.SpacingAfter = 10;
                    //ReleaseTable.SetWidths(new int[] { 6 });
                    //ReleaseTable.WidthPercentage = 100;
                    //ReleaseTable.DefaultCell.Border = 0;
                    //PdfPCell rcell1 = new PdfPCell(new Phrase("Release By:_____________________", BodyFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    //rcell1.Border = 0;
                    //PdfPCell rcell2 = new PdfPCell(new Phrase("Release Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), BodyFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    //rcell2.Border = 0;
                    //ReleaseTable.AddCell(rcell1);
                    //ReleaseTable.AddCell(rcell2);
                    //doc.Add(ReleaseTable);

                    Image signatureImage = Image.GetInstance(SignatureImage);
                    PdfPCell signatureCellImage = new PdfPCell(signatureImage);
                    // Detail - space
                    PdfPTable spaceTable = new PdfPTable(1);
                    float[] spaceWithCells = new float[] { 100f };
                    spaceTable.SetWidths(spaceWithCells);
                    spaceTable.WidthPercentage = 100;
                    spaceTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                    doc.Add(spaceTable);
                    // Detail - footer
                    PdfPTable FooterTable = new PdfPTable(3);
                    float[] FooterTableWithCells = new float[] { 40f, 20f, 40f };
                    FooterTable.SetWidths(FooterTableWithCells);
                    FooterTable.WidthPercentage = 100;
                    FooterTable.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Radiologist:", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(signatureCellImage) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(signatureCellImage) { Border = 0, PaddingTop = 2f, PaddingBottom = 0, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(ProcedureResults.First().MstUser.FullName, TableHeaderFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 1, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(ProcedureResults.First().MstUser.ContactNumber, BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    FooterTable.AddCell(new PdfPCell(new Phrase("OR No.:_________________________", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Release By:______________________", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });

                    FooterTable.AddCell(new PdfPCell(new Phrase("Amount Paid: ____________________", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Result Date: " + ProcedureResults.First().DoctorDateTime.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });

                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Release Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    doc.Add(FooterTable);
                    doc.Add(spaceTable);
                    // Detail - footer label
                    PdfPTable FooterLabelTable = new PdfPTable(1);
                    float[] FooterLabelTableWithCells = new float[] { 100f };
                    FooterLabelTable.SetWidths(FooterLabelTableWithCells);
                    FooterLabelTable.WidthPercentage = 100;
                    FooterLabelTable.AddCell(new PdfPCell(new Phrase("NOT VALID UNLESS SIGNED", fontArial12Bold)) { Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                    doc.Add(FooterLabelTable);

                    doc.Close();
                }

                // ======================
                // Bacalso Medical Clinic
                // ======================
                else if (ProcedureResults.First().TrnProcedure.MstUser.UserName == "bacalsomedical")
                {
                    writer.PageEvent = new PDFHeaderFooter(ProcedureResults.First().TrnProcedure.MstUser.FullName, Id);

                    doc.Open();

                    // Header
                    // doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.FullName, TableHeaderFont));

                    doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.Address, SubHeaderFont));
                    doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.ContactNumber, SubHeaderFont));

                    //// Detail - Header
                    //var HeaderTable = new PdfPTable(2);
                    //HeaderTable.HorizontalAlignment = 0;
                    //HeaderTable.SpacingBefore = 20;
                    //HeaderTable.SpacingAfter = 40;
                    //HeaderTable.DefaultCell.Border = 0;
                    //HeaderTable.SetWidths(new int[] { 2, 6 });
                    //HeaderTable.AddCell(new Phrase("Transaction No.:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.TransactionNumber, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Name:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.PatientName, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Age:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.Age.ToString(), BodyFont));
                    //HeaderTable.AddCell(new Phrase("Sex:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.Gender, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Procedure:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().MstModalityProcedure.ModalityProcedure, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Transaction Date:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.TransactionDateTime.ToShortDateString(), BodyFont));
                    //doc.Add(HeaderTable);

                    doc.Add(line);
                    // Detail - Header
                    PdfPTable HeaderTable = new PdfPTable(4);
                    float[] HeaderTableWithCells = new float[] { 50f, 10f, 10f, 30f };
                    HeaderTable.SetWidths(HeaderTableWithCells);
                    HeaderTable.WidthPercentage = 100;

                    // row = 1
                    var nameLabelData = new Phrase();
                    nameLabelData.Add(new Chunk("NAME: \n", fontArial12Bold));
                    nameLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.PatientName, fontArial12));

                    var ageLabelData = new Phrase();
                    ageLabelData.Add(new Chunk("AGE: \n", fontArial12Bold));
                    ageLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Age.ToString(), fontArial12));

                    var sexLabelData = new Phrase();
                    sexLabelData.Add(new Chunk("SEX: \n", fontArial12Bold));
                    sexLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Gender, fontArial12));

                    var accessionLabelData = new Phrase();
                    accessionLabelData.Add(new Chunk("X-RAY NO.: \n", fontArial12Bold));
                    accessionLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.TransactionNumber, fontArial12));

                    // row = 2
                    var addressLabelData = new Phrase();
                    addressLabelData.Add(new Chunk("ADDRESS: \n", fontArial12Bold));
                    addressLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.PatientAddress, fontArial12));

                    var dateOfExamLabelData = new Phrase();
                    dateOfExamLabelData.Add(new Chunk("DATE OF EXAM: \n", fontArial12Bold));
                    dateOfExamLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.StudyDate, fontArial12));

                    // row = 3
                    var referredByLabelData = new Phrase();
                    referredByLabelData.Add(new Chunk("REFERRED BY: \n", fontArial12Bold));
                    referredByLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.ReferringPhysician, fontArial12));

                    var hospitalNoLabelData = new Phrase();
                    hospitalNoLabelData.Add(new Chunk("HOSPITAL NO.: \n", fontArial12Bold));
                    hospitalNoLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.HospitalNumber, fontArial12));

                    var wardRoomLabelData = new Phrase();
                    wardRoomLabelData.Add(new Chunk("IN/OUT PATIENT: \n", fontArial12Bold));
                    wardRoomLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.HospitalWardNumber, fontArial12));

                    // row = 4
                    var procedureLabelData = new Phrase();
                    procedureLabelData.Add(new Chunk("PROCEDURE: \n", fontArial12Bold));
                    procedureLabelData.Add(new Chunk(ProcedureResults.First().MstModalityProcedure.ModalityProcedure, fontArial12));

                    // row = 5
                    var clinicalInformationLabelData = new Phrase();
                    clinicalInformationLabelData.Add(new Chunk("CLINICAL INFORMATION: \n", fontArial12Bold));
                    clinicalInformationLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Particulars, fontArial12));

                    // add cells
                    HeaderTable.AddCell(new PdfPCell(new Phrase(nameLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(ageLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(sexLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(accessionLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });

                    HeaderTable.AddCell(new PdfPCell(new Phrase(addressLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 3 });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(dateOfExamLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 1 });

                    HeaderTable.AddCell(new PdfPCell(new Phrase(referredByLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(hospitalNoLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 2 });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(wardRoomLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });

                    HeaderTable.AddCell(new PdfPCell(new Phrase(procedureLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 4 });
                    
                    HeaderTable.AddCell(new PdfPCell(new Phrase(clinicalInformationLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 4 });
                    
                    doc.Add(HeaderTable);

                    // Detail - Result
                    var DetailTable = new PdfPTable(1);
                    DetailTable.HorizontalAlignment = 0;
                    DetailTable.SpacingAfter = 20;
                    DetailTable.DefaultCell.Border = 0;
                    DetailTable.SetWidths(new int[] { 6 });
                    DetailTable.WidthPercentage = 100;

                    PdfPCell detailCell1 = new PdfPCell(new Phrase(ProcedureResults.First().TrnProcedure.MstModality.Modality + " Result", TitleFont)) { HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_BOTTOM, PaddingTop = 20f };
                    detailCell1.Border = 0;

                    PdfPCell detailCell2 = new PdfPCell(new Phrase(ProcedureResults.First().Result, BodyFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    detailCell2.Border = 0;

                    DetailTable.AddCell(detailCell1);
                    DetailTable.AddCell(detailCell2);

                    doc.Add(DetailTable);

                    // Signature
                    Image SignatureImage = Image.GetInstance(HttpContext.Current.Server.MapPath("/Images/Signature/" + ProcedureResults.First().MstUser.UserName + ".png"));
                    SignatureImage.ScaleToFit(150f, 58f);

                    Image signatureImage = Image.GetInstance(SignatureImage);
                    PdfPCell signatureCellImage = new PdfPCell(signatureImage);

                    // Detail - space
                    PdfPTable spaceTable = new PdfPTable(1);
                    float[] spaceWithCells = new float[] { 100f };
                    spaceTable.SetWidths(spaceWithCells);
                    spaceTable.WidthPercentage = 100;
                    spaceTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                    doc.Add(spaceTable);

                    // Detail - footer
                    PdfPTable FooterTable = new PdfPTable(3);
                    float[] FooterTableWithCells = new float[] { 40f, 20f, 40f };
                    FooterTable.SetWidths(FooterTableWithCells);
                    FooterTable.WidthPercentage = 100;

                    FooterTable.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Radiologist:", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(signatureCellImage) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(ProcedureResults.First().MstUser.FullName, TableHeaderFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 1, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(ProcedureResults.First().MstUser.ContactNumber, BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Release By: Leonidez C. Zafra, RRT", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Release Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    
                    doc.Add(FooterTable);
                    doc.Add(spaceTable);

                    // Detail - footer label
                    PdfPTable FooterLabelTable = new PdfPTable(3);
                    float[] FooterLabelTableWithCells = new float[] { 40f, 20f, 40f };
                    FooterLabelTable.SetWidths(FooterLabelTableWithCells);
                    FooterLabelTable.WidthPercentage = 100;

                    FooterLabelTable.AddCell(new PdfPCell(new Phrase("NOT VALID UNLESS SIGNED", fontArial12Bold)) { Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterLabelTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterLabelTable.AddCell(new PdfPCell(new Phrase("IMG FORM 03.00", fontArial12Bold)) { Border = 0, PaddingLeft = 5f, PaddingRight = 5f });

                    FooterLabelTable.AddCell(new PdfPCell(Image.GetInstance(HttpContext.Current.Server.MapPath("/Images/Logo/bacalsomedicaltuv.jpg"))) { Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterLabelTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterLabelTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingLeft = 5f, PaddingRight = 5f });

                    doc.Add(FooterLabelTable);

                    //Image image = Image.GetInstance(HttpContext.Current.Server.MapPath("/Images/Logo/bacalsomedicaltuv.jpg"));
                    //image.ScaleToFit(200f, 100f);
                    //image.SetAbsolutePosition(430, 650);
                    //doc.Add(image);

                    doc.Close();
                }
                
                // ==========
                // LifeHealth
                // ==========
                else if (ProcedureResults.First().TrnProcedure.MstUser.UserName == "lifehealth" || ProcedureResults.First().TrnProcedure.MstUser.UserName == "lifehealthmobile")
                {
                    //writer.PageEvent = new PDFHeaderFooter(ProcedureResults.First().TrnProcedure.MstUser.FullName, Id);

                    doc.Open();

                    // Header
                    // doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.FullName, TableHeaderFont));

                    //doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.Address, SubHeaderFont));
                    //doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.ContactNumber, SubHeaderFont));

                    PdfPTable spaceTable = new PdfPTable(1);
                    float[] spaceWithCells = new float[] { 100f };
                    spaceTable.SetWidths(spaceWithCells);
                    spaceTable.WidthPercentage = 100;
                    spaceTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 70f, PaddingLeft = 5f, PaddingRight = 5f });
                    doc.Add(spaceTable);


                    doc.Add(line);
                    // Detail - Header
                    PdfPTable HeaderTable = new PdfPTable(4);
                    float[] HeaderTableWithCells = new float[] { 50f, 10f, 10f, 30f };
                    HeaderTable.SetWidths(HeaderTableWithCells);
                    HeaderTable.WidthPercentage = 100;
                    var nameLabelData = new Phrase();
                    nameLabelData.Add(new Chunk("NAME: \n", fontArial12Bold));
                    nameLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.PatientName, fontArial12));
                    var ageLabelData = new Phrase();
                    ageLabelData.Add(new Chunk("AGE: \n", fontArial12Bold));
                    ageLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Age.ToString(), fontArial12));
                    var sexLabelData = new Phrase();
                    sexLabelData.Add(new Chunk("SEX: \n", fontArial12Bold));
                    sexLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Gender, fontArial12));
                    var accessionLabelData = new Phrase();
                    accessionLabelData.Add(new Chunk("ACCESSION NO.: \n", fontArial12Bold));
                    accessionLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.TransactionNumber, fontArial12));
                    var addressLabelData = new Phrase();
                    addressLabelData.Add(new Chunk("ADDRESS: \n", fontArial12Bold));
                    addressLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.PatientAddress, fontArial12));
                    var hospitalNoLabelData = new Phrase();
                    hospitalNoLabelData.Add(new Chunk("HOSPITAL NO.: \n", fontArial12Bold));
                    hospitalNoLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.HospitalNumber, fontArial12));
                    var wardRoomLabelData = new Phrase();
                    wardRoomLabelData.Add(new Chunk("WARD/ROOM: \n", fontArial12Bold));
                    wardRoomLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.HospitalWardNumber, fontArial12));
                    var referredByLabelData = new Phrase();
                    referredByLabelData.Add(new Chunk("REFERRED BY: \n", fontArial12Bold));
                    referredByLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.ReferringPhysician, fontArial12));
                    var procedureLabelData = new Phrase();
                    procedureLabelData.Add(new Chunk("PROCEDURE: \n", fontArial12Bold));
                    procedureLabelData.Add(new Chunk(ProcedureResults.First().MstModalityProcedure.ModalityProcedure, fontArial12));
                    var dateOfExamLabelData = new Phrase();
                    dateOfExamLabelData.Add(new Chunk("DATE OF EXAM: \n", fontArial12Bold));
                    dateOfExamLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.StudyDate, fontArial12));
                    var clinicalInformationLabelData = new Phrase();
                    clinicalInformationLabelData.Add(new Chunk("CLINICAL INFORMATION: \n", fontArial12Bold));
                    clinicalInformationLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Particulars, fontArial12));
                    HeaderTable.AddCell(new PdfPCell(new Phrase(nameLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(ageLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(sexLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(accessionLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(addressLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(hospitalNoLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 2 });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(wardRoomLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(referredByLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(procedureLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 2 });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(dateOfExamLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(clinicalInformationLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 4 });
                    doc.Add(HeaderTable);

                    // Detail - Result
                    var DetailTable = new PdfPTable(1);
                    DetailTable.HorizontalAlignment = 0;
                    DetailTable.SpacingAfter = 20;
                    DetailTable.DefaultCell.Border = 0;
                    DetailTable.SetWidths(new int[] { 6 });
                    DetailTable.WidthPercentage = 100;

                    PdfPCell detailCell1 = new PdfPCell(new Phrase(ProcedureResults.First().TrnProcedure.MstModality.Modality + " Result", TitleFont)) { HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_BOTTOM, PaddingTop = 20f };
                    detailCell1.Border = 0;

                    PdfPCell detailCell2 = new PdfPCell(new Phrase(ProcedureResults.First().Result, BodyFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    detailCell2.Border = 0;

                    DetailTable.AddCell(detailCell1);
                    DetailTable.AddCell(detailCell2);

                    doc.Add(DetailTable);

                    // Signature
                    //Image SignatureImage = Image.GetInstance(HttpContext.Current.Server.MapPath("/Images/Signature/" + ProcedureResults.First().MstUser.UserName + ".png"));
                    //SignatureImage.ScaleToFit(150f, 58f);
                    //Image signatureImage = Image.GetInstance(SignatureImage);
                    //PdfPCell signatureCellImage = new PdfPCell(signatureImage);
                    // Detail - space
                    //PdfPTable spaceTable = new PdfPTable(1);
                    //float[] spaceWithCells = new float[] { 100f };
                    //spaceTable.SetWidths(spaceWithCells);
                    //spaceTable.WidthPercentage = 100;
                    //spaceTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                    //doc.Add(spaceTable);
                    // Detail - footer
                    //PdfPTable FooterTable = new PdfPTable(3);
                    //float[] FooterTableWithCells = new float[] { 40f, 20f, 40f };
                    //FooterTable.SetWidths(FooterTableWithCells);
                    //FooterTable.WidthPercentage = 100;
                    //FooterTable.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase("Radiologist:", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(signatureCellImage) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(ProcedureResults.First().MstUser.FullName, TableHeaderFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 1, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(ProcedureResults.First().MstUser.ContactNumber, BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase("OR No.:_________________________", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase("Release By:______________________", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase("Amount Paid: ____________________", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //FooterTable.AddCell(new PdfPCell(new Phrase("Release Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    //doc.Add(FooterTable);
                    //doc.Add(spaceTable);
                    //// Detail - footer label
                    //PdfPTable FooterLabelTable = new PdfPTable(1);
                    //float[] FooterLabelTableWithCells = new float[] { 100f };
                    //FooterLabelTable.SetWidths(FooterLabelTableWithCells);
                    //FooterLabelTable.WidthPercentage = 100;
                    //FooterLabelTable.AddCell(new PdfPCell(new Phrase("NOT VALID UNLESS SIGNED", fontArial12Bold)) { Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                    //doc.Add(FooterLabelTable);

                    doc.Close();
                }

                // Others
                // ======
                else
                {
                    writer.PageEvent = new PDFHeaderFooter(ProcedureResults.First().TrnProcedure.MstUser.FullName, Id);

                    doc.Open();

                    // Header
                    // doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.FullName, TableHeaderFont));

                    doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.Address, SubHeaderFont));
                    doc.Add(new Paragraph(ProcedureResults.First().TrnProcedure.MstUser.ContactNumber, SubHeaderFont));

                    //// Detail - Header
                    //var HeaderTable = new PdfPTable(2);
                    //HeaderTable.HorizontalAlignment = 0;
                    //HeaderTable.SpacingBefore = 20;
                    //HeaderTable.SpacingAfter = 40;
                    //HeaderTable.DefaultCell.Border = 0;
                    //HeaderTable.SetWidths(new int[] { 2, 6 });
                    //HeaderTable.AddCell(new Phrase("Transaction No.:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.TransactionNumber, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Name:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.PatientName, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Age:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.Age.ToString(), BodyFont));
                    //HeaderTable.AddCell(new Phrase("Sex:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.Gender, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Procedure:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().MstModalityProcedure.ModalityProcedure, BodyFont));
                    //HeaderTable.AddCell(new Phrase("Transaction Date:", TableHeaderFont));
                    //HeaderTable.AddCell(new Phrase(ProcedureResults.First().TrnProcedure.TransactionDateTime.ToShortDateString(), BodyFont));
                    //doc.Add(HeaderTable);

                    doc.Add(line);
                    // Detail - Header
                    PdfPTable HeaderTable = new PdfPTable(4);
                    float[] HeaderTableWithCells = new float[] { 50f, 10f, 10f, 30f };
                    HeaderTable.SetWidths(HeaderTableWithCells);
                    HeaderTable.WidthPercentage = 100;
                    var nameLabelData = new Phrase();
                    nameLabelData.Add(new Chunk("NAME: \n", fontArial12Bold));
                    nameLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.PatientName, fontArial12));
                    var ageLabelData = new Phrase();
                    ageLabelData.Add(new Chunk("AGE: \n", fontArial12Bold));
                    ageLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Age.ToString(), fontArial12));
                    var sexLabelData = new Phrase();
                    sexLabelData.Add(new Chunk("SEX: \n", fontArial12Bold));
                    sexLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Gender, fontArial12));
                    var accessionLabelData = new Phrase();
                    accessionLabelData.Add(new Chunk("ACCESSION NO.: \n", fontArial12Bold));
                    accessionLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.TransactionNumber, fontArial12));
                    var addressLabelData = new Phrase();
                    addressLabelData.Add(new Chunk("ADDRESS: \n", fontArial12Bold));
                    addressLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.PatientAddress, fontArial12));
                    var hospitalNoLabelData = new Phrase();
                    hospitalNoLabelData.Add(new Chunk("HOSPITAL NO.: \n", fontArial12Bold));
                    hospitalNoLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.HospitalNumber, fontArial12));
                    var wardRoomLabelData = new Phrase();
                    wardRoomLabelData.Add(new Chunk("WARD/ROOM: \n", fontArial12Bold));
                    wardRoomLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.HospitalWardNumber, fontArial12));
                    var referredByLabelData = new Phrase();
                    referredByLabelData.Add(new Chunk("REFERRED BY: \n", fontArial12Bold));
                    referredByLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.ReferringPhysician, fontArial12));
                    var procedureLabelData = new Phrase();
                    procedureLabelData.Add(new Chunk("PROCEDURE: \n", fontArial12Bold));
                    procedureLabelData.Add(new Chunk(ProcedureResults.First().MstModalityProcedure.ModalityProcedure, fontArial12));
                    var dateOfExamLabelData = new Phrase();
                    dateOfExamLabelData.Add(new Chunk("DATE OF EXAM: \n", fontArial12Bold));
                    dateOfExamLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.StudyDate, fontArial12));
                    var clinicalInformationLabelData = new Phrase();
                    clinicalInformationLabelData.Add(new Chunk("CLINICAL INFORMATION: \n", fontArial12Bold));
                    clinicalInformationLabelData.Add(new Chunk(ProcedureResults.First().TrnProcedure.Particulars, fontArial12));
                    HeaderTable.AddCell(new PdfPCell(new Phrase(nameLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(ageLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(sexLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(accessionLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(addressLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(hospitalNoLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 2 });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(wardRoomLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(referredByLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(procedureLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 2 });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(dateOfExamLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    HeaderTable.AddCell(new PdfPCell(new Phrase(clinicalInformationLabelData)) { PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f, Colspan = 4 });
                    doc.Add(HeaderTable);

                    //PdfContentByte cb = writer.DirectContent;
                    //cb.MoveTo(0, 515);
                    //cb.LineTo(doc.PageSize.Width, 515);
                    //cb.Stroke();

                    // Detail - Result
                    var DetailTable = new PdfPTable(1);
                    DetailTable.HorizontalAlignment = 0;
                    DetailTable.SpacingAfter = 20;
                    DetailTable.DefaultCell.Border = 0;
                    DetailTable.SetWidths(new int[] { 6 });
                    DetailTable.WidthPercentage = 100;

                    PdfPCell detailCell1 = new PdfPCell(new Phrase(ProcedureResults.First().TrnProcedure.MstModality.Modality + " Result", TitleFont)) { HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_BOTTOM, PaddingTop = 20f };
                    detailCell1.Border = 0;

                    PdfPCell detailCell2 = new PdfPCell(new Phrase(ProcedureResults.First().Result, BodyFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    detailCell2.Border = 0;

                    DetailTable.AddCell(detailCell1);
                    DetailTable.AddCell(detailCell2);

                    doc.Add(DetailTable);

                    // Signature
                    Image SignatureImage = Image.GetInstance(HttpContext.Current.Server.MapPath("/Images/Signature/" + ProcedureResults.First().MstUser.UserName + ".png"));
                    SignatureImage.ScaleToFit(150f, 58f);

                    //doc.Add(SignatureImage);
                    //var SignatureTable = new PdfPTable(1);
                    //SignatureTable.HorizontalAlignment = 0;
                    //SignatureTable.SpacingBefore = 0;
                    //SignatureTable.SpacingAfter = 10;
                    //SignatureTable.SetWidths(new int[] { 6 });
                    //SignatureTable.WidthPercentage = 100;
                    //SignatureTable.DefaultCell.Border = 0;
                    ////PdfPCell cell1 = new PdfPCell(new Phrase("Radiologist:", TableHeaderFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    ////cell1.Border = 0;
                    //PdfPCell cell2 = new PdfPCell(new Phrase(ProcedureResults.First().MstUser.FullName, TableHeaderFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    ////cell2.FixedHeight = 50f;
                    //cell2.Border = 0;
                    //PdfPCell cell3 = new PdfPCell(new Phrase(ProcedureResults.First().MstUser.ContactNumber, BodyFont)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_BOTTOM };
                    //cell3.Border = 0;
                    ////SignatureTable.AddCell(cell1);
                    //SignatureTable.AddCell(cell2);
                    //SignatureTable.AddCell(cell3);
                    //doc.Add(SignatureTable);

                    Image signatureImage = Image.GetInstance(SignatureImage);
                    PdfPCell signatureCellImage = new PdfPCell(signatureImage);
                    // Detail - space
                    PdfPTable spaceTable = new PdfPTable(1);
                    float[] spaceWithCells = new float[] { 100f };
                    spaceTable.SetWidths(spaceWithCells);
                    spaceTable.WidthPercentage = 100;
                    spaceTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                    doc.Add(spaceTable);
                    // Detail - footer
                    PdfPTable FooterTable = new PdfPTable(3);
                    float[] FooterTableWithCells = new float[] { 40f, 20f, 40f };
                    FooterTable.SetWidths(FooterTableWithCells);
                    FooterTable.WidthPercentage = 100;
                    FooterTable.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Radiologist:", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(signatureCellImage) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(ProcedureResults.First().MstUser.FullName, TableHeaderFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ", fontArial12Bold)) { Border = 1, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(ProcedureResults.First().MstUser.ContactNumber, BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("OR No.:_________________________", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Release By:______________________", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Amount Paid: ____________________", BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    FooterTable.AddCell(new PdfPCell(new Phrase("Release Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), BodyFont)) { Border = 0, PaddingTop = 2f, PaddingBottom = 4f, PaddingLeft = 5f, PaddingRight = 5f });
                    doc.Add(FooterTable);
                    doc.Add(spaceTable);
                    // Detail - footer label
                    PdfPTable FooterLabelTable = new PdfPTable(1);
                    float[] FooterLabelTableWithCells = new float[] { 100f };
                    FooterLabelTable.SetWidths(FooterLabelTableWithCells);
                    FooterLabelTable.WidthPercentage = 100;
                    FooterLabelTable.AddCell(new PdfPCell(new Phrase("NOT VALID UNLESS SIGNED", fontArial12Bold)) { Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                    doc.Add(FooterLabelTable);

                    doc.Close();
                }
            }
            catch { }

            byte[] file = stream.ToArray();
            MemoryStream output = new MemoryStream();
            output.Write(file, 0, file.Length);
            output.Position = 0;

            return output;
        }

        public class PDFHeaderFooter : PdfPageEventHelper
        {
            public string DocumentTitle;
            public string Facility;
            private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();
            public PDFHeaderFooter(string DocumentTitle, Int32 Id)
            {
                var ProcedureResults = from d in db.TrnProcedureResults where d.Id == Id select d;
                this.Facility = ProcedureResults.First().TrnProcedure.MstUser.UserName;
                this.DocumentTitle = DocumentTitle;
            }
            // write on top of document
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                BaseFont BaseFontTimesRoman = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font TitleFont = new Font(BaseFontTimesRoman, 18, Font.BOLD);
                Font BodyFont = new Font(BaseFontTimesRoman, 10, Font.NORMAL);

                base.OnOpenDocument(writer, document);

                document.Add(new Paragraph(DocumentTitle, TitleFont));

                Image image = Image.GetInstance(HttpContext.Current.Server.MapPath("/Images/Logo/" + this.Facility + ".jpg"));
                //image.ScaleToFit(150f, 58f);
                image.ScaleToFit(200f, 100f);
                image.SetAbsolutePosition(430, 650);
                document.Add(image);

                PdfContentByte cb = writer.DirectContent;

                //cb.MoveTo(0, 690);
                //cb.LineTo(document.PageSize.Width, 690);
                //cb.Stroke();
            }

            //// write on start of each page
            //public override void OnStartPage(PdfWriter writer, Document document)
            //{
            //    base.OnStartPage(writer, document);
            //}

            //// write on end of each page
            //public override void OnEndPage(PdfWriter writer, Document document)
            //{
            //    BaseFont BaseFontTimesRoman = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            //    Font EndingMessageFont = new Font(BaseFontTimesRoman, 10, Font.ITALIC);

            //    base.OnEndPage(writer, document);

            //    PdfPTable FooterTable = new PdfPTable(1);
            //    FooterTable.TotalWidth = 200f;
            //    FooterTable.LockedWidth = true;

            //    PdfPCell PageCell = new PdfPCell(new Phrase("Page " + writer.PageNumber.ToString(), EndingMessageFont));
            //    PageCell.Border = 0;

            //    FooterTable.AddCell(PageCell);

            //    FooterTable.WriteSelectedRows(0, -1, 500, 40, writer.DirectContent);

            //    PdfContentByte cb = writer.DirectContent;

            //    cb.MoveTo(0, 50);
            //    cb.LineTo(document.PageSize.Width, 50);
            //    cb.Stroke();
            //}

            ////write on close of document
            //public override void OnCloseDocument(PdfWriter writer, Document document)
            //{
            //    base.OnCloseDocument(writer, document);
            //}
        }
        public class PDFHeaderFooterMargosatubig : PdfPageEventHelper
        {
            public string DocumentTitle;
            public string Facility;
            public string AddressOfFacility;
            public string ContactNoOfFacility;

            private Data.philimagexdbDataContext db = new Data.philimagexdbDataContext();

            public PDFHeaderFooterMargosatubig(string DocumentTitle, Int32 Id)
            {
                var ProcedureResults = from d in db.TrnProcedureResults where d.Id == Id select d;
                this.Facility = ProcedureResults.First().TrnProcedure.MstUser.UserName;
                this.DocumentTitle = DocumentTitle;
                this.AddressOfFacility = ProcedureResults.First().TrnProcedure.MstUser.Address;
                this.ContactNoOfFacility = ProcedureResults.First().TrnProcedure.MstUser.ContactNumber;
            }
            // write on top of document
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                BaseFont BaseFontTimesRoman = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font TitleFont = new Font(BaseFontTimesRoman, 16, Font.BOLD);
                Font BodyFont = new Font(BaseFontTimesRoman, 10, Font.NORMAL);

                base.OnOpenDocument(writer, document);

                var hp1 = new Paragraph("Republic of the Philippines", BodyFont);
                hp1.Alignment = Element.ALIGN_CENTER;
                document.Add(hp1);

                var hp2 = new Paragraph(DocumentTitle, TitleFont);
                hp2.Alignment = Element.ALIGN_CENTER;
                document.Add(hp2);

                var hp3 = new Paragraph(AddressOfFacility, BodyFont);
                hp3.Alignment = Element.ALIGN_CENTER;
                document.Add(hp3);

                var hp4 = new Paragraph(ContactNoOfFacility, BodyFont);
                hp4.Alignment = Element.ALIGN_CENTER;
                document.Add(hp4);

                Image image1 = Image.GetInstance(HttpContext.Current.Server.MapPath("/Images/Logo/" + this.Facility + ".jpg"));
                image1.ScaleToFit(200f, 100f);
                image1.SetAbsolutePosition(50, 670);
                document.Add(image1);

                Image image2 = Image.GetInstance(HttpContext.Current.Server.MapPath("/Images/Logo/" + this.Facility + "-rad.jpg"));
                image2.ScaleToFit(200f, 100f);
                image2.SetAbsolutePosition(470, 670);
                document.Add(image2);

                PdfContentByte cb = writer.DirectContent;

                //cb.MoveTo(0, 690);
                //cb.LineTo(document.PageSize.Width, 690);
                //cb.Stroke();
            }

            //// write on start of each page
            //public override void OnStartPage(PdfWriter writer, Document document)
            //{
            //    base.OnStartPage(writer, document);
            //}

            // write on end of each page
            //public override void OnEndPage(PdfWriter writer, Document document)
            //{
            //    BaseFont BaseFontTimesRoman = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            //    Font EndingMessageFont = new Font(BaseFontTimesRoman, 10, Font.ITALIC);

            //    base.OnEndPage(writer, document);

            //    PdfPTable FooterTable = new PdfPTable(1);
            //    FooterTable.TotalWidth = 200f;
            //    FooterTable.LockedWidth = true;

            //    PdfPCell PageCell = new PdfPCell(new Phrase("Page " + writer.PageNumber.ToString(), EndingMessageFont));
            //    PageCell.Border = 0;

            //    FooterTable.AddCell(PageCell);

            //    FooterTable.WriteSelectedRows(0, -1, 500, 40, writer.DirectContent);

            //    PdfContentByte cb = writer.DirectContent;

            //    cb.MoveTo(0, 50);
            //    cb.LineTo(document.PageSize.Width, 50);
            //    cb.Stroke();
            //}

            //write on close of document
            //public override void OnCloseDocument(PdfWriter writer, Document document)
            //{
            //    base.OnCloseDocument(writer, document);
            //}
        }
    }
}
