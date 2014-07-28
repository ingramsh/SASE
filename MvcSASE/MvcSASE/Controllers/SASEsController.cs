using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcSASE.Models;
using Microsoft.AspNet.Identity;
using SASELibrary;

namespace MvcSASE.Controllers
{
    public class SASEsController : Controller
    {
        private DBContext db = new DBContext();
        string currentUser = System.Web.HttpContext.Current.User.Identity.Name;

        // GET: SASEs
        public ActionResult Index()
        {
            if (currentUser == "" || currentUser == null)
                return RedirectToAction("Login", "Account");

            var userEntries = from m in db.Sase
                              select m;
            userEntries = userEntries.Where(s => s.userEmail.Contains(currentUser));
            return View(userEntries);
            //return View(db.Sase.ToList());
        }

        // GET: SASEs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzureAccountService sASE = db.Sase.Find(id);
            if (sASE == null)
            {
                return HttpNotFound();
            }
            return View(sASE);
        }

        // GET: SASEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SASEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,userEmail,storageAccount,storageKey")] AzureAccountService sASE)
        {
            if (ModelState.IsValid)
            {
                db.Sase.Add(sASE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sASE);
        }

        // GET: SASEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzureAccountService sASE = db.Sase.Find(id);
            if (sASE == null)
            {
                return HttpNotFound();
            }
            return View(sASE);
        }

        // POST: SASEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,userEmail,storageAccount,storageKey")] AzureAccountService sASE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sASE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sASE);
        }

        // GET: SASEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AzureAccountService sASE = db.Sase.Find(id);
            if (sASE == null)
            {
                return HttpNotFound();
            }
            return View(sASE);
        }

        // POST: SASEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AzureAccountService sASE = db.Sase.Find(id);
            db.Sase.Remove(sASE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
