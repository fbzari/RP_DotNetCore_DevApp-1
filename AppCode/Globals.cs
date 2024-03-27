using System.Diagnostics;
using System.Globalization;
//using System.Diagnostics;

namespace RP_DotNetCore_DevApp.AppCode
{
    public class Globals
    {

        private static object LockForTextFile = new Object();
        public static void WriteLog(String action)
        {
            Debug.WriteLine(action);
            //// get call stack
            StackTrace stackTrace = new StackTrace();
            String filepath = Config.root_path + Config.logs_folder;

            try
            {
                String appendText = "\n";
                String method_name = "RP - "; //stackTrace.GetFrame(1).GetMethod().DeclaringType.Name + "." + stackTrace.GetFrame(1).GetMethod().Name;
                appendText = method_name + " [ " + action + " ] ";

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                DateTime dt = DateTime.Now;
                String cur_dt = dt.ToString("yyyy-MM-dd");
                String log_file_name = "Log_" + cur_dt + ".log";
                filepath = filepath + log_file_name;
                lock (LockForTextFile)
                {
                    if (!File.Exists(filepath))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(filepath))
                        {
                            sw.WriteLine(appendText);
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.AppendText(filepath))
                        {
                            sw.WriteLine(appendText);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog("Logs Exception:\n" + ex.Message + "\nStackTrace:\n" + ex.StackTrace);
            }

        }
    }
}
