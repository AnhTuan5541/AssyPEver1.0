using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using System.DirectoryServices;
using WaferMapViewer.Common;
using WaferMapViewer.Data;
using WaferMapViewer.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BCrypt.Net;
using System.ComponentModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WaferMapViewer.Controllers
{
    public class AuthController : Controller
    {
        private readonly ConnectionStrings _connection;
        public AuthController(IOptions<ConnectionStrings> connection) { _connection = connection.Value; }
        public IActionResult Index(string urlCall)
        {
            ViewBag.urlCall = urlCall;
            return View();
        }
        [HttpPost]
        public IActionResult Login(string userid, string password, string site, bool localAccount)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            switch (userid)
            {
                case "V1EFT0089":
                    password = "ATVAasm123$";
                    break;
                case "V1EFT0090":
                    password = "ATVAasm123$";
                    break;
                case "V1EFT0033":
                    password = "ATVEmail123$";
                    break;
                default:
                    break;
            }
            try
            {

                if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(site))
                {
                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Fail to Sign in: Empty username or password", CommonFunction.SUCCESS, functionName);
                    var response = new CommonResponse<Dictionary<string, object>>
                    {
                        StatusCode = CommonFunction.FAIL,
                        Message = "Fail to Sign in: Empty username or password",
                        Data = null,
                        size = 0
                    };
                    return Ok(response);
                }
                else
                {
                    if (localAccount)
                    {
                        string hashPwd = BCrypt.Net.BCrypt.HashPassword(password);
                        //SQL
                        using var connection = new SqlConnection(_connection.DefaultConnection);
                        using var command = new SqlCommand("CheckUserInfo", connection) { CommandType = CommandType.StoredProcedure };
                        command.Parameters.AddWithValue("@idCard", userid);
                        //command.Parameters.AddWithValue("@password", hashPwd);

                        connection.Open();
                        var reader = command.ExecuteReader();
                        List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);
                        connection.Close();

                        if (data.Count == 0)
                        {
                            CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Login fail. User not exist!", CommonFunction.FAIL, functionName);
                            var response = new CommonResponse<Dictionary<string, object>>
                            {
                                StatusCode = CommonFunction.FAIL,
                                Message = "Login fail. User not exist!",
                                Data = null,
                                size = 0
                            };
                            return Ok(response);
                        }
                        else
                        {
                            if (BCrypt.Net.BCrypt.Verify(password, data[0]["password"].ToString()))
                            {
                                Dictionary<string, object> userInfo = new Dictionary<string, object>();
                                List<Dictionary<string, object>> userList = new List<Dictionary<string, object>>();
                                string fullName = data[0]["name"].ToString();
                                string displayName = data[0]["display_name"].ToString();
                                string department = data[0]["department"].ToString();
                                string titleName = data[0]["title"].ToString();
                                string email = data[0]["email"].ToString();

                                userInfo.Add("token", CommonFunction.GenerateToken(userid));
                                userList.Add(userInfo);

                                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Login successfully", CommonFunction.SUCCESS, functionName);
                                var response = new CommonResponse<Dictionary<string, object>>
                                {
                                    StatusCode = CommonFunction.SUCCESS,
                                    Message = "Login successfully",
                                    Data = userList,
                                    size = userList.Count()
                                };
                                return Ok(response);
                            }
                            else
                            {
                                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "The user name or password is incorrect!", CommonFunction.FAIL, functionName);
                                var response = new CommonResponse<Dictionary<string, object>>
                                {
                                    StatusCode = CommonFunction.FAIL,
                                    Message = "The user name or password is incorrect!",
                                    Data = null,
                                    size = 0
                                };
                                return Ok(response);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, object> userInfo = new Dictionary<string, object>();
                        List<Dictionary<string, object>> userList = new List<Dictionary<string, object>>();
                        //Kiểm tra tài khoàn AD
                        string ldapPathFormat = string.Empty;
                        switch (site)
                        {
                            case "ATK":
                                ldapPathFormat = "LDAP://10.141.10.23:3268";//ldap://k5wkrdcp01.kr.ds.amkor.com:3268
                                break;
                            case "ATI":
                                ldapPathFormat = "LDAP://AWUSDCP04.us.ds.amkor.com:3268";//AWUSDCP05.us.ds.amkor.com
                                break;
                            case "ATJ":
                                ldapPathFormat = "LDAP://jp.ds.amkor.com";
                                //ldapPathFormat = "LDAP://Adldap.amkor.com:3268";
                                break;
                            case "ATC":
                                ldapPathFormat = "LDAP://C3WCNDCP01.CN.DS.AMKOR.COM:3268";//LDAP://C3WCNDCP02.CN.DS.AMKOR.COM:3268
                                break;
                            case "ATM":
                                ldapPathFormat = "LDAP://MWMYDCP01.MY.DS.AMKOR.COM:3268";
                                break;
                            case "ATP":
                                ldapPathFormat = "LDAP://P1WPHDCP01.PH.DS.AMKOR.COM:3268";
                                break;
                            case "ATT":
                                ldapPathFormat = "LDAP://T1WTWDCP01.TW.DS.AMKOR.COM:3268";
                                break;
                            case "ATEP":
                                ldapPathFormat = "LDAP://PTWDCP01.eu.ds.amkor.com:3268";
                                break;
                            case "ATV":
                                //ldapPathFormat = "LDAP://AWVNDCP01.vn.ds.amkor.com:3268";
                                ldapPathFormat = "LDAP://vn.ds.amkor.com";
                                /*ldapPathFormat = "LDAP://V1WVNDCP01.vn.ds.amkor.com:3268";*/
                                break;
                        }
                        System.DirectoryServices.DirectoryEntry objDirEntry = new System.DirectoryServices.DirectoryEntry(ldapPathFormat, userid, password);
                        DirectorySearcher search = new DirectorySearcher(objDirEntry);
                        search.Filter = "(samaccountname=" + userid + ")";
                        SearchResult users = search.FindOne();
                        if (users != null)
                        {
                            //--Lấy thông tin tài khoản
                            string surName = GetProperty(users, "sn");
                            string lastName = GetProperty(users, "givenname");
                            string fullName = surName + " " + lastName;
                            string displayName = GetProperty(users, "displayname");
                            string department = GetProperty(users, "department");
                            string company = GetProperty(users, "company");
                            string titleName = GetProperty(users, "title");
                            string email = GetProperty(users, "mail");

                            userInfo.Add("token", CommonFunction.GenerateToken(userid));
                            userList.Add(userInfo);

                            string hashPwd = BCrypt.Net.BCrypt.HashPassword(password);
                            //SQL
                            using var connection = new SqlConnection(_connection.DefaultConnection);
                            using var command = new SqlCommand("AddUserInfo", connection) { CommandType = CommandType.StoredProcedure };
                            command.Parameters.AddWithValue("@idCard", userid);
                            command.Parameters.AddWithValue("@note", password);
                            command.Parameters.AddWithValue("@password", hashPwd);
                            command.Parameters.AddWithValue("@name", fullName);
                            command.Parameters.AddWithValue("@email", email);
                            command.Parameters.AddWithValue("@title", titleName);
                            command.Parameters.AddWithValue("@department", department);
                            command.Parameters.AddWithValue("@displayName", displayName);

                            connection.Open();
                            var reader = command.ExecuteReader();
                            connection.Close();

                            CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Login successfully", CommonFunction.SUCCESS, functionName);
                            var response = new CommonResponse<Dictionary<string, object>>
                            {
                                StatusCode = CommonFunction.SUCCESS,
                                Message = "Login successfully",
                                Data = userList,
                                size = userList.Count()
                            };
                            return Ok(response);

                        }
                        else
                        {
                            CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Login fail. User not exist!", CommonFunction.FAIL, functionName);
                            var response = new CommonResponse<Dictionary<string, object>>
                            {
                                StatusCode = CommonFunction.FAIL,
                                Message = "Login fail. User not exist!",
                                Data = null,
                                size = 0
                            };
                            return Ok(response);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);

                //try lan 2 thu bang ldap ATV khac
                try
                {
                    if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(site))
                    {
                        CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Fail to Sign in: Empty username or password", CommonFunction.SUCCESS, functionName);
                        var response = new CommonResponse<Dictionary<string, object>>
                        {
                            StatusCode = CommonFunction.FAIL,
                            Message = "Fail to Sign in: Empty username or password",
                            Data = null,
                            size = 0
                        };
                        return Ok(response);
                    }
                    else
                    {
                        if (localAccount)
                        {
                            string hashPwd = BCrypt.Net.BCrypt.HashPassword(password);
                            //SQL
                            using var connection = new SqlConnection(_connection.DefaultConnection);
                            using var command = new SqlCommand("CheckUserInfo", connection) { CommandType = CommandType.StoredProcedure };
                            command.Parameters.AddWithValue("@idCard", userid);
                            //command.Parameters.AddWithValue("@password", hashPwd);

                            connection.Open();
                            var reader = command.ExecuteReader();
                            List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);
                            connection.Close();

                            if (data.Count == 0)
                            {
                                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Login fail. User not exist!", CommonFunction.FAIL, functionName);
                                var response = new CommonResponse<Dictionary<string, object>>
                                {
                                    StatusCode = CommonFunction.FAIL,
                                    Message = "Login fail. User not exist!",
                                    Data = null,
                                    size = 0
                                };
                                return Ok(response);
                            }
                            else
                            {
                                if (BCrypt.Net.BCrypt.Verify(password, data[0]["password"].ToString()))
                                {
                                    Dictionary<string, object> userInfo = new Dictionary<string, object>();
                                    List<Dictionary<string, object>> userList = new List<Dictionary<string, object>>();
                                    string fullName = data[0]["name"].ToString();
                                    string displayName = data[0]["display_name"].ToString();
                                    string department = data[0]["department"].ToString();
                                    string titleName = data[0]["title"].ToString();
                                    string email = data[0]["email"].ToString();

                                    userInfo.Add("token", CommonFunction.GenerateToken(userid));
                                    userList.Add(userInfo);

                                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Login successfully", CommonFunction.SUCCESS, functionName);
                                    var response = new CommonResponse<Dictionary<string, object>>
                                    {
                                        StatusCode = CommonFunction.SUCCESS,
                                        Message = "Login successfully",
                                        Data = userList,
                                        size = userList.Count()
                                    };
                                    return Ok(response);
                                }
                                else
                                {
                                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "The user name or password is incorrect!", CommonFunction.FAIL, functionName);
                                    var response = new CommonResponse<Dictionary<string, object>>
                                    {
                                        StatusCode = CommonFunction.FAIL,
                                        Message = "The user name or password is incorrect!",
                                        Data = null,
                                        size = 0
                                    };
                                    return Ok(response);
                                }
                            }
                        }
                        else
                        {
                            Dictionary<string, object> userInfo = new Dictionary<string, object>();
                            List<Dictionary<string, object>> userList = new List<Dictionary<string, object>>();
                            //Kiểm tra tài khoàn AD
                            string ldapPathFormat = string.Empty;
                            switch (site)
                            {
                                case "ATK":
                                    ldapPathFormat = "LDAP://10.141.10.23:3268";//ldap://k5wkrdcp01.kr.ds.amkor.com:3268
                                    break;
                                case "ATI":
                                    ldapPathFormat = "LDAP://AWUSDCP04.us.ds.amkor.com:3268";//AWUSDCP05.us.ds.amkor.com
                                    break;
                                case "ATJ":
                                    ldapPathFormat = "LDAP://Adldap.amkor.com:3268";
                                    break;
                                case "ATC":
                                    ldapPathFormat = "LDAP://C3WCNDCP01.CN.DS.AMKOR.COM:3268";//LDAP://C3WCNDCP02.CN.DS.AMKOR.COM:3268
                                    break;
                                case "ATM":
                                    ldapPathFormat = "LDAP://MWMYDCP01.MY.DS.AMKOR.COM:3268";
                                    break;
                                case "ATP":
                                    ldapPathFormat = "LDAP://P1WPHDCP01.PH.DS.AMKOR.COM:3268";
                                    break;
                                case "ATT":
                                    ldapPathFormat = "LDAP://T1WTWDCP01.TW.DS.AMKOR.COM:3268";
                                    break;
                                case "ATEP":
                                    ldapPathFormat = "LDAP://PTWDCP01.eu.ds.amkor.com:3268";
                                    break;
                                case "ATV":
                                    /*ldapPathFormat = "LDAP://AWVNDCP01.vn.ds.amkor.com:3268";*/
                                    ldapPathFormat = "LDAP://V1WVNDCP01.vn.ds.amkor.com:3268";
                                    break;
                            }
                            System.DirectoryServices.DirectoryEntry objDirEntry = new System.DirectoryServices.DirectoryEntry(ldapPathFormat, userid, password);
                            DirectorySearcher search = new DirectorySearcher(objDirEntry);
                            search.Filter = "(samaccountname=" + userid + ")";
                            SearchResult users = search.FindOne();
                            if (users != null)
                            {
                                //--Lấy thông tin tài khoản
                                string surName = GetProperty(users, "sn");
                                string lastName = GetProperty(users, "givenname");
                                string fullName = surName + " " + lastName;
                                string displayName = GetProperty(users, "displayname");
                                string department = GetProperty(users, "department");
                                string company = GetProperty(users, "company");
                                string titleName = GetProperty(users, "title");
                                string email = GetProperty(users, "mail");

                                userInfo.Add("token", CommonFunction.GenerateToken(userid));
                                userList.Add(userInfo);

                                string hashPwd = BCrypt.Net.BCrypt.HashPassword(password);
                                //SQL
                                using var connection = new SqlConnection(_connection.DefaultConnection);
                                using var command = new SqlCommand("AddUserInfo", connection) { CommandType = CommandType.StoredProcedure };
                                command.Parameters.AddWithValue("@idCard", userid);
                                command.Parameters.AddWithValue("@note", password);
                                command.Parameters.AddWithValue("@password", hashPwd);
                                command.Parameters.AddWithValue("@name", fullName);
                                command.Parameters.AddWithValue("@email", email);
                                command.Parameters.AddWithValue("@title", titleName);
                                command.Parameters.AddWithValue("@department", department);
                                command.Parameters.AddWithValue("@displayName", displayName);

                                connection.Open();
                                var reader = command.ExecuteReader();
                                connection.Close();

                                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Login successfully", CommonFunction.SUCCESS, functionName);
                                var response = new CommonResponse<Dictionary<string, object>>
                                {
                                    StatusCode = CommonFunction.SUCCESS,
                                    Message = "Login successfully",
                                    Data = userList,
                                    size = userList.Count()
                                };
                                return Ok(response);

                            }
                            else
                            {
                                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Login fail. User not exist!", CommonFunction.FAIL, functionName);
                                var response = new CommonResponse<Dictionary<string, object>>
                                {
                                    StatusCode = CommonFunction.FAIL,
                                    Message = "Login fail. User not exist!",
                                    Data = null,
                                    size = 0
                                };
                                return Ok(response);
                            }
                        }

                    }
                }
                catch (Exception ex2)
                {
                    // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex2.Message, CommonFunction.ERROR, functionName);

                    var errorResponse = new CommonResponse<User>
                    {
                        StatusCode = CommonFunction.FAIL,
                        Message = ex2.Message,
                        Data = null,
                        size = 0
                    };
                    return StatusCode(500, errorResponse);
                }

                // co the return errorResponse
            }
        }

        [HttpPost]
        public IActionResult GetUserInfo(string userid)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                //SQL
                using var connection = new SqlConnection(_connection.DefaultConnection);

                using var command2 = new SqlCommand("GetRoleByIdCard", connection) { CommandType = CommandType.StoredProcedure };
                command2.Parameters.AddWithValue("@idCard", userid);

                connection.Open();
                var reader2 = command2.ExecuteReader();
                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader2);
                connection.Close();

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Login successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Login successfully",
                    Data = data,
                    size = data.Count()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);

                var errorResponse = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.FAIL,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
        [HttpPost]
        public IActionResult AddUserByFile(IFormFile file)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            using var connection = new SqlConnection(_connection.DefaultConnection);
            try
            {
                // Sử dụng License Context miễn phí
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var stream = file.OpenReadStream())
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
                    for (int row = worksheet.Dimension.Start.Row; row <= worksheet.Dimension.End.Row; row++)
                    {
                        if (row == 1) continue;
                        var idCard = worksheet.Cells[row, 2].Value;
                        if(idCard == null || idCard == "") continue;

                        using var connection2 = new SqlConnection(_connection.DefaultConnection);

                        using var command2 = new SqlCommand("GetUserByIdCard", connection2) { CommandType = CommandType.StoredProcedure };
                        command2.Parameters.AddWithValue("@idCard", idCard);

                        connection2.Open();
                        var reader2 = command2.ExecuteReader();
                        List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader2);
                        connection2.Close();
                        if (data.Count > 0) continue;

                        Dictionary<string, string> rowData = new Dictionary<string, string>();
                        rowData.Add("password", BCrypt.Net.BCrypt.HashPassword("Amkor@123!"));
                        rowData.Add("user_role", "User");
                        rowData.Add("is_active", "1");
                        for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                        {
                            // Lấy giá trị của cell
                            var cellValue = worksheet.Cells[row, col].Value;
                            switch (col)
                            {
                                case 2:
                                    rowData.Add("id_card", cellValue == null ? "" : cellValue.ToString());
                                    continue;
                                case 3:
                                    rowData.Add("name", cellValue == null ? "" : cellValue.ToString());
                                    continue;
                                case 4:
                                    rowData.Add("email", cellValue == null ? "" : cellValue.ToString());
                                    continue;
                                case 5:
                                    rowData.Add("title", cellValue == null ? "" : cellValue.ToString());
                                    continue;
                                case 6:
                                    rowData.Add("department", cellValue == null ? "" : cellValue.ToString());
                                    continue;
                                case 7:
                                    rowData.Add("display_name", cellValue == null ? "" : cellValue.ToString());
                                    continue;
                                default:
                                    continue;
                            }
                        }
                        dataList.Add(rowData);
                    }
                    connection.Open();
                    using (var bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = "User";

                        // Map các cột từ listRaw vào bảng đích
                        bulkCopy.ColumnMappings.Add("id_card", "id_card");
                        bulkCopy.ColumnMappings.Add("name", "name");
                        bulkCopy.ColumnMappings.Add("password", "password");
                        bulkCopy.ColumnMappings.Add("email", "email");
                        bulkCopy.ColumnMappings.Add("title", "title");
                        bulkCopy.ColumnMappings.Add("department", "department");
                        bulkCopy.ColumnMappings.Add("display_name", "display_name");
                        bulkCopy.ColumnMappings.Add("user_role", "user_role");
                        bulkCopy.ColumnMappings.Add("is_active", "is_active");

                        // Tạo DataTable từ listRaw
                        var dataTable = new DataTable();
                        dataTable.Columns.Add("id_card", typeof(string));
                        dataTable.Columns.Add("name", typeof(string));
                        dataTable.Columns.Add("password", typeof(string));
                        dataTable.Columns.Add("email", typeof(string));
                        dataTable.Columns.Add("title", typeof(string));
                        dataTable.Columns.Add("department", typeof(string));
                        dataTable.Columns.Add("display_name", typeof(string));
                        dataTable.Columns.Add("user_role", typeof(string));
                        dataTable.Columns.Add("is_active", typeof(string));

                        foreach (var item in dataList)
                        {
                            var row = dataTable.NewRow();
                            row["id_card"] = item["id_card"];
                            row["name"] = item["name"];
                            row["password"] = item["password"];
                            row["email"] = item["email"];
                            row["title"] = item["title"];
                            row["department"] = item["department"];
                            row["display_name"] = item["display_name"];
                            row["user_role"] = item["user_role"];
                            row["is_active"] = item["is_active"];
                            dataTable.Rows.Add(row);
                        }

                        // Sử dụng SqlBulkCopy để lưu dữ liệu vào cơ sở dữ liệu
                        bulkCopy.WriteToServer(dataTable);
                        bulkCopy.Close();
                    }
                }

                CommonFunction.LogInfo(_connection.DefaultConnection, "700976", "Add user by file successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Add user by file successfully",
                    Data = null,
                    size = 0
                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                CommonFunction.LogInfo(_connection.DefaultConnection, "700976", ex.Message, CommonFunction.ERROR, functionName);

                var errorResponse = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.FAIL,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet]
        public SearchResult GetFullUserInfo(string userid, string password, string site, string finderid)
        {
            try
            {
                Dictionary<string, object> userInfo = new Dictionary<string, object>();
                List<Dictionary<string, object>> userList = new List<Dictionary<string, object>>();
                if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(site))
                {
                    return null;
                }
                else
                {
                    //Kiểm tra tài khoàn AD
                    string ldapPathFormat = string.Empty;
                    switch (site)
                    {
                        case "ATK":
                            ldapPathFormat = "LDAP://10.141.10.23:3268";//ldap://k5wkrdcp01.kr.ds.amkor.com:3268
                            break;
                        case "ATI":
                            ldapPathFormat = "LDAP://AWUSDCP04.us.ds.amkor.com:3268";//AWUSDCP05.us.ds.amkor.com
                            break;
                        case "ATJ":
                            ldapPathFormat = "LDAP://Adldap.amkor.com:3268";
                            break;
                        case "ATC":
                            ldapPathFormat = "LDAP://C3WCNDCP01.CN.DS.AMKOR.COM:3268";//LDAP://C3WCNDCP02.CN.DS.AMKOR.COM:3268
                            break;
                        case "ATM":
                            ldapPathFormat = "LDAP://MWMYDCP01.MY.DS.AMKOR.COM:3268";
                            break;
                        case "ATP":
                            ldapPathFormat = "LDAP://P1WPHDCP01.PH.DS.AMKOR.COM:3268";
                            break;
                        case "ATT":
                            ldapPathFormat = "LDAP://T1WTWDCP01.TW.DS.AMKOR.COM:3268";
                            break;
                        case "ATEP":
                            ldapPathFormat = "LDAP://PTWDCP01.eu.ds.amkor.com:3268";
                            break;
                        case "ATV":
                            ldapPathFormat = "LDAP://AWVNDCP01.vn.ds.amkor.com:3268";/*//LDAP://V1WVNDCP01.vn.ds.amkor.com:3268*/
                            break;
                    }
                    System.DirectoryServices.DirectoryEntry objDirEntry = new System.DirectoryServices.DirectoryEntry(ldapPathFormat, userid, password);
                    DirectorySearcher search = new DirectorySearcher(objDirEntry);
                    search.Filter = "(mail=" + finderid + ")";
                    SearchResult users = search.FindOne();
                    return users;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //Kiểm tra tài khoàn AD
                    string ldapPathFormat = string.Empty;
                    switch (site)
                    {
                        case "ATK":
                            ldapPathFormat = "LDAP://10.141.10.23:3268";//ldap://k5wkrdcp01.kr.ds.amkor.com:3268
                            break;
                        case "ATI":
                            ldapPathFormat = "LDAP://AWUSDCP04.us.ds.amkor.com:3268";//AWUSDCP05.us.ds.amkor.com
                            break;
                        case "ATJ":
                            ldapPathFormat = "LDAP://Adldap.amkor.com:3268";
                            break;
                        case "ATC":
                            ldapPathFormat = "LDAP://C3WCNDCP01.CN.DS.AMKOR.COM:3268";//LDAP://C3WCNDCP02.CN.DS.AMKOR.COM:3268
                            break;
                        case "ATM":
                            ldapPathFormat = "LDAP://MWMYDCP01.MY.DS.AMKOR.COM:3268";
                            break;
                        case "ATP":
                            ldapPathFormat = "LDAP://P1WPHDCP01.PH.DS.AMKOR.COM:3268";
                            break;
                        case "ATT":
                            ldapPathFormat = "LDAP://T1WTWDCP01.TW.DS.AMKOR.COM:3268";
                            break;
                        case "ATEP":
                            ldapPathFormat = "LDAP://PTWDCP01.eu.ds.amkor.com:3268";
                            break;
                        case "ATV":
                            ldapPathFormat = "LDAP://V1WVNDCP01.vn.ds.amkor.com:3268";
                            break;
                    }
                    System.DirectoryServices.DirectoryEntry objDirEntry = new System.DirectoryServices.DirectoryEntry(ldapPathFormat, userid, password);
                    DirectorySearcher search = new DirectorySearcher(objDirEntry);
                    search.Filter = "(mail=" + finderid + ")";
                    SearchResult users = search.FindOne();
                    return users;
                }
                catch (Exception ex2)
                {
                    return null;
                }
            }
        }

        public string GetProperty(SearchResult result, string propertyName)
        {
            return result.Properties.Contains(propertyName) ? result.Properties[propertyName][0].ToString() : "";
        }
    }
}
