using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TutorApplication.Models;

namespace TutorApplication.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        LoginAndRegisterDbEntities StdLogin = new LoginAndRegisterDbEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(logregTbl hp,string ReturnUr)
        {
            if (isValide(hp)==true)
            {
                FormsAuthentication.SetAuthCookie(hp.Name, false);
                Session["Name"]=hp.Name.ToString();
                if (ReturnUr != null)
                {
                    
                    Redirect(ReturnUr);
                }
                else
                {
                    return RedirectToAction("Teachers","Home");
                }
            }
            else 
            {
                return View(); 
            }
            return View();
        
        }
        public bool isValide(logregTbl hp)
        {
           var credential= StdLogin.logregTbls.Where(model=>model.Name==hp.Name && model.password==hp.password).FirstOrDefault();
            if (credential!=null)
            {
                return (hp.Name==credential.Name && hp.password== credential.password);
            }
            else { return false; }
        }

    }
}