using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text.Json;
using WaferMapViewer.Common;
using WaferMapViewer.Data;
using WaferMapViewer.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WaferMapViewer.Controllers
{
    public class Mold1Controller : Controller
    {
        private readonly ConnectionStrings _connection;
        public Mold1Controller(IOptions<ConnectionStrings> connection)
        {
            _connection = connection.Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetGiaTriDo(int idCustomer)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("Mold1_GetGiaTriDo", connection) { CommandType = CommandType.StoredProcedure };

                // Thêm các tham số cho stored procedure (nếu cần)
                command.Parameters.AddWithValue("@idCustomer", idCustomer);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get gia tri do successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get gia tri do successfully",
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
        public IActionResult GetShift()
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("Mold1_GetShift", connection) { CommandType = CommandType.StoredProcedure };


                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get shift successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get shift successfully",
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
        public IActionResult GetDiemDo(int idCustomer)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("Mold1_GetDiemDo", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.AddWithValue("@idCustomer", idCustomer);

                connection.Open();
                var reader = command.ExecuteReader();

                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get diem do successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get diem do successfully",
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
        public IActionResult SaveGiaTriDo([FromBody] Dictionary<string, JsonElement> dataGiaTriDo)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);

            using var connection2 = new SqlConnection(_connection.DefaultConnection);
            using var command2 = new SqlCommand("Mold1_DeleteDiemDoValueSub", connection2) { CommandType = CommandType.StoredProcedure };

            //command2.Parameters.AddWithValue("@idCustomer", idCustomer);

            connection2.Open();
            var reader2 = command2.ExecuteReader();
            connection2.Close();

            int idShiftType = dataGiaTriDo["id_shift_type"].GetInt32();
            string ngayDo = dataGiaTriDo["ngay_do"].ToString();
            string userId = dataGiaTriDo["user_id"].ToString();

            using var connection7 = new SqlConnection(_connection.DefaultConnection);
            using var command7 = new SqlCommand("Mold1_GetUserActive", connection7) { CommandType = CommandType.StoredProcedure };

            command7.Parameters.AddWithValue("@idCard", userId);

            connection7.Open();
            var reader7 = command7.ExecuteReader();
            List<Dictionary<string, object>> userActive = CommonFunction.GetDataFromProcedure(reader7);
            connection7.Close();
            if(userActive.Count == 0)
            {
                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Operator ID don't have permission. Contact IT to active!", CommonFunction.FAIL, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.FAIL,
                    Message = "Operator ID don't have permission. Contact IT to active!",
                    Data = null,
                    size = 0
                };
                return Ok(response);
            }

            string lotNo = dataGiaTriDo["lot_no"].ToString();
            var listGiaTriDo = dataGiaTriDo["list_gia_tri_do"].EnumerateArray();
            try
            {
                foreach (var item in listGiaTriDo)
                {
                    using var connection = new SqlConnection(_connection.DefaultConnection);
                    using var command = new SqlCommand("Mold1_AddDiemDoValue", connection) { CommandType = CommandType.StoredProcedure };
                    connection.Open();
                    command.Parameters.AddWithValue("@id_shift_type", idShiftType);
                    command.Parameters.AddWithValue("@ngay_do", ngayDo);
                    command.Parameters.AddWithValue("@id_card", userId);
                    command.Parameters.AddWithValue("@lot_no", lotNo);
                    command.Parameters.AddWithValue("@id_diem_do", item.GetProperty("id_diem_do").GetInt32());
                    command.Parameters.AddWithValue("@id_gia_tri_do", item.GetProperty("id_gia_tri_do").GetInt32());
                    command.Parameters.AddWithValue("@diem_do_value", item.GetProperty("value").GetString());

                    var reader = command.ExecuteReader();
                    connection.Close();

                }

                using var connection4 = new SqlConnection(_connection.DefaultConnection);
                using var command4 = new SqlCommand("Mold1_GetDiemDoValueSub", connection4) { CommandType = CommandType.StoredProcedure };

                command4.Parameters.AddWithValue("@giaTriDo1", 3);
                command4.Parameters.AddWithValue("@giaTriDo2", 4);

                connection4.Open();
                var reader4 = command4.ExecuteReader();
                List<Dictionary<string, object>> dataPhai = CommonFunction.GetDataFromProcedure(reader4);
                connection4.Close();

                using var connection5 = new SqlConnection(_connection.DefaultConnection);
                using var command5 = new SqlCommand("Mold1_GetDiemDoValueSub", connection5) { CommandType = CommandType.StoredProcedure };
                command5.Parameters.AddWithValue("@giaTriDo1", 1);
                command5.Parameters.AddWithValue("@giaTriDo2", 2);
                connection5.Open();
                var reader5 = command5.ExecuteReader();
                List<Dictionary<string, object>> dataTrai = CommonFunction.GetDataFromProcedure(reader5);
                connection5.Close();

                List<Dictionary<string, object>> resultTrai = new List<Dictionary<string, object>>();
                string conclusion = "PASS";
                string ngayDoAll = ""; int shiftAll = 0;
                var groupedTrai = dataTrai.GroupBy(d => d["diem_do"]);
                foreach (var trai in groupedTrai)
                {
                    double total = 0;
                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                    int id_customer = 0, id_shift = 0, id_diem_do = 0;
                    string id_card = "", lot_no = "", ngay_do = "", hang_muc = "Giá trị đo mold dịch chuyển tấm trái", do_lech_string = "Độ lệch so với tiêu chuẩn tấm trái";
                    foreach (var item in trai)
                    {
                        total += Convert.ToDouble(item["diem_do_value"]);
                        id_customer = Convert.ToInt32(item["id_customer"]);
                        id_shift = Convert.ToInt32(item["id_shift_type"]);
                        shiftAll = Convert.ToInt32(item["id_shift_type"]);
                        id_diem_do = Convert.ToInt32(item["id_diem_do"]);
                        id_card = item["id_card"].ToString();
                        lot_no = item["lot_no"].ToString();
                        ngay_do = item["ngay_do"].ToString();
                        ngayDoAll = item["ngay_do"].ToString();
                    }
                    double avg = (double)total / 2;
                    double do_lech = Math.Round(Math.Abs(avg - 1.18), 3);
                    if (do_lech > 0.1) conclusion = "FAIL";
                    using var connection3 = new SqlConnection(_connection.DefaultConnection);
                    using var command3 = new SqlCommand("Mold1_AddDuLieuDoVaKetQua", connection3) { CommandType = CommandType.StoredProcedure };
                    command3.Parameters.AddWithValue("@id_customer", id_customer);
                    command3.Parameters.AddWithValue("@id_shift", id_shift);
                    command3.Parameters.AddWithValue("@id_card", id_card);
                    command3.Parameters.AddWithValue("@lot_no", lot_no);
                    command3.Parameters.AddWithValue("@hang_muc", hang_muc);
                    command3.Parameters.AddWithValue("@do_lech", do_lech_string);
                    command3.Parameters.AddWithValue("@ngay_do", ngay_do);
                    command3.Parameters.AddWithValue("@id_diem_do", id_diem_do);
                    command3.Parameters.AddWithValue("@value_du_lieu_do", avg);
                    command3.Parameters.AddWithValue("@value_ket_qua_do", do_lech);
                    connection3.Open();
                    var reader3 = command3.ExecuteReader();
                    connection3.Close();
                }

                List<Dictionary<string, object>> resultPhai = new List<Dictionary<string, object>>();
                var groupedPhai = dataPhai.GroupBy(d => d["diem_do"]);
                foreach (var phai in groupedPhai)
                {
                    double total = 0;
                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                    int id_customer = 0, id_shift = 0, id_diem_do = 0;
                    string id_card = "", lot_no = "", ngay_do = "", hang_muc = "Giá trị đo mold dịch chuyển tấm phải", do_lech_string = "Độ lệch so với tiêu chuẩn tấm phải";
                    foreach (var item in phai)
                    {
                        total += Convert.ToDouble(item["diem_do_value"]);
                        id_customer = Convert.ToInt32(item["id_customer"]);
                        id_shift = Convert.ToInt32(item["id_shift_type"]);
                        id_diem_do = Convert.ToInt32(item["id_diem_do"]);
                        id_card = item["id_card"].ToString();
                        lot_no = item["lot_no"].ToString();
                        ngay_do = item["ngay_do"].ToString();
                    }
                    double avg = (double)total / 2;
                    double do_lech = Math.Round(Math.Abs(avg - 1.18), 3);
                    if (do_lech > 0.1) conclusion = "FAIL";
                    using var connection3 = new SqlConnection(_connection.DefaultConnection);
                    using var command3 = new SqlCommand("Mold1_AddDuLieuDoVaKetQua", connection3) { CommandType = CommandType.StoredProcedure };
                    command3.Parameters.AddWithValue("@id_customer", id_customer);
                    command3.Parameters.AddWithValue("@id_shift", id_shift);
                    command3.Parameters.AddWithValue("@id_card", id_card);
                    command3.Parameters.AddWithValue("@lot_no", lot_no);
                    command3.Parameters.AddWithValue("@hang_muc", hang_muc);
                    command3.Parameters.AddWithValue("@do_lech", do_lech_string);
                    command3.Parameters.AddWithValue("@ngay_do", ngay_do);
                    command3.Parameters.AddWithValue("@id_diem_do", id_diem_do);
                    command3.Parameters.AddWithValue("@value_du_lieu_do", avg);
                    command3.Parameters.AddWithValue("@value_ket_qua_do", do_lech);
                    connection3.Open();
                    var reader3 = command3.ExecuteReader();
                    connection3.Close();
                }

                using var connection6 = new SqlConnection(_connection.DefaultConnection);
                using var command6 = new SqlCommand("Mold1_UpdateConclusionDuLieuDoVaKetQua", connection6) { CommandType = CommandType.StoredProcedure };
                command6.Parameters.AddWithValue("@ngayDoAll", ngayDoAll);
                command6.Parameters.AddWithValue("@shiftAll", shiftAll);
                command6.Parameters.AddWithValue("@conclusion", conclusion);

                connection6.Open();
                var reader6 = command6.ExecuteReader();
                connection6.Close();


                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Add gia tri do success", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Add gia tri do success",
                    Data = null,
                    size = 0
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
        public IActionResult GetDiemDoValueSub()
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                //Phải là 3, 4
                using var connection = new SqlConnection(_connection.DefaultConnection);
                connection.Open();

                using var command = new SqlCommand("Mold1_GetDiemDoValueSub", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@giaTriDo1", 3);
                command.Parameters.AddWithValue("@giaTriDo2", 4);

                List<Dictionary<string, object>> dataPhai = new List<Dictionary<string, object>>();
                using (var reader = command.ExecuteReader())
                {
                    dataPhai = CommonFunction.GetDataFromProcedure(reader);
                }

                //Trái là 1, 2
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@giaTriDo1", 1);
                command.Parameters.AddWithValue("@giaTriDo2", 2);
                List<Dictionary<string, object>> dataTrai = new List<Dictionary<string, object>>();
                using (var reader2 = command.ExecuteReader())
                {
                    dataTrai = CommonFunction.GetDataFromProcedure(reader2);
                }


                string customer = "", shift = "", ngayDo = "", operatorId = "", lotNo = "", hangMucTrai = "Giá trị đo mold dịch chuyển tấm trái", doLechTrai = "Độ lệch so với tiêu chuẩn tấm trái";
                string conclusion = "PASS";
                int idShift = 0;
                List<Dictionary<string, object>> resultTrai = new List<Dictionary<string, object>>();
                var groupedTrai = dataTrai.GroupBy(d => d["diem_do"]);
                foreach (var trai in groupedTrai)
                {
                    double total = 0;
                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                    foreach (var item in trai)
                    {
                        total += Convert.ToDouble(item["diem_do_value"]);
                        customer = item["customer"].ToString();
                        ngayDo = item["ngay_do"].ToString();
                        shift = item["shift_type"].ToString();
                        idShift = Convert.ToInt32(item["id_shift_type"]);
                        operatorId = item["id_card"].ToString();
                        lotNo = item["lot_no"].ToString();
                    }
                    double avg = (double)total / 2;
                    double do_lech = Math.Round(Math.Abs(avg - 1.18), 3);
                    if (do_lech > 0.1) conclusion = "FAIL";
                    keyValuePairs.Add("diem_do", trai.Key);
                    keyValuePairs.Add("avg", avg);
                    keyValuePairs.Add("do_lech", do_lech);
                    resultTrai.Add(keyValuePairs);

                }
                Dictionary<string, object> duLieuTrai = new Dictionary<string, object>();
                duLieuTrai.Add("customer", customer);
                duLieuTrai.Add("shift", shift);
                duLieuTrai.Add("idShift", idShift);
                duLieuTrai.Add("ngayDo", ngayDo);
                duLieuTrai.Add("operatorId", operatorId);
                duLieuTrai.Add("lotNo", lotNo);
                duLieuTrai.Add("hangMuc", hangMucTrai);
                duLieuTrai.Add("doLech", doLechTrai);
                duLieuTrai.Add("duLieuDo", resultTrai);
                

                string hangMucPhai = "Giá trị đo mold dịch chuyển tấm phải", doLechPhai = "Độ lệch so với tiêu chuẩn tấm phải";
                List<Dictionary<string, object>> resultPhai = new List<Dictionary<string, object>>();
                var groupedPhai = dataPhai.GroupBy(d => d["diem_do"]);
                foreach (var phai in groupedPhai)
                {
                    double total = 0;
                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                    foreach (var item in phai)
                    {
                        total += Convert.ToDouble(item["diem_do_value"]);
                    }
                    double avg = (double)total / 2;
                    double do_lech = Math.Round(Math.Abs(avg - 1.18), 3);
                    if (do_lech > 0.1) conclusion = "FAIL";
                    keyValuePairs.Add("diem_do", phai.Key);
                    keyValuePairs.Add("avg", avg);
                    keyValuePairs.Add("do_lech", do_lech);
                    resultPhai.Add(keyValuePairs);
                }
                Dictionary<string, object> duLieuPhai = new Dictionary<string, object>();
                duLieuPhai.Add("customer", customer);
                duLieuPhai.Add("shift", shift);
                duLieuPhai.Add("idShift", idShift);
                duLieuPhai.Add("ngayDo", ngayDo);
                duLieuPhai.Add("operatorId", operatorId);
                duLieuPhai.Add("lotNo", lotNo);
                duLieuPhai.Add("hangMuc", hangMucPhai);
                duLieuPhai.Add("doLech", doLechPhai);
                duLieuPhai.Add("duLieuDo", resultPhai);

                duLieuTrai.Add("conclusion", conclusion);
                duLieuPhai.Add("conclusion", conclusion);

                List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
                result.Add(duLieuTrai);
                result.Add(duLieuPhai);

                connection.Close();

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get diem do successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get diem do successfully",
                    Data = result,
                    size = result.Count
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
        public IActionResult GetLichSuDo()
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("Mold1_GetDuLieuDo", connection) { CommandType = CommandType.StoredProcedure };

                connection.Open();
                var reader = command.ExecuteReader();
                List<Dictionary<string, object>> listDuLieuDo = CommonFunction.GetDataFromProcedure(reader);
                connection.Close();

                List<Dictionary<string, object>> shiftClone = new List<Dictionary<string, object>>();

                var groupNgayDo = listDuLieuDo.GroupBy(d => d["ngay_do"]);
                foreach (var ngayDo in groupNgayDo)
                {
                    var groupShift = ngayDo.GroupBy(d => d["id_shift"]);
                    foreach (var shift in groupShift)
                    {
                        var groupCountNum = shift.GroupBy(d => d["count_num"]);
                        foreach(var countNum in groupCountNum)
                        {
                            foreach (var item in countNum)
                            {
                                shiftClone.Add(item);
                            }

                            using var connection2 = new SqlConnection(_connection.DefaultConnection);
                            using var command2 = new SqlCommand("Mold1_GetKetQuaDo", connection2) { CommandType = CommandType.StoredProcedure };
                            command2.Parameters.AddWithValue("@ngay_do", ngayDo.Key);
                            command2.Parameters.AddWithValue("@id_shift", shift.Key);
                            command2.Parameters.AddWithValue("@count_num", countNum.Key);

                            connection2.Open();
                            var reader2 = command2.ExecuteReader();
                            List<Dictionary<string, object>> listKetQuaDo = CommonFunction.GetDataFromProcedure(reader2);
                            connection2.Close();
                            foreach (var item in listKetQuaDo)
                            {
                                shiftClone.Add(item);
                            }
                        }
                    }
                }

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get history mold1 successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get history mold1 successfully",
                    Data = shiftClone,
                    size = shiftClone.Count
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
        public IActionResult GetActive()
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("Mold1_GetActive", connection) { CommandType = CommandType.StoredProcedure };
                connection.Open();
                var reader = command.ExecuteReader();
                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);
                connection.Close();

                CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Get active mold1 successfully", CommonFunction.SUCCESS, functionName);
                var response = new CommonResponse<Dictionary<string, object>>
                {
                    StatusCode = CommonFunction.SUCCESS,
                    Message = "Get active mold1 successfully",
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
        public IActionResult ActiveSystem(string password)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("Mold1_PasswordActive", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                var reader = command.ExecuteReader();
                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);
                connection.Close();

                if(data.Count == 0)
                {
                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Active mold1 fail. Password incorrect!", CommonFunction.FAIL, functionName);
                    var response = new CommonResponse<Dictionary<string, object>>
                    {
                        StatusCode = CommonFunction.FAIL,
                        Message = "Active mold1 fail. Password incorrect!",
                        Data = data,
                        size = data.Count
                    };
                    return Ok(response);
                }
                else
                {
                    using var connection2 = new SqlConnection(_connection.DefaultConnection);
                    using var command2 = new SqlCommand("Mold1_ActiveSystem", connection2) { CommandType = CommandType.StoredProcedure };
                    //command.Parameters.AddWithValue("@password", password);

                    connection2.Open();
                    var reader2 = command2.ExecuteReader();
                    connection.Close();

                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Active mold1 successfully", CommonFunction.SUCCESS, functionName);
                    var response = new CommonResponse<Dictionary<string, object>>
                    {
                        StatusCode = CommonFunction.SUCCESS,
                        Message = "Active mold1 successfully",
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
        public IActionResult RecheckSystem(string password, string ngayDo, int idShift)
        {
            string functionName = ControllerContext.ActionDescriptor.ControllerName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string userid = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                using var connection = new SqlConnection(_connection.DefaultConnection);
                using var command = new SqlCommand("Mold1_PasswordActive", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                var reader = command.ExecuteReader();
                List<Dictionary<string, object>> data = CommonFunction.GetDataFromProcedure(reader);
                connection.Close();

                if (data.Count == 0)
                {
                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Recheck mold1 fail. Password incorrect!", CommonFunction.FAIL, functionName);
                    var response = new CommonResponse<Dictionary<string, object>>
                    {
                        StatusCode = CommonFunction.FAIL,
                        Message = "Recheck mold1 fail. Password incorrect!",
                        Data = data,
                        size = data.Count
                    };
                    return Ok(response);
                }
                else
                {
                    using var connection2 = new SqlConnection(_connection.DefaultConnection);
                    using var command2 = new SqlCommand("Mold1_RecheckSystem", connection2) { CommandType = CommandType.StoredProcedure };
                    command2.Parameters.AddWithValue("@ngayDo", ngayDo);
                    command2.Parameters.AddWithValue("@idShift", idShift);

                    connection2.Open();
                    var reader2 = command2.ExecuteReader();
                    connection.Close();

                    CommonFunction.LogInfo(_connection.DefaultConnection, userid, "Recheck mold1 successfully", CommonFunction.SUCCESS, functionName);
                    var response = new CommonResponse<Dictionary<string, object>>
                    {
                        StatusCode = CommonFunction.SUCCESS,
                        Message = "Recheck mold1 successfully",
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

    }
}
