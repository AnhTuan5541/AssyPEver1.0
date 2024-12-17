using WaferMapViewer.Common;
using WaferMapViewer.Data;
using WaferMapViewer.Models;
using WaferMapViewer.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;



namespace WaferMapViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConnectionStrings _connection;
        public HomeController(ILogger<HomeController> logger, IOptions<ConnectionStrings> connection)
        {
            _logger = logger;
            _connection = connection.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WaferMapViewer()
        {
            return View();
        }
        public IActionResult AddUser()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetProfile()
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("GetProfile", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idCard", userid);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get wafer map success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get wafer map success",
                    Data = data,
                    size = data.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetWaferMap(int id)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("GetWaferMap", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", id);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get wafer map success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get wafer map success",
                    Data = data,
                    size = data.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetAllFrame(int id)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("GetAllFrame", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", id);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get all frame success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get all frame success",
                    Data = data,
                    size = data.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetTotalFrame(int id)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("GetTotalFrame", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", id);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get total frame success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get total frame success",
                    Data = data,
                    size = data.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult GetUnitByFrameId(int id, string frame_id)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("GetUnitByFrameId", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", id);
                command.Parameters.AddWithValue("@frameId", frame_id == null ? "" : frame_id);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get unit by frame id success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get unit by frame id success",
                    Data = data,
                    size = data.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetWaferMapValue(int idWaferMap, string frameId)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("GetWaferMapValue", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", idWaferMap);
                command.Parameters.AddWithValue("@frameId", frameId == null ? "" : frameId);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get wafer map value successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get wafer map value successfully",
                    Data = data,
                    size = data.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateProfile(string profileName, int lotRow, int frameIdRow, int rowStart, int x, int y, int columnNumberX, int columnNumberY)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("CreateProfile", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idCard", userid);
                command.Parameters.AddWithValue("@profileName", profileName);
                command.Parameters.AddWithValue("@lotRow", lotRow);
                command.Parameters.AddWithValue("@frameIdRow", frameIdRow);
                command.Parameters.AddWithValue("@rowStart", rowStart);
                command.Parameters.AddWithValue("@x", x);
                command.Parameters.AddWithValue("@y", y);
                command.Parameters.AddWithValue("@columnNumberX", columnNumberX);
                command.Parameters.AddWithValue("@columnNumberY", columnNumberY);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                if (data[0]["status"].ToString() == "FAIL")
                {
                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Profile already exists", CommonFunction.FAIL, functionName);
                    var response = new CommonResponse<Dictionary<string, object>>
                    {
                        StatusCode = CommonFunction.FAIL,
                        Message = "Profile already exists",
                        Data = data,
                        size = data.Count
                    };
                    return Ok(response);
                }
                else
                {
                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Create new profile success", CommonFunction.SUCCESS, functionName);
                    var response = new CommonResponse<Dictionary<string, object>>
                    {
                        StatusCode = CommonFunction.SUCCESS,
                        Message = "Create new profile success",
                        Data = data,
                        size = data.Count
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteProfile(int id)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("DeleteWaferMap", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", id);
                command.Parameters.AddWithValue("@idCard", userid);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                if (data[0]["status"].ToString() == "FAIL")
                {
                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Profile not exists or you dont have permission", CommonFunction.FAIL, functionName);
                    var response = new CommonResponse<Dictionary<string, object>>
                    {
                        StatusCode = CommonFunction.FAIL,
                        Message = "Profile not exists or you dont have permission",
                        Data = data,
                        size = data.Count
                    };
                    return Ok(response);
                }
                else
                {
                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Delete profile successfully", CommonFunction.SUCCESS, functionName);
                    var response = new CommonResponse<Dictionary<string, object>>
                    {
                        StatusCode = CommonFunction.SUCCESS,
                        Message = "Delete profile successfully",
                        Data = data,
                        size = data.Count
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetXYUnit(int idWaferMap, string frameId, string stringUnit)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("GetXYUnit", connection) { CommandType = CommandType.StoredProcedure };


                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", idWaferMap);
                command.Parameters.AddWithValue("@frameId", frameId == null ? "" : frameId);
                command.Parameters.AddWithValue("@stringUnit", stringUnit == null ? "" : stringUnit);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get unit by frame id success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get unit by frame id success",
                    Data = data,
                    size = data.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult CountPositionUnit([FromBody] Dictionary<string, JsonElement> dataUnit)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("CountPositionUnit", connection) { CommandType = CommandType.StoredProcedure };

                int idWaferMap = dataUnit["idWaferMap"].GetInt32();
                var frameId = dataUnit["frameId"].ValueKind == JsonValueKind.Null ? "" : dataUnit["frameId"].GetString();
                List<string> listUnit = System.Text.Json.JsonSerializer.Deserialize<List<string>>(dataUnit["listUnit"].GetRawText());
                string stringUnit = "";
                if (listUnit.Count > 0) {
                    stringUnit = string.Join(";", listUnit);
                }

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", idWaferMap);
                command.Parameters.AddWithValue("@frameId", frameId);
                command.Parameters.AddWithValue("@stringUnit", stringUnit == null ? "" : stringUnit);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Count unit by frame id success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Count unit by frame id success",
                    Data = data,
                    size = data.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetWaferMapRaw(int idWaferMap)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("GetWaferMapRaw", connection) { CommandType = CommandType.StoredProcedure };


                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", idWaferMap);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get wafer map raw success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get wafer map raw success",
                    Data = data,
                    size = data.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddWaferMapRaw([FromBody] List<Dictionary<string, int>> listMapRaw)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                connection.Open();
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "wafer_map_raw";

                    // Map các cột từ listMapRaw vào bảng đích
                    bulkCopy.ColumnMappings.Add("id_wafer_map", "id_wafer_map");
                    bulkCopy.ColumnMappings.Add("x_raw", "x_raw");
                    bulkCopy.ColumnMappings.Add("y_raw", "y_raw");

                    // Tạo DataTable từ listMapRaw
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("id_wafer_map", typeof(int));
                    dataTable.Columns.Add("x_raw", typeof(int));
                    dataTable.Columns.Add("y_raw", typeof(int));

                    foreach (var item in listMapRaw)
                    {
                        var row = dataTable.NewRow();
                        row["id_wafer_map"] = item["id_wafer_map"];
                        row["x_raw"] = item["x_raw"];
                        row["y_raw"] = item["y_raw"];
                        dataTable.Rows.Add(row);
                    }

                    // Sử dụng SqlBulkCopy để lưu dữ liệu vào cơ sở dữ liệu
                    bulkCopy.WriteToServer(dataTable);
                }


                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Add wafer map raw success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Add wafer map raw success",
                    Data = null,
                    size = listMapRaw.Count
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFile(IEnumerable<IFormFile> files, int idWaferMap, int lotRow, int rowStart, int frameIdRow, int columnNumberX, int columnNumberY)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                //Trước khi upload thì xóa hết dữ liệu cũ
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("DeleteWaferMapValue", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idWaferMap", idWaferMap);

                connection.Open();
                var reader = command.ExecuteReader();
                connection.Close();

                List<Dictionary<string, object>> listData = new List<Dictionary<string, object>>();
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var readerFile = new StreamReader(file.OpenReadStream()))
                        {
                            string line;
                            int i =0;
                            string lotValue = "";
                            while ((line = await readerFile.ReadLineAsync()) != null)
                            {
                                // Đọc từng file
                                i++;
                                if (i == lotRow)
                                {
                                    string[] listLotString = line.Split(":");
                                    lotValue = listLotString[listLotString.Length - 1];
                                }
                                if (i < rowStart) continue;
                                string[] listString = line.Split("\t");
                                Dictionary<string, object> dt = new Dictionary<string, object>();
                                if (listString.Length <= 1) continue;
                                dt.Add("unit_id", listString[0]);
                                dt.Add("frame_id", listString[frameIdRow - 1]);
                                dt.Add("lot_value", lotValue);
                                dt.Add("value_x", listString[columnNumberX - 1]);
                                dt.Add("value_y", listString[columnNumberY - 1]);
                                dt.Add("id_wafer_map", idWaferMap);

                                listData.Add(dt);
                            }

                        }
                    }
                }

                connection.Open();
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "wafer_map_value";

                    // Map các cột từ listData vào bảng đích
                    bulkCopy.ColumnMappings.Add("unit_id", "unit_id");
                    bulkCopy.ColumnMappings.Add("frame_id", "frame_id");
                    bulkCopy.ColumnMappings.Add("lot_value", "lot_value");
                    bulkCopy.ColumnMappings.Add("value_x", "value_x");
                    bulkCopy.ColumnMappings.Add("value_y", "value_y");
                    bulkCopy.ColumnMappings.Add("id_wafer_map", "id_wafer_map");

                    // Tạo DataTable từ listData
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("unit_id", typeof(string));
                    dataTable.Columns.Add("frame_id", typeof(string));
                    dataTable.Columns.Add("lot_value", typeof(string));
                    dataTable.Columns.Add("value_x", typeof(int));
                    dataTable.Columns.Add("value_y", typeof(int));
                    dataTable.Columns.Add("id_wafer_map", typeof(int));

                    foreach (var item in listData)
                    {
                        var row = dataTable.NewRow();
                        row["unit_id"] = item["unit_id"];
                        row["frame_id"] = item["frame_id"];
                        row["lot_value"] = item["lot_value"];
                        row["value_x"] = item["value_x"];
                        row["value_y"] = item["value_y"];
                        row["id_wafer_map"] = item["id_wafer_map"];
                        dataTable.Rows.Add(row);
                    }

                    // Sử dụng SqlBulkCopy để lưu dữ liệu vào cơ sở dữ liệu
                    bulkCopy.WriteToServer(dataTable);
                }

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Upload file successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Upload file successfully",
                    Data = listData,
                    size = listData.Count
                };
                connection.Close();
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi, trả về phản hồi lỗi)
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, ex.Message, CommonFunction.ERROR, functionName);
                var errorResponse = new CommonResponse<User>
                {
                    StatusCode = CommonFunction.ERROR,
                    Message = ex.Message,
                    Data = null,
                    size = 0
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}
