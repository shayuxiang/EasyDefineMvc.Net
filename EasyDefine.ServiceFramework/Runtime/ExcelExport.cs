using EasyDefine.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasyDefine.ServiceFramework.Runtime
{
    /// <summary>
    /// Excel 导出工具
    /// </summary>
    public class ExcelExport
    {
        /// <summary>
        /// Excel文件根目录
        /// </summary>
        //private string Root = "";
        //private string FileName = "";
        private ExcelPackage package = default(ExcelPackage);

        public ExcelExport()
        {
            try
            {
                package = new ExcelPackage();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 对象导出Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Datas"></param>
        /// <returns></returns>
        public Stream AddExcelPage<T>(string packageName,IEnumerable<T> Datas)
        {
            using (package)
            {
                // 添加worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(packageName);
                int RowId = 2;
                foreach (var t in Datas)
                {
                    int ColId = 1;
                    foreach (var p in t.GetType().GetProperties())
                    {
                        var excelfield = p.GetCustomAttributes(typeof(ApiExcelField), true);
                        //拿到字段属性
                        if (excelfield.Length > 0)
                        {
                            var excel = excelfield[0] as ApiExcelField;
                            //设置标题
                            if (worksheet.Cells[1, ColId].Value == null || string.IsNullOrEmpty(worksheet.Cells[1, ColId].Value.ToString()))
                            {
                                worksheet.Cells[1, ColId].Value = excel.DisplayName;
                            }
                            //设置内容
                            string RowName = ToNumberSystem26(ColId).ToString() + RowId.ToString();
                            var val = p.GetValue(t);
                            if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime)) {
                                val = ((DateTime)val).ToString("yyyy/MM/dd hh:mm:ss");
                            }
                            worksheet.Cells[RowName].Value = val;
                            //粗体设置
                            worksheet.Cells[RowName].Style.Font.Bold = excel.IsBlod;
                            ColId += 1;
                        }
                    }
                    RowId += 1;
                }
                package.SaveAs(new FileInfo(@"F:\学习强国-党员修养.xlsx"));
                //保存到内存流
                Stream retstream = new MemoryStream();
                package.SaveAs(retstream);
                retstream.Seek(0, SeekOrigin.Begin);
                return retstream;
            }
        }


        /// <summary>
        /// 获取行号 如 A,AB AC 
        /// </summary>
        /// <returns></returns>
        private string ToNumberSystem26(int ColId)
        {
            string s = string.Empty;
            while (ColId > 0)
            {
                int m = ColId % 26;
                if (m == 0) m = 26;
                s = (char)(m + 64) + s;
                ColId = (ColId - m) / 26;
            }
            return s;
        }

    }
}
