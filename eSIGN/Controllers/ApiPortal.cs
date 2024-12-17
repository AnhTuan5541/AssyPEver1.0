using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace WaferMapViewer.Controllers
{
    [Route("api/common")]
    [ApiController]
    public class ApiPortal : Controller
    {
        private const string ConnectionStringTemplate = @"Data Source=10.201.21.84,50150;Initial Catalog={0};Persist Security Info=True;User ID=cimitar2;Password=TFAtest1!2!;Trust Server Certificate=True";

        private async Task<string> ReadRequestBodyAsync()
        {
            using var reader = new StreamReader(Request.Body);
            return await reader.ReadToEndAsync();
        }

        private bool TryParseJson(string json, out JObject jsonObject, out IActionResult errorResult)
        {
            try
            {
                jsonObject = JObject.Parse(json);
                errorResult = null;
                return true;
            }
            catch (JsonReaderException)
            {
                jsonObject = null;
                errorResult = BadRequest("Invalid JSON format.");
                return false;
            }
        }

        private async Task<DataSet> ExecuteStoredProcedureAsync(string storedProcedure, List<SqlParameter> sqlParameters, string catalog)
        {
            var connectionString = string.Format(ConnectionStringTemplate, catalog);

            using var conn = new SqlConnection(connectionString);
            using var command = new SqlCommand(storedProcedure, conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 300
            };

            if (sqlParameters != null)
            {
                command.Parameters.AddRange(sqlParameters.ToArray());
            }

            var dataSet = new DataSet();
            await conn.OpenAsync();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);

            return dataSet;
        }
        
        [HttpGet("getUSP")]
        public async Task<IActionResult> GetAsync(string catalogStr, string uspName, string flagStr, string parameterStr)
        {
            if (string.IsNullOrWhiteSpace(catalogStr) || string.IsNullOrWhiteSpace(uspName))
            {
                return BadRequest("Missing required parameters.");
            }

            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@flag", flagStr),
                new SqlParameter("@paramString", parameterStr)
            };

            try
            {
                var dataSet = await ExecuteStoredProcedureAsync(uspName, sqlParameters, catalogStr);
                var dataReturn = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                return Ok(JsonConvert.SerializeObject(dataReturn));
            }
            catch (Exception ex)
            {
                // Log the error using a logging framework
                Console.Error.WriteLine(ex.Message); // Replace with a proper logging mechanism
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost("updateUSP")]
        public async Task<IActionResult> UpsertAsync()
        {
            string jsonBody = await ReadRequestBodyAsync();
            if (!TryParseJson(jsonBody, out var jsonObject, out var errorResult))
            {
                return errorResult;
            }

            string catalogStr = jsonObject["catalogStr"]?.Value<string>();
            string uspName = jsonObject["uspName"]?.Value<string>();
            string flagStr = jsonObject["flagStr"]?.Value<string>();
            string paramStr = jsonObject["parameterStr"]?.Value<string>();

            if (string.IsNullOrEmpty(catalogStr) || string.IsNullOrEmpty(uspName) || string.IsNullOrEmpty(flagStr) || string.IsNullOrEmpty(paramStr))
            {
                return BadRequest("Missing required parameters: uspName, flagStr, paramStr, catalogStr.");
            }

            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@flag", flagStr),
                new SqlParameter("@paramString", paramStr)
            };

            try
            {
                await ExecuteStoredProcedureAsync(uspName, sqlParameters, catalogStr);
                return Ok("SUCCESS");
            }
            catch (Exception ex)
            {
                // Log the error using a logging framework
                Console.Error.WriteLine(ex.Message); // Replace with a proper logging mechanism
                return StatusCode(500, $"Error executing stored procedure: {ex.Message}");
            }
        }

        // Api mail
    }
    public class UpsertRequestData
    {
        public string CatalogStr { get; set; }
        public string UspName { get; set; }
        public string FlagStr { get; set; }
        public string ParamStr { get; set; }
    }
}
