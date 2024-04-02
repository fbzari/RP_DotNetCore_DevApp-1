using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RP_DotNetCore_DevApp.AppCode;
using System.Data;
using Vertica.Data.VerticaClient;

namespace RP_DotNetCore_DevApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PivotController : ControllerBase
    {
        [HttpGet(Name = "GetPivotData")]
        public object Get()
        {
            return JsonConvert.SerializeObject(GetPivotData());
        }

        public dynamic GetPivotData()
        {
            try
            {
                // Get the Overrides Config file.
                string config_path = Path.Combine(Config.root_path, Config.config_path);
                string config_file = Path.Combine(config_path, Config.config_file);
                String myJsonString = System.IO.File.ReadAllText(config_file);
                JObject myJObject = JObject.Parse(myJsonString);

                string db_host = myJObject.SelectToken("$.override_connection_details.host").Value<string>();
                string db_database = myJObject.SelectToken("$.override_connection_details.database").Value<string>();
                string db_port = myJObject.SelectToken("$.override_connection_details.port").Value<string>();
                string db_user = myJObject.SelectToken("$.override_connection_details.user").Value<string>();
                string db_password = myJObject.SelectToken("$.override_connection_details.password").Value<string>();
                string ssl_flag = myJObject.SelectToken("$.override_connection_details.ssl").Value<string>();

                string query = "SELECT * FROM Override.PivotData;";
                VerticaConnection _VConn = Connection_Manager.Create_Vertica_Connection(db_host, db_database, int.Parse(db_port), db_user, db_password, bool.Parse(ssl_flag));
                _VConn.Open();
                VerticaCommand cmd = new VerticaCommand(query, _VConn);
                VerticaDataAdapter da = new VerticaDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                _VConn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                Globals.WriteLog("Error in GetPostgreSQLResult" + ex);
                return null;
            }
           
        }
    }
}
