using MyMVCTest04.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMVCTest04.Controllers
{
    public class 客戶列表Controller : Controller
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶列表Repository repo = RepositoryHelper.Get客戶列表Repository();



        // GET: 客戶列表
        public ActionResult Index()
        {           
            return View(repo.All());
        }
        [HttpPost]
        public ActionResult Index(string custName)
        {
            if (!string.IsNullOrEmpty(custName))
            {
                var data = repo.Find(custName);
                return View(data);
            }
            return View(repo.All());
        }
    }
}