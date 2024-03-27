using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RP_DotNetCore_DevApp.Models
{
    public class DataGridModel
    {
        public string SelectedOption { get; set; }

        public DataTable SourceDt { get; set; }

        public List<string> DG_button_Options { get; set; }

        public DataColumnCollection DG_DataColumn { get; set; }  
    }
}
