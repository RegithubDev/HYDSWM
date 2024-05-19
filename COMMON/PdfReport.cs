using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace COMMON
{
    public class PdfReport
    {
        public byte[] GenerateQRCode(Bitmap qr, string VehicleNo, string VehicleType, string Owner, string Zone, string Circle, string Ward, string UId, int OperationTypeId, string LandMark)
        {
            var pgSize = new iTextSharp.text.Rectangle(300, 400);
            Document doc = new Document(pgSize, 0, 0, 0, 0);
            MemoryStream stream = new MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, stream);
            pdfWriter.CloseStream = false;
            doc.Open();

            PdfPCell pcell;
            iTextSharp.text.pdf.PdfPTable datatable;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            //iTextSharp.text.Font stimes8 = new iTextSharp.text.Font(bfTimes, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font stimes8 = new iTextSharp.text.Font(bfTimes, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            datatable = new iTextSharp.text.pdf.PdfPTable(2);
            float[] widths = new float[] { 135f, 195f };
            datatable.TotalWidth = 330f;
            datatable.SetWidths(widths);
            datatable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            #region QrBody

            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);


            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            //datatable.AddCell(pcell);

            if (OperationTypeId != (int)Enums.OperationType.PRIMARY_COLLECTION)
            {
                pcell = new PdfPCell(new Phrase(" ", stimes8));
                pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                pcell.Colspan = 2;
                datatable.AddCell(pcell);

                pcell = new PdfPCell(new Phrase(" ", stimes8));
                pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                pcell.Colspan = 2;
                datatable.AddCell(pcell);

                pcell = new PdfPCell(new Phrase(" ", stimes8));
                pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                pcell.Colspan = 2;
                datatable.AddCell(pcell);

            }

            iTextSharp.text.Image pnglogoRight = iTextSharp.text.Image.GetInstance(qr, System.Drawing.Imaging.ImageFormat.Bmp);
            pnglogoRight.ScaleAbsolute(200f, 200f);
            pcell = new PdfPCell(pnglogoRight);
            pcell.HorizontalAlignment = Element.ALIGN_CENTER;
            pcell.Colspan = 2;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);


            //pcell = new PdfPCell(new Phrase("VEHICLE REG NO", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase(": " + VehicleNo.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase("VEHICLE TYPE", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase(": " + VehicleType.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase("OWNER", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase(": " + Owner.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase("ZONE", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase(": " + Zone.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase("CIRCLE", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase(": " + Circle.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase("WARD", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase(": " + Ward.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            int Length = 30;

            if (Owner.Length > Length)
                Owner = Owner.Substring(0, 19);
            //pcell = new PdfPCell(new Phrase("OWNER TYPE       :   " + Owner.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 3;
            //pcell.PaddingLeft = 55f;
            //datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase("     OWNER TYPE", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase(":    " + Owner.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);


            if (VehicleType.Length > Length)
                VehicleType = VehicleType.Substring(0, 19);
            //pcell = new PdfPCell(new Phrase("VEHICLE TYPE      :   " + VehicleType.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 3;
            //pcell.PaddingLeft = 55f;
            //datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase("     VEHICLE TYPE", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase(":    " + VehicleType.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);


            if (VehicleNo.Length > Length)
                VehicleNo = VehicleNo.Substring(0, 19);
            //pcell = new PdfPCell(new Phrase("VEH. REG. NO       :   " + VehicleNo.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 3;
            //pcell.PaddingLeft = 55f;
            //datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase("     VEH. REG. NO", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase(":    " + VehicleNo.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);




            if (OperationTypeId == (int)Enums.OperationType.PRIMARY_COLLECTION)
            {
                //LandMark = " ghhhhhh";
                string landmarkText = LandMark.ToUpper();
                if (LandMark.Length > Length)
                    LandMark = LandMark.Substring(0, 30);
                //pcell = new PdfPCell(new Phrase("VEH.ALLOC.LOC   :   " + LandMark.ToUpper(), stimes8));
                //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //pcell.Colspan = 3;
                //pcell.PaddingLeft = 55f;
                //datatable.AddCell(pcell);
                if(landmarkText.Length > 19)
                {
                    pcell = new PdfPCell(new Phrase("     VEH.ALLOC.LOC", stimes8));
                    pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pcell.Rowspan = 3;
                    datatable.AddCell(pcell);
                }

                else
                {
                    pcell = new PdfPCell(new Phrase("     VEH.ALLOC.LOC", stimes8));
                    pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //pcell.Rowspan = 3;
                    datatable.AddCell(pcell);
                }
                //pcell = new PdfPCell(new Phrase(":    " + LandMark.ToUpper(), stimes8));
                //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //pcell.Rowspan = 3;
                //datatable.AddCell(pcell);
                int cnt = 0;
                if (landmarkText.Length > 19)
                {
                    // If the text is longer than 19 characters, split it into multiple lines with proper formatting
                    string[] lines = SplitTextIntoLines(landmarkText, 19);
                    foreach (string line in lines)
                    {
                        cnt++;
                        if(cnt==1)
                        {
                            pcell = new PdfPCell(new Phrase(":    " + line, stimes8));
                        }
                        else
                        {
                            pcell = new PdfPCell(new Phrase("     " + line, stimes8));
                        }
                        
                        pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        // Rowspan should only be set for the last cell to avoid extra empty rows
                        if (line == lines[lines.Length - 1])
                        {
                            pcell.Rowspan = lines.Length;
                        }
                        datatable.AddCell(pcell);
                    }
                }
                else
                {
                    // If the text is not longer than 19 characters, create a single cell with the text
                    pcell = new PdfPCell(new Phrase(":    " + landmarkText, stimes8));
                    pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    datatable.AddCell(pcell);
                }

                if (Zone.Length > Length)
                    Zone = Zone.Substring(0, 19);
                //pcell = new PdfPCell(new Phrase("ZONE                     :   " + Zone.ToUpper(), stimes8));
                //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //pcell.Colspan = 3;
                //pcell.PaddingLeft = 55f;
                //datatable.AddCell(pcell);

                pcell = new PdfPCell(new Phrase("     ZONE", stimes8));
                pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                datatable.AddCell(pcell);

                pcell = new PdfPCell(new Phrase(":    " + Zone.ToUpper(), stimes8));
                pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                datatable.AddCell(pcell);


                if (Circle.Length > Length)
                    Circle = Circle.Substring(0, 19);
                //pcell = new PdfPCell(new Phrase("CIRCLE                  :   " + Circle.ToUpper(), stimes8));
                //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //pcell.Colspan = 3;
                //pcell.PaddingLeft = 55f;
                //datatable.AddCell(pcell);
                pcell = new PdfPCell(new Phrase("     CIRCLE", stimes8));
                pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                datatable.AddCell(pcell);

                pcell = new PdfPCell(new Phrase(":    " + Circle.ToUpper(), stimes8));
                pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                datatable.AddCell(pcell);



                if (Ward.Length > Length)
                    Ward = Ward.Substring(0, 19);
                //pcell = new PdfPCell(new Phrase("WARD                    :   " + Ward.ToUpper(), stimes8));
                //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //pcell.Colspan = 3;
                //pcell.PaddingLeft = 55f;
                //datatable.AddCell(pcell);
                pcell = new PdfPCell(new Phrase("     WARD", stimes8));
                pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                datatable.AddCell(pcell);

                pcell = new PdfPCell(new Phrase(":    " + Ward.ToUpper(), stimes8));
                pcell.HorizontalAlignment = Element.ALIGN_LEFT;
                pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                datatable.AddCell(pcell);

            }

            if (UId.Length > Length)
                UId = UId.Substring(0, 19);
            //pcell = new PdfPCell(new Phrase("UID                         :   " + UId.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 3;
            //pcell.PaddingLeft = 55f;
            //datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase("     UID", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase(":    " + UId.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);

            doc.Add(datatable);
            doc.Close();

            stream.Flush();
            stream.Position = 0;

            return stream.ToArray();
            #endregion 


        }
        public string[] SplitTextIntoLines(string text, int maxLineLength)
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < text.Length; i += maxLineLength)
            {
                int length = Math.Min(maxLineLength, text.Length - i);
                lines.Add(text.Substring(i, length));
            }
            return lines.ToArray();
        }
        public byte[] GenerateContainerQRCode(Bitmap qr, string ContainerCode, string ContainerType, string UId)
        {
            var pgSize = new iTextSharp.text.Rectangle(320, 400);
            Document doc = new Document(pgSize, 0, 0, 0, 0);
            MemoryStream stream = new MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, stream);
            pdfWriter.CloseStream = false;
            doc.Open();

            PdfPCell pcell;
            iTextSharp.text.pdf.PdfPTable datatable;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            iTextSharp.text.Font stimes8 = new iTextSharp.text.Font(bfTimes, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            datatable = new iTextSharp.text.pdf.PdfPTable(2);
            float[] widths = new float[] { 160f, 160f };
            datatable.TotalWidth = 320f;
            datatable.SetWidths(widths);
            datatable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            #region QrBody

            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);
            pcell = new PdfPCell(new Phrase(" ", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 2;
            datatable.AddCell(pcell);






            iTextSharp.text.Image pnglogoRight = iTextSharp.text.Image.GetInstance(qr, System.Drawing.Imaging.ImageFormat.Bmp);
            pnglogoRight.ScaleAbsolute(200f, 200f);
            pcell = new PdfPCell(pnglogoRight);
            pcell.HorizontalAlignment = Element.ALIGN_CENTER;
            pcell.Colspan = 2;
            pcell.PaddingRight = 10f;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);


            //pcell = new PdfPCell(new Phrase("CONTAINER CODE", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.PaddingLeft = 25f;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase(": " + ContainerCode.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase("CONTAINER TYPE", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.PaddingLeft = 25f;
            //datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase(": " + ContainerType.ToUpper(), stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase("CONTAINER CODE : " + ContainerCode.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 50f;
            datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase("CONTAINER TYPE : " + ContainerType.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 50f;
            datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase("UID : " + UId.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 50f;
            datatable.AddCell(pcell);





            doc.Add(datatable);
            doc.Close();

            stream.Flush();
            stream.Position = 0;

            return stream.ToArray();
            #endregion 


        }
        public byte[] ExportToPdfData(string txttbl, string Name, string FPath)
        {
            byte[] Response = null;
            try
            {
                string PrintDatetime = "Print: " + " " + " " + DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                string StrContent = "";
                System.Text.StringBuilder htmlContent = new System.Text.StringBuilder();
                string line;

                //StreamReader reader = new StreamReader(Server.MapPath("~/Files/sample-invoice.html"));
                using (System.IO.StreamReader htmlReader = new System.IO.StreamReader(FPath))
                {

                    while ((line = htmlReader.ReadLine()) != null)
                    {
                        htmlContent.Append(line);
                    }
                }


                StrContent = htmlContent.ToString();

                StrContent = StrContent.Replace("[RHeading]", Name);
                StrContent = StrContent.Replace("[PrintDate]", PrintDatetime);
                StrContent = StrContent.Replace("[lstTBody]", txttbl);

                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A3;
                converter.Options.MarginTop = 20;
                converter.Options.MarginRight = 10;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(StrContent);

                // save pdf document
                Response = doc.Save();


                doc.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Response;

        }
    }
}
