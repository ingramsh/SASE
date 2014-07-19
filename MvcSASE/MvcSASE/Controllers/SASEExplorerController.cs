﻿using MvcSASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSASE.Controllers
{
    public class SASEExplorerController : Controller
    {
        private SASE s;
        private SASEExplorer explorer;
        private SASEDBContext db = new SASEDBContext();
        private string currentUser = System.Web.HttpContext.Current.User.Identity.Name;

        // GET: SASEExplorer
        public ActionResult Index(int? ID)
        {
            if (ID == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == ID select i).FirstOrDefault();
            CheckLogin();
            
            explorer = new SASEExplorer(s.storageAccount, s.storageKey);

            return View(explorer);
        }
        [HttpGet]
        public ActionResult CreateContainer(int id)
        {
            return View(explorer);
        }
        
        public ActionResult CreateQueue()
        {
            return View(explorer);
        }
        public ActionResult Container(string ContainerName)
        {
            return View();
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

        private void CheckLogin()
        {
            if (s == null)
                RedirectToLocal("/Home");
            else if (s.userEmail != currentUser)
                RedirectToLocal("/Home");
        }
    }
}