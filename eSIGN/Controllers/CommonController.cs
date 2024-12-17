using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Reflection.Metadata;
using System.Text;

namespace WaferMapViewer.Controllers
{
    public class CommonController : Controller
    {
        string dbContextStr = "Data Source=10.201.21.84,50150;Initial Catalog=WaferMapViewer;Persist Security Info=True;User ID=cimitar2;Password=TFAtest1!2!;Trust Server Certificate=True";

        [HttpPost]
        public string CallUSP(string flag, string paramStr)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@flag", flag));
                parameters.Add(new SqlParameter("@paramString", paramStr));

                string resJson = ExcuteJson("[dbo].[USP_ASSY_Call_SP]", parameters);

                return resJson;
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message);
            }
        }
        [HttpPost]
        public string callUSPDTS(string flag, string paramStr)
        {
            try
            {
                string Catalog = "WaferMapViewer";
                //string dbContext = $@"Data Source=10.201.21.84,50150;Initial Catalog={Catalog};Persist Security Info=True;User ID=cimitar2;Password=TFAtest1!2!;Trust Server Certificate=True";

                DataSet dataSet = new DataSet();
                DataTable dataReturn = new DataTable();
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@flag", flag));
                sqlParameters.Add(new SqlParameter("@paramString", paramStr));
                dataSet = ExcuteSP("[dbo].[USP_ASSY_Call_SP]", sqlParameters, Catalog);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    dataReturn = dataSet.Tables[0];
                }

                return JsonConvert.SerializeObject(dataReturn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        // Lấy dữ liệu data về
        [HttpGet]
        public string uspGET(string catalogStr, string uspName, string flagStr, string parameterStr)
        {
            try
            {
                DataSet dataSet = new DataSet();
                DataTable dataReturn = new DataTable();
                List<SqlParameter> sqlParameters = new List<SqlParameter>();

                sqlParameters.Add(new SqlParameter("@flag", flagStr));
                sqlParameters.Add(new SqlParameter("@paramString", parameterStr));
                dataSet = ExcuteSP(uspName, sqlParameters, catalogStr);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    dataReturn = dataSet.Tables[0];
                }

                return JsonConvert.SerializeObject(dataReturn);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        // Cập nhật dữ liệu
        [HttpPut]
        public IActionResult uspPUT()
        {
            try
            {
                string jsonBody;
                //Đọc body của request
                using (var reader = new StreamReader(Request.Body))
                {
                    jsonBody = reader.ReadToEndAsync().Result;
                }

                var jsonObject = JObject.Parse(jsonBody);

                string catalogStr = jsonObject["catalogStr"]?.ToString();
                string uspName = jsonObject["uspName"]?.ToString();
                string flagStr = jsonObject["flagStr"]?.ToString();
                string paramStr = jsonObject["paramStr"]?.ToString();

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("@flag", flagStr),
                    new SqlParameter("@paramString", paramStr)
                };

                ExcuteSP(uspName, sqlParameters, catalogStr);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Thêm dữ liệu
        [HttpPost]
        public IActionResult uspPOST() 
        {
            try
            {
                string jsonBody;
                //Đọc body của request
                using (var reader = new StreamReader(Request.Body))
                {
                    jsonBody = reader.ReadToEndAsync().Result;
                }

                var jsonObject = JObject.Parse(jsonBody);

                string catalogStr = jsonObject["catalogStr"]?.ToString();
                string uspName = jsonObject["uspName"]?.ToString();
                string flagStr = jsonObject["flagStr"]?.ToString();
                string paramStr = jsonObject["paramStr"]?.ToString();

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("@flag", flagStr),
                    new SqlParameter("@paramString", paramStr)
                };

                ExcuteSP(uspName, sqlParameters, catalogStr);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public string ExcuteJson(string storeName, List<SqlParameter> sqlParametetList)
        {
            SqlConnection conn = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                conn = new SqlConnection(dbContextStr);
                command.Connection = conn;

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeName;
                command.CommandTimeout = 300;

                if (sqlParametetList != null)
                {
                    for (int i = 0; i < sqlParametetList.Count; i++)
                    {
                        command.Parameters.Add(sqlParametetList[i]);
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                command.Connection.Close();
                var jsonStringBuilder = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    jsonStringBuilder.Append("[");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        jsonStringBuilder.Append("{");
                        for (int j = 0; j < dt.Columns.Count; j++)
                            jsonStringBuilder.AppendFormat("\"{0}\":\"{1}\"{2}",
                            dt.Columns[j].ColumnName.ToString(),
                            dt.Rows[i][j].ToString(),
                                    j < dt.Columns.Count - 1 ? "," : string.Empty);
                        jsonStringBuilder.Append(i == dt.Rows.Count - 1 ? "}" : "},");
                    }
                    jsonStringBuilder.Append("]");
                }
                string rek = jsonStringBuilder.ToString();
                return rek;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public DataSet ExcuteSP(string SP, List<SqlParameter> sqlParametetList, string catalogStr)
        {
            string dbContext = $@"Data Source=10.201.21.84,50150;Initial Catalog={catalogStr};Persist Security Info=True;User ID=cimitar2;Password=TFAtest1!2!;Trust Server Certificate=True";
            SqlConnection conn = null;
            DataSet ds = new DataSet();
            try
            {
                SqlCommand command = new SqlCommand();
                conn = new SqlConnection(dbContext);
                command.Connection = conn;

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SP;
                command.CommandTimeout = 300;

                if (sqlParametetList != null)
                {
                    for (int i = 0; i < sqlParametetList.Count; i++)
                    {
                        command.Parameters.Add(sqlParametetList[i]);
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);

                command.Connection.Close();
            }
            catch
            {
                return ds;
            }
            return ds;
        }
    }
}
