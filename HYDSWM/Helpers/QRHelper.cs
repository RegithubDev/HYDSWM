using COMMON.SWMENTITY;
using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace HYDSWM.Helpers
{
    public static class QRHelper
    {
        public static byte[] RenderQrCode(string QrCode, string FPath)
        {

            Bitmap bmp = null;
            string level = "L";
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                //Bitmap icon = new Bitmap(Server.MapPath("~/cssANDjs/Images/agenv-icon.png"));
                // Bitmap iconLogo = new Bitmap(Server.MapPath("~/cssANDjs/Images/noidaAuthority.png"));
                Bitmap iconLogo = new Bitmap(FPath);
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(QrCode, eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        bmp = qrCode.GetGraphic(20, Color.Black, Color.White, null, 8, 3, true);
                        Graphics graphicImage = Graphics.FromImage(bmp);
                        graphicImage.SmoothingMode = SmoothingMode.AntiAlias;
                        // graphicImage.DrawString("CODE: " + info.QrCode, new Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, new Point(160, 500));
                        graphicImage.DrawImage(iconLogo, new Point(230, -5));

                    }
                }
            }
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bmp, typeof(byte[]));
        }
        public static Bitmap MergeImages(List<Image> images)
        {
            int outputImageWidth = 0;
            int outputImageHeight = 1;
            foreach (Image image in images)
            {
                outputImageHeight += image.Height;
                if (image.Width > outputImageWidth)
                {
                    outputImageWidth = image.Width;
                }
            }
            //outputImageWidth = 1000;
            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.Clear(Color.White);
                graphics.DrawImage(images[0], new Rectangle(new Point(), images[0].Size), new Rectangle(new Point(), images[0].Size), GraphicsUnit.Pixel);
                graphics.DrawImage(images[1], new Rectangle(new Point(0, images[0].Height + 1), images[1].Size), new Rectangle(new Point(), images[1].Size), GraphicsUnit.Pixel);
                graphics.DrawImage(images[2], new Rectangle(new Point(0, images[0].Height + images[1].Height + 1), images[2].Size), new Rectangle(new Point(), images[2].Size), GraphicsUnit.Pixel);
                //float p = images[0].Height + images[1].Height + 10;
                //graphics.DrawString("CODE: abcd", new Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, new Point( 0,1030));
            }

            return outputImage;
        }
        public static byte[] RenderQrCodeTest(string QrCode, string FPath)
        {
            Bitmap bmp1 = null;
            Bitmap bmp = null;
            string level = "L";
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                //Bitmap icon = new Bitmap(Server.MapPath("~/cssANDjs/Images/agenv-icon.png"));
                // Bitmap iconLogo = new Bitmap(Server.MapPath("~/cssANDjs/Images/noidaAuthority.png"));
                Bitmap iconLogo = new Bitmap(FPath);
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(QrCode, eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        bmp = qrCode.GetGraphic(20, Color.Black, Color.White, null, 8, 30, true);
                        //bmp1 = DrawFilledRectangle(300,600);
                        bmp1 = new Bitmap(bmp, new Size(250, 250));
                        Graphics graphicImage = Graphics.FromImage(bmp1);
                        graphicImage.SmoothingMode = SmoothingMode.AntiAlias;
                        //  graphicImage.DrawString("CODE: " + QrCode, new Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, new Point(0, 2));
                        graphicImage.DrawImage(new Bitmap(iconLogo, new Size(80, 80)), new Point(80, -50));
                    }
                }
            }
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bmp1, typeof(byte[]));
        }
        public static byte[] VehicleQrCodeWithJobject(JObject Item, string FPath)
        {
            string QrCode = Item.GetValue("UId").ToString();
            string OwnerType = Item.GetValue("OwnerType").ToString();
            string VehicleNo = Item.GetValue("VehicleNo").ToString();
            string VehicleType = Item.GetValue("VehicleType").ToString();
            string Ward = Item.GetValue("Ward").ToString();
            string Circle = Item.GetValue("Circle").ToString();
            string Zone = Item.GetValue("Zone").ToString();


            Bitmap iconLogo = new Bitmap(FPath);
            Bitmap bmp = null;
            string level = "L";
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {

                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(QrCode, eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        bmp = qrCode.GetGraphic(20, Color.Black, Color.White, null, 8, 3, true);
                        Graphics graphicImage = Graphics.FromImage(bmp);
                        graphicImage.SmoothingMode = SmoothingMode.HighQuality;

                        //graphicImage.DrawString("CODE: " + QrCode, new Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, new Point(0, 2));
                    }
                }
            }
            Bitmap txtbmp = new Bitmap(800, 800);
            Graphics graphictxt = Graphics.FromImage(txtbmp);
            /*
            graphictxt.DrawString("Vehicle Reg No ", new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(10, 0));
            graphictxt.DrawString(": " + VehicleNo, new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(320, 0));
            graphictxt.DrawString("Vehicle Type   ", new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(10, 40));
            graphictxt.DrawString(": " + VehicleType, new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(320, 40));
            graphictxt.DrawString("Owner          ", new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(10, 80));
            graphictxt.DrawString(": " + OwnerType, new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(320, 80));
            graphictxt.DrawString("Zone           ", new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(10, 120));
            graphictxt.DrawString(": " + Zone, new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(320, 120));
            graphictxt.DrawString("Circle         ", new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(10, 160));
            graphictxt.DrawString(": " + Circle, new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(320, 160));
            graphictxt.DrawString("Ward           ", new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(10, 200));
            graphictxt.DrawString(": " + Ward, new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(320, 200));

            */


            Bitmap txtbmp1 = new Bitmap(500, 480);



            Image img1 = (Image)txtbmp1;
            Image img2 = (Image)bmp;
            Image img3 = (Image)txtbmp;

            List<Image> fileList = new List<Image>();
            fileList.Add(img1);
            fileList.Add(img2);
            fileList.Add(img3);
            Bitmap bmp1 = MergeImages(fileList);
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bmp1, typeof(byte[]));
        }
        public static byte[] ContainerQrCodeWithJobject(JObject Item, string FPath)
        {
            string QrCode = Item.GetValue("UId").ToString();
            string ContainerCode = Item.GetValue("ContainerCode").ToString();
            string ContainerName = Item.GetValue("ContainerName").ToString();
            string ContainerType = Item.GetValue("ContainerType").ToString();


            Bitmap iconLogo = new Bitmap(FPath);
            Bitmap bmp = null;
            string level = "L";
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {

                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(QrCode, eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        bmp = qrCode.GetGraphic(20, Color.Black, Color.White, null, 8, 3, true);
                        Graphics graphicImage = Graphics.FromImage(bmp);
                        graphicImage.SmoothingMode = SmoothingMode.HighQuality;

                        //graphicImage.DrawString("CODE: " + QrCode, new Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, new Point(0, 2));
                    }
                }
            }


            /*
            Bitmap txtbmp = new Bitmap(800, 800);
            Graphics graphictxt = Graphics.FromImage(txtbmp);
            graphictxt.DrawString("Container Code  ", new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(10, 0));
            graphictxt.DrawString(": " + ContainerCode, new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(320, 0));
            graphictxt.DrawString("Container Type  ", new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(10, 40));
            graphictxt.DrawString(": " + ContainerType, new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(320, 40));
            //graphictxt.DrawString("Container Type              ", new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(10, 160));
            //graphictxt.DrawString(": " + ContainerType, new Font("Arial", 30, FontStyle.Bold), SystemBrushes.WindowText, new Point(420, 160));


            */

            Bitmap txtbmp1 = new Bitmap(500, 480);



            Image img1 = (Image)txtbmp1;
            Image img2 = (Image)bmp;
            //Image img3 = (Image)txtbmp;

            List<Image> fileList = new List<Image>();
            fileList.Add(img1);
            fileList.Add(img2);
            //fileList.Add(img3);
            Bitmap bmp1 = MergeImages(fileList);
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bmp1, typeof(byte[]));
        }
        private static Bitmap DrawFilledRectangle(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(Brushes.Red, ImageSize);
            }
            return bmp;
        }
        public static Bitmap VehicleQrCodeTest(string UId)
        {
            string QrCode = UId;

            Bitmap bmp = null;
            string level = "L";
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {

                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(QrCode, eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        bmp = qrCode.GetGraphic(20, Color.Black, Color.White, null, 8, 3, true);
                        Graphics graphicImage = Graphics.FromImage(bmp);
                        graphicImage.SmoothingMode = SmoothingMode.HighQuality;

                        //graphicImage.DrawString("UID: " + QrCode, new Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, new Point(0, 2));
                    }
                }
            }
            return bmp;
        }

    }
}
