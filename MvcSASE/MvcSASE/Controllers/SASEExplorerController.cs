using MvcSASE.Models;
using System.Linq;
using System.Web.Mvc;

namespace MvcSASE.Controllers
{
    public class SASEExplorerController : Controller
    {
        private SASE s;
        private SASEDBContext db = new SASEDBContext();
        private string currentUser = System.Web.HttpContext.Current.User.Identity.Name;

        // GET: SASEExplorer
        public ActionResult Index(int? ID)
        {
            if (ID == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == ID select i).FirstOrDefault();
            s.passID = ID;
            CheckLogin();

            return View(s);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateContainer(string container, int? saseid)
        {
            if (saseid == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == saseid select i).FirstOrDefault();
            s.passID = saseid;
            CheckLogin();
            
            if (s.service.CreateContainer(container))
                return RedirectToLocal("/SASEExplorer/Index/" + saseid);
            else
            {
                //TODO:  Container not created handling
                return RedirectToLocal("/SASEExplorer/InvalidCharacter/" + saseid);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateQueue(string queue, int? saseid)
        {
            if (saseid == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == saseid select i).FirstOrDefault();
            s.passID = saseid;
            CheckLogin();
            
            if (s.service.CreateQueue(queue))
                return RedirectToLocal("/SASEExplorer/Index/" + saseid);
            else
            {
                //TODO:  Container not created handling
                return RedirectToLocal("/SASEExplorer/InvalidCharacter/" + saseid);
            }
        }

        public ActionResult Queue(string queuename, int? saseid)
        {
            if (saseid == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == saseid select i).FirstOrDefault();
            s.passID = saseid;
            s.queueName = queuename;
            CheckLogin();

            return View(s);
        }

        [HttpPost]
        public ActionResult Dequeue(string queuename, int? saseid)
        {
            if (saseid == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == saseid select i).FirstOrDefault();
            s.passID = saseid;
            s.queueName = queuename;
            CheckLogin();

            s.service.DequeueMessage(queuename);

            return RedirectToLocal("/SASEExplorer/Queue?queuename=" + s.queueName + "&saseid=" + saseid);
        }

        [HttpPost]
        public ActionResult Enqueue(string message, string queuename, int? saseid)
        {
            CheckLogin();

            if (queuename == "sase-youtube-in")
            {
                s = (from i in db.Sase where i.ID == 2 select i).FirstOrDefault();
                s.service.EnqueueMessage("sase-youtube-id", saseid.ToString());
            }

            s = (from i in db.Sase where i.ID == saseid select i).FirstOrDefault();
            s.passID = saseid;
            s.queueName = queuename;
            CheckLogin();

            s.service.EnqueueMessage(queuename, message);

            return RedirectToLocal("/SASEExplorer/Queue?queuename=" + s.queueName + "&saseid=" + saseid);
        }

        public ActionResult WorkerDemo(int? ID)
        {
            {
                if (ID == null)
                    CheckLogin();

                s = (from i in db.Sase where i.ID == ID select i).FirstOrDefault();
                s.passID = ID;
                CheckLogin();

                return View(s);
            }
        }
                
        public ActionResult InvalidCharacter(int? ID)
        {
            if (ID == null)
                CheckLogin();

            s = (from i in db.Sase where i.ID == ID select i).FirstOrDefault();
            s.passID = ID;
            CheckLogin();

            return View(s);
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