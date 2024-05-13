using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TutorApplication.Models;

namespace TutorApplication.Controllers
{
    public class HomeController : Controller
    {
        CHATTERJEECDEntities1 dbMaster = new CHATTERJEECDEntities1();
        dbrrgisloginEntities1 db = new dbrrgisloginEntities1();
        LoginAndRegisterDbEntities StdLogin = new LoginAndRegisterDbEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var list = new List<string>() { "English", "bengali", "Math" };
            ViewBag.list = list;
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Contact_tbl con)
        {

            if (ModelState.IsValid == true)
            {


                dbMaster.Contact_tbl.Add(con);
                int a = dbMaster.SaveChanges();
                if (a > 0)
                {
                    Session["successMsg"] = "<script> alert('Submited Successfully') </script>";
                    ModelState.Clear();


                }
                else
                {
                    Session["errorMsg"] = "<script> alert('Something Wrong') </script>";
                    ModelState.Clear();

                }
            }
            var list = new List<string>() { "English", "bengali", "Math" };
            ViewBag.list = list;

            return View();
        }

        public ActionResult Subject
            ()
        {
            return View();
        }


        [Authorize]
        public ActionResult Teachers
            ()
        {
            int row = 5;
            Session["data"] = row;
           var data= dbMaster.Tutorregis.ToList().Take(row);
            return View(data);
        }

        [HttpPost]
        public ActionResult Teachers(string q)
        {
          

            if (string.IsNullOrEmpty(q)==false)
            {
                List<Tutorregi> tutor = dbMaster.Tutorregis.Where(model => model.Fullname.StartsWith(q) || model.degree.StartsWith(q)).ToList();
                return PartialView("_searchData",tutor);
            }
            else
            {
                List<Tutorregi> tutor = dbMaster.Tutorregis.ToList();
                return PartialView("_searchData", tutor);
            }
            //int row = Convert.ToInt32(Session["data"]) + 5;
            //var data = dbMaster.Tutorregis.ToList().Take(row);
            //Session["data"] = row;
            //return PartialView("_searchData", data);
        }
        public ActionResult Details
            (int id)
        {
            var data = dbMaster.Tutorregis.Where(model=>model.Id == id).FirstOrDefault();
            Session["Image"] = data.Image;
            return View(data);
        }

        public ActionResult Registration
          ()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(logregTbl hp)

        {
            if (ModelState.IsValid == true)
            {

                string fileName = Path.GetFileNameWithoutExtension(hp.ImgFile.FileName);
                string extentionName = Path.GetExtension(hp.ImgFile.FileName);
                HttpPostedFileBase postedFile = hp.ImgFile;
                int length = postedFile.ContentLength;
                if (extentionName.ToLower() == ".jpg" || extentionName.ToLower() == ".png")
                {
                    if (length <= 1000000)
                    {
                        fileName = fileName + extentionName;
                        hp.ImagePath = "~/ImageSub/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/ImageSub/"), fileName);
                        hp.ImgFile.SaveAs(fileName);

                        StdLogin.logregTbls.Add(hp);
                        int a = StdLogin.SaveChanges();
                        if (a > 0)
                        {
                            Session["suMsg"] = "<script> alert('registration successfully done') </script>";
                            ModelState.Clear();
                            return RedirectToAction("Teachers","Home");   
                            
                        }
                        else
                        {
                            Session["erMsg"] = "<script> alert('Something Wrong') </script>";
                            return RedirectToAction("Registration", "Home");

                        }

                    }
                    else
                    {
                        ViewBag.m_Length = "<script> alert('Length Not Support') </script>";
                    }
                }
                else
                {
                    ViewBag.m_extention = "<script> alert('Extention must be either png or jpg') </script>";
                }

            }

            return View();

        }
    }

}