using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyMVCTest04.Models;

namespace MyMVCTest04.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();

        public ActionResult ExcelFile()
        {
            return File(repo.GetExcelFile(), "application/vnd.ms-excel", "客戶聯絡人.xls");
        }

        // GET: 客戶聯絡人
        [CountExecutionTime()]
        public ActionResult Index(string position , string sortBy)
        {
            IQueryable<客戶聯絡人> data = null;
            if (!string.IsNullOrEmpty(position))
            {
                data = repo.All().Where(p => p.職稱.Contains(position));
                return View(data);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "職稱":    
                            data = repo.All().OrderBy(p => p.職稱);
                        break;
                    case "姓名":
                            data = repo.All().OrderBy(p => p.姓名);
                        break;
                    case "Email":
                            data = repo.All().OrderBy(p => p.Email);
                        break;
                }
                return View(data);
            }


            return View(repo.All());
        }
        [HttpPost]
        public ActionResult Index(IList<客戶聯絡人> data)
        {
                foreach (var contact in data)
                {
                    var m_data = repo.Find(contact.Id);
                    m_data.電話 = contact.電話;
                    m_data.手機 = contact.手機;
                    m_data.職稱 = contact.職稱;                    
                }
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
  
            客戶聯絡人 客戶聯絡人 = repo.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            var repoCust = RepositoryHelper.Get客戶資料Repository(repo.UnitOfWork);
            ViewBag.客戶Id = new SelectList(repoCust.All() , "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶聯絡人);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(RepositoryHelper.Get客戶資料Repository(repo.UnitOfWork).All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(RepositoryHelper.Get客戶資料Repository(repo.UnitOfWork).All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db = (客戶資料Entities)repo.UnitOfWork.Context;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(RepositoryHelper.Get客戶資料Repository(repo.UnitOfWork).All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            客戶聯絡人 客戶聯絡人 = repo.Find(id);
            客戶聯絡人.是否已刪除 = true;
            repo.UnitOfWork.Commit();

            return RedirectToAction("Index");
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
