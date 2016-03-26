using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace MyMVCTest04.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(p => !p.是否已刪除);
        }

        public 客戶聯絡人 Find(int? id)
        {
            return this.All().Where(p => p.Id == id).FirstOrDefault();
        }

        public  IQueryable<客戶聯絡人> All(bool isAll)
        {
            if (isAll)
            {
                return base.All();
            }
            else
            {
                return base.All().Where(p => !p.是否已刪除);
            }          
        }

        public MemoryStream GetExcelFile()
        {
            var exportData = this.All().ToList();


            HSSFWorkbook workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("客戶職稱");

            sheet.CreateRow(0).CreateCell(0).SetCellValue("客戶Id");
            sheet.GetRow(0).CreateCell(1).SetCellValue("職稱");
            sheet.GetRow(0).CreateCell(2).SetCellValue("姓名");
            sheet.GetRow(0).CreateCell(3).SetCellValue("Email");
            sheet.GetRow(0).CreateCell(4).SetCellValue("手機");
            sheet.GetRow(0).CreateCell(5).SetCellValue("電話");


            //第一Row 已經被Title用掉了 所以從1開始
            for (var i = 1; i <= exportData.Count(); i++)
            {
                sheet.CreateRow(i).CreateCell(0).SetCellValue(exportData[i-1].客戶Id);
                sheet.GetRow(i).CreateCell(1).SetCellValue(exportData[i-1].職稱);
                sheet.GetRow(i).CreateCell(2).SetCellValue(exportData[i-1].姓名);             
                sheet.GetRow(i).CreateCell(3).SetCellValue(exportData[i-1].Email);
                sheet.GetRow(i).CreateCell(4).SetCellValue(exportData[i-1].手機);
                sheet.GetRow(i).CreateCell(5).SetCellValue(exportData[i-1].電話);
            }
                       
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);

            var file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "sample.xls", FileMode.Create);
            workbook.Write(file);
            file.Close();




            return ms;
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}