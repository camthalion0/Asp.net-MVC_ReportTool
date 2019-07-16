using Models;
using Resources;
using System.Collections.Generic;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace Helper
{
	/// <summary>
	/// 根據field命名嘗試於TitleItemResource中取得對應名稱
	/// </summary>
    public static class TableHeaderHelper
    {
        public static List<string> GetModelTableHeaderByTitleItemResource(IViewModel model)
        {
            var titleList = new List<string>();
            string title;

            var properties = model.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {			
                    title = TitleItemResource.ResourceManager.GetString(property.Name);
                    titleList.Add(title ?? property.Name);
                }
                catch
                {
                    titleList.Add(property.Name);
                }
            }

            return titleList;
        }
    }
	
	/// <summary>
	/// Create the ExcelWorkbook and write out to MemoryStream
	/// </summary>
	public static class ExportExcelHelper
    {
        public static MemoryStream CreateExcelWorkbook(List<ExcelSheetModel> excelSheetModelList)
        {
            // 建立工作表
            var workbook = new HSSFWorkbook();
            var sheet = new List<HSSFSheet>();

            for (var i = 0; i < excelSheetModelList.Count; i++)
            {
                // 建立分頁
                sheet.Add((HSSFSheet)workbook.CreateSheet(excelSheetModelList[i].WorksheetsName));

                // 寫入上方Memo
                if (excelSheetModelList[i].Memo.Count > 0)
                {
                    for (var j = 0; j < excelSheetModelList[i].Memo.Count; j++)
                    {
                        IRow memoRow = sheet[i].CreateRow(j);
                        memoRow.CreateCell(0).SetCellValue(excelSheetModelList[i].Memo[j]);
                    }
                }

                // 寫入標題列
                IRow titleRow = sheet[i].CreateRow(excelSheetModelList[i].Memo.Count);
                for (var j = 0; j < excelSheetModelList[i].TitleList.Count; j++)
                {
                    titleRow.CreateCell(j).SetCellValue(excelSheetModelList[i].TitleList[j]);
                }

                // 寫入資料列
                if (excelSheetModelList[i].DataList.Count > 0)
                {
                    ICellStyle CellCentertTopAlignment = workbook.CreateCellStyle();
                    CellCentertTopAlignment = workbook.CreateCellStyle();
                    CellCentertTopAlignment.Alignment = HorizontalAlignment.Center;
                    CellCentertTopAlignment.VerticalAlignment = VerticalAlignment.Center;

                    int rowIdx = excelSheetModelList[i].Memo.Count+1;
                    foreach (var rowData in excelSheetModelList[i].DataList)
                    {
                        IRow dataRow = sheet[i].CreateRow(rowIdx);
                        int cellIdx = 0;
                        foreach (var datacnt in rowData)
                        {
                            ICell cell = dataRow.CreateCell(cellIdx);
                            cell.SetCellValue(datacnt);
                            cell.CellStyle = CellCentertTopAlignment;
                            cell.CellStyle.WrapText = true;
                            cellIdx++;
                        }
                        rowIdx++;
                    }
                }
                for (var k = 0; k < excelSheetModelList[i].TitleList.Count; k++)
                {
                    sheet[i].AutoSizeColumn(k);
                }
            }

            var strem = new MemoryStream();
            workbook.Write(strem);
            return strem;
        }
    }
}