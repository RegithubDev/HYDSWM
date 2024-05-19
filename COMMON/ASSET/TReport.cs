using COMMON.ASSET;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using System.Text;
using SelectPdf;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text.Json;
using COMMON.DEPLOYMENT;
using Microsoft.AspNetCore.Http;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace COMMON.ASSET
{
    public class TReport
    {
        public byte[] ExportMAppDataLog(string result, string Name)
        {

            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 13].Value = Name; worksheet.Cells[1, 1, 2, 13].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Date"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "Operation Type"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Vehicle No"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Zone"; worksheet.Column(5).Width = 15;
                    worksheet.Cells[3, 6].Value = "TS/SCTP"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Owner Of The Vehicle"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Vehicle Type"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Container No"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Start Time At TS/SCTP"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "End Time At JN Site"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Distance"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "Trip No"; worksheet.Column(13).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 13];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("DatedOn").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("OperationType").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("TStationName").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("OwnerType").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("ContainerCode").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("StartTime").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("WBTDate").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("Distance").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("TotalTrip").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 13])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public byte[] ExportQRCodeScanAtSCTP(string result, string Name)
        {

            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 8].Value = Name; worksheet.Cells[1, 1, 2, 8].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Vehicle No"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "Driver Name"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Contact No"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Name of the contractor"; worksheet.Column(5).Width = 15;

                    worksheet.Cells[3, 6].Value = "Date and Time of Scanned"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Trip No for the Day"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "[QRCode] Label (masking Transaction No) for validation"; worksheet.Column(8).Width = 25;


                    var cells = worksheet.Cells[3, 1, 3, 8];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("DriverName").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("ContactNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("NameOfContractor").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("DateAndTimeofScanned").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("TripNoForday").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("QRCodeLabel").ToString();


                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 8])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public string ExportUserLogSummaryCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray()
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Login Id,Login Type,Status,Login Date & Time,Logout Date & Time,Total Active Time(In HH:MM)");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6}\n",
                        Count,
                        item.GetValue("LoginId").ToString().Replace(",", " "),
                        item.GetValue("LoginType").ToString().Replace(",", " "),
                        item.GetValue("Status").ToString().Replace(",", " "),
                        item.GetValue("LoginTDate").ToString().Replace(",", " "),
                        item.GetValue("LogoutTDate").ToString().Replace(",", " "),
                        item.GetValue("TotalActiveTime").ToString().Replace(",", " "));

                    //$"\"{item.GetValue("Address").ToString()}\"",
                    //item.GetValue("NetWt").ToString().Replace(",", " "),
                    //item.GetValue("AVGTon").ToString().Replace(",", " "));


                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public string ExportAllHKLWisePerfSummaryCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray()
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Hookloader No,Status,Total Trips,Total Working Time(In HH:MM),Total Waiting Time(In HH:MM),Total Weight Actual At WB(In MT),Average Ton/Trip(MT)");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}\n",
                        Count,
                        item.GetValue("VehicleNo").ToString().Replace(",", " "),
                        item.GetValue("Status").ToString().Replace(",", " "),
                        item.GetValue("TotalTrip").ToString().Replace(",", " "),
                        item.GetValue("WorkingHourInHHMM").ToString().Replace(",", " "),
                        item.GetValue("WaitingHourInHHMM").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("NetWt").ToString().Replace(",", " "),
                        item.GetValue("AVGTon").ToString().Replace(",", " "));


                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public string ExportAllContainerWisePerfSummaryCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray();
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Container No,Status,Total Trips,Total Primary Vehicles Loaded,Total Working Time(In HH:MM),Total Waiting Time(In HH:MM),Total Weight Estimated(In MT),Total Weight Actual At WB(In MT),Total Weight Variance");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n",
                        Count,
                        item.GetValue("ContainerCode").ToString().Replace(",", " "),
                        item.GetValue("Status").ToString().Replace(",", " "),
                        item.GetValue("TotalTrip").ToString().Replace(",", " "),
                        item.GetValue("TotalPV").ToString().Replace(",", " "),
                        item.GetValue("WorkingHourInHHMM").ToString().Replace(",", " "),
                        item.GetValue("WaitingHourInHHMM").ToString().Replace(",", " "),
                        item.GetValue("TotalWTPV").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("NetWt").ToString().Replace(",", " "),
                        item.GetValue("WTVariance").ToString().Replace(",", " "));


                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public string ExportMasterWBCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray();
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Vehicle No,Gross Wt.(In Kg),Tare Wt.(In Kg),Net Wt.(In Kg),Site ID,Transaction Date & Time,Material,Party,Transporter,Bill DC No,Bill Weight,In Date,User1, User2, Status, SW Site ID,Trip No,Shift No,Transfer Waste IE,Transfer Waste,Remarks,Manifest Number,Manifest Weight,Member Ship Code,In Gate Pass No,In Meter Reading,Out Gate Pass No,Out Meter Reading,Transfer ID,Type Of Waste,Total KMS Travelled,Billable Weight,Total Transport Charges");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33}\n",
                        Count,
                        item.GetValue("Vehicleno").ToString().Replace(",", " "),
                        item.GetValue("GrossWt").ToString().Replace(",", " "),
                        item.GetValue("TareWt").ToString().Replace(",", " "),
                        item.GetValue("Netwt").ToString().Replace(",", " "),
                        item.GetValue("SiteID").ToString().Replace(",", " "),
                        item.GetValue("TDate").ToString().Replace(",", " "),
                        item.GetValue("Material").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("Party").ToString().Replace(",", " "),
                        item.GetValue("Transporter").ToString().Replace(",", " "),
                        item.GetValue("BillDCno").ToString().Replace(",", " "),
                        item.GetValue("Billweight").ToString().Replace(",", " "),
                        item.GetValue("InDate").ToString().Replace(",", " "),
                        item.GetValue("User1").ToString().Replace(",", " "),
                        item.GetValue("User2").ToString().Replace(",", " "),
                        item.GetValue("Status").ToString().Replace(",", " "),
                        item.GetValue("SW_SiteID").ToString().Replace(",", " "),
                        item.GetValue("TripNo").ToString().Replace(",", " "),
                        item.GetValue("ShiftNo").ToString().Replace(",", " "),
                        item.GetValue("TransferWasteIE").ToString().Replace(",", " "),
                        item.GetValue("TransferWaste").ToString().Replace(",", " "),
                        item.GetValue("Remarks").ToString().Replace(",", " "),
                        item.GetValue("ManifestNumber").ToString().Replace(",", " "),
                        item.GetValue("ManifestWeight").ToString().Replace(",", " "),
                        item.GetValue("MembershipCode").ToString().Replace(",", " "),
                        item.GetValue("InGatePassNo").ToString().Replace(",", " "),
                        item.GetValue("InMeterReading").ToString().Replace(",", " "),
                        item.GetValue("OutGatePassNo").ToString().Replace(",", " "),
                        item.GetValue("OutMeterReading").ToString().Replace(",", " "),
                        item.GetValue("TransferID").ToString().Replace(",", " "),
                        item.GetValue("TypeOfWaste").ToString().Replace(",", " "),
                        item.GetValue("TotalKMSTravelled").ToString().Replace(",", " "),
                        item.GetValue("BillableWeight").ToString().Replace(",", " "),
                        item.GetValue("TotalTransportCharges").ToString().Replace(",", " "));

                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public byte[] GenerateQRCode(string VehicleNo, string DriverName, string ContactNo, string NameOfContractor, string DateAndTimeofScanned, int TripNoForday, string QRCodeLabel, Bitmap QrCode, string TStationName)
        {
            var pgSize = new iTextSharp.text.Rectangle(300, 250);
            //pgSize.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            //pgSize.BorderWidth = 5;
            //pgSize.BorderColor = new BaseColor(0, 0, 0);
            Document doc = new Document(pgSize, 0, 0, 0, 0);
            MemoryStream stream = new MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, stream);
            pdfWriter.CloseStream = false;
            doc.Open();

            //iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(10, 10, 150, 15, 0);

            PdfPCell pcell;
            iTextSharp.text.pdf.PdfPTable datatable;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            iTextSharp.text.Font stimes8 = new iTextSharp.text.Font(bfTimes, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            datatable = new iTextSharp.text.pdf.PdfPTable(2);
            float[] widths = new float[] { 135f, 195f };
            datatable.TotalWidth = 330f;
            datatable.SetWidths(widths);
            //datatable.DefaultCell.BorderWidth = iTextSharp.text.Rectangle.TOP_BORDER;
            //datatable.DefaultCell.BorderWidth = iTextSharp.text.Rectangle.LEFT_BORDER;



            // Paragraph paragraph = new Paragraph("    Acknowledgment Reciept");
            //// iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance("");
            // //Resize image depend upon your need
            // //jpg.ScaleToFit(100f, 120f);
            // ////Give space before image
            // //jpg.SpacingBefore = 10f;
            // paragraph.SpacingAfter = 10f;
            // //Give some space after the image
            // //jpg.SpacingAfter = 10f;
            // paragraph.SpacingBefore = 10f;

            //// jpg.Alignment = Element.ALIGN_LEFT;
            // paragraph.Alignment = Element.ALIGN_LEFT;
            //// pcell = new PdfPCell(jpg);
            //// datatable.AddCell(pcell);
            // datatable.AddCell(paragraph);

            pcell = new PdfPCell(new Phrase("Acknowledgment Reciept", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_CENTER;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 5f;
            datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase("  ", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_TOP;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 2;
            //pcell = new PdfPCell(new Phrase("  ", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_TOP;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 2;
            //pcell = new PdfPCell(new Phrase("  ", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_TOP;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 2;
            //pcell = new PdfPCell(new Phrase("  ", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_TOP;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 2;
            //pcell = new PdfPCell(new Phrase("  " + "", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 3;
            //pcell.PaddingLeft = 35f;
            //datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase("  " + "", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 35f;
            datatable.AddCell(pcell);


            pcell = new PdfPCell(new Phrase("TS : " + TStationName.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            // pcell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
            pcell.BorderColorBottom = BaseColor.GREEN;

            pcell.Colspan = 3;
            pcell.PaddingLeft = 10f;
            datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase("Vehicle No : " + VehicleNo.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            //pcell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;

            //pcell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 10f;
            datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase("Driver Name : " + DriverName.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 10f;
            datatable.AddCell(pcell);


            pcell = new PdfPCell(new Phrase("Contact No : " + ContactNo.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 10f;
            datatable.AddCell(pcell);

            pcell = new PdfPCell(new Phrase("Name of the contractor : " + NameOfContractor.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 10f;
            datatable.AddCell(pcell);



            pcell = new PdfPCell(new Phrase("Date and Time of Scanned : " + DateAndTimeofScanned.ToString(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 10f;
            datatable.AddCell(pcell);


            pcell = new PdfPCell(new Phrase("Trip No for the Day : " + TripNoForday, stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            pcell.Colspan = 3;
            pcell.PaddingLeft = 10f;
            datatable.AddCell(pcell);


            pcell = new PdfPCell(new Phrase("Transaction Id : " + QRCodeLabel.ToUpper(), stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            //pcell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            pcell.Colspan = 2;
            pcell.PaddingLeft = 10f;
            datatable.AddCell(pcell);

            //pcell = new PdfPCell(new Phrase("  " + "", stimes8));
            //pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            //pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pcell.Colspan = 3;
            //pcell.PaddingLeft = 35f;
            //datatable.AddCell(pcell);


            #region QRCode

            iTextSharp.text.Image pnglogoRight = iTextSharp.text.Image.GetInstance(QrCode, System.Drawing.Imaging.ImageFormat.Bmp);
            pnglogoRight.ScaleAbsolute(80f, 80f);
            pcell = new PdfPCell(pnglogoRight);
            pcell.HorizontalAlignment = Element.ALIGN_CENTER;
            pcell.Colspan = 2;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            datatable.AddCell(pcell);







            pcell = new PdfPCell(new Phrase("System Generated Reciept", stimes8));
            pcell.HorizontalAlignment = Element.ALIGN_CENTER;
            pcell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            pcell.Colspan = 2;
            datatable.AddCell(pcell);

            #endregion

            doc.Add(datatable);
            doc.Close();

            stream.Flush();
            stream.Position = 0;

            return stream.ToArray();



        }

        public string ExportArvlEntityCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray()
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Date & Time,Zone,Transfer Station Name,Entity Type,Entity No");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    string EType = "";
                    if (item.GetValue("UIdType").ToString() == "CONTAINER")
                    {
                        EType = item.GetValue("UIdType").ToString().Replace(",", " ");
                    }
                    else
                    {
                        EType = item.GetValue("EntityType").ToString().Replace(",", " ");

                    }
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5}\n",
                        Count,
                        item.GetValue("CreatedOn").ToString().Replace(",", " "),
                        item.GetValue("ZoneNo").ToString().Replace(",", " "),
                        item.GetValue("TStationName").ToString().Replace(",", " "),
                        EType,


                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("EntityCode").ToString().Replace(",", " "));


                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public string ExportAllOperationSummaryCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray();
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Dated On,Operation Type,Vehicle No,Zone,TS/SCTP,Owner Of The Vehicle,Vehicle Type,Container No,Start Time At TS/SCTP,End Time At JN Site,Distance,Trip No");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}\n",
                        Count,
                        item.GetValue("DatedOn").ToString().Replace(",", " "),
                        item.GetValue("OperationType").ToString().Replace(",", " "),
                        item.GetValue("VehicleNo").ToString().Replace(",", " "),
                        item.GetValue("ZoneNo").ToString().Replace(",", " "),
                        item.GetValue("TStationName").ToString().Replace(",", " "),
                        item.GetValue("OwnerType").ToString().Replace(",", " "),
                        item.GetValue("VehicleType").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("ContainerCode").ToString().Replace(",", " "),
                        item.GetValue("StartTime").ToString().Replace(",", " "),
                        item.GetValue("WBTDate").ToString().Replace(",", " "),
                        item.GetValue("Distance").ToString().Replace(",", " "),
                        item.GetValue("TotalTrip").ToString().Replace(",", " "));

                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public string ExportAllEntityDeploymentCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            JArray _lst1 = new JArray();
            JArray _lstalrt = new JArray();

            if (!string.IsNullOrEmpty(result))
            {
                dynamic FResult = JObject.Parse(result);
                _lst1 = FResult.Table;
                _lstalrt = FResult.Table1;
            }

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Entity No,Entity Type,Zone,Circle,Ward,Address,Description,Owner Type,Deployment Date & Time,Actual Shift,Deployed By");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n",
                        Count,
                        item.GetValue("EntityNo").ToString().Replace(",", " "),
                        item.GetValue("VEntityNo").ToString().Replace(",", " "),
                        item.GetValue("ZoneNo").ToString().Replace(",", " "),
                        item.GetValue("CircleName").ToString().Replace(",", " "),
                        item.GetValue("WardNo").ToString().Replace(",", " "),
                        item.GetValue("Address").ToString().Replace(",", " "),
                        item.GetValue("Remarks").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("OwnerType").ToString().Replace(",", " "),
                        item.GetValue("CreatedOn").ToString().Replace(",", " "),
                        item.GetValue("ShiftName").ToString().Replace(",", " "),
                        item.GetValue("CreatedBy").ToString().Replace(",", " "));

                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public string ExportPVehicleWiseCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray()
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Date & Time,Zone,Transfer Station Name,Vehicle No,Owner Type,Type Of Vehicle,Percentage(%) of waste,Waste Type");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {

                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n",
                        Count,
                        item.GetValue("TDate").ToString().Replace(",", " "),
                        item.GetValue("ZoneNo").ToString().Replace(",", " "),
                        item.GetValue("TStationName").ToString().Replace(",", " "),
                        item.GetValue("VehicleNo").ToString().Replace(",", " "),
                        item.GetValue("OwnerType").ToString().Replace(",", " "),
                        item.GetValue("VehicleType").ToString().Replace(",", " "),
                        item.GetValue("FilledPerc").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("WasteType").ToString().Replace(",", " "));


                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public string ExportAllGeoPointsVisitCSV(string lst, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            JArray _lst1 = new JArray();
            JArray _lstalrt = new JArray();

            if (!string.IsNullOrEmpty(lst))
            {
                dynamic FResult = JObject.Parse(lst);
                _lst1 = FResult.Table;
                _lstalrt = FResult.Table1;
            }
            //JArray _lst1 = JArray.Parse(lst);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Zone,Circle,Ward,Point Name,Category, Point Status, Total Visit, Last Visit Date,Days Lapsed, Cordinates");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},\n",
                        Count,
                        item.GetValue("ZoneNo").ToString().Replace(",", " "),
                        item.GetValue("CircleName").ToString().Replace(",", " "),
                        item.GetValue("WardNo").ToString().Replace(",", " "),
                        item.GetValue("GeoPointId").ToString().Replace(",", " "),
                        item.GetValue("GeoPointCategory").ToString().Replace(",", " "),
                        item.GetValue("GeoPointStatus").ToString().Replace(",", " "),
                        item.GetValue("TotalVisit").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("PDate").ToString().Replace(",", " "),
                        item.GetValue("GeoPointLifecycle").ToString().Replace(",", " "),
                        item.GetValue("GeoCordinate").ToString().Replace(",", " "));


                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public byte[] ExportGetAllStaffComplaint_Paging(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 15].Value = Name; worksheet.Cells[1, 1, 2, 15].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Complaint No"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "Complaint Category"; worksheet.Column(3).Width = 15;
                    worksheet.Cells[3, 4].Value = "Location"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "GPS Location"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Description"; worksheet.Column(6).Width = 15;
                    worksheet.Cells[3, 7].Value = "Created By"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Complaint Created On"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Complaint Closure On"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Actual Duration(Hr:Min)"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Status"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Close Time Location"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "Action Remarks"; worksheet.Column(13).Width = 25;
                    worksheet.Cells[3, 14].Value = "Closed By"; worksheet.Column(14).Width = 25;
                    worksheet.Cells[3, 15].Value = "Transfer Station"; worksheet.Column(15).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 15];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("Complaintcode").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("ComplaintType").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("Location").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("FAddress").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("Remarks").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("CreatedBy").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("ComplaintOn").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("ClosedOn").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("TotalTimeTaken").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("Status").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("CAddress").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("CRemarks").ToString();
                        worksheet.Cells[RowCount, 14].Value = item.GetValue("ClosedBy").ToString();
                        worksheet.Cells[RowCount, 15].Value = item.GetValue("TStationName").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 15].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 15].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 15])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportContainerWisePerfData(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 10].Value = Name; worksheet.Cells[1, 1, 2, 10].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Container No"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "Status"; worksheet.Column(3).Width = 15;
                    worksheet.Cells[3, 4].Value = "Total Trips"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Total Primary Vehicles Loaded"; worksheet.Column(5).Width = 25;
                    // worksheet.Cells[3, 4].Value = "CO"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 6].Value = "Total Working Time(In HH:MM)"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Total Waiting Time(In HH:MM)"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Total Weight Estimated(In MT)"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Total Weight Actual At WB(In MT)"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Total Weight Variance"; worksheet.Column(10).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 10];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("ContainerCode").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("Status").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TotalTrip").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("TotalPV").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("WorkingHourInHHMM").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("WaitingHourInHHMM").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("TotalWTPV").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("NetWt").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("WTVariance").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 10])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public string ExportAllGeoPointNotCollected_PagingCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray();
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Zone ,Circle,Ward,Point Name,Category,Point Status, Last Visit Date, Cordinates");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n",
                        Count,
                        item.GetValue("ZoneNo").ToString().Replace(",", " "),
                        item.GetValue("CircleName").ToString().Replace(",", " "),
                        item.GetValue("WardNo").ToString().Replace(",", " "),
                        item.GetValue("GeoPointName").ToString().Replace(",", " "),
                        item.GetValue("GeoPointCategory").ToString().Replace(",", " "),
                        item.GetValue("GeoPointStatus").ToString().Replace(",", " "),
                        item.GetValue("PDate").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("GeoCordinate").ToString().Replace(",", " "));


                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public byte[] ExportAllEntityDeployment(string lst, string Name)
        {
            dynamic FResult = JObject.Parse(lst);
            JArray jresult = FResult.Table;
            JArray uhfrowdata = FResult.Table1;
            List<VDepInfo> _lstuhfdata = uhfrowdata.ToObject<List<VDepInfo>>();
            byte[] Response = null;
            // JArray jresult = JArray.Parse(lst);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name + " -Details" + DateTime.Now.ToString("MMMM");

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 12].Value = Name; worksheet.Cells[1, 1, 2, 12].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Entity No"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "Entity Type"; worksheet.Column(3).Width = 15;
                    worksheet.Cells[3, 4].Value = "Zone"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Circle"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Ward"; worksheet.Column(6).Width = 22;
                    worksheet.Cells[3, 7].Value = "Address"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Description"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Owner Type"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Deployment Date & Time"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Actual Shift"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Deployed By"; worksheet.Column(12).Width = 25;



                    var cells = worksheet.Cells[3, 1, 3, 12];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {
                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("EntityNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("VEntityNo").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("Address").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("Remarks").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("OwnerType").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("CreatedOn").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("ShiftName").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("CreatedBy").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        RowCount++;

                        //List<VDepInfo> iuhfdata = new List<VDepInfo>();
                        //if (item.GetValue("EntityType").ToString() == "CONTAINER")
                        //{
                        //    iuhfdata = _lstuhfdata.Where(i => i.UIdType == "CONTAINER" && i.CreatedDate.Date == Convert.ToDateTime(item.GetValue("CreatedDate").ToString()).Date && i.UId == item.GetValue("UId").ToString())
                        //                                    .ToList();
                        //}
                        //else
                        //{
                        //    iuhfdata = _lstuhfdata.Where(i => i.UIdType == "VEHICLE" && i.CreatedDate.Date == Convert.ToDateTime(item.GetValue("CreatedDate").ToString()).Date && i.UId == item.GetValue("UId").ToString())
                        //                                   .ToList();
                        //}
                        //int IUhfCount = 1;
                        //foreach (VDepInfo idata in iuhfdata)
                        //{
                        //    // ExcelRange rng = worksheet.ProtectedRanges(worksheet.Cells[3, 1], worksheet.Cells[3, 20]);// worksheet.Cells[3, 1, 3, 20]

                        //    //worksheet.Row(RowCount).Collapsed = false;

                        //    if (IUhfCount == 1)
                        //    {

                        //        worksheet.Cells[RowCount, 2].Value = "Sr. No.";
                        //        worksheet.Cells[RowCount, 3].Value = "Transfer Station Name";
                        //        worksheet.Cells[RowCount, 4].Value = "Operation Type";

                        //        worksheet.Cells[RowCount, 5].Value = "Entity Type";
                        //        worksheet.Cells[RowCount, 6].Value = "Entity No";
                        //        worksheet.Cells[RowCount, 7].Value = "Date & Time";

                        //        var Icells = worksheet.Cells[RowCount, 2, RowCount, 7];
                        //        var Ifill = Icells.Style.Fill;
                        //        //cells.AutoFitColumns();
                        //        Icells.Style.Locked = true;
                        //        Icells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        //        Icells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //        Icells.Style.Font.Bold = true;
                        //        Icells.Style.WrapText = true;
                        //        Icells.Style.ShrinkToFit = true;

                        //        Ifill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //        Ifill.BackgroundColor.SetColor(Color.FromArgb(189, 183, 107));

                        //        var Iborder = Icells.Style.Border;
                        //        Iborder.Top.Style = Iborder.Left.Style = Iborder.Bottom.Style = Iborder.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        //        worksheet.Row(RowCount).OutlineLevel = 1;
                        //        worksheet.Row(RowCount).Collapsed = true;
                        //        RowCount++;
                        //    }
                        //    worksheet.Row(RowCount).OutlineLevel = 1;
                        //    worksheet.Row(RowCount).Collapsed = true;


                        //    worksheet.Cells[RowCount, 2].Value = IUhfCount;
                        //    worksheet.Cells[RowCount, 3].Value = idata.TStationName;
                        //    worksheet.Cells[RowCount, 4].Value = idata.OperationType;

                        //    worksheet.Cells[RowCount, 5].Value = idata.EntityType;
                        //    worksheet.Cells[RowCount, 6].Value = idata.EntityNo;
                        //    worksheet.Cells[RowCount, 7].Value = idata.CreatedOn;


                        //    worksheet.Cells[RowCount, 1, RowCount, 7].Style.WrapText = true;
                        //    worksheet.Cells[RowCount, 1, RowCount, 7].Style.ShrinkToFit = true;
                        //    worksheet.Cells[RowCount, 1, RowCount, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //    worksheet.Cells[RowCount, 1, RowCount, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 228, 196));
                        //    worksheet.Cells[RowCount, 1, RowCount, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //    worksheet.Cells[RowCount, 1, RowCount, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        //    RowCount++;
                        //    IUhfCount++;
                        //}

                        Count++;

                    }
                    #endregion

                    #region formatting
                    //using (var range = worksheet.Cells[3, 1, 3, 20])
                    //{
                    //    // Setting bold font
                    //    range.Style.Font.Bold = true;
                    //    // Setting fill type solid
                    //    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //    // Setting background gray
                    //    range.Style.Fill.BackgroundColor.SetColor(Color.Goldenrod);
                    //    // Setting font color
                    //    range.Style.Font.Color.SetColor(Color.Black);

                    //    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    //    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //    range.Style.Font.Size = 12;
                    //    range.Style.WrapText = true;
                    //    range.Style.ShrinkToFit = true;
                    //    range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    //}
                    using (var range = worksheet.Cells[1, 1, 2, 10])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }

                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public string ExportWeightBridgeSummaryCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray();
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Weighbridge Id,WB Trans. Code,Transaction Date & Time,Status,TS/SCTP,Vehicle No,Container No,Gross Weight(In MT),Tare Weight(In MT),Net Weight(In MT),Total Est. Weight(In MT),Total Weight Variance,Sync Date & Time");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}\n",
                        Count,
                        item.GetValue("WBId").ToString().Replace(",", " "),
                        item.GetValue("WTCode").ToString().Replace(",", " "),
                        item.GetValue("TDate").ToString().Replace(",", " "),
                        item.GetValue("Status").ToString().Replace(",", " "),
                        item.GetValue("TCode").ToString().Replace(",", " "),
                        item.GetValue("EntityNo").ToString().Replace(",", " "),
                        item.GetValue("ContainerCode").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("GrossWt").ToString().Replace(",", " "),
                        item.GetValue("TareWt").ToString().Replace(",", " "),
                        item.GetValue("NetWt").ToString().Replace(",", " "),
                        item.GetValue("TotalWTPV").ToString().Replace(",", " "),
                        item.GetValue("WTVariance").ToString().Replace(",", " "),
                        item.GetValue("CreatedDate").ToString().Replace(",", " "));

                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public string ExportAllCollectionCSV(string result, string name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            // JArray _lst1 = new JArray();
            //JArray _lstalrt = new JArray();
            JArray _lst1 = JArray.Parse(result);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Vehicle No,Route Id,Trip,Point Name,Status,Address,Collection Date & Time,Before Photo, After Photo,Cordinates,Shift Name,Shift Date");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}\n",
                        Count,
                        item.GetValue("VehicleNo").ToString().Replace(",", " "),
                        item.GetValue("RouteCode").ToString().Replace(",", " "),
                        item.GetValue("TripName").ToString().Replace(",", " "),
                        item.GetValue("PointName").ToString().Replace(",", " "),
                        item.GetValue("Status").ToString().Replace(",", " "),
                        item.GetValue("Address").ToString().Replace(",", " "),
                        item.GetValue("PickDTime").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("Img1Url").ToString().Replace(",", " "),
                        item.GetValue("Img2Url").ToString().Replace(",", " "),
                        item.GetValue("GeoCordinate").ToString().Replace(",", " "),
                        item.GetValue("ShiftName").ToString().Replace(",", " "),
                        item.GetValue("ShiftDate").ToString().Replace(",", " "));


                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }







        //public byte[] ExportAllVDeployment(string lst, string Name)//ChangeS for seprate summary
        //{
        //    dynamic FResult = JObject.Parse(lst);
        //    JArray jresult = FResult.Table;
        //    JArray uhfrowdata = FResult.Table1;
        //    List<VDepInfo> _lstuhfdata = uhfrowdata.ToObject<List<VDepInfo>>();
        //    byte[] Response = null;
        //    // JArray jresult = JArray.Parse(lst);
        //    try
        //    {
        //        using (ExcelPackage excel = new ExcelPackage())
        //        {
        //            int Count = 1;
        //            int RowCount = 4;
        //            excel.Workbook.Properties.Author = "Ajeevi Technologies";
        //            excel.Workbook.Properties.Title = Name + " -Details" + DateTime.Now.ToString("MMMM");

        //            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

        //            #region Creating Header
        //            worksheet.Cells[1, 1, 1, 11].Value = Name; worksheet.Cells[1, 1, 2, 11].Merge = true;


        //            worksheet.Cells[3, 1].Value = "Sr No.";
        //            worksheet.Cells[3, 2].Value = "Vehicle No"; worksheet.Column(2).Width = 15;
        //            worksheet.Cells[3, 3].Value = "Vehicle Type"; worksheet.Column(3).Width = 15;
        //            worksheet.Cells[3, 4].Value = "Zone"; worksheet.Column(4).Width = 15;
        //            worksheet.Cells[3, 5].Value = "Circle"; worksheet.Column(5).Width = 25;
        //            worksheet.Cells[3, 6].Value = "Ward"; worksheet.Column(6).Width = 22;
        //            worksheet.Cells[3, 7].Value = "LandMark"; worksheet.Column(7).Width = 25;
        //            worksheet.Cells[3, 8].Value = "Address"; worksheet.Column(8).Width = 25;
        //            worksheet.Cells[3, 9].Value = "Owner Type"; worksheet.Column(9).Width = 25;
        //            worksheet.Cells[3, 10].Value = "Deployment Date & Time"; worksheet.Column(10).Width = 25;
        //            worksheet.Cells[3, 11].Value = "Deployed By"; worksheet.Column(11).Width = 25;
        //            //worksheet.Cells[3, 12].Value = "Deployed By"; worksheet.Column(12).Width = 25;



        //            var cells = worksheet.Cells[3, 1, 3, 11];
        //            var fill = cells.Style.Fill;
        //            //cells.AutoFitColumns();
        //            cells.Style.Locked = true;
        //            cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //            cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            cells.Style.Font.Bold = true;
        //            cells.Style.WrapText = true;
        //            cells.Style.ShrinkToFit = true;

        //            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

        //            var border = cells.Style.Border;
        //            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //            #endregion



        //            #region filling data

        //            foreach (JObject item in jresult)
        //            {
        //                worksheet.Cells[RowCount, 1].Value = Count;
        //                worksheet.Cells[RowCount, 2].Value = item.GetValue("EntityNo").ToString();
        //                worksheet.Cells[RowCount, 3].Value = item.GetValue("VEntityNo").ToString();
        //                worksheet.Cells[RowCount, 4].Value = item.GetValue("ZoneNo").ToString();
        //                worksheet.Cells[RowCount, 5].Value = item.GetValue("CircleName").ToString();
        //                worksheet.Cells[RowCount, 6].Value = item.GetValue("WardNo").ToString();
        //                worksheet.Cells[RowCount, 7].Value = item.GetValue("LandMark").ToString();
        //                worksheet.Cells[RowCount, 8].Value = item.GetValue("Address").ToString();
        //                worksheet.Cells[RowCount, 9].Value = item.GetValue("OwnerType").ToString();
        //                worksheet.Cells[RowCount, 10].Value = item.GetValue("CreatedOn").ToString();
        //                worksheet.Cells[RowCount, 11].Value = item.GetValue("CreatedBy").ToString();
        //                //worksheet.Cells[RowCount, 12].Value = item.GetValue("CreatedBy").ToString();

        //                worksheet.Cells[RowCount, 1, RowCount, 11].Style.WrapText = true;
        //                worksheet.Cells[RowCount, 1, RowCount, 11].Style.ShrinkToFit = true;
        //                worksheet.Cells[RowCount, 1, RowCount, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                worksheet.Cells[RowCount, 1, RowCount, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                RowCount++;

        //                List<VDepInfo> iuhfdata = new List<VDepInfo>();
        //                if (item.GetValue("EntityType").ToString() == "CONTAINER")
        //                {
        //                    iuhfdata = _lstuhfdata.Where(i => i.UIdType == "CONTAINER" && i.CreatedDate.Date == Convert.ToDateTime(item.GetValue("CreatedDate").ToString()).Date && i.UId == item.GetValue("UId").ToString())
        //                                                    .ToList();
        //                }
        //                else
        //                {
        //                    iuhfdata = _lstuhfdata.Where(i => i.UIdType == "VEHICLE" && i.CreatedDate.Date == Convert.ToDateTime(item.GetValue("CreatedDate").ToString()).Date && i.UId == item.GetValue("UId").ToString())
        //                                                   .ToList();
        //                }
        //                int IUhfCount = 1;
        //                foreach (VDepInfo idata in iuhfdata)
        //                {
        //                    // ExcelRange rng = worksheet.ProtectedRanges(worksheet.Cells[3, 1], worksheet.Cells[3, 20]);// worksheet.Cells[3, 1, 3, 20]

        //                    //worksheet.Row(RowCount).Collapsed = false;

        //                    if (IUhfCount == 1)
        //                    {

        //                        worksheet.Cells[RowCount, 2].Value = "Sr. No.";
        //                        worksheet.Cells[RowCount, 3].Value = "Transfer Station Name";
        //                        worksheet.Cells[RowCount, 4].Value = "Operation Type";

        //                        worksheet.Cells[RowCount, 5].Value = "Entity Type";
        //                        worksheet.Cells[RowCount, 6].Value = "Entity No";
        //                        worksheet.Cells[RowCount, 7].Value = "Date & Time";

        //                        var Icells = worksheet.Cells[RowCount, 2, RowCount, 7];
        //                        var Ifill = Icells.Style.Fill;
        //                        //cells.AutoFitColumns();
        //                        Icells.Style.Locked = true;
        //                        Icells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //                        Icells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                        Icells.Style.Font.Bold = true;
        //                        Icells.Style.WrapText = true;
        //                        Icells.Style.ShrinkToFit = true;

        //                        Ifill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                        Ifill.BackgroundColor.SetColor(Color.FromArgb(189, 183, 107));

        //                        var Iborder = Icells.Style.Border;
        //                        Iborder.Top.Style = Iborder.Left.Style = Iborder.Bottom.Style = Iborder.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //                        worksheet.Row(RowCount).OutlineLevel = 1;
        //                        worksheet.Row(RowCount).Collapsed = true;
        //                        RowCount++;
        //                    }
        //                    worksheet.Row(RowCount).OutlineLevel = 1;
        //                    worksheet.Row(RowCount).Collapsed = true;


        //                    worksheet.Cells[RowCount, 2].Value = IUhfCount;
        //                    worksheet.Cells[RowCount, 3].Value = idata.TStationName;
        //                    worksheet.Cells[RowCount, 4].Value = idata.OperationType;

        //                    worksheet.Cells[RowCount, 5].Value = idata.EntityType;
        //                    worksheet.Cells[RowCount, 6].Value = idata.EntityNo;
        //                    worksheet.Cells[RowCount, 7].Value = idata.CreatedOn;


        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.WrapText = true;
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.ShrinkToFit = true;
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 228, 196));
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                    RowCount++;
        //                    IUhfCount++;
        //                }

        //                Count++;

        //            }
        //            #endregion

        //            #region formatting
        //            //using (var range = worksheet.Cells[3, 1, 3, 20])
        //            //{
        //            //    // Setting bold font
        //            //    range.Style.Font.Bold = true;
        //            //    // Setting fill type solid
        //            //    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            //    // Setting background gray
        //            //    range.Style.Fill.BackgroundColor.SetColor(Color.Goldenrod);
        //            //    // Setting font color
        //            //    range.Style.Font.Color.SetColor(Color.Black);

        //            //    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            //    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            //    range.Style.Font.Size = 12;
        //            //    range.Style.WrapText = true;
        //            //    range.Style.ShrinkToFit = true;
        //            //    range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
        //            //}
        //            using (var range = worksheet.Cells[1, 1, 2, 11])
        //            {
        //                // Setting bold font
        //                range.Style.Font.Bold = true;
        //                // Setting fill type solid
        //                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                // Setting background gray
        //                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

        //                // Setting font color
        //                range.Style.Font.Color.SetColor(Color.Black);

        //                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                range.Style.Font.Size = 12;
        //                range.Style.WrapText = true;
        //                range.Style.ShrinkToFit = true;
        //                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                var border1 = range.Style.Border;
        //                border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            }

        //            worksheet.PrinterSettings.FitToPage = true;
        //            worksheet.PrinterSettings.ShowGridLines = true;

        //            #endregion

        //            Response = excel.GetAsByteArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return Response;
        //}
        public byte[] ExportAllVDeployment(string lst, string Name)
        {
            byte[] Response = null;
            //JArray jresult = JArray.Parse(result);
            JArray _lst1 = new JArray();
            JArray _lst = new JArray();
            JArray _lstalrt = new JArray();
            // JArray _lst1;
            // JArray _lstalrt ;
            if (!string.IsNullOrEmpty(lst))
            {
                dynamic FResult = JObject.Parse(lst);
                _lst1 = FResult.Table;
                _lstalrt = FResult.Table1;
            }
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);
                    //ExcelWorksheet worksheet1 = excel.Workbook.Worksheets.Add("SAT Trips Report");

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 12].Value = Name; worksheet.Cells[1, 1, 2, 12].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Vehicle No"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "SAT UId"; worksheet.Column(3).Width = 15;
                    worksheet.Cells[3, 4].Value = "Vehicle Type"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Zone"; worksheet.Column(5).Width = 15;
                    worksheet.Cells[3, 6].Value = "Circle"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Ward"; worksheet.Column(7).Width = 22;
                    worksheet.Cells[3, 8].Value = "LandMark"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Address"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Owner Type"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Deployment Date & Time"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Deployed By"; worksheet.Column(12).Width = 25;
                    //worksheet.Cells[3, 12].Value = "Deployed By"; worksheet.Column(12).Width = 25;



                    var cells = worksheet.Cells[3, 1, 3, 12];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in _lst1)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("EntityNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("UId").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("VEntityNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("LandMark").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("Address").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("OwnerType").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("CreatedOn").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("CreatedBy").ToString();

                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 12])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion



                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 12])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }





            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public string ExportAllVDeploymentToCSV(string lst, string Name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            JArray _lst1 = new JArray();
            JArray _lstalrt = new JArray();

            if (!string.IsNullOrEmpty(lst))
            {
                dynamic FResult = JObject.Parse(lst);
                _lst1 = FResult.Table;
                _lstalrt = FResult.Table1;
            }

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Vehicle No,SAT UId,Vehicle Type,Zone,Circle,Ward,LandMark,Address,Owner Type,Deployment Date & Time,Deployed By");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n",
                        Count,
                        item.GetValue("EntityNo").ToString(),
                        item.GetValue("UId").ToString(),
                        item.GetValue("VEntityNo").ToString(),
                        item.GetValue("ZoneNo").ToString(),
                        item.GetValue("CircleName").ToString(),
                        item.GetValue("WardNo").ToString(),
                        item.GetValue("LandMark").ToString(),
                        address,
                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("OwnerType").ToString(),
                        item.GetValue("CreatedOn").ToString(),
                        item.GetValue("CreatedBy").ToString());

                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }

        public string ExportAllSATTripsDataToCSV(string lst, string Name)
        {
            StringBuilder csvContent = new StringBuilder();

            JArray _lst1 = JArray.Parse(lst);

            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,Transfer Station Name,Vehicle Type,Vehicle No,Reported On");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    csvContent.AppendFormat("{0},{1},{2},{3},{4}\n",
                        Count,
                        item.GetValue("TStationName").ToString(),
                        item.GetValue("VehicleType").ToString(),
                        item.GetValue("VehicleNo").ToString(),
                        item.GetValue("ReportedOn").ToString());


                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }
        public byte[] ExportAllSATTripsData(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 5].Value = Name; worksheet.Cells[1, 1, 2, 5].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Transfer Station Name"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Vehicle Type"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Vehicle No"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Reported On"; worksheet.Column(5).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 5];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("TStationName").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("ReportedOn").ToString();



                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 5])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public byte[] ExportMasterWBData(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 34].Value = Name; worksheet.Cells[1, 1, 2, 34].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Vehicle No"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Gross Wt.(In Kg)"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Tare Wt.(In Kg)"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Net Wt.(In Kg)"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Site ID"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Transaction Date & Time"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Material"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Party"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Transporter"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Bill DC No"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Bill Weight"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "In Date"; worksheet.Column(13).Width = 25;
                    worksheet.Cells[3, 14].Value = "User1"; worksheet.Column(14).Width = 25;
                    worksheet.Cells[3, 15].Value = "User2"; worksheet.Column(15).Width = 25;
                    worksheet.Cells[3, 16].Value = "Status"; worksheet.Column(16).Width = 25;
                    worksheet.Cells[3, 17].Value = "SW Site ID"; worksheet.Column(17).Width = 25;
                    worksheet.Cells[3, 18].Value = "Trip No"; worksheet.Column(18).Width = 25;
                    worksheet.Cells[3, 19].Value = "Shift No"; worksheet.Column(19).Width = 25;
                    worksheet.Cells[3, 20].Value = "Transfer Waste IE"; worksheet.Column(20).Width = 25;
                    worksheet.Cells[3, 21].Value = "Transfer Waste"; worksheet.Column(21).Width = 25;
                    worksheet.Cells[3, 22].Value = "Remarks"; worksheet.Column(22).Width = 25;
                    worksheet.Cells[3, 23].Value = "Manifest Number"; worksheet.Column(23).Width = 25;
                    worksheet.Cells[3, 24].Value = "Manifest Weight"; worksheet.Column(24).Width = 25;
                    worksheet.Cells[3, 25].Value = "Member Ship Code"; worksheet.Column(25).Width = 25;
                    worksheet.Cells[3, 26].Value = "In Gate Pass No"; worksheet.Column(26).Width = 25;
                    worksheet.Cells[3, 27].Value = "In Meter Reading"; worksheet.Column(27).Width = 25;
                    worksheet.Cells[3, 28].Value = "Out Gate Pass No"; worksheet.Column(28).Width = 25;
                    worksheet.Cells[3, 29].Value = "Out Meter Reading"; worksheet.Column(29).Width = 25;
                    worksheet.Cells[3, 30].Value = "Transfer ID"; worksheet.Column(30).Width = 25;
                    worksheet.Cells[3, 31].Value = "Type Of Waste"; worksheet.Column(31).Width = 25;
                    worksheet.Cells[3, 32].Value = "Total KMS Travelled"; worksheet.Column(32).Width = 25;
                    worksheet.Cells[3, 33].Value = "Billable Weight"; worksheet.Column(33).Width = 25;
                    worksheet.Cells[3, 34].Value = "Total Transport Charges"; worksheet.Column(34).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 34];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("Vehicleno").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("GrossWt").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TareWt").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("Netwt").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("SiteID").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("TDate").ToString().Replace("T", " ");

                        worksheet.Cells[RowCount, 8].Value = item.GetValue("Material").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("Party").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("Transporter").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("BillDCno").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("Billweight").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("InDate").ToString().Replace("T", " ");
                        worksheet.Cells[RowCount, 14].Value = item.GetValue("User1").ToString();
                        worksheet.Cells[RowCount, 15].Value = item.GetValue("User2").ToString();
                        worksheet.Cells[RowCount, 16].Value = item.GetValue("Status").ToString();
                        worksheet.Cells[RowCount, 17].Value = item.GetValue("SW_SiteID").ToString();
                        worksheet.Cells[RowCount, 18].Value = item.GetValue("TripNo").ToString();
                        worksheet.Cells[RowCount, 19].Value = item.GetValue("ShiftNo").ToString();
                        worksheet.Cells[RowCount, 20].Value = item.GetValue("TransferWasteIE").ToString();
                        worksheet.Cells[RowCount, 21].Value = item.GetValue("TransferWaste").ToString();
                        worksheet.Cells[RowCount, 22].Value = item.GetValue("Remarks").ToString();
                        worksheet.Cells[RowCount, 23].Value = item.GetValue("ManifestNumber").ToString();
                        worksheet.Cells[RowCount, 24].Value = item.GetValue("ManifestWeight").ToString();
                        worksheet.Cells[RowCount, 25].Value = item.GetValue("MembershipCode").ToString();
                        worksheet.Cells[RowCount, 26].Value = item.GetValue("InGatePassNo").ToString();
                        worksheet.Cells[RowCount, 27].Value = item.GetValue("InMeterReading").ToString();
                        worksheet.Cells[RowCount, 28].Value = item.GetValue("OutGatePassNo").ToString();
                        worksheet.Cells[RowCount, 29].Value = item.GetValue("OutMeterReading").ToString();
                        worksheet.Cells[RowCount, 30].Value = item.GetValue("TransferID").ToString();
                        worksheet.Cells[RowCount, 31].Value = item.GetValue("TypeOfWaste").ToString();
                        worksheet.Cells[RowCount, 32].Value = item.GetValue("TotalKMSTravelled").ToString();
                        worksheet.Cells[RowCount, 33].Value = item.GetValue("BillableWeight").ToString();
                        worksheet.Cells[RowCount, 34].Value = item.GetValue("TotalTransportCharges").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 34].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 34].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 34].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 34].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 34])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }


        //public byte[] ExportMasterata(string result, string Name)
        //{
        //    byte[] Response = null;
        //    JArray jresult = JArray.Parse(result);
        //    try
        //    {
        //        using (ExcelPackage excel = new ExcelPackage())
        //        {
        //            int Count = 1;
        //            int RowCount = 4;
        //            excel.Workbook.Properties.Author = "Ajeevi Technologies";
        //            excel.Workbook.Properties.Title = Name;

        //            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

        //            #region Creating Header
        //            worksheet.Cells[1, 1, 1, 34].Value = Name; worksheet.Cells[1, 1, 2, 34].Merge = true;


        //            worksheet.Cells[3, 1].Value = "Sr No.";
        //            worksheet.Cells[3, 2].Value = "Vehicle Type"; worksheet.Column(2).Width = 25;
        //            worksheet.Cells[3, 3].Value = "Active"; worksheet.Column(3).Width = 25;
        //            worksheet.Cells[3, 4].Value = "InActive"; worksheet.Column(4).Width = 15;
        //            worksheet.Cells[3, 5].Value = "Repair"; worksheet.Column(5).Width = 25;
        //            worksheet.Cells[3, 6].Value = "Condemed"; worksheet.Column(6).Width = 25;
        //            worksheet.Cells[3, 7].Value = "Total Master"; worksheet.Column(7).Width = 25;
        //            worksheet.Cells[3, 8].Value = "Material"; worksheet.Column(8).Width = 25;
        //            worksheet.Cells[3, 9].Value = "Party"; worksheet.Column(9).Width = 25;
        //            worksheet.Cells[3, 10].Value = "Transporter"; worksheet.Column(10).Width = 25;
        //            worksheet.Cells[3, 11].Value = "Bill DC No"; worksheet.Column(11).Width = 25;
        //            worksheet.Cells[3, 12].Value = "Bill Weight"; worksheet.Column(12).Width = 25;
        //            worksheet.Cells[3, 13].Value = "In Date"; worksheet.Column(13).Width = 25;
        //            worksheet.Cells[3, 14].Value = "User1"; worksheet.Column(14).Width = 25;
        //            worksheet.Cells[3, 15].Value = "User2"; worksheet.Column(15).Width = 25;
        //            worksheet.Cells[3, 16].Value = "Status"; worksheet.Column(16).Width = 25;

        //            var cells = worksheet.Cells[3, 1, 3, 34];
        //            var fill = cells.Style.Fill;
        //            //cells.AutoFitColumns();
        //            cells.Style.Locked = true;
        //            cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //            cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            cells.Style.Font.Bold = true;
        //            cells.Style.WrapText = true;
        //            cells.Style.ShrinkToFit = true;

        //            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

        //            var border = cells.Style.Border;
        //            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //            #endregion



        //            #region filling data

        //            foreach (JObject item in jresult)
        //            {

        //                worksheet.Cells[RowCount, 1].Value = Count;
        //                worksheet.Cells[RowCount, 2].Value = item.GetValue("VehicleType").ToString();
        //                worksheet.Cells[RowCount, 3].Value = item.GetValue("GrossWt").ToString();
        //                worksheet.Cells[RowCount, 4].Value = item.GetValue("TareWt").ToString();
        //                worksheet.Cells[RowCount, 5].Value = item.GetValue("Netwt").ToString();
        //                worksheet.Cells[RowCount, 6].Value = item.GetValue("SiteID").ToString();
        //                worksheet.Cells[RowCount, 7].Value = item.GetValue("TDate").ToString().Replace("T", " ");

        //                worksheet.Cells[RowCount, 8].Value = item.GetValue("Material").ToString();
        //                worksheet.Cells[RowCount, 9].Value = item.GetValue("Party").ToString();
        //                worksheet.Cells[RowCount, 10].Value = item.GetValue("Transporter").ToString();
        //                worksheet.Cells[RowCount, 11].Value = item.GetValue("BillDCno").ToString();
        //                worksheet.Cells[RowCount, 12].Value = item.GetValue("Billweight").ToString();
        //                worksheet.Cells[RowCount, 13].Value = item.GetValue("InDate").ToString().Replace("T", " ");
        //                worksheet.Cells[RowCount, 14].Value = item.GetValue("User1").ToString();
        //                worksheet.Cells[RowCount, 15].Value = item.GetValue("User2").ToString();
        //                worksheet.Cells[RowCount, 16].Value = item.GetValue("Status").ToString();
        //                worksheet.Cells[RowCount, 17].Value = item.GetValue("SW_SiteID").ToString();
        //                worksheet.Cells[RowCount, 18].Value = item.GetValue("TripNo").ToString();


        //                worksheet.Cells[RowCount, 1, RowCount, 34].Style.WrapText = true;
        //                worksheet.Cells[RowCount, 1, RowCount, 34].Style.ShrinkToFit = true;
        //                worksheet.Cells[RowCount, 1, RowCount, 34].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                worksheet.Cells[RowCount, 1, RowCount, 34].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


        //                Count++;
        //                RowCount++;
        //            }
        //            #endregion


        //            #region formatting

        //            using (var range = worksheet.Cells[1, 1, 2, 34])
        //            {
        //                // Setting bold font
        //                range.Style.Font.Bold = true;
        //                // Setting fill type solid
        //                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                // Setting background gray
        //                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

        //                // Setting font color
        //                range.Style.Font.Color.SetColor(Color.Black);

        //                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                range.Style.Font.Size = 12;
        //                range.Style.WrapText = true;
        //                range.Style.ShrinkToFit = true;
        //                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                var border1 = range.Style.Border;
        //                border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            }


        //            worksheet.PrinterSettings.FitToPage = true;
        //            worksheet.PrinterSettings.ShowGridLines = true;

        //            #endregion

        //            Response = excel.GetAsByteArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return Response;
        //}

        public byte[] ExportGetAllRptVehicleInfo(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 5].Value = Name; worksheet.Cells[1, 1, 2, 5].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Types Of Vehicle"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Total Vehicles as per Master"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Arrived"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Trip Made"; worksheet.Column(5).Width = 25;



                    var cells = worksheet.Cells[3, 1, 3, 5];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("TotalCount").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TotalArrivalCount").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("TotalTripCount").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 5])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportAllRptContainerforHKL(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 6].Value = Name; worksheet.Cells[1, 1, 2, 6].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Dated ON"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Zone"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Station Name"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Container No"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "HookLoder No"; worksheet.Column(5).Width = 25;


                    var cells = worksheet.Cells[3, 1, 3, 6];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("CreatedDate").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TStationName").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("ContainerCode").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("VehicleNo").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 6])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportAllGeoPointNotCollected_Paging(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);//SSKK
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 10].Value = Name; worksheet.Cells[1, 1, 2, 10].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Point Name"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Categaory"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Point Status"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Last Visit Date"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Geo Cordinate"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Map View Link"; worksheet.Column(10).Width = 30;


                    var cells = worksheet.Cells[3, 1, 3, 10];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("GeoPointId").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("GeoPointCategory").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("GeoPointStatus").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("PDate").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("GeoCordinate").ToString();
                        worksheet.Cells[RowCount, 10].Value = "http://maps.google.co.in/maps?q=" + item.GetValue("GeoCordinate").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 10])
                    {

                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportPendingContainerforHKL(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 5].Value = Name; worksheet.Cells[1, 1, 2, 5].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Dated On"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Zone"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Station Name"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Container No"; worksheet.Column(5).Width = 25;


                    var cells = worksheet.Cells[3, 1, 3, 5];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("CreatedDate").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TStationName").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("ContainerCode").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 5])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportAllGeoPoint(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 12].Value = Name; worksheet.Cells[1, 1, 2, 12].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Status"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Geo Point Name"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Category"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Zone"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Circle"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Ward"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Lattitude | Longitude"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Radius(In Mtr.)"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Remarks"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Created By"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Last Modified On"; worksheet.Column(12).Width = 25;


                    var cells = worksheet.Cells[3, 1, 3, 12];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;

                        if (item.GetValue("IsActive").ToString() == "True")
                            worksheet.Cells[RowCount, 2].Value = "ACTIVE";
                        else
                            worksheet.Cells[RowCount, 2].Value = "DE-ACTIVE";

                        worksheet.Cells[RowCount, 3].Value = item.GetValue("GeoPointName").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("GeoPointCategory").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("Lat").ToString() + " | " + item.GetValue("Lng").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("Radius").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("Remarks").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("CreatedBy").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("ModifiedOn").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 12])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportAllArvlOfEntityOpt1Noti(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 8].Value = Name; worksheet.Cells[1, 1, 2, 8].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Uid"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Entity Type"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Entity Code"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Transfer Station Name"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Status"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Deviation Status "; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Transaction Done On "; worksheet.Column(8).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 8];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("UId").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("EntityType").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("EntityCode").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("TStationName").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("Status").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("DeviationStatus").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("CreatedOn").ToString();
                        //worksheet.Cells[RowCount, 7].Value = item.GetValue("DeviationStatus").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 8])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportUserLogData(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 8].Value = Name; worksheet.Cells[1, 1, 2, 8].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Login Id"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Login Type"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Status"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Login Date & Time"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Logout Date & Time"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Total Active Time(In HH:MM)"; worksheet.Column(7).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 8];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("LoginId").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("LoginType").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("Status").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("LoginTDate").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("LogoutTDate").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("TotalActiveTime").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 8])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportHKLWisePerfData(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 8].Value = Name; worksheet.Cells[1, 1, 2, 8].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Hookloader No"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "Status"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Total Trips"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Total Working Time(In HH:MM)"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Total Waiting Time(In HH:MM)"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Total Weight Actual At WB(In MT)"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Average Ton/Trip(MT)"; worksheet.Column(8).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 8];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("Status").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TotalTrip").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("WorkingHourInHHMM").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("WaitingHourInHHMM").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("NetWt").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("AVGTon").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 8])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportOpenProcessData(string result, string Name)
        {

            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 15].Value = Name; worksheet.Cells[1, 1, 2, 15].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Date & Time"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "TS/SCTP"; worksheet.Column(3).Width = 15;
                    worksheet.Cells[3, 4].Value = "AC"; worksheet.Column(4).Width = 25;
                    // worksheet.Cells[3, 4].Value = "CO"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "PC"; worksheet.Column(5).Width = 15;
                    worksheet.Cells[3, 6].Value = "CC"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "AH"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "LH"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "MP"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "WB"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "WB. WT(In MT)"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "EST. WT(In MT)"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "Status"; worksheet.Column(13).Width = 25;
                    worksheet.Cells[3, 14].Value = "Operator"; worksheet.Column(14).Width = 25;
                    worksheet.Cells[3, 15].Value = "Last Activity On"; worksheet.Column(15).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 15];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion

                    #region Short Name
                    worksheet.Cells[3, 17].Value = "Short Name"; worksheet.Column(17).Width = 25;
                    worksheet.Cells[3, 18].Value = "Full Description"; worksheet.Column(18).Width = 25;

                    var cells1 = worksheet.Cells[3, 17, 3, 18];
                    var fill1 = cells1.Style.Fill;
                    //cells.AutoFitColumns();
                    cells1.Style.Locked = true;
                    cells1.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells1.Style.Font.Bold = true;
                    cells1.Style.WrapText = true;
                    cells1.Style.ShrinkToFit = true;

                    fill1.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill1.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border2 = cells1.Style.Border;
                    border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    worksheet.Cells[4, 17].Value = "AC"; worksheet.Cells[4, 18].Value = "Arrival Of Container";
                    worksheet.Cells[5, 17].Value = "PC"; worksheet.Cells[5, 18].Value = "Primary Vehicle";
                    worksheet.Cells[6, 17].Value = "CC"; worksheet.Cells[6, 18].Value = "Closed Container";
                    worksheet.Cells[7, 17].Value = "AH"; worksheet.Cells[7, 18].Value = "Arrival Of Hookloader";
                    worksheet.Cells[8, 17].Value = "LH"; worksheet.Cells[8, 18].Value = "Linkage To Hookloader";
                    worksheet.Cells[9, 17].Value = "MP"; worksheet.Cells[9, 18].Value = "Moved To Processing Facility";
                    worksheet.Cells[10, 17].Value = "WB"; worksheet.Cells[10, 18].Value = "Weight Bridge Weight In MT";
                    worksheet.Cells[11, 17].Value = "EST. WT"; worksheet.Cells[11, 18].Value = "Estimated Weight In MT";

                    worksheet.Cells[4, 17, 11, 18].Style.WrapText = true;
                    worksheet.Cells[4, 17, 11, 18].Style.ShrinkToFit = true;
                    worksheet.Cells[4, 17, 11, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[4, 17, 11, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                    #endregion

                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("DCreatedDate").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("TSName").ToString();

                        string AC = "NO";
                        if (item.GetValue("DHLTId").ToString() != "0")
                            AC = item.GetValue("ContainerCode").ToString() + Environment.NewLine + item.GetValue("DCreatedDate").ToString();
                        worksheet.Cells[RowCount, 4].Value = AC;

                        //string CO = "N/A";
                        //if (!string.IsNullOrEmpty(item.GetValue("COPrimaryCreatedDate").ToString()))
                        //    CO = item.GetValue("ContainerCode").ToString() + Environment.NewLine + item.GetValue("COPrimaryCreatedDate").ToString();
                        //worksheet.Cells[RowCount, 4].Value = CO;

                        string PC = "NO";
                        if (item.GetValue("TotalPV").ToString() != "0")
                            PC = "PV-" + item.GetValue("TotalPV").ToString();
                        worksheet.Cells[RowCount, 5].Value = PC;

                        string CC = "NO";
                        if (!string.IsNullOrEmpty(item.GetValue("CCPrimaryCreatedDate").ToString()))
                            CC = item.GetValue("CCPrimaryCreatedDate").ToString();
                        //CC = item.GetValue("ContainerCode").ToString() + Environment.NewLine + item.GetValue("CCPrimaryCreatedDate").ToString();
                        worksheet.Cells[RowCount, 6].Value = CC;

                        string AH = "NO";
                        if (!string.IsNullOrEmpty(item.GetValue("LHARCreatedDate").ToString()))
                            AH = item.GetValue("LHARVehicleNo").ToString() + Environment.NewLine + item.GetValue("LHARCreatedDate").ToString();
                        worksheet.Cells[RowCount, 7].Value = AH;

                        string LH = "NO";
                        if (!string.IsNullOrEmpty(item.GetValue("LHCreatedDate").ToString()))
                            LH = item.GetValue("LHVehicleNo").ToString() + Environment.NewLine + item.GetValue("LKHContainerNo").ToString() + Environment.NewLine + item.GetValue("LHCreatedDate").ToString();
                        worksheet.Cells[RowCount, 8].Value = LH;

                        string MP = "NO";
                        if (!string.IsNullOrEmpty(item.GetValue("SyncedOnWB").ToString()))
                            MP = "YES";//item.GetValue("SyncedOnWB").ToString();
                        worksheet.Cells[RowCount, 9].Value = MP;

                        string WB = "NO";
                        if (!string.IsNullOrEmpty(item.GetValue("WBCreatedDate").ToString()))
                            WB = item.GetValue("WBHKLNo").ToString() + Environment.NewLine + item.GetValue("NetWt").ToString() + Environment.NewLine + item.GetValue("WBCreatedDate").ToString();
                        worksheet.Cells[RowCount, 10].Value = WB;

                        string WBWT = "0";
                        if (!string.IsNullOrEmpty(item.GetValue("WBCreatedDate").ToString()))
                            WBWT = item.GetValue("NetWt").ToString();
                        worksheet.Cells[RowCount, 11].Value = WBWT;

                        worksheet.Cells[RowCount, 12].Value = item.GetValue("TotalWTPV").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("Status").ToString();
                        worksheet.Cells[RowCount, 14].Value = item.GetValue("DCreatedBy").ToString();
                        worksheet.Cells[RowCount, 15].Value = item.GetValue("LastActivityOn").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 15].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 15].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 15])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public byte[] ExportDataLogByTS(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 16].Value = Name; worksheet.Cells[1, 1, 2, 16].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Transfer Station"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "Dated On"; worksheet.Column(3).Width = 15;
                    worksheet.Cells[3, 4].Value = "Entity Type"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Entity No"; worksheet.Column(5).Width = 15;
                    worksheet.Cells[3, 6].Value = "Arrival Time"; worksheet.Column(6).Width = 15;
                    worksheet.Cells[3, 7].Value = "Start Time"; worksheet.Column(7).Width = 15;
                    worksheet.Cells[3, 8].Value = "Filled Time"; worksheet.Column(8).Width = 15;
                    worksheet.Cells[3, 9].Value = "Operational Time(HH:MM)"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "No Of Primary Vehicle"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Estimated Weight(MT)"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Hook Loader"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "Time In"; worksheet.Column(13).Width = 25;
                    worksheet.Cells[3, 14].Value = "Time Out"; worksheet.Column(14).Width = 25;
                    worksheet.Cells[3, 15].Value = "Total Weight As Per WB(MT)"; worksheet.Column(15).Width = 25;
                    worksheet.Cells[3, 16].Value = "Time In"; worksheet.Column(16).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 16];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("TSName").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("DatedOn").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("ContainerCode").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("ArrivalTime").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("StartTime").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("EndTime").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("TotalTimeTaken").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("TotalVehicleScanned").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("TotalPVehicleWt").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("HookLoader").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("InTime").ToString();
                        worksheet.Cells[RowCount, 14].Value = item.GetValue("OutTime").ToString();
                        worksheet.Cells[RowCount, 15].Value = item.GetValue("WBNetWt").ToString();
                        worksheet.Cells[RowCount, 16].Value = item.GetValue("WBInTime").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 16].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 16].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 16].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 16])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportWeightBridgeReportData(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 14].Value = Name; worksheet.Cells[1, 1, 2, 14].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Weighbridge Id"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "WB Trans. Code"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Transaction Date & Time"; worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Status"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "TS/SCTP"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Vehicle RFID Code"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Container No"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Gross Weight(In MT)"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Tare Weight(In MT)"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Net Weight(In MT)"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Total Est. Weight(In MT)"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "Total Weight Variance(%)"; worksheet.Column(13).Width = 25;
                    worksheet.Cells[3, 14].Value = "Sync Date & Time"; worksheet.Column(14).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 14];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("WBId").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("WTCode").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TDate").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("Status").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("TCode").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("EntityNo").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("ContainerCode").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("GrossWt").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("TareWt").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("NetWt").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("TotalWTPV").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("WTVariance").ToString();
                        worksheet.Cells[RowCount, 14].Value = item.GetValue("CreatedDate").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 14].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 14].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 14])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public byte[] ExportRptArvlEntityData(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 6].Value = Name; worksheet.Cells[1, 1, 2, 6].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Date & Time"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "Zone"; worksheet.Column(3).Width = 15;
                    worksheet.Cells[3, 4].Value = "Transfer Station Name"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Entity Type"; worksheet.Column(5).Width = 25;
                    // worksheet.Cells[3, 4].Value = "CO"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 6].Value = "Entity No"; worksheet.Column(6).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 10];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("CreatedOn").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TStationName").ToString();
                        if (item.GetValue("UIdType").ToString() == "CONTAINER")
                            worksheet.Cells[RowCount, 5].Value = item.GetValue("UIdType").ToString();
                        else
                            worksheet.Cells[RowCount, 5].Value = item.GetValue("EntityType").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("EntityCode").ToString();


                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 6])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public byte[] ExportRptPVehicleWiseData(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 10].Value = Name; worksheet.Cells[1, 1, 2, 10].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Date & Time"; worksheet.Column(2).Width = 15;
                    worksheet.Cells[3, 3].Value = "Zone"; worksheet.Column(3).Width = 15;
                    worksheet.Cells[3, 4].Value = "Transfer Station Name"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Vehicle No"; worksheet.Column(5).Width = 25;
                    // worksheet.Cells[3, 4].Value = "CO"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 6].Value = "Owner Type"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Type Of Vehicle"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Percentage(%) of waste"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 9].Value = "Waste Type"; worksheet.Column(7).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 9];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("TDate").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TStationName").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("OwnerType").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("FilledPerc").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("WasteType").ToString();


                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 8])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public byte[] ExportAllGeoPointsVisitSummary(string result, string Name, string Sname)
        {
            byte[] Response = null;
            //JArray jresult = JArray.Parse(result);
            JArray _lst1 = new JArray();
            JArray _lst = new JArray();
            JArray _lstalrt = new JArray();
            // JArray _lst1;
            // JArray _lstalrt ;
            if (!string.IsNullOrEmpty(result))
            {
                dynamic FResult = JObject.Parse(result);
                _lst1 = FResult.Table;
                _lstalrt = FResult.Table1;
            }
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Summary SH 1");
                    ExcelWorksheet worksheet1 = excel.Workbook.Worksheets.Add("Summary SH 2");

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 12].Value = Name; worksheet.Cells[1, 1, 2, 12].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone-No"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle-No"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward-No"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Point Name"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Category"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Geo Point Status"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Total Visit"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Visit Date"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Days lapsed since last Visit"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Geo Cordinate"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Map View Link"; worksheet.Column(12).Width = 30;



                    var cells = worksheet.Cells[3, 1, 3, 12];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in _lst1)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("GeoPointName").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("GeoPointCategory").ToString();
                        if (item.GetValue("GeoPointStatus").ToString() == "True")
                        {

                            worksheet.Cells[RowCount, 7].Value = "ACTIVE";
                        }
                        else
                        {
                            worksheet.Cells[RowCount, 7].Value = "DE-ACTIVE";
                        }

                        worksheet.Cells[RowCount, 8].Value = item.GetValue("TotalVisit").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("PDate").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("GeoPointLifecycle").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("GeoCordinate").ToString();
                        worksheet.Cells[RowCount, 12].Value = "http://maps.google.co.in/maps?q=" + item.GetValue("GeoCordinate").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 12])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    #region Creating Header
                    worksheet1.Cells[1, 1, 1, 10].Value = Name; worksheet1.Cells[1, 1, 2, 10].Merge = true;


                    worksheet1.Cells[3, 1].Value = "Sr No.";
                    worksheet1.Cells[3, 2].Value = "Zone-Number"; worksheet1.Column(2).Width = 25;
                    worksheet1.Cells[3, 3].Value = "Circle-Number"; worksheet1.Column(3).Width = 25;
                    worksheet1.Cells[3, 4].Value = "Ward-Number"; worksheet1.Column(4).Width = 15;
                    worksheet1.Cells[3, 5].Value = "Route Code"; worksheet1.Column(5).Width = 25;
                    worksheet1.Cells[3, 6].Value = "Route Trip Code"; worksheet1.Column(6).Width = 25;
                    worksheet1.Cells[3, 7].Value = "Point Name"; worksheet1.Column(7).Width = 25;
                    worksheet1.Cells[3, 8].Value = "Category"; worksheet1.Column(8).Width = 25;
                    worksheet1.Cells[3, 9].Value = "Visit Date"; worksheet1.Column(9).Width = 25;
                    worksheet1.Cells[3, 10].Value = "Visit Time"; worksheet1.Column(10).Width = 25;



                    var cells1 = worksheet1.Cells[3, 1, 3, 10];
                    var fill1 = cells1.Style.Fill;
                    //cells.AutoFitColumns();
                    cells1.Style.Locked = true;
                    cells1.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells1.Style.Font.Bold = true;
                    cells1.Style.WrapText = true;
                    cells1.Style.ShrinkToFit = true;

                    fill1.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill1.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border1 = cells1.Style.Border;
                    border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data
                    Count = 1;
                    RowCount = 4;
                    foreach (JObject item1 in _lst1)
                    {
                        string PointId = item1.GetValue("GeoPointId").ToString();
                        foreach (JObject item in _lstalrt)
                        {
                            string IPointId = item.GetValue("PointId").ToString();
                            if (PointId == IPointId)
                            {
                                worksheet1.Cells[RowCount, 1].Value = Count;
                                worksheet1.Cells[RowCount, 2].Value = item.GetValue("ZoneNo").ToString();
                                worksheet1.Cells[RowCount, 3].Value = item.GetValue("CircleName").ToString();
                                worksheet1.Cells[RowCount, 4].Value = item.GetValue("WardNo").ToString();
                                worksheet1.Cells[RowCount, 5].Value = item.GetValue("routecode").ToString();
                                worksheet1.Cells[RowCount, 6].Value = item.GetValue("TId").ToString();
                                worksheet1.Cells[RowCount, 7].Value = item.GetValue("GeoPointName").ToString();
                                worksheet1.Cells[RowCount, 8].Value = item.GetValue("GeoPointCategory").ToString();
                                worksheet1.Cells[RowCount, 9].Value = item.GetValue("PDate").ToString();
                                worksheet1.Cells[RowCount, 10].Value = item.GetValue("PTime").ToString();

                                worksheet1.Cells[RowCount, 1, RowCount, 10].Style.WrapText = true;
                                worksheet1.Cells[RowCount, 1, RowCount, 10].Style.ShrinkToFit = true;
                                worksheet1.Cells[RowCount, 1, RowCount, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet1.Cells[RowCount, 1, RowCount, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                                Count++;
                                RowCount++;
                            }
                        }
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet1.Cells[1, 1, 2, 10])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet1.PrinterSettings.FitToPage = true;
                    worksheet1.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }





            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] EmergencyExportAllPoint(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 6].Value = Name; worksheet.Cells[1, 1, 2, 6].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Vehicle No"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Point Name"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Status"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Address"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Collection DateTime"; worksheet.Column(6).Width = 25;



                    var cells = worksheet.Cells[3, 1, 3, 6];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;



                        worksheet.Cells[RowCount, 2].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("PointName").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("Status").ToString();
                        //if (item.GetValue("Status").ToString() == "1")
                        //    worksheet.Cells[RowCount, 4].Value = "ACTIVE";
                        //else
                        //    worksheet.Cells[RowCount, 4].Value = "DE-ACTIVE";
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("Address").ToString();

                        worksheet.Cells[RowCount, 6].Value = item.GetValue("PickDTime").ToString();



                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 6])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public byte[] ExportAllCollectdPoint(string result, string Name)
        {
            byte[] Response = null;//KKSS
            JArray jresult = JArray.Parse(result);
            //try
            //{
            using (ExcelPackage excel = new ExcelPackage())
            {
                int Pic1 = 1;
                int Pic2 = 2;
                int Count = 1;
                int RowCount = 4;
                excel.Workbook.Properties.Author = "Ajeevi Technologies";
                excel.Workbook.Properties.Title = Name;

                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                #region Creating Header
                worksheet.Cells[1, 1, 1, 12].Value = Name; worksheet.Cells[1, 1, 2, 12].Merge = true;


                worksheet.Cells[3, 1].Value = "Sr No.";
                worksheet.Cells[3, 2].Value = "Vehicle No"; worksheet.Column(2).Width = 25;
                worksheet.Cells[3, 3].Value = "RouteCode"; worksheet.Column(3).Width = 25;
                worksheet.Cells[3, 4].Value = "TripName"; worksheet.Column(4).Width = 25;
                worksheet.Cells[3, 5].Value = "Point Name"; worksheet.Column(5).Width = 25;
                worksheet.Cells[3, 6].Value = "Status"; worksheet.Column(6).Width = 15;
                worksheet.Cells[3, 7].Value = "Address"; worksheet.Column(7).Width = 25;
                worksheet.Cells[3, 8].Value = "Collection DateTime"; worksheet.Column(8).Width = 25;
                worksheet.Cells[3, 9].Value = "Geo Cordinate"; worksheet.Column(9).Width = 25;
                worksheet.Cells[3, 10].Value = "Map View Link"; worksheet.Column(10).Width = 30;
                worksheet.Cells[3, 11].Value = "Shift Name"; worksheet.Column(11).Width = 25;
                worksheet.Cells[3, 12].Value = "Shift Date"; worksheet.Column(12).Width = 30;



                var cells = worksheet.Cells[3, 1, 3, 12];
                var fill = cells.Style.Fill;
                //cells.AutoFitColumns();
                cells.Style.Locked = true;
                cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                cells.Style.Font.Bold = true;
                cells.Style.WrapText = true;
                cells.Style.ShrinkToFit = true;

                fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                var border = cells.Style.Border;
                border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                #endregion



                #region filling data

                foreach (JObject item in jresult)
                {

                    worksheet.Cells[RowCount, 1].Value = Count;
                    worksheet.Cells[RowCount, 2].Value = item.GetValue("VehicleNo").ToString();
                    worksheet.Cells[RowCount, 3].Value = item.GetValue("RouteCode").ToString();
                    worksheet.Cells[RowCount, 4].Value = item.GetValue("TripName").ToString();
                    worksheet.Cells[RowCount, 5].Value = item.GetValue("PointName").ToString();
                    worksheet.Cells[RowCount, 6].Value = item.GetValue("Status").ToString();
                    //if (item.GetValue("Status").ToString() == "1")
                    //    worksheet.Cells[RowCount, 6].Value = "ACTIVE";
                    //else
                    //    worksheet.Cells[RowCount, 6].Value = "DE-ACTIVE";
                    worksheet.Cells[RowCount, 7].Value = item.GetValue("Address").ToString();

                    worksheet.Cells[RowCount, 8].Value = item.GetValue("PickDTime").ToString();
                    worksheet.Cells[RowCount, 9].Value = item.GetValue("GeoCordinate").ToString();
                    worksheet.Cells[RowCount, 10].Value = "http://maps.google.co.in/maps?q=" + item.GetValue("GeoCordinate").ToString();
                    worksheet.Cells[RowCount, 11].Value = item.GetValue("ShiftName").ToString();
                    worksheet.Cells[RowCount, 12].Value = item.GetValue("ShiftDate").ToString();

                    worksheet.Cells[RowCount, 1, RowCount, 12].Style.WrapText = true;
                    worksheet.Cells[RowCount, 1, RowCount, 12].Style.ShrinkToFit = true;
                    worksheet.Cells[RowCount, 1, RowCount, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[RowCount, 1, RowCount, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                    Pic1++;
                    Pic2++;
                    Count++;
                    RowCount++;
                }
                #endregion


                #region formatting

                using (var range = worksheet.Cells[1, 1, 2, 12])
                {
                    // Setting bold font
                    range.Style.Font.Bold = true;
                    // Setting fill type solid
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    // Setting background gray
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    // Setting font color
                    range.Style.Font.Color.SetColor(Color.Black);

                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.Font.Size = 12;
                    range.Style.WrapText = true;
                    range.Style.ShrinkToFit = true;
                    range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                    var border1 = range.Style.Border;
                    border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }


                worksheet.PrinterSettings.FitToPage = true;
                worksheet.PrinterSettings.ShowGridLines = true;

                #endregion

                Response = excel.GetAsByteArray();
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return Response;
        }


        public byte[] ExportAllVehicleDeploySummary(string result, string Name, string Sname)
        {
            byte[] Response = null;
            //JArray jresult = JArray.Parse(result);
            JArray _lst1 = new JArray();
            JArray _lst = new JArray();
            JArray _lstalrt = new JArray();
            JArray _lstalrt1 = new JArray();
            // JArray _lst1;
            // JArray _lstalrt ;
            if (!string.IsNullOrEmpty(result))
            {
                dynamic FResult = JObject.Parse(result);
                _lst1 = FResult.Table;
                _lstalrt = FResult.Table1;
                _lstalrt1 = FResult.Table2;
            }
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Swachh Autos(Master)");
                    ExcelWorksheet worksheet1 = excel.Workbook.Worksheets.Add("Swachh Autos(Active)");
                    ExcelWorksheet worksheet2 = excel.Workbook.Worksheets.Add("Swachh Autos (Active) Deployed Vs Reported");

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 6].Value = "SA- Swachh Autos(Master)"; worksheet.Cells[1, 1, 2, 6].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Active"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "InActive"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Repair"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Condemned"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Total"; worksheet.Column(6).Width = 25;




                    var cells = worksheet.Cells[3, 1, 3, 6];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in _lst1)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("TotalActive").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("TotalInActive").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TotalRepair").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("TotalCondemed").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("Total").ToString();


                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 6])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    #region Creating Header
                    worksheet1.Cells[1, 1, 1, 4].Value = "SB- Swachh Autos(Active)"; worksheet1.Cells[1, 1, 2, 4].Merge = true;


                    worksheet1.Cells[3, 1].Value = "Sr No.";
                    worksheet1.Cells[3, 2].Value = "Deployed"; worksheet1.Column(2).Width = 25;
                    worksheet1.Cells[3, 3].Value = "Not Deployed"; worksheet1.Column(3).Width = 25;
                    worksheet1.Cells[3, 4].Value = "Total"; worksheet1.Column(4).Width = 15;




                    var cells1 = worksheet1.Cells[3, 1, 3, 4];
                    var fill1 = cells1.Style.Fill;
                    //cells.AutoFitColumns();
                    cells1.Style.Locked = true;
                    cells1.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells1.Style.Font.Bold = true;
                    cells1.Style.WrapText = true;
                    cells1.Style.ShrinkToFit = true;

                    fill1.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill1.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border1 = cells1.Style.Border;
                    border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data
                    Count = 1;
                    RowCount = 4;

                    foreach (JObject item in _lstalrt)
                    {


                        worksheet1.Cells[RowCount, 1].Value = Count;
                        worksheet1.Cells[RowCount, 2].Value = item.GetValue("Deployed").ToString();
                        worksheet1.Cells[RowCount, 3].Value = item.GetValue("NotDeployed").ToString();
                        worksheet1.Cells[RowCount, 4].Value = item.GetValue("TotalDeployed").ToString();


                        worksheet1.Cells[RowCount, 1, RowCount, 4].Style.WrapText = true;
                        worksheet1.Cells[RowCount, 1, RowCount, 4].Style.ShrinkToFit = true;
                        worksheet1.Cells[RowCount, 1, RowCount, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet1.Cells[RowCount, 1, RowCount, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;

                    }

                    #endregion


                    #region formatting

                    using (var range = worksheet1.Cells[1, 1, 2, 4])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet1.PrinterSettings.FitToPage = true;
                    worksheet1.PrinterSettings.ShowGridLines = true;

                    #endregion


                    #region Creating Header
                    worksheet2.Cells[1, 1, 1, 7].Value = "SC- Swachh Autos (Active) Deployed Vs Reported"; worksheet2.Cells[1, 1, 2, 7].Merge = true;


                    worksheet2.Cells[3, 1].Value = "Sr No.";
                    worksheet2.Cells[3, 2].Value = "Active Master"; worksheet2.Column(2).Width = 25;
                    worksheet2.Cells[3, 3].Value = "Deployed"; worksheet2.Column(3).Width = 25;
                    worksheet2.Cells[3, 4].Value = "Deployed & Reported"; worksheet2.Column(4).Width = 25;
                    worksheet2.Cells[3, 5].Value = "Not Deployed"; worksheet2.Column(5).Width = 25;
                    worksheet2.Cells[3, 6].Value = "Not Deployed & Reported"; worksheet2.Column(6).Width = 25;
                    //worksheet2.Cells[3, 7].Value = "Total"; worksheet2.Column(1).Width = 20;




                    var cells2 = worksheet2.Cells[3, 1, 3, 7];
                    var fill2 = cells2.Style.Fill;
                    //cells.AutoFitColumns();
                    cells2.Style.Locked = true;
                    cells2.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells2.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells2.Style.Font.Bold = true;
                    cells2.Style.WrapText = true;
                    cells2.Style.ShrinkToFit = true;

                    fill2.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill2.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border3 = cells2.Style.Border;
                    border3.Top.Style = border3.Left.Style = border3.Bottom.Style = border3.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data
                    Count = 1;
                    RowCount = 4;


                    foreach (JObject item in _lstalrt1)
                    {


                        worksheet2.Cells[RowCount, 1].Value = Count;
                        worksheet2.Cells[RowCount, 2].Value = item.GetValue("TotalActiveMaster").ToString();
                        worksheet2.Cells[RowCount, 3].Value = item.GetValue("Deployed").ToString();
                        worksheet2.Cells[RowCount, 4].Value = item.GetValue("DeployedReported").ToString();
                        worksheet2.Cells[RowCount, 5].Value = item.GetValue("NotDeployed").ToString();
                        worksheet2.Cells[RowCount, 6].Value = item.GetValue("NotDeployedReported").ToString();
                        //worksheet2.Cells[RowCount, 7].Value = item.GetValue("Total").ToString();


                        worksheet2.Cells[RowCount, 1, RowCount, 7].Style.WrapText = true;
                        worksheet2.Cells[RowCount, 1, RowCount, 7].Style.ShrinkToFit = true;
                        worksheet2.Cells[RowCount, 1, RowCount, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet2.Cells[RowCount, 1, RowCount, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;

                    }

                    #endregion

                    #region formatting

                    using (var range = worksheet2.Cells[1, 1, 2, 7])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border4 = range.Style.Border;
                        border4.Top.Style = border4.Left.Style = border4.Bottom.Style = border4.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet2.PrinterSettings.FitToPage = true;
                    worksheet2.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }





            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        //Image DownloadImage(string fromUrl)
        //{
        //    using (System.Net.WebClient webClient = new System.Net.WebClient())
        //    {
        //        using (Stream stream = webClient.OpenRead(fromUrl))
        //        {
        //            return Image.FromStream(stream);
        //        }
        //    }
        //}
        //public byte[] ExportAllTripPoint(string lst, string Name)
        //{
        //    byte[] Response = null;
        //    JArray jresult = JArray.Parse(lst);
        //    try
        //    {
        //        using (ExcelPackage excel = new ExcelPackage())
        //        {
        //            int Count = 1;
        //            int RowCount = 4;
        //            excel.Workbook.Properties.Author = "Ajeevi Technologies";
        //            excel.Workbook.Properties.Title = Name + " -Details" + DateTime.Now.ToString("MMMM");

        //            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

        //            #region Creating Header
        //            worksheet.Cells[1, 1, 1, 10].Value = Name; worksheet.Cells[1, 1, 2, 10].Merge = true;


        //            worksheet.Cells[3, 1].Value = "Sr No.";
        //            worksheet.Cells[3, 2].Value = "Route Code"; worksheet.Column(2).Width = 15;
        //            worksheet.Cells[3, 3].Value = "TId"; worksheet.Column(3).Width = 15;
        //            worksheet.Cells[3, 4].Value = "VehicleNo"; worksheet.Column(4).Width = 15;
        //            worksheet.Cells[3, 5].Value = "BufferMin"; worksheet.Column(5).Width = 25;
        //            worksheet.Cells[3, 6].Value = "TotalStop"; worksheet.Column(6).Width = 22;



        //            var cells = worksheet.Cells[3, 1, 3, 10];
        //            var fill = cells.Style.Fill;
        //            //cells.AutoFitColumns();
        //            cells.Style.Locked = true;
        //            cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //            cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            cells.Style.Font.Bold = true;
        //            cells.Style.WrapText = true;
        //            cells.Style.ShrinkToFit = true;

        //            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

        //            var border = cells.Style.Border;
        //            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //            #endregion



        //            #region filling data

        //            foreach (JObject item in jresult)
        //            {

        //                worksheet.Cells[RowCount, 1].Value = Count;
        //                worksheet.Cells[RowCount, 2].Value = item.GetValue("RouteCode").ToString();
        //                worksheet.Cells[RowCount, 3].Value = item.GetValue("TId").ToString();
        //                worksheet.Cells[RowCount, 4].Value = item.GetValue("VehicleNo").ToString();
        //                worksheet.Cells[RowCount, 5].Value = item.GetValue("BufferMin").ToString();
        //                worksheet.Cells[RowCount, 6].Value = item.GetValue("TotalStop").ToString();


        //                worksheet.Cells[RowCount, 1, RowCount, 10].Style.WrapText = true;
        //                worksheet.Cells[RowCount, 1, RowCount, 10].Style.ShrinkToFit = true;
        //                worksheet.Cells[RowCount, 1, RowCount, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                worksheet.Cells[RowCount, 1, RowCount, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                Count++;
        //                RowCount++;
        //            }
        //            #endregion


        //            #region formatting
        //            //using (var range = worksheet.Cells[3, 1, 3, 17])
        //            //{
        //            //    // Setting bold font
        //            //    range.Style.Font.Bold = true;
        //            //    // Setting fill type solid
        //            //    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            //    // Setting background gray
        //            //    range.Style.Fill.BackgroundColor.SetColor(Color.Goldenrod);
        //            //    // Setting font color
        //            //    range.Style.Font.Color.SetColor(Color.Black);

        //            //    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            //    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            //    range.Style.Font.Size = 12;
        //            //    range.Style.WrapText = true;
        //            //    range.Style.ShrinkToFit = true;
        //            //    range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
        //            //}
        //            using (var range = worksheet.Cells[1, 1, 2, 10])
        //            {
        //                // Setting bold font
        //                range.Style.Font.Bold = true;
        //                // Setting fill type solid
        //                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                // Setting background gray
        //                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

        //                // Setting font color
        //                range.Style.Font.Color.SetColor(Color.Black);

        //                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                range.Style.Font.Size = 12;
        //                range.Style.WrapText = true;
        //                range.Style.ShrinkToFit = true;
        //                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                var border1 = range.Style.Border;
        //                border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            }

        //            worksheet.PrinterSettings.FitToPage = true;
        //            worksheet.PrinterSettings.ShowGridLines = true;

        //            #endregion

        //            Response = excel.GetAsByteArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return Response;
        //}
        //public byte[] ExportAllTripPoint(string lst, string Name)
        //{
        //    dynamic FResult = JObject.Parse(lst);
        //    JArray jresult = FResult.Table;
        //    JArray uhfrowdata = FResult.Table1;
        //    List<UHFAssetData1> _lstuhfdata = uhfrowdata.ToObject<List<UHFAssetData1>>();
        //    byte[] Response = null;
        //    // JArray jresult = JArray.Parse(lst);
        //    try
        //    {
        //        using (ExcelPackage excel = new ExcelPackage())
        //        {
        //            int Count = 1;
        //            int RowCount = 4;
        //            excel.Workbook.Properties.Author = "Ajeevi Technologies";
        //            excel.Workbook.Properties.Title = Name + " -Details" + DateTime.Now.ToString("MMMM");

        //            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

        //            #region Creating Header
        //            worksheet.Cells[1, 1, 1, 16].Value = Name; worksheet.Cells[1, 1, 2, 16].Merge = true;


        //            worksheet.Cells[3, 1].Value = "Sr No.";
        //            worksheet.Cells[3, 2].Value = "Route Code"; worksheet.Column(2).Width = 15;
        //            worksheet.Cells[3, 3].Value = "TId"; worksheet.Column(3).Width = 15;
        //            worksheet.Cells[3, 4].Value = "VehicleNo"; worksheet.Column(4).Width = 15;
        //            worksheet.Cells[3, 5].Value = "BufferMin"; worksheet.Column(5).Width = 25;
        //            worksheet.Cells[3, 6].Value = "TotalStop"; worksheet.Column(6).Width = 22;



        //            var cells = worksheet.Cells[3, 1, 3, 16];
        //            var fill = cells.Style.Fill;
        //            //cells.AutoFitColumns();
        //            cells.Style.Locked = true;
        //            cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //            cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            cells.Style.Font.Bold = true;
        //            cells.Style.WrapText = true;
        //            cells.Style.ShrinkToFit = true;

        //            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

        //            var border = cells.Style.Border;
        //            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //            #endregion



        //            #region filling data

        //            foreach (JObject item in jresult)
        //            {
        //                worksheet.Cells[RowCount, 1].Value = Count;
        //                worksheet.Cells[RowCount, 2].Value = item.GetValue("RouteCode").ToString();
        //                worksheet.Cells[RowCount, 3].Value = item.GetValue("TId").ToString();
        //                worksheet.Cells[RowCount, 4].Value = item.GetValue("VehicleNo").ToString();
        //                worksheet.Cells[RowCount, 5].Value = item.GetValue("BufferMin").ToString();
        //                worksheet.Cells[RowCount, 6].Value = item.GetValue("TotalStop").ToString();

        //                worksheet.Cells[RowCount, 1, RowCount, 15].Style.WrapText = true;
        //                worksheet.Cells[RowCount, 1, RowCount, 15].Style.ShrinkToFit = true;
        //                //worksheet.Cells[RowCount, 1, RowCount, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                // worksheet.Cells[RowCount, 1, RowCount, 11].Style.Font.Color.SetColor(Color.Black);
        //                worksheet.Cells[RowCount, 1, RowCount, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                worksheet.Cells[RowCount, 1, RowCount, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                RowCount++;

        //                //DateTime PrevDate = FDate;//Convert.ToDateTime(item.GetValue("PDate").ToString());
        //                //DateTime NextDate = TDate;//Convert.ToDateTime(item.GetValue("Dated").ToString());
        //               // NextDate = NextDate.AddSeconds(5);
        //                List<UHFAssetData1> iuhfdata = _lstuhfdata.Where(i => i.RouteId == Convert.ToInt32(item.GetValue("RouteId")) && i.TripId== Convert.ToInt32(item.GetValue("TripId")))
        //                                                .OrderBy(i => i.SPickupTime).ToList();

        //                int IUhfCount = 1;
        //                foreach (UHFAssetData1 idata in iuhfdata)
        //                {
        //                    // ExcelRange rng = worksheet.ProtectedRanges(worksheet.Cells[3, 1], worksheet.Cells[3, 20]);// worksheet.Cells[3, 1, 3, 20]

        //                    //worksheet.Row(RowCount).Collapsed = false;

        //                    if (IUhfCount == 1)
        //                    {

        //                        worksheet.Cells[RowCount, 2].Value = "Sr. No.";
        //                        worksheet.Cells[RowCount, 3].Value = "GeoPointName";
        //                        worksheet.Cells[RowCount, 4].Value = "SPickupTime";
        //                        worksheet.Cells[RowCount, 5].Value = "ZoneNo";
        //                        worksheet.Cells[RowCount, 6].Value = "GeoPointCategory";


        //                        var Icells = worksheet.Cells[RowCount, 2, RowCount, 6];
        //                        var Ifill = Icells.Style.Fill;
        //                        //cells.AutoFitColumns();
        //                        Icells.Style.Locked = true;
        //                        Icells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //                        Icells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                        Icells.Style.Font.Bold = true;
        //                        Icells.Style.WrapText = true;
        //                        Icells.Style.ShrinkToFit = true;

        //                        Ifill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                        Ifill.BackgroundColor.SetColor(Color.FromArgb(189, 183, 107));

        //                        var Iborder = Icells.Style.Border;
        //                        Iborder.Top.Style = Iborder.Left.Style = Iborder.Bottom.Style = Iborder.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //                        worksheet.Row(RowCount).OutlineLevel = 1;
        //                        worksheet.Row(RowCount).Collapsed = true;
        //                        RowCount++;
        //                    }
        //                    worksheet.Row(RowCount).OutlineLevel = 1;
        //                    worksheet.Row(RowCount).Collapsed = true;


        //                    worksheet.Cells[RowCount, 2].Value = IUhfCount;
        //                    worksheet.Cells[RowCount, 3].Value = idata.GeoPointName;
        //                    worksheet.Cells[RowCount, 4].Value = idata.SPickupTime;
        //                    worksheet.Cells[RowCount, 5].Value = idata.ZoneNo;//idata.Dated.ToString("dd-MM-yyyy HH:mm tt");
        //                    worksheet.Cells[RowCount, 6].Value = idata.GeoPointCategory;


        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.WrapText = true;
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.ShrinkToFit = true;
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 228, 196));
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                    worksheet.Cells[RowCount, 1, RowCount, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                    RowCount++;
        //                    IUhfCount++;
        //                }

        //                Count++;

        //            }
        //            #endregion

        //            #region formatting
        //            //using (var range = worksheet.Cells[3, 1, 3, 20])
        //            //{
        //            //    // Setting bold font
        //            //    range.Style.Font.Bold = true;
        //            //    // Setting fill type solid
        //            //    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            //    // Setting background gray
        //            //    range.Style.Fill.BackgroundColor.SetColor(Color.Goldenrod);
        //            //    // Setting font color
        //            //    range.Style.Font.Color.SetColor(Color.Black);

        //            //    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            //    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            //    range.Style.Font.Size = 12;
        //            //    range.Style.WrapText = true;
        //            //    range.Style.ShrinkToFit = true;
        //            //    range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
        //            //}
        //            using (var range = worksheet.Cells[1, 1, 2, 16])
        //            {
        //                // Setting bold font
        //                range.Style.Font.Bold = true;
        //                // Setting fill type solid
        //                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                // Setting background gray
        //                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

        //                // Setting font color
        //                range.Style.Font.Color.SetColor(Color.Black);

        //                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                range.Style.Font.Size = 12;
        //                range.Style.WrapText = true;
        //                range.Style.ShrinkToFit = true;
        //                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                var border1 = range.Style.Border;
        //                border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            }

        //            worksheet.PrinterSettings.FitToPage = true;
        //            worksheet.PrinterSettings.ShowGridLines = true;

        //            #endregion

        //            Response = excel.GetAsByteArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return Response;
        //}
        public byte[] ExportAllTripPoint(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 10].Value = Name; worksheet.Cells[1, 1, 2, 10].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Route Code"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Route Trip Code"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Vehicle No"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Geo Point Name"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Geo Point Category"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Latitude"; worksheet.Column(7).Width = 35;
                    worksheet.Cells[3, 8].Value = "Longitude"; worksheet.Column(8).Width = 35;
                    worksheet.Cells[3, 9].Value = "Pickup Time"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Zone No"; worksheet.Column(10).Width = 25;


                    var cells = worksheet.Cells[3, 1, 3, 10];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion


                    string previousRoute = string.Empty;

                    #region filling data

                    foreach (JObject item in jresult)
                    {
                        if (previousRoute != item.GetValue("RouteCode").ToString())
                        {
                            Count = 1;
                        }
                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("RouteCode").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("RouteTripCode").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("GeoPointName").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("GeoPointCategory").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("Lat").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("Lng").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("SPickupTime").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("ZoneNo").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        previousRoute = item.GetValue("RouteCode").ToString();
                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 10])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        //private void GenerateDocument(int Id)
        //{
        //    #region Drive Path
        //    string AttchmentFolder = "DownloadedFile" + @"\";
        //    string path = Server.MapPath("");
        //    int Symbolcount = path.IndexOf(path.Split('\\').Last());

        //    string splitpath = path.Substring(0, Symbolcount);
        //    string finalstring = splitpath + AttchmentFolder;
        //    #endregion

        //    string EmpName = string.Empty;
        //    string sourcefile = string.Empty;
        //    string destinationfile = string.Empty;
        //    EmployeeInfo empinfo = null;
        //    string PdfFileName = string.Empty;
        //    string Extension = string.Empty;
        //    if (ddlLetterType.SelectedIndex > 0)
        //    {
        //        switch (ddlLetterType.SelectedValue)
        //        {
        //            case "1":
        //                sourcefile = finalstring + "OfferLetter.docx";

        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "OfferLetter" + ".pdf";
        //                destinationfile = finalstring + EmpName + "OfferLetter" + ".docx";
        //                if (File.Exists(finalstring + "OfferLetter.docx"))
        //                {

        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }

        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " OfferLetter" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //            case "3":
        //                //  int? letterid = 3;
        //                sourcefile = finalstring + "Confirmation Letter.docx";

        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "Confirmation Letter" + ".pdf";
        //                destinationfile = finalstring + EmpName + "Confirmation Letter" + ".docx";
        //                if (File.Exists(finalstring + "Confirmation Letter.docx"))
        //                {

        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " ConfirmationLetter" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }

        //                break;
        //            case "4":

        //                sourcefile = finalstring + "Experience Letter.docx";


        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "Experience Letter" + ".pdf";
        //                destinationfile = finalstring + EmpName + "Experience Letter.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " Experience Letter" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }

        //                break;
        //            case "5":


        //                sourcefile = finalstring + "Appointment Letter Emp.docx";

        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "Appointment Letter Emp" + ".pdf";
        //                destinationfile = finalstring + EmpName + " Appointment Letter Emp.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " Appointment Emp" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //            case "2":
        //                sourcefile = finalstring + "Appointment Letter Official.docx";

        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "Appointment Letter Official" + ".pdf";
        //                destinationfile = finalstring + EmpName + " Appointment Letter Official.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " Appointment Official" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //            case "6":
        //                sourcefile = finalstring + "Termination Letter.docx";


        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "Termination Letter" + ".pdf";
        //                destinationfile = finalstring + EmpName + "Termination Letter.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " Termination Letter" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //            case "7":
        //                sourcefile = finalstring + "Warning Letter.docx";


        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "Warning Letter" + ".pdf";
        //                destinationfile = finalstring + EmpName + "Warning Letter.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " Warning Letter" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //            case "8":
        //                sourcefile = finalstring + "Increment Letter.docx";

        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "Increment Letter" + ".pdf";
        //                destinationfile = finalstring + EmpName + "Increment Letter.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " Increment Letter" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //            case "9":
        //                sourcefile = finalstring + "No Dues.docx";

        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "No Dues" + ".pdf";
        //                destinationfile = finalstring + EmpName + "No Dues.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " No Dues" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //            case "10":
        //                sourcefile = finalstring + "Joining form.docx";
        //                this.EmpInformationArray = oEmp.GetEmployeeInfoById(this.EmpCode);
        //                if (this.EmpInformationArray != null && this.EmpInformationArray.Count > 0)
        //                {
        //                    EmployeeInfo objInfo = this.EmpInformationArray[0] as EmployeeInfo;
        //                    PersonalDetails objPersonalDetails = this.EmpInformationArray[2] as PersonalDetails;
        //                    List<CustomFile> objIdentityDetails = this.EmpInformationArray[3] as List<CustomFile>;
        //                    List<CustomAddressDetails> _lstObject = this.EmpInformationArray[4] as List<CustomAddressDetails>;

        //                    CustomAddressDetails objAddressInfo = _lstObject.FirstOrDefault();

        //                    EmpName = objInfo.FirstName + " " + objInfo.MiddleName + " " + objInfo.LastName; //objInfo.FirstName.Trim().Replace("\t", "");
        //                    EmpName = EmpName.Trim().Replace("\t", "");
        //                    PdfFileName = finalstring + EmpName + "Joining form" + ".pdf";
        //                    destinationfile = finalstring + EmpName + "Joining form.docx";
        //                    if (File.Exists(sourcefile))
        //                    {
        //                        CreateEmpDocument(sourcefile, destinationfile, EmpName, objInfo, objPersonalDetails, objAddressInfo, objIdentityDetails);
        //                        Extension = Id == 1 ? ".pdf" : ".docx";
        //                        if (Id == 1)
        //                        {
        //                            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                            wordDocument = appWord.Documents.Open(destinationfile);
        //                            wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                            appWord.Documents.Close();
        //                        }
        //                    }

        //                }
        //                byte[] FContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                //code for downloading files
        //                Response.Clear();
        //                Response.Buffer = true;
        //                Response.ContentType = "application/octet-stream";
        //                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " Joining form" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                Response.BinaryWrite(FContent);
        //                Response.End();
        //                break;

        //            case "11":
        //                sourcefile = finalstring + "DriverPerforma.docx";

        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "DriverPerforma" + ".pdf";
        //                destinationfile = finalstring + EmpName + "DriverPerforma.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " DriverPerforma" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //            case "12":
        //                sourcefile = finalstring + "WarningCause.docx";

        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "SoCause Notice 1" + ".pdf";
        //                destinationfile = finalstring + EmpName + "SoCause Notice 1.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " SoCause Notice 1" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //            case "13":
        //                sourcefile = finalstring + "WarningCause1.docx";

        //                empinfo = oEmp.GetEmployeeDetails(this.EmpCode);
        //                EmpName = empinfo.FirstName.Trim().Replace("\t", "");
        //                PdfFileName = finalstring + EmpName + "SoCause Notice 2" + ".pdf";
        //                destinationfile = finalstring + EmpName + "SoCause Notice 2.docx";
        //                if (File.Exists(sourcefile))
        //                {
        //                    CreateDocument(sourcefile, destinationfile, EmpName, empinfo);
        //                    Extension = Id == 1 ? ".pdf" : ".docx";
        //                    if (Id == 1)
        //                    {
        //                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
        //                        wordDocument = appWord.Documents.Open(destinationfile);
        //                        wordDocument.ExportAsFixedFormat(PdfFileName, WdExportFormat.wdExportFormatPDF);
        //                        appWord.Documents.Close();
        //                    }
        //                    byte[] FileContent = File.ReadAllBytes(Id == 1 ? PdfFileName : destinationfile);
        //                    //code for downloading files
        //                    Response.Clear();
        //                    Response.Buffer = true;
        //                    Response.ContentType = "application/octet-stream";
        //                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EmpName + " SoCause Notice 2" + Extension.Replace(" ", "_") + "\"");     // to open file prompt Box open or Save file         
        //                    Response.BinaryWrite(FileContent);
        //                    Response.End();
        //                }
        //                break;
        //        }
        //    }
        //}
        public byte[] ExportAllDeployLocation(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 10].Value = Name; worksheet.Cells[1, 1, 2, 10].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Status"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "LandMark"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Zone"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Circle"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Ward"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Lattitude | Longitude"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Radius(In Mtr.)"; worksheet.Column(8).Width = 25;

                    worksheet.Cells[3, 9].Value = "Created By"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Last Modified On"; worksheet.Column(10).Width = 25;


                    var cells = worksheet.Cells[3, 1, 3, 10];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {
                        Count = 1;
                        RowCount = 4;

                        worksheet.Cells[RowCount, 1].Value = Count;

                        if (item.GetValue("IsActive").ToString() == "True")
                            worksheet.Cells[RowCount, 2].Value = "ACTIVE";
                        else
                            worksheet.Cells[RowCount, 2].Value = "DE-ACTIVE";

                        worksheet.Cells[RowCount, 3].Value = item.GetValue("LandMark").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("Lat").ToString() + " | " + item.GetValue("Lng").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("Radius").ToString();

                        worksheet.Cells[RowCount, 9].Value = item.GetValue("CreatedBy").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("ModifiedOn").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 10])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }


        public byte[] ExportAllVehiclMasterDetail(string result, string result1, string Name, string Sname)
        {
            byte[] Response = null;
            JArray _lst = new JArray();
            JArray _lst1 = new JArray();


            if (!string.IsNullOrEmpty(result1))
            {
                JArray jresult = JArray.Parse(result1);
                _lst = jresult;
            }

            if (!string.IsNullOrEmpty(result))
            {
                JArray jresult1 = JArray.Parse(result);
                _lst1 = jresult1;
            }




            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Vehicle Details");
                    ExcelWorksheet worksheet1 = excel.Workbook.Worksheets.Add("Vehicle Summary");


                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 23].Value = "Vehicle Details"; worksheet.Cells[1, 1, 2, 23].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "UId"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Vehicle Reg. No"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Chassis No"; worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Owner Type"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Vehicle Operation"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Zone"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Circle Name"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Ward"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Gross Weight"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Tare Weight"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "Net Weight"; worksheet.Column(13).Width = 25;
                    worksheet.Cells[3, 14].Value = "Driver Name"; worksheet.Column(14).Width = 25;
                    worksheet.Cells[3, 15].Value = "Driver ContactNo"; worksheet.Column(15).Width = 25;
                    worksheet.Cells[3, 16].Value = "Incharge Name"; worksheet.Column(16).Width = 25;
                    worksheet.Cells[3, 17].Value = "Incharge ContactNo"; worksheet.Column(17).Width = 25;
                    worksheet.Cells[3, 18].Value = "LandMark"; worksheet.Column(18).Width = 25;
                    worksheet.Cells[3, 19].Value = "Landmark Cordinate"; worksheet.Column(19).Width = 25;
                    worksheet.Cells[3, 20].Value = "Landmark Radius(In Mtr.)"; worksheet.Column(20).Width = 25;
                    worksheet.Cells[3, 21].Value = "Transfer Station"; worksheet.Column(21).Width = 25;
                    worksheet.Cells[3, 22].Value = "TS Cordinates"; worksheet.Column(22).Width = 25;
                    worksheet.Cells[3, 23].Value = "TS Radius(In Mtr.)"; worksheet.Column(23).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 23];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                    #endregion

                    #region filling data
                    Count = 1;
                    RowCount = 4;


                    foreach (JObject item in _lst)
                    {


                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("UId").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("ChassisNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("OwnerType").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("OperationType").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("GrossWt").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("TareWt").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("NetWt").ToString();
                        worksheet.Cells[RowCount, 14].Value = item.GetValue("DriverName").ToString();
                        worksheet.Cells[RowCount, 15].Value = item.GetValue("ContactNo").ToString();
                        worksheet.Cells[RowCount, 16].Value = item.GetValue("InchargeName").ToString();
                        worksheet.Cells[RowCount, 17].Value = item.GetValue("InchargeContactNo").ToString();
                        worksheet.Cells[RowCount, 18].Value = item.GetValue("LandMark").ToString();
                        worksheet.Cells[RowCount, 19].Value = item.GetValue("DLCordinates").ToString();
                        worksheet.Cells[RowCount, 20].Value = item.GetValue("DLRadius").ToString();
                        worksheet.Cells[RowCount, 21].Value = item.GetValue("DTStationName").ToString();
                        worksheet.Cells[RowCount, 22].Value = item.GetValue("DTCordinates").ToString();
                        worksheet.Cells[RowCount, 23].Value = item.GetValue("DTRadius").ToString();






                        worksheet.Cells[RowCount, 1, RowCount, 23].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 23].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 23].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 23].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 23])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    #region Creating Header
                    worksheet1.Cells[1, 1, 1, 7].Value = "Vehicle Summary"; worksheet1.Cells[1, 1, 2, 7].Merge = true;


                    worksheet1.Cells[3, 1].Value = "Sr No.";
                    worksheet1.Cells[3, 2].Value = "VehicleType"; worksheet1.Column(2).Width = 25;
                    worksheet1.Cells[3, 3].Value = "Active"; worksheet1.Column(3).Width = 25;
                    worksheet1.Cells[3, 4].Value = "InActive"; worksheet1.Column(4).Width = 25;
                    worksheet1.Cells[3, 5].Value = "Repair"; worksheet1.Column(5).Width = 15;
                    worksheet1.Cells[3, 6].Value = "Condemed"; worksheet1.Column(6).Width = 25;
                    worksheet1.Cells[3, 7].Value = "Total"; worksheet1.Column(7).Width = 25;




                    var cells1 = worksheet1.Cells[3, 1, 3, 7];
                    var fill1 = cells1.Style.Fill;
                    //cells.AutoFitColumns();
                    cells1.Style.Locked = true;
                    cells1.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells1.Style.Font.Bold = true;
                    cells1.Style.WrapText = true;
                    cells1.Style.ShrinkToFit = true;

                    fill1.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill1.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border3 = cells1.Style.Border;
                    border3.Top.Style = border3.Left.Style = border3.Bottom.Style = border3.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data
                    Count = 1;
                    RowCount = 4;

                    foreach (JObject item in _lst1)
                    {

                        worksheet1.Cells[RowCount, 1].Value = Count;
                        worksheet1.Cells[RowCount, 2].Value = item.GetValue("VehicleType").ToString();
                        worksheet1.Cells[RowCount, 3].Value = item.GetValue("TotalActive").ToString();
                        worksheet1.Cells[RowCount, 4].Value = item.GetValue("TotalInActive").ToString();
                        worksheet1.Cells[RowCount, 5].Value = item.GetValue("TotalRepair").ToString();
                        worksheet1.Cells[RowCount, 6].Value = item.GetValue("TotalCondemed").ToString();
                        worksheet1.Cells[RowCount, 7].Value = item.GetValue("Total").ToString();


                        worksheet1.Cells[RowCount, 1, RowCount, 7].Style.WrapText = true;
                        worksheet1.Cells[RowCount, 1, RowCount, 7].Style.ShrinkToFit = true;
                        worksheet1.Cells[RowCount, 1, RowCount, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet1.Cells[RowCount, 1, RowCount, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet1.Cells[1, 1, 2, 7])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border4 = range.Style.Border;
                        border4.Top.Style = border4.Left.Style = border4.Bottom.Style = border4.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet1.PrinterSettings.FitToPage = true;
                    worksheet1.PrinterSettings.ShowGridLines = true;

                    #endregion



                    Response = excel.GetAsByteArray();
                }

            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public string ExportAllVehicleDetailsCSV(string lst, string Name)
        {
            StringBuilder csvContent = new StringBuilder();

            byte[] Response = null;
            JArray _lst1 = JArray.Parse(lst);



            try
            {
                int Count = 1;

                #region Creating Header
                // Add column headers to CSV
                csvContent.AppendLine("Sr No.,ALL,Status,UId(QR Code),Print QRCode,Vehicle Reg. No,Chassis No,Vehicle Type,Owner Type,Vehicle Operation,Zone, Circle,Ward,Gross Weight,Tare Weight,Net Weight,Image,Driver Name,Driver Contact No,Incharge Name,Incharge Contact No,Landmark,Landmark Cordinates,Landmark Radius(In Mtr.),Transfer Station,TS Cordinates,TS Radius(In Mtr.),Action");
                #endregion

                #region Filling data
                foreach (JObject item in _lst1)
                {
                    // string address = item.GetValue("Address").ToString().Replace(",", " ");
                    csvContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27}\n",
                        Count,
                        "",
                        item.GetValue("AssetStatus").ToString().Replace(",", " "),
                        item.GetValue("UId").ToString().Replace(",", " "),
                        "Print QRCode",
                        item.GetValue("VehicleNo").ToString().Replace(",", " "),
                        item.GetValue("ChassisNo").ToString().Replace(",", " "),
                        item.GetValue("VehicleType").ToString().Replace(",", " "),
                        item.GetValue("OwnerType").ToString().Replace(",", " "),
                        item.GetValue("OperationType").ToString().Replace(",", " "),

                        //$"\"{item.GetValue("Address").ToString()}\"",
                        item.GetValue("ZoneNo").ToString().Replace(",", " "),
                        item.GetValue("CircleName").ToString().Replace(",", " "),
                        item.GetValue("WardNo").ToString().Replace(",", " "),
                        item.GetValue("GrossWt").ToString().Replace(",", " "),
                        item.GetValue("TareWt").ToString().Replace(",", " "),
                        item.GetValue("NetWt").ToString().Replace(",", " "),
                        "",
                        item.GetValue("DriverName").ToString().Replace(",", " "),
                        item.GetValue("ContactNo").ToString().Replace(",", " "),
                        item.GetValue("InchargeName").ToString().Replace(",", " "),
                        item.GetValue("InchargeContactNo").ToString().Replace(",", " "),
                        item.GetValue("LandMark").ToString().Replace(",", " "),
                        item.GetValue("DLCordinates").ToString().Replace(",", " "),
                        item.GetValue("DLRadius").ToString().Replace(",", " "),
                        item.GetValue("DTStationName").ToString().Replace(",", " "),
                        item.GetValue("DTCordinates").ToString().Replace(",", " "),
                        item.GetValue("DTRadius").ToString().Replace(",", " "),
                        "");

                    Count++;
                }
                #endregion

                // Convert the CSV content to a string and return it
                return csvContent.ToString();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return string.Empty;
            }
        }
        public byte[] ExportAllAttendenceDeploymentSummary(string result, string Name, string Sname, string result1)
        {
            byte[] Response = null;
            //JArray jresult = JArray.Parse(result);
            JArray _lst1 = new JArray();
            JArray _lst = new JArray();
            JArray _lstalrt = new JArray();
            JArray _lstalrt1 = new JArray();
            // JArray _lst1;
            // JArray _lstalrt ;
            if (!string.IsNullOrEmpty(result))
            {
                dynamic FResult = JObject.Parse(result);
                _lst1 = FResult.Table;
                _lstalrt = FResult.Table1;
                _lstalrt1 = FResult.Table2;
            }

            if (!string.IsNullOrEmpty(result1))
            {
                JArray FResult1 = JArray.Parse(result1);

                _lstalrt1 = FResult1;
            }
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Deployment Summary");
                    ExcelWorksheet worksheet1 = excel.Workbook.Worksheets.Add("Vehicle Trip Summary");
                    ExcelWorksheet worksheet2 = excel.Workbook.Worksheets.Add("Attendance Vs Reporting to TS");

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 8].Value = "Deployment Summary"; worksheet.Cells[1, 1, 2, 8].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Vehicle Type"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Deployed"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Not Deployed"; worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Reported at TS/SCTP"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Not Reported at TS/SCTP"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Not Deployed But Reported at TS/SCTP"; worksheet.Column(7).Width = 30;
                    worksheet.Cells[3, 8].Value = "Total"; worksheet.Column(8).Width = 25;




                    var cells = worksheet.Cells[3, 1, 3, 8];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in _lst1)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("Deployed").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("TotalDeployed").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("ReportedTs").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("NotReportedTs").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("NotDeployReportedTs").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("Total").ToString();


                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 8])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    #region Creating Header
                    worksheet1.Cells[1, 1, 1, 9].Value = "Vehicle Trip Summary"; worksheet1.Cells[1, 1, 2, 9].Merge = true;


                    worksheet1.Cells[3, 1].Value = "Sr No.";
                    worksheet1.Cells[3, 2].Value = "Vehicle Type"; worksheet1.Column(2).Width = 25;
                    worksheet1.Cells[3, 3].Value = "Deployed"; worksheet1.Column(3).Width = 25;
                    worksheet1.Cells[3, 4].Value = "1 Trip Reported at TS/SCTP"; worksheet1.Column(4).Width = 30;
                    worksheet1.Cells[3, 5].Value = "2 Trip Reported at TS/SCTP"; worksheet1.Column(5).Width = 30;
                    worksheet1.Cells[3, 6].Value = "3 Trip Reported at TS/SCTP"; worksheet1.Column(6).Width = 30;
                    worksheet1.Cells[3, 7].Value = "4 Trip Reported at TS/SCTP"; worksheet1.Column(7).Width = 30;
                    worksheet1.Cells[3, 8].Value = ">4 Trips Reported at TS/SCTP"; worksheet1.Column(8).Width = 35;
                    worksheet1.Cells[3, 9].Value = "Total"; worksheet1.Column(9).Width = 15;




                    var cells1 = worksheet1.Cells[3, 1, 3, 9];
                    var fill1 = cells1.Style.Fill;
                    //cells.AutoFitColumns();
                    cells1.Style.Locked = true;
                    cells1.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells1.Style.Font.Bold = true;
                    cells1.Style.WrapText = true;
                    cells1.Style.ShrinkToFit = true;

                    fill1.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill1.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border1 = cells1.Style.Border;
                    border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data
                    Count = 1;
                    RowCount = 4;

                    foreach (JObject item in _lstalrt)
                    {


                        worksheet1.Cells[RowCount, 1].Value = Count;
                        worksheet1.Cells[RowCount, 2].Value = item.GetValue("VehicleType").ToString();
                        worksheet1.Cells[RowCount, 3].Value = item.GetValue("Deployed").ToString();
                        worksheet1.Cells[RowCount, 4].Value = item.GetValue("OneTripCount").ToString();
                        worksheet1.Cells[RowCount, 5].Value = item.GetValue("TwoTripCount").ToString();
                        worksheet1.Cells[RowCount, 6].Value = item.GetValue("ThreeTripCount").ToString();
                        worksheet1.Cells[RowCount, 7].Value = item.GetValue("FourTripCount").ToString();
                        worksheet1.Cells[RowCount, 8].Value = item.GetValue("MorethanFourTripCount").ToString();
                        worksheet1.Cells[RowCount, 9].Value = item.GetValue("AllTripCount").ToString();


                        worksheet1.Cells[RowCount, 1, RowCount, 9].Style.WrapText = true;
                        worksheet1.Cells[RowCount, 1, RowCount, 9].Style.ShrinkToFit = true;
                        worksheet1.Cells[RowCount, 1, RowCount, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet1.Cells[RowCount, 1, RowCount, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;

                    }


                    #endregion


                    #region formatting

                    using (var range = worksheet1.Cells[1, 1, 2, 9])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet1.PrinterSettings.FitToPage = true;
                    worksheet1.PrinterSettings.ShowGridLines = true;

                    #endregion


                    #region Creating Header
                    worksheet2.Cells[1, 1, 1, 11].Value = "Attendance Vs Reporting to TS"; worksheet2.Cells[1, 1, 2, 11].Merge = true;


                    worksheet2.Cells[3, 1].Value = "Sr No.";
                    worksheet2.Cells[3, 2].Value = "Veh Reg. No"; worksheet2.Column(2).Width = 25;
                    worksheet2.Cells[3, 3].Value = "Vehicle Type"; worksheet2.Column(3).Width = 25;
                    worksheet2.Cells[3, 4].Value = "Owner Type"; worksheet2.Column(4).Width = 25;
                    worksheet2.Cells[3, 5].Value = "Deployed On Time Stamp"; worksheet2.Column(5).Width = 25;
                    worksheet2.Cells[3, 6].Value = "Deployed By SFA (Name/Contact)"; worksheet2.Column(6).Width = 25;
                    worksheet2.Cells[3, 7].Value = "Allotted TS/SCTP Name"; worksheet2.Column(7).Width = 25;
                    worksheet2.Cells[3, 8].Value = "Reported TS/SCTP Name"; worksheet2.Column(8).Width = 25;
                    worksheet2.Cells[3, 9].Value = "Reported on Timestamp"; worksheet2.Column(9).Width = 25;
                    worksheet2.Cells[3, 10].Value = "Trip No"; worksheet2.Column(10).Width = 25;
                    worksheet2.Cells[3, 11].Value = "Zone|Ward|Circle"; worksheet2.Column(11).Width = 25;




                    var cells2 = worksheet2.Cells[3, 1, 3, 11];
                    var fill2 = cells2.Style.Fill;
                    //cells.AutoFitColumns();
                    cells2.Style.Locked = true;
                    cells2.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells2.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells2.Style.Font.Bold = true;
                    cells2.Style.WrapText = true;
                    cells2.Style.ShrinkToFit = true;

                    fill2.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill2.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border3 = cells2.Style.Border;
                    border3.Top.Style = border3.Left.Style = border3.Bottom.Style = border3.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data
                    Count = 1;
                    RowCount = 4;


                    foreach (JObject item in _lstalrt1)
                    {


                        worksheet2.Cells[RowCount, 1].Value = Count;
                        worksheet2.Cells[RowCount, 2].Value = item.GetValue("VehicleNo").ToString();
                        worksheet2.Cells[RowCount, 3].Value = item.GetValue("VehicleType").ToString();
                        worksheet2.Cells[RowCount, 4].Value = item.GetValue("OwnerType").ToString();
                        worksheet2.Cells[RowCount, 5].Value = item.GetValue("DeployedOn").ToString();
                        worksheet2.Cells[RowCount, 6].Value = item.GetValue("ConcatDeployed").ToString();
                        worksheet2.Cells[RowCount, 7].Value = item.GetValue("AllotedTSName").ToString();
                        worksheet2.Cells[RowCount, 8].Value = item.GetValue("RTSName").ToString();
                        worksheet2.Cells[RowCount, 9].Value = item.GetValue("ReportedOn").ToString();
                        worksheet2.Cells[RowCount, 10].Value = item.GetValue("TripNo").ToString();
                        worksheet2.Cells[RowCount, 11].Value = item.GetValue("ZWC").ToString();




                        worksheet2.Cells[RowCount, 1, RowCount, 10].Style.WrapText = true;
                        worksheet2.Cells[RowCount, 1, RowCount, 10].Style.ShrinkToFit = true;
                        worksheet2.Cells[RowCount, 1, RowCount, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet2.Cells[RowCount, 1, RowCount, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;

                    }

                    #endregion

                    #region formatting

                    using (var range = worksheet2.Cells[1, 1, 2, 11])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border4 = range.Style.Border;
                        border4.Top.Style = border4.Left.Style = border4.Bottom.Style = border4.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet2.PrinterSettings.FitToPage = true;
                    worksheet2.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }





            }
            catch (Exception ex)
            {
            }
            return Response;
        }


        public byte[] ExportAllVehicleSummaryDetail(string result, string Name, string Sname)
        {
            byte[] Response = null;
            JArray _lst = new JArray();



            if (!string.IsNullOrEmpty(result))
            {
                JArray jresult1 = JArray.Parse(result);
                _lst = jresult1;
            }




            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Vehicle Summary Detail");



                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 23].Value = "Vehicle Summary Details"; worksheet.Cells[1, 1, 2, 23].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "UId"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Vehicle Reg. No"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Chassis No"; worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Owner Type"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Vehicle Operation"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Zone"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Circle Name"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Ward"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Gross Weight"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Tare Weight"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "Net Weight"; worksheet.Column(13).Width = 25;
                    worksheet.Cells[3, 14].Value = "Driver Name"; worksheet.Column(14).Width = 25;
                    worksheet.Cells[3, 15].Value = "Driver ContactNo"; worksheet.Column(15).Width = 25;
                    worksheet.Cells[3, 16].Value = "Incharge Name"; worksheet.Column(16).Width = 25;
                    worksheet.Cells[3, 17].Value = "Incharge ContactNo"; worksheet.Column(17).Width = 25;
                    worksheet.Cells[3, 18].Value = "LandMark"; worksheet.Column(18).Width = 25;
                    worksheet.Cells[3, 19].Value = "Landmark Cordinate"; worksheet.Column(19).Width = 25;
                    worksheet.Cells[3, 20].Value = "Landmark Radius(In Mtr.)"; worksheet.Column(20).Width = 25;
                    worksheet.Cells[3, 21].Value = "Transfer Station"; worksheet.Column(21).Width = 25;
                    worksheet.Cells[3, 22].Value = "TS Cordinates"; worksheet.Column(22).Width = 25;
                    worksheet.Cells[3, 23].Value = "TS Radius(In Mtr.)"; worksheet.Column(23).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 23];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                    #endregion

                    #region filling data
                    Count = 1;
                    RowCount = 4;


                    foreach (JObject item in _lst)
                    {


                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("UId").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("ChassisNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("OwnerType").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("OperationType").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("GrossWt").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("TareWt").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("NetWt").ToString();
                        worksheet.Cells[RowCount, 14].Value = item.GetValue("DriverName").ToString();
                        worksheet.Cells[RowCount, 15].Value = item.GetValue("ContactNo").ToString();
                        worksheet.Cells[RowCount, 16].Value = item.GetValue("InchargeName").ToString();
                        worksheet.Cells[RowCount, 17].Value = item.GetValue("InchargeContactNo").ToString();
                        worksheet.Cells[RowCount, 18].Value = item.GetValue("LandMark").ToString();
                        worksheet.Cells[RowCount, 19].Value = item.GetValue("DLCordinates").ToString();
                        worksheet.Cells[RowCount, 20].Value = item.GetValue("DLRadius").ToString();
                        worksheet.Cells[RowCount, 21].Value = item.GetValue("DTStationName").ToString();
                        worksheet.Cells[RowCount, 22].Value = item.GetValue("DTCordinates").ToString();
                        worksheet.Cells[RowCount, 23].Value = item.GetValue("DTRadius").ToString();






                        worksheet.Cells[RowCount, 1, RowCount, 23].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 23].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 23].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 23].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 23])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    Response = excel.GetAsByteArray();
                }

            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportAllVehicleDepNotDepSummary(string result, string Name, string Sname)
        {
            byte[] Response = null;
            JArray _lst = new JArray();

            if (!string.IsNullOrEmpty(result))
            {
                JArray jresult1 = JArray.Parse(result);
                _lst = jresult1;
            }

            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Vehicle Summary Detail");

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 12].Value = "Vehicle Summary Details"; worksheet.Cells[1, 1, 2, 12].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle Name"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward"; worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Driver Name"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Driver ContactNo"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Vehicle Reg. No"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "SFA Name"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "SFA ContactNo"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Last Deployed Date and Time"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Days to the current date"; worksheet.Column(12).Width = 25;


                    var cells = worksheet.Cells[3, 1, 3, 12];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                    #endregion

                    #region filling data
                    Count = 1;
                    RowCount = 4;


                    foreach (JObject item in _lst)
                    {
                        string NoofTotalDays = "";
                        if (item.GetValue("LastDeployedDate").ToString() == "N/A")
                        {
                            NoofTotalDays = "N/A";
                        }
                        else
                        {
                            NoofTotalDays = item.GetValue("LastDeployedDate").ToString();
                        }
                        worksheet.Cells[RowCount, 1].Value = Count;

                        worksheet.Cells[RowCount, 2].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("DriverName").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("ContactNo").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("InchargeName").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("InchargeContactNo").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("LastDeployedDate").ToString();
                        worksheet.Cells[RowCount, 12].Value = NoofTotalDays;



                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 12])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    Response = excel.GetAsByteArray();
                }

            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public byte[] ExportDeployedVsNotDeployedSummary(string result, string Name, string Sname)
        {
            byte[] Response = null;

            dynamic FResult = JObject.Parse(result);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;

            List<DepZoneInfo> zonelst = _lst1.ToObject<List<DepZoneInfo>>();
            List<DepOutDataInfo_New> outlst = _lstout.ToObject<List<DepOutDataInfo_New>>();

            //// Calculate totals for each zone
            //zonelst.Select(i =>
            //{
            //    i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
            //    i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Deployed);
            //    i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.NotDeployed);
            //    return i;
            //}).ToList();

            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Deployed Vs Not Deployed Report");
                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 9].Value = "Deployed Vs Not Deployed Report";
                    worksheet.Cells[1, 1, 2, 9].Merge = true;

                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone";
                    worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle Name";
                    worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward";
                    worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type";
                    worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Total Vehicles";
                    worksheet.Column(6).Width = 10;
                    worksheet.Cells[3, 7].Value = "Deployed";
                    worksheet.Column(7).Width = 10;
                    worksheet.Cells[3, 8].Value = "Not Deployed";
                    worksheet.Column(8).Width = 10;
                    worksheet.Cells[3, 9].Value = "Deployed(%)";
                    worksheet.Column(9).Width = 12;

                    var cells = worksheet.Cells[3, 1, 3, 9];
                    var fill = cells.Style.Fill;
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion


                    Count = 1;
                    RowCount = 4;
                    int CRowCount = 4;

                    // Grouping logic for 3rd and 4th columns

                    for (int i = 0; i < outlst.Count; i++)
                    {
                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = outlst[i].Zonecode;
                        worksheet.Cells[RowCount, 6].Value = outlst[i].Deployed + outlst[i].NotDeployed;
                        //worksheet.Cells[RowCount, 5].Value = outlst[i].VehicleType;
                        worksheet.Cells[RowCount, 7].Value = outlst[i].Deployed;
                        worksheet.Cells[RowCount, 8].Value = outlst[i].NotDeployed;
                        worksheet.Cells[RowCount, 9].Value = outlst[i].DepPercentage + " %";

                        worksheet.Cells[RowCount, 1, RowCount, 9].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 9].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        Count++;
                        RowCount++;
                    }

                    List<string> circlelst = outlst.Select(i => i.CircleCode).Distinct().ToList();
                    int WCount = 4;

                    foreach (string item in circlelst)
                    {
                        int icount = outlst.Count(k => k.CircleCode == item);
                        int IColcount = (CRowCount + icount) - 1;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Value = item;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Merge = true;

                        List<string> wardlst = outlst.Where(i => i.CircleCode == item && !string.IsNullOrEmpty(i.WardNo)).Select(i => i.WardName).Distinct().ToList();

                        foreach (string witem in wardlst)
                        {
                            int wicount = outlst.Count(j => j.WardName == witem);
                            int IWColCount = (WCount + wicount) - 1;

                            worksheet.Cells[WCount, 4, IWColCount, 4].Value = witem;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Merge = true;

                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.WrapText = true;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.ShrinkToFit = true;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            // Now, group by VehicleType within the ward
                            List<string> vehicleTypeList = outlst.Where(i => i.CircleCode == item && i.WardName == witem).Select(i => i.VehicleType).Distinct().ToList();

                            foreach (string vType in vehicleTypeList)
                            {
                                int vTypeCount = outlst.Count(k => k.CircleCode == item && k.WardName == witem && k.VehicleType == vType);
                                int IVTColCount = (WCount + vTypeCount) - 1;

                                worksheet.Cells[WCount, 5, IVTColCount, 5].Value = vType;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Merge = true;

                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.WrapText = true;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.ShrinkToFit = true;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                                WCount = IVTColCount + 1;
                            }

                            //WCount++; // Add space between different ward groups
                        }

                        if (wardlst.Count() == 0)
                        {
                            worksheet.Cells[WCount, 4].Value = "";
                            // worksheet.Cells[CRowCount, 3, IColcount, 3].Style.Font.Bold = true;
                            WCount = WCount + 1;
                        }

                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.WrapText = true;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.ShrinkToFit = true;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        CRowCount = IColcount + 1;
                    }
                    int grandTotalDeployed = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(item => item.Deployed);
                    int grandTotalNotDeployed = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.NotDeployed);
                    double grandTotalDepPercentage = (double)grandTotalDeployed / (grandTotalDeployed + grandTotalNotDeployed) * 100;

                    // Find the last row in the worksheet
                    int lastRow = worksheet.Dimension.End.Row + 1;

                    // Add Grand Total row
                    worksheet.Cells[lastRow, 2].Value = "Grand Total";
                    worksheet.Cells[lastRow, 6].Value = grandTotalDeployed + grandTotalNotDeployed;
                    worksheet.Cells[lastRow, 7].Value = grandTotalDeployed;
                    worksheet.Cells[lastRow, 8].Value = grandTotalNotDeployed;
                    worksheet.Cells[lastRow, 9].Value = Math.Round(grandTotalDepPercentage, 2) + " %";

                    // Style the Grand Total row
                    worksheet.Cells[lastRow, 1, lastRow, 9].Style.Font.Bold = true;
                    worksheet.Cells[lastRow, 1, lastRow, 9].Style.WrapText = true;
                    worksheet.Cells[lastRow, 1, lastRow, 9].Style.ShrinkToFit = true;
                    worksheet.Cells[lastRow, 1, lastRow, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 9])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
            }
            return Response;
        }

        //public byte[] ExportDeployedVsNotDeployedSummary(string result, string Name, string Sname)
        //{
        //    byte[] Response = null;


        //    dynamic FResult = JObject.Parse(result);
        //    JArray _lst1 = FResult.Table;
        //    JArray _lstout = FResult.Table1;
        //    //JArray _lstcircle = FResult.Table1;
        //    //JArray _lstward = FResult.Table2;
        //    //JArray _lstvehicledata = FResult.Table3;
        //    //JArray _lstvehicletype = FResult.Table4;

        //    List<DepZoneInfo> zonelst = _lst1.ToObject<List<DepZoneInfo>>();
        //    List<DepOutDataInfo_New> outlst = _lstout.ToObject<List<DepOutDataInfo_New>>();


        //    zonelst.Select(i =>
        //    {
        //        i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
        //        i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Deployed);
        //        i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.NotDeployed);
        //        //i.Outlst = outlst.Where(j => j.ZId == i.ZId).ToList();
        //        //i.TotalZRowCount = i.Outlst.Count();
        //        //i.TotalCircleRowCount = i.Outlst.Select(i => i.CircleId).Distinct().Count();
        //        //i.TotalWardRowCount = i.Outlst.Select(i => i.WardId).Distinct().Count();
        //        return i;
        //    }
        //   ).ToList();


        //    try
        //    {
        //        using (ExcelPackage excel = new ExcelPackage())
        //        {
        //            int Count = 1;
        //            int RowCount = 4;
        //            excel.Workbook.Properties.Author = "Ajeevi Technologies";
        //            excel.Workbook.Properties.Title = Name;

        //            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Vehicle Summary Detail");

        //            #region Creating Header
        //            worksheet.Cells[1, 1, 1, 8].Value = "Vehicle Summary Details";
        //            worksheet.Cells[1, 1, 2, 8].Merge = true;

        //            worksheet.Cells[3, 1].Value = "Sr No.";
        //            worksheet.Cells[3, 2].Value = "Zone";
        //            worksheet.Column(2).Width = 25;
        //            worksheet.Cells[3, 3].Value = "Circle Name";
        //            worksheet.Column(3).Width = 25;
        //            worksheet.Cells[3, 4].Value = "Ward";
        //            worksheet.Column(4).Width = 25;
        //            worksheet.Cells[3, 5].Value = "Vehicle Type";
        //            worksheet.Column(5).Width = 25;
        //            worksheet.Cells[3, 6].Value = "Deployed";
        //            worksheet.Column(6).Width = 25;
        //            worksheet.Cells[3, 7].Value = "Not Deployed";
        //            worksheet.Column(7).Width = 25;
        //            worksheet.Cells[3, 8].Value = "Deployed Percentage";
        //            worksheet.Column(8).Width = 25;

        //            var cells = worksheet.Cells[3, 1, 3, 8];
        //            var fill = cells.Style.Fill;
        //            cells.Style.Locked = true;
        //            cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //            cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            cells.Style.Font.Bold = true;
        //            cells.Style.WrapText = true;
        //            cells.Style.ShrinkToFit = true;

        //            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

        //            var border = cells.Style.Border;
        //            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //            #endregion

        //            #region Filling Data
        //            Count = 1;
        //            RowCount = 4;

        //            // Grouping logic for 3rd and 4th columns
        //            string lastGroup = "";

        //            List<string> circleNames = new List<string>();

        //            for (int i = 0; i < outlst.Count; i++)
        //            {
        //                string circleName = outlst[i].CircleName; 
        //                circleNames.Add(circleName);
        //            }


        //            foreach (JObject item in _lstout)
        //            {
        //                string groupValue = item.GetValue("CircleName").ToString() + "-" + item.GetValue("WardNo").ToString();
        //                if (groupValue != lastGroup)
        //                {
        //                    // Insert a group row
        //                    worksheet.Cells[RowCount, 3, RowCount, 8].Merge = true;
        //                    worksheet.Cells[RowCount, 3].Value = "Group: " + groupValue;
        //                    lastGroup = groupValue;
        //                    RowCount++;
        //                }

        //                worksheet.Cells[RowCount, 1].Value = Count;

        //                worksheet.Cells[RowCount, 2].Value = item.GetValue("ZoneNo").ToString();

        //                worksheet.Cells[RowCount, 3].Value = "";
        //                worksheet.Cells[RowCount, 4].Value = "";

        //                worksheet.Cells[RowCount, 5].Value = item.GetValue("VehicleType").ToString();
        //                worksheet.Cells[RowCount, 6].Value = item.GetValue("Deployed").ToString();
        //                worksheet.Cells[RowCount, 7].Value = item.GetValue("NotDeployed").ToString();
        //                worksheet.Cells[RowCount, 8].Value = item.GetValue("DepPercentage").ToString();


        //                worksheet.Cells[RowCount, 1, RowCount, 8].Style.WrapText = true;
        //                worksheet.Cells[RowCount, 1, RowCount, 8].Style.ShrinkToFit = true;
        //                worksheet.Cells[RowCount, 1, RowCount, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                worksheet.Cells[RowCount, 1, RowCount, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

        //                Count++;
        //                RowCount++;
        //            }
        //            #endregion

        //            #region Formatting
        //            using (var range = worksheet.Cells[1, 1, 2, 8])
        //            {
        //                // Setting bold font
        //                range.Style.Font.Bold = true;
        //                // Setting fill type solid
        //                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                // Setting background gray
        //                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));
        //                // Setting font color
        //                range.Style.Font.Color.SetColor(Color.Black);
        //                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                range.Style.Font.Size = 12;
        //                range.Style.WrapText = true;
        //                range.Style.ShrinkToFit = true;
        //                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
        //                var border2 = range.Style.Border;
        //                border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            }

        //            worksheet.PrinterSettings.FitToPage = true;
        //            worksheet.PrinterSettings.ShowGridLines = true;
        //            #endregion

        //            Response = excel.GetAsByteArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions here
        //    }
        //    return Response;
        //}
        public byte[] ExportDeployedVsReportedSummary(string result, string Name, string Sname)
        {
            byte[] Response = null;
            JArray _lst = new JArray();

            if (!string.IsNullOrEmpty(result))
            {
                JArray jresult1 = JArray.Parse(result);
                _lst = jresult1;
            }

            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Vehicle Summary Detail");

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 14].Value = "Vehicle Summary Details"; worksheet.Cells[1, 1, 2, 14].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle Name"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward"; worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Driver Name"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Driver ContactNo"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Vehicle Reg. No"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "SFA Name"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "SFA ContactNo"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Deployed Date and Time"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Reported Date and Time"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "Trip 1 Time"; worksheet.Column(13).Width = 25;
                    worksheet.Cells[3, 14].Value = "Trip 2 Time"; worksheet.Column(14).Width = 25;


                    var cells = worksheet.Cells[3, 1, 3, 14];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                    #endregion

                    #region filling data
                    Count = 1;
                    RowCount = 4;


                    foreach (JObject item in _lst)
                    {


                        worksheet.Cells[RowCount, 1].Value = Count;

                        worksheet.Cells[RowCount, 2].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("DriverName").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("ContactNo").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("InchargeName").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("InchargeContactNo").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("LastDeployedDate").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("ReportedDate").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("Trip1Time").ToString();
                        worksheet.Cells[RowCount, 14].Value = item.GetValue("Trip2Time").ToString();



                        worksheet.Cells[RowCount, 1, RowCount, 14].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 14].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 14])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    Response = excel.GetAsByteArray();
                }

            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        public byte[] ExportDeployedVsReportedVehicleSummary(string result, string Name, string Sname)
        {
            byte[] Response = null;

            dynamic FResult = JObject.Parse(result);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;

            List<DepZoneInfo> zonelst = _lst1.ToObject<List<DepZoneInfo>>();
            List<RepDataInfo> outlst = _lstout.ToObject<List<RepDataInfo>>();



            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Deployed Vs Reported Summary");
                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 12].Value = "Deployed Vs Reported Summary";
                    worksheet.Cells[1, 1, 2, 12].Merge = true;

                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone";
                    worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle Name";
                    worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward";
                    worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type";
                    worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Deployed";
                    worksheet.Column(6).Width = 10;
                    worksheet.Cells[3, 7].Value = "Reported Unique";
                    worksheet.Column(7).Width = 10;
                    worksheet.Cells[3, 8].Value = "1 Trips";
                    worksheet.Column(8).Width = 10;
                    worksheet.Cells[3, 9].Value = "2 Trips";
                    worksheet.Column(9).Width = 10;
                    worksheet.Cells[3, 10].Value = "3 Trips";
                    worksheet.Column(10).Width = 10;
                    worksheet.Cells[3, 11].Value = "4 Trips";
                    worksheet.Column(11).Width = 10;
                    worksheet.Cells[3, 12].Value = "> 4 Trips";
                    worksheet.Column(12).Width = 10;
                    var cells = worksheet.Cells[3, 1, 3, 12];
                    var fill = cells.Style.Fill;
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion


                    Count = 1;
                    RowCount = 4;
                    int CRowCount = 4;

                    // Grouping logic for 3rd and 4th columns

                    for (int i = 0; i < outlst.Count; i++)
                    {
                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = outlst[i].Zonecode;
                        // worksheet.Cells[RowCount, 6].Value = outlst[i].Deployed + outlst[i].NotDeployed;
                        //worksheet.Cells[RowCount, 5].Value = outlst[i].VehicleType;
                        worksheet.Cells[RowCount, 6].Value = outlst[i].Deployed;
                        worksheet.Cells[RowCount, 7].Value = outlst[i].ReportedUnique;
                        worksheet.Cells[RowCount, 8].Value = outlst[i].Trips1;
                        worksheet.Cells[RowCount, 9].Value = outlst[i].Trips2;
                        worksheet.Cells[RowCount, 10].Value = outlst[i].Trips3;
                        worksheet.Cells[RowCount, 11].Value = outlst[i].Trips4;
                        worksheet.Cells[RowCount, 12].Value = outlst[i].MoreThan4Trips;

                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        Count++;
                        RowCount++;
                    }

                    List<string> circlelst = outlst.Select(i => i.CircleCode).Distinct().ToList();
                    int WCount = 4;

                    foreach (string item in circlelst)
                    {
                        int icount = outlst.Count(k => k.CircleCode == item);
                        int IColcount = (CRowCount + icount) - 1;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Value = item;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Merge = true;

                        List<string> wardlst = outlst.Where(i => i.CircleCode == item && !string.IsNullOrEmpty(i.WardNo)).Select(i => i.WardName).Distinct().ToList();

                        foreach (string witem in wardlst)
                        {
                            int wicount = outlst.Count(j => j.WardName == witem);
                            int IWColCount = (WCount + wicount) - 1;

                            worksheet.Cells[WCount, 4, IWColCount, 4].Value = witem;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Merge = true;

                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.WrapText = true;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.ShrinkToFit = true;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            // Now, group by VehicleType within the ward
                            List<string> vehicleTypeList = outlst.Where(i => i.CircleCode == item && i.WardName == witem).Select(i => i.VehicleType).Distinct().ToList();

                            foreach (string vType in vehicleTypeList)
                            {
                                int vTypeCount = outlst.Count(k => k.CircleCode == item && k.WardName == witem && k.VehicleType == vType);
                                int IVTColCount = (WCount + vTypeCount) - 1;

                                worksheet.Cells[WCount, 5, IVTColCount, 5].Value = vType;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Merge = true;

                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.WrapText = true;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.ShrinkToFit = true;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                                WCount = IVTColCount + 1;
                            }

                            //WCount++; // Add space between different ward groups
                        }

                        if (wardlst.Count() == 0)
                        {
                            worksheet.Cells[WCount, 4].Value = "";
                            // worksheet.Cells[CRowCount, 3, IColcount, 3].Style.Font.Bold = true;
                            WCount = WCount + 1;
                        }

                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.WrapText = true;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.ShrinkToFit = true;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        CRowCount = IColcount + 1;
                    }
                    int grandTotalDeployed = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(item => item.Deployed);
                    int grandTotalReportedUnique = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.ReportedUnique);
                    int grandTotalTrips1 = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.Trips1);
                    int grandTotalTrips2 = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.Trips2);
                    int grandTotalTrips3 = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.Trips3);
                    int grandTotalTrips4 = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.Trips4);
                    int grandTotalMoreThan4Trips = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.MoreThan4Trips);


                    // Find the last row in the worksheet
                    int lastRow = worksheet.Dimension.End.Row + 1;

                    // Add Grand Total row
                    worksheet.Cells[lastRow, 2].Value = "Grand Total";
                    worksheet.Cells[lastRow, 6].Value = grandTotalDeployed;
                    worksheet.Cells[lastRow, 7].Value = grandTotalReportedUnique;
                    worksheet.Cells[lastRow, 8].Value = grandTotalTrips1;
                    worksheet.Cells[lastRow, 9].Value = grandTotalTrips2;
                    worksheet.Cells[lastRow, 10].Value = grandTotalTrips3;
                    worksheet.Cells[lastRow, 11].Value = grandTotalTrips4;
                    worksheet.Cells[lastRow, 12].Value = grandTotalMoreThan4Trips;


                    // Style the Grand Total row
                    worksheet.Cells[lastRow, 1, lastRow, 12].Style.Font.Bold = true;
                    worksheet.Cells[lastRow, 1, lastRow, 12].Style.WrapText = true;
                    worksheet.Cells[lastRow, 1, lastRow, 12].Style.ShrinkToFit = true;
                    worksheet.Cells[lastRow, 1, lastRow, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 12])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
            }
            return Response;
        }



        // Export to excel for Deployed Vs Not Reported Paging
        public byte[] ExportDeployedVsNotReported_Paging(string result, string Name, string Sname)
        {
            byte[] Response = null;
            JArray _lst = new JArray();

            if (!string.IsNullOrEmpty(result))
            {
                JArray jresult1 = JArray.Parse(result);
                _lst = jresult1;
            }

            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Deployed Vs Not Reported");

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 11].Value = "Deployed Vs Not Reported"; worksheet.Cells[1, 1, 2, 11].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle Name"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward"; worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Driver Name"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Driver ContactNo"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Vehicle Reg. No"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "SFA Name"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "SFA ContactNo"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Deployed Date and Time"; worksheet.Column(11).Width = 25;



                    var cells = worksheet.Cells[3, 1, 3, 11];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                    #endregion

                    #region filling data
                    Count = 1;
                    RowCount = 4;


                    foreach (JObject item in _lst)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;

                        worksheet.Cells[RowCount, 2].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("DriverName").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("ContactNo").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("InchargeName").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("InchargeContactNo").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("LastDeployedDate").ToString();




                        worksheet.Cells[RowCount, 1, RowCount, 11].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 11].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 11])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    Response = excel.GetAsByteArray();
                }

            }
            catch (Exception ex)
            {
            }
            return Response;
        }
        // Export to excel for Deployed Vs Not Reported Paging
        public byte[] ExportDeployedVsNotReportedSummary(string result, string Name, string Sname)
        {
            byte[] Response = null;

            dynamic FResult = JObject.Parse(result);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;

            List<DepZoneInfo> zonelst = _lst1.ToObject<List<DepZoneInfo>>();
            List<DepOutDataInfo_New> outlst = _lstout.ToObject<List<DepOutDataInfo_New>>();

            //// Calculate totals for each zone
            //zonelst.Select(i =>
            //{
            //    i.TotalAsset = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.TotalAsset);
            //    i.Active = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.Deployed);
            //    i.InActive = outlst.Where(j => j.ZId == i.ZId).Sum(j => j.NotDeployed);
            //    return i;
            //}).ToList();

            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Deployed Vs Not Reported Summary");
                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 7].Value = "Deployed Vs Not Reported Summary";
                    worksheet.Cells[1, 1, 2, 7].Merge = true;

                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone";
                    worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle Name";
                    worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward";
                    worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type";
                    worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Deployed";
                    worksheet.Column(6).Width = 10;
                    worksheet.Cells[3, 7].Value = "Not Reported";
                    worksheet.Column(7).Width = 10;


                    var cells = worksheet.Cells[3, 1, 3, 7];
                    var fill = cells.Style.Fill;
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion


                    Count = 1;
                    RowCount = 4;
                    int CRowCount = 4;

                    // Grouping logic for 3rd and 4th columns

                    for (int i = 0; i < outlst.Count; i++)
                    {
                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = outlst[i].Zonecode;
                        worksheet.Cells[RowCount, 6].Value = outlst[i].Deployed;
                        //worksheet.Cells[RowCount, 5].Value = outlst[i].VehicleType;
                        worksheet.Cells[RowCount, 7].Value = outlst[i].NotReported;


                        worksheet.Cells[RowCount, 1, RowCount, 7].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 7].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        Count++;
                        RowCount++;
                    }

                    List<string> circlelst = outlst.Select(i => i.CircleCode).Distinct().ToList();
                    int WCount = 4;

                    foreach (string item in circlelst)
                    {
                        int icount = outlst.Count(k => k.CircleCode == item);
                        int IColcount = (CRowCount + icount) - 1;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Value = item;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Merge = true;

                        List<string> wardlst = outlst.Where(i => i.CircleCode == item && !string.IsNullOrEmpty(i.WardNo)).Select(i => i.WardName).Distinct().ToList();

                        foreach (string witem in wardlst)
                        {
                            int wicount = outlst.Count(j => j.WardName == witem);
                            int IWColCount = (WCount + wicount) - 1;

                            worksheet.Cells[WCount, 4, IWColCount, 4].Value = witem;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Merge = true;

                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.WrapText = true;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.ShrinkToFit = true;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            // Now, group by VehicleType within the ward
                            List<string> vehicleTypeList = outlst.Where(i => i.CircleCode == item && i.WardName == witem).Select(i => i.VehicleType).Distinct().ToList();

                            foreach (string vType in vehicleTypeList)
                            {
                                int vTypeCount = outlst.Count(k => k.CircleCode == item && k.WardName == witem && k.VehicleType == vType);
                                int IVTColCount = (WCount + vTypeCount) - 1;

                                worksheet.Cells[WCount, 5, IVTColCount, 5].Value = vType;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Merge = true;

                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.WrapText = true;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.ShrinkToFit = true;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                                WCount = IVTColCount + 1;
                            }

                            //WCount++; // Add space between different ward groups
                        }

                        if (wardlst.Count() == 0)
                        {
                            worksheet.Cells[WCount, 4].Value = "";
                            // worksheet.Cells[CRowCount, 3, IColcount, 3].Style.Font.Bold = true;
                            WCount = WCount + 1;
                        }

                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.WrapText = true;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.ShrinkToFit = true;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        CRowCount = IColcount + 1;
                    }
                    int grandTotalDeployed = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(item => item.Deployed);
                    int grandTotalNotReported = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.NotReported);

                    // Find the last row in the worksheet
                    int lastRow = worksheet.Dimension.End.Row + 1;

                    // Add Grand Total row
                    worksheet.Cells[lastRow, 2].Value = "Grand Total";
                    worksheet.Cells[lastRow, 6].Value = grandTotalDeployed;
                    worksheet.Cells[lastRow, 7].Value = grandTotalNotReported;


                    // Style the Grand Total row
                    worksheet.Cells[lastRow, 1, lastRow, 7].Style.Font.Bold = true;
                    worksheet.Cells[lastRow, 1, lastRow, 7].Style.WrapText = true;
                    worksheet.Cells[lastRow, 1, lastRow, 7].Style.ShrinkToFit = true;
                    worksheet.Cells[lastRow, 1, lastRow, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 7])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
            }
            return Response;
        }

        public byte[] ExportAllOptVehicle(string result, string Name)
        {
            byte[] Response = null;
            JArray jresult = JArray.Parse(result);
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(Name);

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 13].Value = Name; worksheet.Cells[1, 1, 2, 13].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Operation Type"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Container Code"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Container Name"; worksheet.Column(4).Width = 15;
                    worksheet.Cells[3, 5].Value = "Container UId"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Vehicle Type"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Vehicle No"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Vehicle UId"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "Transfer Station"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "Transaction Done On"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Scanned By"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Contact No"; worksheet.Column(12).Width = 25;
                    worksheet.Cells[3, 13].Value = "Is Deviated From Transafer Station"; worksheet.Column(13).Width = 25;

                    var cells = worksheet.Cells[3, 1, 3, 13];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion



                    #region filling data

                    foreach (JObject item in jresult)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = item.GetValue("OperationType").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("ContainerCode").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("ContainerName").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("Step1UId").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("Step2UId").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("TStationName").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("CreatedOn").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("CreatedBy").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("ContactNo").ToString();
                        if (item.GetValue("IsDeviated").ToString() == "1")
                            worksheet.Cells[RowCount, 13].Value = "OUTSIDE";
                        else
                            worksheet.Cells[RowCount, 13].Value = "OUTSIDE";
                        //worksheet.Cells[RowCount, 7].Value = item.GetValue("DeviationStatus").ToString();

                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 13])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border1 = range.Style.Border;
                        border1.Top.Style = border1.Left.Style = border1.Bottom.Style = border1.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion

                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Response;
        }

        public byte[] ExportNotDeployedVsReportedSummary(string result, string Name, string Sname)
        {
            byte[] Response = null;

            dynamic FResult = JObject.Parse(result);
            JArray _lst1 = FResult.Table;
            JArray _lstout = FResult.Table1;

            List<DepZoneInfo> zonelst = _lst1.ToObject<List<DepZoneInfo>>();
            List<RepDataInfo> outlst = _lstout.ToObject<List<RepDataInfo>>();



            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Not Deployed Vs Reported Summary");
                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 12].Value = "Not Deployed Vs Reported Summary";
                    worksheet.Cells[1, 1, 2, 12].Merge = true;

                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone";
                    worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle Name";
                    worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward";
                    worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type";
                    worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Not Deployed";
                    worksheet.Column(6).Width = 10;
                    worksheet.Cells[3, 7].Value = "Reported Unique";
                    worksheet.Column(7).Width = 10;
                    worksheet.Cells[3, 8].Value = "1 Trips";
                    worksheet.Column(8).Width = 10;
                    worksheet.Cells[3, 9].Value = "2 Trips";
                    worksheet.Column(9).Width = 10;
                    worksheet.Cells[3, 10].Value = "3 Trips";
                    worksheet.Column(10).Width = 10;
                    worksheet.Cells[3, 11].Value = "4 Trips";
                    worksheet.Column(11).Width = 10;
                    worksheet.Cells[3, 12].Value = "> 4 Trips";
                    worksheet.Column(12).Width = 10;
                    var cells = worksheet.Cells[3, 1, 3, 12];
                    var fill = cells.Style.Fill;
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #endregion


                    Count = 1;
                    RowCount = 4;
                    int CRowCount = 4;

                    // Grouping logic for 3rd and 4th columns

                    for (int i = 0; i < outlst.Count; i++)
                    {
                        worksheet.Cells[RowCount, 1].Value = Count;
                        worksheet.Cells[RowCount, 2].Value = outlst[i].Zonecode;
                        // worksheet.Cells[RowCount, 6].Value = outlst[i].Deployed + outlst[i].NotDeployed;
                        //worksheet.Cells[RowCount, 5].Value = outlst[i].VehicleType;
                        worksheet.Cells[RowCount, 6].Value = outlst[i].NotDeployed;
                        worksheet.Cells[RowCount, 7].Value = outlst[i].ReportedUnique;
                        worksheet.Cells[RowCount, 8].Value = outlst[i].Trips1;
                        worksheet.Cells[RowCount, 9].Value = outlst[i].Trips2;
                        worksheet.Cells[RowCount, 10].Value = outlst[i].Trips3;
                        worksheet.Cells[RowCount, 11].Value = outlst[i].Trips4;
                        worksheet.Cells[RowCount, 12].Value = outlst[i].MoreThan4Trips;

                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        Count++;
                        RowCount++;
                    }

                    List<string> circlelst = outlst.Select(i => i.CircleCode).Distinct().ToList();
                    int WCount = 4;

                    foreach (string item in circlelst)
                    {
                        int icount = outlst.Count(k => k.CircleCode == item);
                        int IColcount = (CRowCount + icount) - 1;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Value = item;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Merge = true;

                        List<string> wardlst = outlst.Where(i => i.CircleCode == item && !string.IsNullOrEmpty(i.WardNo)).Select(i => i.WardName).Distinct().ToList();

                        foreach (string witem in wardlst)
                        {
                            int wicount = outlst.Count(j => j.WardName == witem);
                            int IWColCount = (WCount + wicount) - 1;

                            worksheet.Cells[WCount, 4, IWColCount, 4].Value = witem;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Merge = true;

                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.WrapText = true;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.ShrinkToFit = true;
                            worksheet.Cells[WCount, 4, IWColCount, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            // Now, group by VehicleType within the ward
                            List<string> vehicleTypeList = outlst.Where(i => i.CircleCode == item && i.WardName == witem).Select(i => i.VehicleType).Distinct().ToList();

                            foreach (string vType in vehicleTypeList)
                            {
                                int vTypeCount = outlst.Count(k => k.CircleCode == item && k.WardName == witem && k.VehicleType == vType);
                                int IVTColCount = (WCount + vTypeCount) - 1;

                                worksheet.Cells[WCount, 5, IVTColCount, 5].Value = vType;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Merge = true;

                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.WrapText = true;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.ShrinkToFit = true;
                                worksheet.Cells[WCount, 5, IVTColCount, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                                WCount = IVTColCount + 1;
                            }

                            //WCount++; // Add space between different ward groups
                        }

                        if (wardlst.Count() == 0)
                        {
                            worksheet.Cells[WCount, 4].Value = "";
                            // worksheet.Cells[CRowCount, 3, IColcount, 3].Style.Font.Bold = true;
                            WCount = WCount + 1;
                        }

                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.WrapText = true;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.ShrinkToFit = true;
                        worksheet.Cells[CRowCount, 3, IColcount, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        CRowCount = IColcount + 1;
                    }
                    int grandTotalDeployed = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(item => item.NotDeployed);
                    int grandTotalReportedUnique = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.ReportedUnique);
                    int grandTotalTrips1 = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.Trips1);
                    int grandTotalTrips2 = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.Trips2);
                    int grandTotalTrips3 = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.Trips3);
                    int grandTotalTrips4 = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.Trips4);
                    int grandTotalMoreThan4Trips = outlst.Where(item => !string.IsNullOrEmpty(item.ZoneNo)).Sum(i => i.MoreThan4Trips);


                    // Find the last row in the worksheet
                    int lastRow = worksheet.Dimension.End.Row + 1;

                    // Add Grand Total row
                    worksheet.Cells[lastRow, 2].Value = "Grand Total";
                    worksheet.Cells[lastRow, 6].Value = grandTotalDeployed;
                    worksheet.Cells[lastRow, 7].Value = grandTotalReportedUnique;
                    worksheet.Cells[lastRow, 8].Value = grandTotalTrips1;
                    worksheet.Cells[lastRow, 9].Value = grandTotalTrips2;
                    worksheet.Cells[lastRow, 10].Value = grandTotalTrips3;
                    worksheet.Cells[lastRow, 11].Value = grandTotalTrips4;
                    worksheet.Cells[lastRow, 12].Value = grandTotalMoreThan4Trips;


                    // Style the Grand Total row
                    worksheet.Cells[lastRow, 1, lastRow, 12].Style.Font.Bold = true;
                    worksheet.Cells[lastRow, 1, lastRow, 12].Style.WrapText = true;
                    worksheet.Cells[lastRow, 1, lastRow, 12].Style.ShrinkToFit = true;
                    worksheet.Cells[lastRow, 1, lastRow, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 12])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    Response = excel.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
            }
            return Response;
        }

        public byte[] ExportNotDeployedVsReported_Paging(string result, string Name, string Sname)
        {
            byte[] Response = null;
            JArray _lst = new JArray();

            if (!string.IsNullOrEmpty(result))
            {
                JArray jresult1 = JArray.Parse(result);
                _lst = jresult1;
            }

            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int Count = 1;
                    int RowCount = 4;
                    excel.Workbook.Properties.Author = "Ajeevi Technologies";
                    excel.Workbook.Properties.Title = Name;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Not Deployed Vs  Reported");

                    #region Creating Header
                    worksheet.Cells[1, 1, 1, 11].Value = "Not Deployed Vs Not Reported"; worksheet.Cells[1, 1, 2, 11].Merge = true;


                    worksheet.Cells[3, 1].Value = "Sr No.";
                    worksheet.Cells[3, 2].Value = "Zone"; worksheet.Column(2).Width = 25;
                    worksheet.Cells[3, 3].Value = "Circle Name"; worksheet.Column(3).Width = 25;
                    worksheet.Cells[3, 4].Value = "Ward"; worksheet.Column(4).Width = 25;
                    worksheet.Cells[3, 5].Value = "Vehicle Type"; worksheet.Column(5).Width = 25;
                    worksheet.Cells[3, 6].Value = "Driver Name"; worksheet.Column(6).Width = 25;
                    worksheet.Cells[3, 7].Value = "Driver ContactNo"; worksheet.Column(7).Width = 25;
                    worksheet.Cells[3, 8].Value = "Vehicle Reg. No"; worksheet.Column(8).Width = 25;
                    worksheet.Cells[3, 9].Value = "SFA Name"; worksheet.Column(9).Width = 25;
                    worksheet.Cells[3, 10].Value = "SFA ContactNo"; worksheet.Column(10).Width = 25;
                    worksheet.Cells[3, 11].Value = "Deployment Date & Time"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 12].Value = "Reported Date"; worksheet.Column(11).Width = 25;
                    worksheet.Cells[3, 13].Value = "Trip No"; worksheet.Column(11).Width = 25;



                    var cells = worksheet.Cells[3, 1, 3, 13];
                    var fill = cells.Style.Fill;
                    //cells.AutoFitColumns();
                    cells.Style.Locked = true;
                    cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    cells.Style.Font.Bold = true;
                    cells.Style.WrapText = true;
                    cells.Style.ShrinkToFit = true;

                    fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                    var border = cells.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                    #endregion

                    #region filling data
                    Count = 1;
                    RowCount = 4;


                    foreach (JObject item in _lst)
                    {

                        worksheet.Cells[RowCount, 1].Value = Count;

                        worksheet.Cells[RowCount, 2].Value = item.GetValue("ZoneNo").ToString();
                        worksheet.Cells[RowCount, 3].Value = item.GetValue("CircleName").ToString();
                        worksheet.Cells[RowCount, 4].Value = item.GetValue("WardNo").ToString();
                        worksheet.Cells[RowCount, 5].Value = item.GetValue("VehicleType").ToString();
                        worksheet.Cells[RowCount, 6].Value = item.GetValue("DriverName").ToString();
                        worksheet.Cells[RowCount, 7].Value = item.GetValue("ContactNo").ToString();
                        worksheet.Cells[RowCount, 8].Value = item.GetValue("VehicleNo").ToString();
                        worksheet.Cells[RowCount, 9].Value = item.GetValue("InchargeName").ToString();
                        worksheet.Cells[RowCount, 10].Value = item.GetValue("InchargeContactNo").ToString();
                        worksheet.Cells[RowCount, 11].Value = item.GetValue("LastDeployedDate").ToString();
                        worksheet.Cells[RowCount, 12].Value = item.GetValue("ReportedDate").ToString();
                        worksheet.Cells[RowCount, 13].Value = item.GetValue("Trip1Time").ToString();
                        //worksheet.Cells[RowCount, 11].Value = item.GetValue("LastDeployedDate").ToString();




                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.WrapText = true;
                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.ShrinkToFit = true;
                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[RowCount, 1, RowCount, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        Count++;
                        RowCount++;
                    }
                    #endregion


                    #region formatting

                    using (var range = worksheet.Cells[1, 1, 2, 13])
                    {
                        // Setting bold font
                        range.Style.Font.Bold = true;
                        // Setting fill type solid
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        // Setting background gray
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 190, 151));

                        // Setting font color
                        range.Style.Font.Color.SetColor(Color.Black);

                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                        range.Style.WrapText = true;
                        range.Style.ShrinkToFit = true;
                        range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        var border2 = range.Style.Border;
                        border2.Top.Style = border2.Left.Style = border2.Bottom.Style = border2.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.ShowGridLines = true;

                    #endregion


                    Response = excel.GetAsByteArray();
                }

            }
            catch (Exception ex)
            {
            }
            return Response;
        }
    }
}
