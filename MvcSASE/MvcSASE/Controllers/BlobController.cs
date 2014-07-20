﻿using MvcSASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;


namespace MvcSASE.Controllers
{
    public class BlobController : Controller
    {
        private SASE s;
        private SASEDBContext db = new SASEDBContext();
        private string currentUser = System.Web.HttpContext.Current.User.Identity.Name;

        // GET: Blob
        public ActionResult Index(string containername, int? saseid, int blobid)
        {
            if (saseid == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == saseid select i).FirstOrDefault();
            s.passID = saseid;
            s.containerName = containername;
            s.blobID = blobid;

            if (blobid >= 0)
                s.blobInfo = s.service.SASEBlobInfo(containername, s.service.SASEBlobItemNames(containername)[blobid]);

            CheckLogin();

            return View(s);
        }
               
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string container, int? saseid)
        {
            if (saseid == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == saseid select i).FirstOrDefault();
            s.passID = saseid;
            s.containerName = container;
            CheckLogin();

            if (file != null && file.ContentLength > 0)
            {
                string name = file.FileName;
                //Byte[] convert = new Byte[file.ContentLength];
                file.InputStream.Position = 0;
                //file.InputStream.Read(convert, 0, file.ContentLength);

                //s.service.SASEUploadBlockBlobBytes(container, name, convert);
                s.service.SASEUploadBlockBlobStream(container, name, file.InputStream);
            }

            return RedirectToLocal("/Blob/Index?containername=" + s.containerName + "&saseid=" + s.passID + "&blobid=-1");
        }

        public ActionResult Download(string containername, int? saseid, int blobid)
        {
            if (saseid == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == saseid select i).FirstOrDefault();
            s.passID = saseid;
            s.containerName = containername;
            s.blobID = -1;
            CheckLogin();

            string fileName = s.service.SASEBlobItemNames(containername)[blobid];
            byte[] file = s.service.SASEDownloadBlobBytes(containername, fileName);

            return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            //return RedirectToLocal("/Blob/Index?containername=" + s.containerName + "&saseid=" + s.passID + "&blobid=-1");
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