using Newtonsoft.Json.Linq;
using System.Data;

namespace RP_DotNetCore_DevApp.Models
{
    public class DataGridsModel
    {
        public string id { get; set; }
        public string TableName { get; set; }
        public string Title { get; set; }
        public DataTable DataSource { get; set; }

        public List<string> toolBarItems { get; set; }
        public List<string> primaryKeyColumn { get; set; }

        public DataColumnCollection dataColumns { get; set; }

        public int rowNum { get; set; }
        public bool pivotTable { get; set; }
        public List<JObject> rows { get; set; }
        public List<JObject> columns { get; set; }
        public List<JObject> values { get; set; }
    }
}
