using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyMVCTest04.Models;
using PagedList;
using System.Web.Security;
namespace MyMVCTest04.Controllers
{
    public class 客戶資料Controller : Controller
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        private 客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();

        // GET: 客戶資料
        public ActionResult Index(string custCategory,int pageNo = 1)
        {
            var Category = repo.All().Select(p => p.客戶分類).Distinct();
            ViewBag.custCategory = new SelectList(Category);

            if (!string.IsNullOrEmpty(custCategory))
            {
                var data = repo.All().Where(p => p.客戶分類.Contains(custCategory)).OrderBy(p => p.Id);
                return View(data.ToPagedList(pageNo,5));
            }            

            return View(repo.All().OrderBy(p => p.Id).ToPagedList(pageNo, 5));
        }

        public ActionResult test()
        {
            return View();
        }


        // GET: 客戶資料/Details/5
        [HandleError(ExceptionType =typeof(ArgumentException),View ="ErrorArgument")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (id == 12)
            {
                throw new Exception("這是一個自己丟的錯誤");
            }
            if (id == 13)
            {
                throw new ArgumentException("這是一個自己丟的錯誤");
            }


            客戶資料 客戶資料 = repo.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,帳號,密碼")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            客戶資料 客戶資料 = repo.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,是否已刪除")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = (客戶資料Entities)repo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            客戶資料 客戶資料 = repo.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.Find(id);
            客戶資料.是否已刪除 = true;
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (CheckLogin(login.Email, login.Password))
            {
                FormsAuthentication.RedirectFromLoginPage(login.Email, false);
                return RedirectToAction("ChangeCustomerData", "客戶資料",new {Email = login.Email });
            }
            ModelState.AddModelError("Password","有問題!!!");
            return View();
        }

        private bool CheckLogin(string email, string password)
        {
            //var pass = FormsAuthentication.HashPasswordForStoringInConfigFile(password,"MD5").ToLower();
            return repo.All().Where(p => p.帳號 == email && p.密碼 == password).Any();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "客戶資料");
        }


 
        [Authorize]
        public ActionResult ChangeCustomerData(string Email)
        {
            var data = repo.All().Where(p => p.帳號 == Email).FirstOrDefault();            

            return View(data);
        }

        [HttpPost]
        public ActionResult ChangeCustomerData(string 帳號 , FormCollection form)
        {
            
            var userData = repo.All().Where(p => p.帳號 == 帳號).FirstOrDefault();
            if (TryUpdateModel<客戶資料>(userData, new string[] { "密碼", "電話", "傳真", "地址", "Email" }))
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
