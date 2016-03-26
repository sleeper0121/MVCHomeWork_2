using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MyMVCTest04.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => !p.是否已刪除);
        }

        public 客戶資料 Find(int? id)
        {
            return this.All().Where(p => p.Id == id).FirstOrDefault();
        }

        public IQueryable<客戶資料> All(bool isAll)
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
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}