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
using System.Drawing;
using NuGet.Packaging.Signing;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol.Plugins;
using System.Text.RegularExpressions;
using System.Globalization;


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
            JObject datagridConfig = myJObject.SelectToken("$.rating.Data_Grid_Config.PeopleData").Value<JObject>();
            HttpContext.Session.SetString("datagridConfig", datagridConfig.ToString());
            Dictionary<int, List<DataGridsModel>> dataGrids = GetDataSource(datagridConfig,_VConn);
            ViewBag.DataGrids = GetDataSource(datagridConfig, _VConn);
            // Need to Get the values from the Config--
            return View(dataGrids);

        }

        private Dictionary<int, List<DataGridsModel>> GetDataSource(JObject DataGridConfig,VerticaConnection __VConn)
        {
            //List<DataGridsModel> datagrids = new List<DataGridsModel>();
            Dictionary<int, List<DataGridsModel>> datagrids = new Dictionary<int, List<DataGridsModel>>();

            foreach (var dataGrid in DataGridConfig) {

                var singleGrid = new DataGridsModel();
                singleGrid.id = dataGrid.Key;
                JObject gridProp = dataGrid.Value as JObject;
                Execute_Manager executeManager = new Execute_Manager();
                String selectQuery = gridProp?["DML_SQL"]?.ToString() ?? "";
                string tableName = gridProp?["targetTableName"]?.ToString()??"";
                string curSchema = gridProp?["SchemaName"]?.ToString()??"";
                singleGrid.TableName = tableName;

                selectQuery = selectQuery.Replace("$$Schema_Name$$", curSchema).Replace("$$Limit$$", "");
                DataTable dataTable = executeManager.getTableDataSet(__VConn, selectQuery, tableName);
                singleGrid.DataSource = dataTable;

                //int rowNum = gridProp["RowNum"] == null ? 1 : Convert.ToInt32(gridProp["RowNum"]);
                int rowNum = Convert.ToInt32(gridProp["RowNum"] ?? 1);
                string  title = gridProp["Title"].ToString() ?? "Grid Title";
                singleGrid.dataColumns = dataTable.Columns;

                List<string> toolbarItems = gridProp["Data_Grid_CRUD_Settings"]?.ToObject<List<string>>() ?? new List<string>();


                List<string> primaryKeyArray = gridProp["primaryKeyColumn"]?.ToObject<List<string>>() ?? new List<string>();

                bool pivotTable = gridProp["PivotTable"]?.Value<bool>() ?? false;
               
                if (pivotTable)
                {
                   
                    singleGrid.pivotTable = pivotTable;

                    List<JObject> rowslist = new List<JObject>();
                    List<JObject> columnsList = new List<JObject>();
                    List<JObject> valuesList = new List<JObject>();

                    JToken rowsToken = gridProp["rows"];
                    if (rowsToken != null && rowsToken.HasValues)
                    {
                        foreach (JProperty property in rowsToken.Children<JProperty>())
                        {
                            // Construct a new JObject containing all the properties of the current object
                            JObject obj = new JObject();
                            foreach (JProperty childProperty in property.Value.Children<JProperty>())
                            {
                                obj.Add(childProperty.Name, childProperty.Value);
                            }
                            rowslist.Add(obj);
                        }
                    }


                    JToken columnsToken = gridProp["columns"];
                    if (columnsToken != null && columnsToken.HasValues)
                    {
                        foreach (JProperty property in columnsToken.Children<JProperty>())
                        {
                            // Construct a new JObject containing all the properties of the current object
                            JObject obj = new JObject();
                            foreach (JProperty childProperty in property.Value.Children<JProperty>())
                            {
                                obj.Add(childProperty.Name, childProperty.Value);
                            }
                            columnsList.Add(obj);
                        }
                    }

                    JToken valuesToken = gridProp["values"];
                    if (valuesToken != null && valuesToken.HasValues)
                    {
                        foreach (JProperty property in valuesToken.Children<JProperty>())
                        {
                            // Construct a new JObject containing all the properties of the current object
                            JObject obj = new JObject();
                            foreach (JProperty childProperty in property.Value.Children<JProperty>())
                            {
                                obj.Add(childProperty.Name, childProperty.Value);
                            }
                            valuesList.Add(obj);
                        }
                    }

                    singleGrid.rows = rowslist;
                    singleGrid.columns = columnsList;
                    singleGrid.values = valuesList;


                }
                else
                {
                    singleGrid.pivotTable = pivotTable;
                }
                

                singleGrid.toolBarItems = toolbarItems;
                singleGrid.primaryKeyColumn = primaryKeyArray;
                singleGrid.rowNum = rowNum;
                singleGrid.Title = title;
                
                
                if (datagrids.ContainsKey(rowNum))
                {
                    datagrids[rowNum].Add(singleGrid);
                }
                else
                {
                    datagrids[rowNum] = new List<DataGridsModel> { singleGrid };
                }

            }

            return datagrids;
        }

        [HttpPost]
        public IActionResult Update([FromBody] dynamic jsonObject)
        {
            try
            {
                Globals.WriteLog("\t\t Enter in Update Method \n");
                JObject jObject = JsonConvert.DeserializeObject<JObject>(jsonObject.ToString());
                // Get the Overrides Config file.
                string config_path = Path.Combine(Config.root_path, Config.config_path);
                string config_file = Path.Combine(config_path, Config.config_file);
                String myJsonString = System.IO.File.ReadAllText(config_file);
                JObject myJObject = JObject.Parse(myJsonString);

                JArray jsonDataArray = (JArray)jObject["jsonData"];
                string gridName = (string)jObject["gridId"] ?? "";
                string updateQuery = myJObject.SelectToken("$.querys.update_data_grid_table").Value<string>();
                //string grid = JsonConvert.DeserializeObject<string>(gridId.ToString());

                DataTable updatedDT = JsonConvert.DeserializeObject<DataTable>(jsonDataArray.ToString()) ?? new DataTable();

                string datagridConfigstr = HttpContext.Session.GetString("datagridConfig") ?? "";
                JObject datagridConfig = JObject.Parse(datagridConfigstr);
                JObject gridProp = JObject.Parse(datagridConfig[gridName].ToString()) ?? new JObject();
                var primaryKeyColumn = gridProp["primaryKeyColumn"] ?? "";
                var schemaName = gridProp["SchemaName"] ?? "";
                var tableName = gridProp["targetTableName"] ?? "";

                string columns = "";

                foreach (DataColumn column in updatedDT.Columns)
                {
                    columns += $"{column.ColumnName} = @{column.ColumnName}, ";
                }

                columns = columns.TrimEnd(',', ' ');

                string whereClause = "";

                foreach (var item in primaryKeyColumn)
                {
                    whereClause += $"{item.ToString()} = @{item.ToString()} AND ";
                }

                whereClause = whereClause = Regex.Replace(whereClause, @"\sAND\s$", "");

                updateQuery = updateQuery.Replace("$$Table_Name$$", tableName.ToString
                    ()).Replace("$$Schema_Name$$", schemaName.ToString()).Replace("$$Columns$$", columns).Replace("$$Where_Condition$$", whereClause);


                string db_host = myJObject.SelectToken("$.override_connection_details.host").Value<string>();
                string db_database = myJObject.SelectToken("$.override_connection_details.database").Value<string>();
                string db_port = myJObject.SelectToken("$.override_connection_details.port").Value<string>();
                string db_user = myJObject.SelectToken("$.override_connection_details.user").Value<string>();
                string db_password = myJObject.SelectToken("$.override_connection_details.password").Value<string>();
                string ssl_flag = myJObject.SelectToken("$.override_connection_details.ssl").Value<string>();

                VerticaConnection _VConn = Connection_Manager.Create_Vertica_Connection(db_host, db_database, int.Parse(db_port), db_user, db_password, bool.Parse(ssl_flag));

                VerticaCommand command = new VerticaCommand(updateQuery,_VConn);

                foreach (DataRow row in updatedDT.Rows)
                {
                    foreach (DataColumn column in updatedDT.Columns)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "@" + column.ColumnName;
                        if (column.DataType == typeof(DateTime) && row[column] != DBNull.Value)
                        {
                            string dateString = row[column].ToString();
                            DateTime dateValue;
                            if (DateTime.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                            {
                                parameter.Value = dateValue.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                parameter.Value = DBNull.Value; 
                            }
                        }
                        else
                        {
                            parameter.Value = row[column];
                        }
                        command.Parameters.Add(parameter);
                    }
                }
                string message = "";
                int rowsAffected = 0;
                try
                {
                    if (_VConn.State != System.Data.ConnectionState.Open) { _VConn.Open(); }
                    rowsAffected = command.ExecuteNonQuery();
                    Globals.WriteLog($"{rowsAffected} row(s) updated.");
                    message = $"Successfully {rowsAffected} row(s) updated.";
                }
                catch (Exception ex)
                {
                    Globals.WriteLog($"Error updating record: {ex.Message}");
                    message = $"Error updating record: {ex.Message}";
                }
                Execute_Manager executeManager = new Execute_Manager();

                string selectQry = $"SELECT * FROM {schemaName.ToString()}.{tableName.ToString()};";

                if (Execute_Manager.all_Get_Source_Data.ContainsKey(selectQry))
                {
                    Execute_Manager.all_Get_Source_Data.Remove(selectQry);
                }


                DataTable NewDT = executeManager.getTableDataSet(_VConn, selectQry, tableName.ToString());

                _VConn.Close();

                string res = JsonConvert.SerializeObject(NewDT);

                return Ok(res);



            }
            catch (Exception ex)
            {
                Globals.WriteLog("\t\t Exception Occurs in Update Method \n" + ex);
                return BadRequest(ex.Message);
            }
            
            
        }
    }
}
