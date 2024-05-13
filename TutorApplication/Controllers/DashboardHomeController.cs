using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TutorApplication.Models;


namespace TutorApplication.Controllers
{
    public class DashboardHomeController : Controller
    {
        // GET: DashboardHome
        dbrrgisloginEntities1 db =new dbrrgisloginEntities1();
        CHATTERJEECDEntities1 dbMaster=new CHATTERJEECDEntities1();
        LoginAndRegisterDbEntities StdDb=new LoginAndRegisterDbEntities();

        public ActionResult Index()
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("Login", "DashboardHome");
            }
            else
            {
               return View();
            }
            
        }
        public ActionResult Login()
        {
            HttpCookie cookie = Request.Cookies["MyAdminCoockie"];
            if (cookie != null)
            {

                Session["userName"] = cookie["userName"].ToString();
                Session["Password"] = cookie["Password"].ToString();

            }
            return View();
        }
        public ActionResult Logout()
        {
            if(Session["userName"] != null)
            {
                Session.Abandon();
                return RedirectToAction("Login", "DashboardHome");
            }
            return View();
            
           
        }
        [HttpPost]
        public ActionResult Login(UserTbl user)
        {
            if(ModelState.IsValid==true)
            {
                
                  var row= db.UserTbls.Where(model=>model.Username==user.Username && model.Password==user.Password).FirstOrDefault();
                HttpCookie coockie = new HttpCookie("MyAdminCoockie");
                if (user.Rememberme==true)
                {
                   
                    coockie["userName"] = row.Username;
                    coockie["Password"] = row.Password;
                    coockie.Expires = DateTime.Now.AddDays(2);
                    HttpContext.Response.Cookies.Add(coockie);

                }
                else
                {
                    coockie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Response.Cookies.Add(coockie);
                }

                if (row!=null)
                {
                    Session["userName"] = row.Username.ToString();
                    Session["Password"] = row.Username.ToString();
                    Session["suMsg"]= "<script> alert('Login Successfull') </script>";
                    return RedirectToAction("Index", "DashboardHome");
                }
                else {
                    Session["erMsg"] = "<script> alert('Incorrect credentials') </script>";
                    return View(); 
                }
                
            }
            return View();
        }

        public ActionResult AddTeacher()
        {

            if (Session["userName"] == null)
            {
                return RedirectToAction("Login", "DashboardHome");
            }
            else
            {
               
                return View();
            }
           
            
          
        }
        [HttpPost]
        public ActionResult AddTeacher(Tutorregi tutor)
        {
            


            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(tutor.Img_File.FileName);
                string extensionName = Path.GetExtension(tutor.Img_File.FileName);
                HttpPostedFileBase postedFile = tutor.Img_File;
                int length = postedFile.ContentLength;

                if (extensionName.ToLower() == ".jpg" || extensionName.ToLower() == ".png")
                {
                    if (length <= 1000000)
                    {
                        fileName = fileName + extensionName;

                        tutor.Image = "~/ImageSub/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/ImageSub/"), fileName);
                        tutor.Img_File.SaveAs(fileName);
                        dbMaster.Tutorregis.Add(tutor);
                        int a = dbMaster.SaveChanges();
                        if (a > 0)
                        {
                            Session["taddMsg"] = "<script> alert('Teacher Added Successfully') </script>";
                            ModelState.Clear();
                            return View();
                        }
                        else
                        {
                            Session["terMsg"] = "<script> alert('Something wrong ') </script>";
                            return View();
                        }

                    }
                    else
                    {
                        ViewBag.lengthMessage = "<script> alert('length Not Support') </script>";
                    }
                }
                else
                {
                    ViewBag.ExtentionMessage = "<script> alert('Extention Not Support') </script>";
                }
               

            }
           
                return View();
            
          
          
        }

        public ActionResult StdTbl()
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("Login", "DashboardHome");
            }
            else
            {

                var data = StdDb.logregTbls.ToList();
                return View(data);
            }
           
        }

        public ActionResult Details(int id)
        { 
            var row= StdDb.logregTbls.Where(model=>model.Id == id).FirstOrDefault();
            Session["StdImage"] = row.ImagePath;
            return View(row);
        }
        public ActionResult Edit(int id)
        {
            var row = StdDb.logregTbls.Where(model => model.Id == id).FirstOrDefault();
            Session["StdImage"] = row.ImagePath;
            return View(row);
        }
        [HttpPost]
        public ActionResult Edit(logregTbl hp)
        {
            if (hp != null)
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

                            StdDb.Entry(hp).State = EntityState.Modified;
                            int a = StdDb.SaveChanges();
                            if (a > 0)
                            {
                                Session["StdsuMsg"] = "<script> alert('Updated successfully ') </script>";
                                ModelState.Clear();
                                return RedirectToAction("stdTbl", "DashboardHome");

                            }
                            else
                            {
                                Session["StderMsg"] = "<script> alert('Something Wrong') </script>";
                                return RedirectToAction("Edit", "DashboardHome");

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
            } else {
                Session["StdImage"] = hp.ImagePath.ToString();
                StdDb.Entry(hp).State = EntityState.Modified;
                int a = StdDb.SaveChanges();
                if (a > 0)
                {
                    Session["suMsg"] = "<script> alert('Updated successfully ') </script>";
                    ModelState.Clear();
                    return RedirectToAction("stdTbl", "DashboardHome");

                }
                else
                {
                    Session["erMsg"] = "<script> alert('Something Wrong') </script>";
                    return RedirectToAction("Edit", "DashboardHome");

                }
            }


            return View();
        }
            public ActionResult TeacherTbl()
        {
            return View();
        }
           
    }
}