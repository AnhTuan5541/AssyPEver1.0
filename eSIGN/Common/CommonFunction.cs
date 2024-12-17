using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using WaferMapViewer.Response;
using System.IO;
using System.Globalization;

namespace WaferMapViewer.Common
{
    public class CommonFunction
    {
        public static readonly string SUCCESS = "SUCCESS";
        public static readonly string FAIL = "FAIL";
        public static readonly string ERROR = "ERROR";
        private readonly IHostEnvironment _environment;
        public CommonFunction(IHostEnvironment environment)
        {
            _environment = environment;
        }
        public static List<Dictionary<string, object>> GetDataFromProcedure(SqlDataReader reader)
        {
            List<Dictionary<string, object>> dictionary = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    var columnValue = reader.GetValue(i);
                    row.Add(columnName, columnValue);
                }
                dictionary.Add(row);
            }
            return dictionary;
        }

        public static string GenerateToken(string empID)
        {
            //Create token

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Amkor_Technology_Vietnam_is_the_best_company_in_Viet_Nam"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "eSign",
                audience: "https://localhost:44309/",
                claims: new[] { new Claim(ClaimTypes.Name, empID) },
                expires: DateTime.UtcNow.AddHours(168),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            //End create token
        }
        public static DateTime StartDate(string timeSpan)
        {
            string startDate = timeSpan.Substring(0, 10);
            DateTime date = DateTime.ParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            return date;
        }

        public static DateTime EndDate(string timeSpan)
        {
            string startDate = timeSpan.Substring(timeSpan.Length - 10);
            DateTime date = DateTime.ParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            return date;
        }
        public static void LogInfo(string DefaultConnection, string idCard, string info, string typeLog, string function)
        {
            using var connection = new SqlConnection(DefaultConnection);
            using var command = new SqlCommand("AddLogInfo", connection) { CommandType = CommandType.StoredProcedure };

            // Thêm các tham số cho stored procedure (nếu cần)
            command.Parameters.AddWithValue("@idCard", idCard);
            command.Parameters.AddWithValue("@info", info);
            command.Parameters.AddWithValue("@typeLog", typeLog);
            command.Parameters.AddWithValue("@function", function);

            connection.Open();
            var reader = command.ExecuteReader();
        }

        public async static Task<CommonResponse<Dictionary<string, object>>> UploadFile(IFormFile file)
        {
            var response = new CommonResponse<Dictionary<string, object>>
            {
                StatusCode = CommonFunction.SUCCESS,
                Message = "Upload file succsessfully.",
                Data = null,
                size = 0
            };

            // Kiểm tra loại tệp
            var allowedExtensions = new[] { ".docx", ".xls", ".xlsx", ".pdf", ".pptx" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.FAIL,
                    Message = "File type is not supported. Please upload file .docx, .xls, .xlsx, .pdf, .pptx",
                    Data = null,
                    size = 0
                };
                return response;
            }

            // Kiểm tra kích thước tệp (giới hạn 5MB)
            var maxFileSize = 5 * 1024 * 1024; // 5MB
            if (file.Length > maxFileSize)
            {
                response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.FAIL,
                    Message = "File size exceeds allowed limit (5MB limit).",
                    Data = null,
                    size = 0
                };
                return response;
            }

            // Xử lý tệp ở đây (lưu vào thư mục, lưu vào CSDL, ...)
            // Ví dụ: lưu vào thư mục upload
            // Thu muc luu file tren server
            //var uploadDirectory = Path.Combine("D:\\UploadFile", "eSign");
            // Thu muc luu file tren local
            //string uploadDirectory = Path.Combine("C:\\UploadFile", "eSign");
            string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            string uuid = Guid.NewGuid().ToString();
            //string uniqueFileName = DateTime.Now.ToString("yyyyMMdd") + "_" + uuid + Path.GetExtension(file.FileName);
            string uniqueFileName = file.FileName;
            string filePath = Path.Combine(uploadDirectory, uniqueFileName);

            //Upload in folder
            /*using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("fileUrl", "/upload/" + uniqueFileName);
            List<Dictionary<string, object>> fileUrl = new List<Dictionary<string, object>>();
            fileUrl.Add(result);

            response = new CommonResponse<Dictionary<string, object>>
            {
                StatusCode = CommonFunction.SUCCESS,
                Message = "Upload file succsessfully.",
                Data = fileUrl,
                size = 0
            };
            return response;*/

            //Upload in db
            string base64String;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var fileBytes = stream.ToArray();
                base64String = Convert.ToBase64String(fileBytes);
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("fileUrl", base64String);
            result.Add("fileName", uniqueFileName);
            List<Dictionary<string, object>> fileUrl = new List<Dictionary<string, object>>();
            fileUrl.Add(result);

            response = new CommonResponse<Dictionary<string, object>>
            {
                StatusCode = CommonFunction.SUCCESS,
                Message = "Upload file succsessfully.",
                Data = fileUrl,
                size = 0
            };
            return response;
        }
        //Đọc file dùng cho wafer map
        public async static Task<CommonResponse<Dictionary<string, object>>> MultipleFile(IFormFile file)
        {
            var response = new CommonResponse<Dictionary<string, object>>
            {
                StatusCode = CommonFunction.SUCCESS,
                Message = "Upload file succsessfully.",
                Data = null,
                size = 0
            };

            // Kiểm tra loại tệp
            var allowedExtensions = new[] { ".txt", ".ESV" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.FAIL,
                    Message = "File type is not supported. Please upload file .txt, .ESV",
                    Data = null,
                    size = 0
                };
                return response;
            }

            // Kiểm tra kích thước tệp (giới hạn 5MB)
            //var maxFileSize = 5 * 1024 * 1024; // 5MB
            //if (file.Length > maxFileSize)
            //{
            //    response = new CommonResponse<Dictionary<string, object>>
            //    {
            //        StatusCode = CommonFunction.FAIL,
            //        Message = "File size exceeds allowed limit (5MB limit).",
            //        Data = null,
            //        size = 0
            //    };
            //    return response;
            //}

            

            //Upload in db
            string base64String;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var fileBytes = stream.ToArray();
                base64String = Convert.ToBase64String(fileBytes);
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("fileUrl", base64String);
            List<Dictionary<string, object>> fileUrl = new List<Dictionary<string, object>>();
            fileUrl.Add(result);

            response = new CommonResponse<Dictionary<string, object>>
            {
                StatusCode = CommonFunction.SUCCESS,
                Message = "Upload file succsessfully.",
                Data = fileUrl,
                size = 0
            };
            return response;
        }
    }
}
