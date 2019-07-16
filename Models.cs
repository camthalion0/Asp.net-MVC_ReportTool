namespace Models
{
	public class PageViewModel
	{
		public int? TotalCount { get; set; }     //資料筆數
		public int PerPageCount { get; set; } = 10;
		public int CurrentPage { get; set; } = 0;   //第1頁
		public string DataUrl { get; set; }    //ajax url
		public string QueryObjJson { get; set; }    //query model in json string
		public string AjaxDivID { get; set; } = "div_Loglist";
	}
		
	/// <summary>
	/// 僅用於列表輸出
	/// </summary>
	public interface IViewModel
	{
	}
	
	public class SampleViewModel: IViewModel
	{
		public string ViewCollumn1 { get; set; }
		public string ViewCollumn2 { get; set; }
	}
	
	public class SampleQueryModel
	{
		public string Condition1 { get; set; } 
        public int PageNumber { get; set; } = 0;
        public int RecordCount { get; set; } = 10;
	}
	
	public class SampleDataModel
	{
		public string DataCollumn1 { get; set; }
		public string DataCollumn2 { get; set; }
	}
	
	
	/// <summary>
	/// Excel工作表
	/// </summary>
	public class ExcelSheetModel
    {
        public string WorksheetsName { get; set; }     //分頁名稱
        public List<string> TitleList { get; set; } = new List<string>();  //headers名稱
        public List<string[]> DataList { get; set; } = new List<string[]>();   //表格內容
        public List<string> Memo { get; set; } = new List<string>(); //Excel補充說明(置於table上方)
    }

	/// <summary>
	/// Excel
	/// </summary>
    public class ExcelQueryModel<T>
    {
        public string LogType { get; set; }     //報表種類(單頁面多報表時使用)
        public List<string> TitleList { get; set; }//headers名稱
        public string ExportActionPath { get; set; }    //ExportAction路徑
        public T Query { get; set; }    //搜尋條件
    }
}