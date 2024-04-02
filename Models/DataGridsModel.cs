using System.Data;

namespace RP_DotNetCore_DevApp.Models
{
    public class DataGridsModel
    {
        public string id { get; set; }
        public string Title { get; set; }
        public DataTable DataSource { get; set; }

        public List<string> toolBarItems { get; set; }
        public List<string> primaryKeyColumn { get; set; }

        public DataColumnCollection dataColumns { get; set; }

        public int rowNum { get; set; }
    }
}
