namespace RP_DotNetCore_DevApp.AppCode
{
    public class Config
    {
        public Config()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //------PRODUCTION-CONFIG---------------------------------------//
        //public static String root_url = "http://10.97.93.203:8003/";  //
        //public static String root_path = @"C:\inetpub\Rules Portal\"; //
        //------PRODUCTION-CONFIG------------END------------------------//

        //------DEVELOPMENT-CONFIG---------------------------------------//
        public static String root_url = "http://localhost:2645/";
        // 063023 code [**] Updated the Application Base Directory Dynamically from the AppDomain Class
        // NOTE: This will get the current Base path of the Application installed Base Directory
        //public static String root_path = @"C:\Rating Portal\";
        public static String root_path = AppDomain.CurrentDomain.BaseDirectory;
        // 063023 code [**] -End- //
        //------DEVELOPMENT-CONFIG------------END------------------------//


        public static String config_path = @"Config_JSON\";
        public static String config_file = "overrides_config.json";
        public static String json_file_path = @"Rulesjson\\";
        public static String logs_folder = @"Logs\";
        public static String download_log = "Logs/";
        public static String app_data = @"App_Data\";
        public static String email_template = "EmailTemplate.html";

        public static String remainder_template = "Remind.html"; // 092223 [####]Remainder Email Template Added

    }
}
