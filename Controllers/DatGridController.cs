//using Cake.Core.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RP_DotNetCore_DevApp.AppCode;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Vertica.Data;
using Vertica.Data.VerticaClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RP_DotNetCore_DevApp.Models;
using System.Data;


namespace RP_DotNetCore_DevApp.Controllers
{
    public class DatGridController : Controller
    {
        [ResponseCache(Duration = 60)]
        public IActionResult Index()
        {
            // create db connection --

            // Get the Overrides Config file.
            string config_path = Path.Combine(Config.root_path, Config.config_path);
            string config_file = Path.Combine(config_path, Config.config_file);
            String myJsonString = System.IO.File.ReadAllText(config_file);
            JObject myJObject = JObject.Parse(myJsonString);
            VerticaConnection _VConn = null;

            var viewModel = new DataGridModel();


            Connection_Manager conManager = new Connection_Manager();
           
            string db_host = myJObject.SelectToken("$.override_connection_details.host").Value<string>();
            string db_database = myJObject.SelectToken("$.override_connection_details.database").Value<string>();
            string db_port = myJObject.SelectToken("$.override_connection_details.port").Value<string>();
            string db_user = myJObject.SelectToken("$.override_connection_details.user").Value<string>();
            string db_password = myJObject.SelectToken("$.override_connection_details.password").Value<string>();
            string ssl_flag = myJObject.SelectToken("$.override_connection_details.ssl").Value<string>();
            string version_num = myJObject.SelectToken("$.Version_Number").Value<string>();
            string master_table = myJObject.SelectToken("$.override_tables.master_table").Value<string>();
            string rules_table = myJObject.SelectToken("$.override_tables.rules_table").Value<string>();
            string field_map = myJObject.SelectToken("$.override_tables.field_map_table").Value<string>();
            string override_object_table = myJObject.SelectToken("$.override_tables.override_object_table").Value<string>();
            _VConn = Connection_Manager.Create_Vertica_Connection(db_host, db_database, int.Parse(db_port), db_user, db_password, bool.Parse(ssl_flag));

            string schema_name = myJObject.SelectToken("$.override_schema_name").Value<string>();

            string selectQuery = myJObject.SelectToken("$.querys.select").Value<string>();
            selectQuery = selectQuery.Replace("$$Schema_Name$$", schema_name);
            Execute_Manager executeManager = new Execute_Manager();
            DataTable masterDt = executeManager.getTableDataSet(_VConn, selectQuery, master_table);
            // Set the Source DataTable        
            viewModel.SourceDt = masterDt;
            DataColumnCollection columns = masterDt.Columns;
            //DataColumn[] dataColumns = new DataColumn[100];
            //columns.CopyTo(dataColumns, 1);

            // setting the DataColumns--
            ViewBag.DG_DataColumn = masterDt.Columns;

            ViewBag.DG_button_Options = new List<string>() { "add",
            "edit",
            "delete",
            "update",
            "cancel"};

            // configuring the grid Edit Setting--
            bool allowAdding =false , allowDeleting = false, allowEditing = true;

            // editmode 
            //string gridEditMode = "Batch";

            ViewBag.allowAdding = allowAdding;
            ViewBag.allowDeleting = allowDeleting;
            ViewBag.allowEditing = allowEditing;
            //ViewBag.gridEditMode  = Syncfusion;



            ViewBag.datasource = masterDt;
            //viewModel.

            // Need to Get the values from the Config--
            return View(viewModel);

        }
    }
}
