using System;
using System.Data;
using System.Data.Common;
using Vertica.Data.VerticaClient;

namespace RP_DotNetCore_DevApp.AppCode
{
    public class Connection_Manager
    {
 
        public Connection_Manager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //
        //Create Connection to Vertica Database 
        //
        public static VerticaConnection Create_Vertica_Connection(String host, String database, int port, String user, String password, bool ssl_flag)
        {
            try
            {
                // 0917
                Globals.WriteLog("\n************** In Create_Vertica_Connection() ****************");
                Globals.WriteLog("************** Start Time ****************: " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss.ffff"));

                VerticaConnectionStringBuilder config_info = new VerticaConnectionStringBuilder();
                config_info.Host = host;
                config_info.Database = database;
                config_info.User = user;
                config_info.Port = port;
                config_info.Password = password;
                config_info.SSL = ssl_flag;

                config_info.ConnectionTimeout = 60;

                Globals.WriteLog("\t_____ Connecting to: Host: " + config_info.Host + "; Database: " + database + "; Port: " + port);
                VerticaConnection conn = new VerticaConnection(config_info.ToString());
                Globals.WriteLog("\t_____ Connection Successful. Connection Object: " + conn);

                // 0917
                Globals.WriteLog("************** End Time ****************: " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss.ffff"));
                return conn;
            }
            catch (Exception ex)
            {
                Globals.WriteLog("\t____ Error in Connecting to Vertica Db: " + host + "\n" + ex);
                VerticaConnection conn = null;
                return conn;
            }
        }

        //
        //Generic Method to Create Connection to Database
        //
        public static DbConnection Create_Connection_Gen(string connection_string, string provider_name)
        {
            Globals.WriteLog("\n************** Create_Connection_Gen(). ****************\n");
            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(provider_name);
                DbConnection conn = factory.CreateConnection();
                conn.ConnectionString = connection_string;
                Globals.WriteLog("\t_____ Connection Object :  " + conn);
                return conn;

            }
            catch (DbException ex)
            {
                Globals.WriteLog("\t****** Exception in CreateConnectionGen () \n" + ex);
                DbConnection conn = null;
                return conn;
            }
        }
    }
}
