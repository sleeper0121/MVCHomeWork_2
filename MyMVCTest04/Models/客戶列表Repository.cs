using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MyMVCTest04.Models
{   
	public  class 客戶列表Repository : EFRepository<客戶列表>, I客戶列表Repository
	{


        public 客戶列表 Find(string custName)
        {
            return this.All().Where(p => p.客戶名稱.Contains(custName)).FirstOrDefault();
        }

   
    }

	public  interface I客戶列表Repository : IRepository<客戶列表>
	{

	}
}