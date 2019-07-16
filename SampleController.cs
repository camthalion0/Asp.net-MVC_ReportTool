using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Helper;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Controllers
{
    public partial class SampleController
    {
        public ActionResult Index(SampleQueryModel query)
        {
            return View();
        }

		/// <summary>
		/// Report List (PartialView)
		/// </summary>
        public ActionResult _ReportList(SampleQueryModel query)
        {
            var model = new PageViewModel();
            model.PerPageCount = query.RecordCount;
            model.CurrentPage = query.PageNumber;
            model.DataUrl = Url.Action(RouteData.Values["action"].ToString(), RouteData.Values["controller"]);
            model.QueryObjJson = JsonConvert.SerializeObject(query);
            model.AjaxDivID = "div_SampleList";

            ViewBag.Header = TableHeaderHelper.GetModelTableHeaderByTitleItemResource(new SampleViewModel());
            ViewBag.TableData = GetSampleData(query);

            model.TotalCount = ViewBag.TotalCount;  //取資料function回傳

            //excel用
            var excelQueryModel = new ExcelQueryModel<SampleQueryModel>
            {
                ExportActionPath = Url.Action("ExportLogListExcel", RouteData.Values["controller"].ToString()),
                Query = query,
                LogType = "Sample",
                TitleList = ViewBag.Header
            };
            ViewBag.ExcelQueryModel = excelQueryModel;
            ViewBag.ExcelQueryModel = excelQueryModel;

            model.TotalCount = ViewBag.TotalCount;  //取資料function回傳
            return PartialView("_List", model);
        }

		/// <summary>
		/// Get Data
		/// </summary>
        public List<SampleDataModel> GetSampleData(SampleQueryModel query, bool getExcel = false)
        {
            string slitStr = getExcel ? "\n" : "<br>";
            var data = new List<SampleDataModel>();
            var apiResult = /* get data */
            if ( /* get some data success */)   //有資料則轉成報表格式
            {
                data = apiResult.Select(x => new SampleViewModel()
                {
                    ViewCollumn1 = x.DataCollumn1,
                    ViewCollumn2 = x.DataCollumn2,                    
                }).ToList();
            }

            ViewBag.TotalCount = /* apiResult total count */
            return data;
        }
		
		/// <summary>
		/// Export Excel
		/// </summary>
		[ValidateInput(false)]
        public ActionResult ExportLogListExcel(string querymodelstr)
        {
            try
            {
                var querymodel = JsonConvert.DeserializeObject<ExcelQueryModel<Object>>(querymodelstr);
                var exportExcelModels = new List<ExcelSheetModel>();
                string fileName = "";
                IEnumerable<IViewModel> dataList = null;
                var querysamplereport = JsonConvert.DeserializeObject<SampleQueryModel>(querymodel.Query.ToString());
                var memoList = new List<string>();

                switch (querymodel.LogType)
                {
                    case "Sample":
                        fileName = "Sample Report";
                        dataList = GetSampleData(querysamplereport, true);
                        break;

                    default:
                        break;
                }

                exportExcelModels.Add(new ExcelSheetModel()
                {
                    WorksheetsName = fileName,
                    TitleList = querymodel.TitleList,
                    Memo = memoList
                });

                foreach (var data in dataList)
                {
                    var properties = data.GetType().GetProperties();
                    var list = new List<string>();

                    foreach (PropertyInfo property in properties)
                    {
                        list.Add(property.GetValue(data)?.ToString());   //每筆資料轉成string[]
                    }
                    exportExcelModels[0].DataList.Add(list.ToArray());
                }

                var strem = ExportExcelHelper.CreateExcelWorkbook(exportExcelModels);
                return File(new MemoryStream(strem.GetBuffer()), "application/vnd.ms-excel", fileName + ".xls");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}