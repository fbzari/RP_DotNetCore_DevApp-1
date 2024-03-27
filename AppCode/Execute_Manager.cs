using System.Collections;
using System.Data;
using Vertica.Data.VerticaClient;

namespace RP_DotNetCore_DevApp.AppCode
{
    public class Execute_Manager
    {
        public static Hashtable all_Get_Source_Data = new Hashtable();

        public Execute_Manager()
        {
               //  all_Get_Source_Data.Clear();
                 all_Get_Source_Data = new Hashtable();
        }
        public DataTable getTableDataSet(VerticaConnection connection, String query, String table_name)
        {
            Globals.WriteLog("--------------------------------------------------------------------------------------------------");
            Globals.WriteLog("-------------------------------------- In getTableDataSet(). -------------------------------------");

            Globals.WriteLog("\tQuery:\n\t" + query);
            Globals.WriteLog("\ttable_name: " + table_name);

            // 030123 - Bypass_Cache_Str
            String Bypass_Cache_Str = "~~BYPASS_CACHE~~";

            query = query.Replace("$$Table_Name$$", table_name);

            Globals.WriteLog("\tFinal Query after replacing table name:\n\t" + query);

            // 011523 - Declare dt outside try.
            DataTable dt = new DataTable();

            try
            {
                // 030123 - Bypass_Cache_Str
                //
                if (query.Contains(Bypass_Cache_Str))
                {
                    Globals.WriteLog("\tBypass_Cache is TRUE. So, do not check the SQL Cache. Execute this SQL and get a new dataset.");
                    query = query.Replace(Bypass_Cache_Str, "");
                }
                else
                {
                    //
                    // code added for optimizing DB access - Cache data.
                    if (all_Get_Source_Data.ContainsKey(query))
                    {
                        //// 052722
                        //Globals.WriteLog("\tData Found for this SQL in the Collection. Returning data from collection.");
                        //Globals.WriteLog("\tData Found for this SQL in the Collection: Rows: " + ((DataTable)
                        //    all_Get_Source_Data[query]).Rows.Count + "; Columns: " + ((DataTable)all_Get_Source_Data[query]).Columns.Count);
                        //// 052722

                        return (DataTable)all_Get_Source_Data[query];
                    }
                }
                //
                // 030123 - End - Bypass_Cache_Str

                // 011523 - Commented
                // DataTable dt = new DataTable();

                Globals.WriteLog("\tData NOT Found for this SQL in the Collection. Reload data into collection.");

                VerticaCommand command = connection.CreateCommand();
                command.CommandText = query;
                VerticaDataAdapter vdp = new VerticaDataAdapter(command);
                vdp.Fill(dt);
                vdp.Dispose();
                Globals.WriteLog("\tData Set loaded successfully. Column Count: " + dt.Columns.Count + "; Row Count: " + dt.Rows.Count);

                // 052722 -- !query.Contains("NEXTVAL") && table_name != "Override"
                //20SEP2021 
                // code added for optimizing the DB Access
                if (dt != null && !query.Contains("NEXTVAL"))
                {
                    Globals.WriteLog("\tAdding new Data Source to the Collection.");
                    all_Get_Source_Data[query] = dt;
                }

                Globals.WriteLog("----------------------------------- In getTableDataSet() - End. ----------------------------------");
                Globals.WriteLog("--------------------------------------------------------------------------------------------------");

                return dt;
            }
            catch (VerticaException ex)
            {
                Globals.WriteLog("\t******* Exception in getTableDataSet():\n\t" + ex);

                // 011523 - Enhanced error handling
                dt.ExtendedProperties.Add("error", ex.ToString());
                return dt;
                //
            }
        }
    }
}
