using COMMON.COLLECTION;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace COMMON
{
    public static class CommonHelper
    {
        private static object lockObject { get; set; }

        public static object LockObject
        {
            get
            {
                if (lockObject == null)
                    lockObject = new object();
                return lockObject;
            }
        }
        public static DateTime IndianStandard(DateTime currentDate)
        {
            TimeZoneInfo mountain = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime utc = currentDate;
            return TimeZoneInfo.ConvertTimeFromUtc(utc, mountain);
            //return DateTime.Now;
        }
        public static bool ExtensionType(string Extension)
        {
            bool status;
            if (Extension == ".png" || Extension == ".jpg" || Extension == ".jpeg" || Extension == ".gif" || Extension == ".bmp")
            {
                status = true;
                return status;
            }
            else
            {
                status = false;
                return status;

            }

        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string RemoveSpecialChars(string str)
        {
            string[] chars = new string[] { "$", "#", "*", "?" };
            if (str != null)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    if (str.Contains(chars[i]))
                    {
                        str = str.Replace(chars[i], "");
                    }
                }
            }
            return str;
        }
        public static string generateID()
        {
            long i = 1;

            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }

            string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

            return number;
        }
        public static string Get6DigitOTP()
        {
            Random generator = new Random();
            int r = generator.Next(1, 1000000);
            string result = r.ToString().PadLeft(6, '0');

            return result;
        }
        public static DataTable toDataTable(string json)
        {
            var result = new DataTable();
            var jArray = JArray.Parse(json);
            //Initialize the columns, If you know the row type, replace this   
            foreach (var row in jArray)
            {
                foreach (var jToken in row)
                {
                    var jproperty = jToken as JProperty;
                    if (jproperty == null) continue;
                    if (result.Columns[jproperty.Name] == null)
                        result.Columns.Add(jproperty.Name, typeof(string));
                }
            }
            // result.Columns.Add("OrderId", typeof(int));
            int Count = 1;
            foreach (var row in jArray)
            {
                var datarow = result.NewRow();
                foreach (var jToken in row)
                {
                    var jProperty = jToken as JProperty;
                    if (jProperty == null) continue;
                    datarow[jProperty.Name] = jProperty.Value.ToString();
                }
                // datarow["OrderId"] = Count.ToString();
                result.Rows.Add(datarow);
                Count++;
            }

            return result;
        }
        public static string SmsApiUrl(string ContactNo, string Msg)
        {
            //ContactNo = "7042618366";
            string FMsg = HttpUtility.UrlEncode(Msg);
            string Sms = "http://sms.commcryptnetworksolutions.com/submitsms.jsp?user=Ramky&key=aa2b63a63dXX&mobile=" + ContactNo + "&message=" + FMsg + "&senderid=CHESPL&accusage=1";
            return Sms;
        }
        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        public static DataTable JarraytoDataTable(JArray jArray)
        {
            var result = new DataTable();
            //var jArray = JArray.Parse(json);
            //Initialize the columns, If you know the row type, replace this   
            foreach (var row in jArray)
            {
                foreach (var jToken in row)
                {
                    var jproperty = jToken as JProperty;
                    if (jproperty == null) continue;
                    if (result.Columns[jproperty.Name] == null)
                        result.Columns.Add(jproperty.Name, typeof(string));
                }
            }
            // result.Columns.Add("OrderId", typeof(int));
            int Count = 1;
            foreach (var row in jArray)
            {
                var datarow = result.NewRow();
                foreach (var jToken in row)
                {
                    var jProperty = jToken as JProperty;
                    if (jProperty == null) continue;
                    datarow[jProperty.Name] = jProperty.Value.ToString();
                }
                // datarow["OrderId"] = Count.ToString();
                result.Rows.Add(datarow);
                Count++;
            }

            return result;
        }
        public static DataTable ToDataTable(List<NRouteStopInfo> _lst)

        {



            DataTable dt = new DataTable();


            dt.Columns.Add("StopId", typeof(string));
            dt.Columns.Add("OrderId", typeof(int));
            dt.Columns.Add("Arvltime", typeof(TimeSpan));
            dt.Columns.Add("DeptTime", typeof(TimeSpan));
            dt.Columns.Add("PickupTime", typeof(TimeSpan));
            dt.Columns.Add("TripId", typeof(int));
            dt.Columns.Add("VehicleUId", typeof(string));
            dt.Columns.Add("BufferMin", typeof(int));
            dt.Columns.Add("PointCatId", typeof(int));

            int Count = 1;
            foreach (var item in _lst)
            {

                DataRow dtrow = dt.NewRow();
                TimeSpan arvl = new TimeSpan(0, 0, item.BufferMin, 0);

                dtrow["StopId"] = item.StopId;
                if (item.PointCatId == (int)Enums.PointCategory.START_POINT || item.PointCatId == (int)Enums.PointCategory.END_POINT)
                    dtrow["OrderId"] = 0;
                else
                {
                    dtrow["OrderId"] = Count;
                    Count++;
                }

                dtrow["Arvltime"] = item.PickupTime.Subtract(arvl);
                dtrow["DeptTime"] = item.PickupTime.Add(arvl); ;
                dtrow["PickupTime"] = item.PickupTime;
                dtrow["TripId"] = item.TripId;
                dtrow["VehicleUId"] = item.VehicleUId;
                dtrow["BufferMin"] = item.BufferMin;
                dtrow["PointCatId"] = item.PointCatId;

                dt.Rows.Add(dtrow);


            }

            return dt;

        }

      

        public static void WriteToJsonFile(string FName, string text, string path)
        {
            lock (LockObject)
            {
                string FileName = FName + DateTime.Now.ToString("dd-MM-yyyy") + ".json";
                path = path + FileName;
                //string path = HttpContext.Current.Server.MapPath("~/Files/" + FileName);
                // string path = HostingEnvironment.MapPath("/Files/" + FileName);
                if (!File.Exists(path))
                    File.Create(path).Dispose();
                using (TextWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(text);
                    writer.Close();
                }
            }
        }
        public static Bitmap AdjustContrast(Bitmap Image, float Value)
        {
            Value = (100.0f + Value) / 100.0f;
            Value *= Value;
            Bitmap NewBitmap = (Bitmap)Image.Clone();
            BitmapData data = NewBitmap.LockBits(
                new Rectangle(0, 0, NewBitmap.Width, NewBitmap.Height),
                ImageLockMode.ReadWrite,
                NewBitmap.PixelFormat);
            int Height = NewBitmap.Height;
            int Width = NewBitmap.Width;

            unsafe
            {
                for (int y = 0; y < Height; ++y)
                {
                    byte* row = (byte*)data.Scan0 + (y * data.Stride);
                    int columnOffset = 0;
                    for (int x = 0; x < Width; ++x)
                    {
                        byte B = row[columnOffset];
                        byte G = row[columnOffset + 1];
                        byte R = row[columnOffset + 2];

                        float Red = R / 255.0f;
                        float Green = G / 255.0f;
                        float Blue = B / 255.0f;
                        Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
                        Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
                        Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;

                        int iR = (int)Red;
                        iR = iR > 255 ? 255 : iR;
                        iR = iR < 0 ? 0 : iR;
                        int iG = (int)Green;
                        iG = iG > 255 ? 255 : iG;
                        iG = iG < 0 ? 0 : iG;
                        int iB = (int)Blue;
                        iB = iB > 255 ? 255 : iB;
                        iB = iB < 0 ? 0 : iB;

                        row[columnOffset] = (byte)iB;
                        row[columnOffset + 1] = (byte)iG;
                        row[columnOffset + 2] = (byte)iR;

                        columnOffset += 4;
                    }
                }
            }

            NewBitmap.UnlockBits(data);

            return NewBitmap;
        }
        public static string GenerateRandomNumber(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            string s;
            for (int i = 0; i < size; i++)
            {
                s = Convert.ToString(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(s);
            }

            return builder.ToString();
        }



        public static string ValidatePassword(string password)

        {
            var input = password;
            string ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Password should not be empty";
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one lower case letter.";
                return ErrorMessage;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one upper case letter.";
                return ErrorMessage;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be lesser than 8 or greater than 15 characters.";
                return ErrorMessage;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one numeric value.";
                return ErrorMessage;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one special case character.";
                return ErrorMessage;
            }
            else
            {
                return "Success";
            }
        }
        #region Constants
        public const string CCOde = "ORG/00001";
        public const string Application_Type = "WEB APP";
        public const string Web_Api_Local_Path = @"D:\Source Code\Hyderabad\WebApp_Api\HYDSWM\HYDSWMAPI\wwwroot\";//@"C:\\Projects\\CHENNAI\\CHENNAISWM\\CHENNAISWMAPI\\wwwroot\";

        #endregion
    }



}
