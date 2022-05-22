using ormilitarism.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace ormilitarism.Controllers
{
    public class AdminController : Controller
    {
        context c = new context();
        // GET: Admin
        public ActionResult Index()
        {

            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);
            if (values == null)
            {
                return RedirectToAction("login");
            }
            return View();

        }


        //report
        public ActionResult reportview()
        {
            List<SelectListItem> val1 = (from x in c.statuses.ToList()
                                         select new SelectListItem
                                         {
                                             Text = x.statusad,
                                             Value = x.statusid.ToString()
                                         }).ToList();
            ViewBag.val1 = val1;
            return View();
        }
        [HttpPost]
        public ActionResult reportview(report r, bool? herkeseaciq)
        {
            r.reportdate = DateTime.Now;
            if (herkeseaciq ==true)
            {
                r.statusid = null; 
            }
            r.reportavailable = true;
            var dead = c.reports.Where(x => x.reportdeadline == DateTime.Now);
            foreach (var deaded in dead)
            {
                if (deaded.reportdeadline==DateTime.Now)
                {
                    deaded.reportavailable = false;
                }
            }
            c.reports.Add(r);
            c.SaveChanges();
            return RedirectToAction("reportdata");
        }
        public ActionResult reportdata()
        {
            var values = c.reports.ToList();
            return View(values);
        }


        //Esas cedveller
        public ActionResult customer()
        {
            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);
            if (values == null)
            {
                return RedirectToAction("login");
            }
            var list = c.customers.ToList();
            return View(list);
        }
        public ActionResult basliqlar()
        {
            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);
            if (values == null)
            {
                return RedirectToAction("login");
            }
            var list = c.titles.Include("customer").OrderByDescending(x=>x.titleregister).ToList();
            return View(list);
        }
        public ActionResult waitingroom()
        {
            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);
            if (values == null)
            {
                return RedirectToAction("login");
            }
            var list = c.titles.Include("customer").Where(x=>x.sorguid==1).OrderByDescending(x=>x.titleregister).ToList();
            return View(list);
        }
        public ActionResult postlar()
        {
            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);
            if (values == null)
            {
                return RedirectToAction("login");
            }
            var list = c.posts.ToList();
            return View(list);
        }
        public ActionResult postimglar()
        {
            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);
            if (values == null)
            {
                return RedirectToAction("login", "admin");
            }
            var list = c.postimgs.ToList();
            return View(list);
        }

        //Silmek
        public ActionResult delbasliq(FormCollection formCollection)
        {
            string[] ids = formCollection["titleid"].Split(new char[] { ',' });
            foreach (string idi in ids)
            {
                var employee = this.c.titles.Find(int.Parse(idi));
                this.c.titles.Remove(employee);
                c.customers.FirstOrDefault(x => x.customerid == employee.customerid).titlecount--;
                c.SaveChanges();
            }

            c.SaveChanges();
            return RedirectToAction("basliqlar");
        }
        public ActionResult delpost(FormCollection formCollection)
        {
            string[] ids = formCollection["postid"].Split(new char[] { ',' });
            foreach (string idi in ids)
            {
                var employee = this.c.posts.Find(int.Parse(idi));
                this.c.posts.Remove(employee);
                c.customers.FirstOrDefault(x => x.customerid == employee.Customerid).postcount--;
                c.SaveChanges();
            }

            c.SaveChanges();
            return RedirectToAction("postlar");
        }
        public ActionResult delsimppost(int id)
        {
            var b = c.posts.Find(id);
            this.c.posts.Remove(b);
            c.customers.FirstOrDefault(x => x.customerid == b.Customerid).postcount--;
            //c.titles.FirstOrDefault(x => x.titleid == b.Titleid).postcount--;

            c.SaveChanges();
            return RedirectToAction("postlar");
        }
        public ActionResult delreport(FormCollection formCollection)
        {
            string[] ids = formCollection["reportid"].Split(new char[] { ',' });
            foreach (string idi in ids)
            {
                var employee = this.c.reports.Find(int.Parse(idi));
                this.c.reports.Remove(employee);
            }

            c.SaveChanges();
            return RedirectToAction("basliqlar");
        }



        //ayri ayriliqda detalli sehifeler
        public ActionResult basliqdetal(int id)
        {
            var list = c.titles.First(x=>x.titleid==id);
            return View(list);
        }
        public ActionResult postdetal(int id)
        {
            var list = c.posts.FirstOrDefault(x => x.postid == id);
            return View(list);
        }
        public ActionResult likedetal(int id)
        {
            var list = c.likes.Where(x => x.Post.postid == id).ToList();
            return View(list);
        }
        public ActionResult favdetal(int id)
        {
            var list = c.favorits.Where(x => x.Post.postid == id).ToList();
            return View(list);
        }
        public ActionResult likedetalcus(int id)
        {
            var list = c.likes.Where(x => x.Post.Customerid == id).ToList();
            return View(list);
        }
        public ActionResult postdetalcus(int id)
        {
            var list = c.posts.Where(x => x.Customerid == id).ToList();
            return View(list);
        }
        public ActionResult basliqdetalcus(int id)
        {
            var list = c.titles.Where(x => x.customerid == id).ToList();
            return View(list);
        }


        //Kartlar
        public PartialViewResult titlesay()
        {
            var value = c.titles.Where(x => x.titleregister.Day == DateTime.Now.Day).ToList();
            return PartialView(value);
        }
        public PartialViewResult postsay()
        {
            var value = c.posts.Where(x => x.posttime.Day == DateTime.Now.Day).ToList();
            return PartialView(value);
        }
        public PartialViewResult custsay()
        {
            var value = c.customers.Where(x => x.registertime.Day == DateTime.Now.Day).ToList();
            return PartialView(value);
        }
        public PartialViewResult imgsay()
        {
            var imglar = c.posts.Include("postimgs").Where(x => x.posttime.Day == DateTime.Now.Day).ToList();
            return PartialView(imglar); 
        }

        //chartlar
        public PartialViewResult statuspart()
        {

            var value = c.statuses.ToList();
            return PartialView(value);

        }
        public PartialViewResult monthtitle()
        {

            var value = c.statuses.ToList();
            return PartialView(value);

        }
        public ActionResult visual()
        {
            return Json(monthlist(), JsonRequestBehavior.AllowGet);
        }
        public List<statuschart> monthlist()
        {
            List<statuschart> list = new List<statuschart>();
            list.Add(new statuschart()
            {
                month = "yanvar",
                say = 50
            });
            list.Add(new statuschart()
            {
                month = "fevral",
                say = 30
            });
            list.Add(new statuschart()
            {
                month = "mart",
                say = 40
            });
            list.Add(new statuschart()
            {
                month = "aprel",
                say = 46
            });
            list.Add(new statuschart()
            {
                month = "may",
                say = 59
            });
            list.Add(new statuschart()
            {
                month = "iyun",
                say = 12
            });
            return list;

        }





        //public ActionResult UserPerMonth()
        //{

        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();

        //    var results = (from d in c.titles select d);

        //    var axis = results.Select(r => new statuschart
        //            {
        //                month = r.Key,
        //                say = r.Count()
        //            }).ToList();

        //    foreach (var item in axis)
        //    {
        //        xValue.Add(item.month);
        //        yValue.Add(item.say);
        //    }


        //    var chart = new Chart(width: 300, height: 200)
        //        .AddTitle("Titles per month")
        //        .AddLegend()
        //        .AddSeries(
        //            chartType: "Column",
        //            xValue: xValue,
        //            yValues: yValue)
        //        .GetBytes("png");
        //    return File(chart, "image/png");
        //}





        //mesajlar
        public PartialViewResult basliqadlar()
        {
            var value = c.titles.Where(x=>x.sorguid==1).ToList();
            return PartialView(value);
        }
        public PartialViewResult adminpart()
        {
            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);
            var son = c.admins.Find(values.adminid);
            return PartialView(son);
        }


        //Admin login isleri
        public ActionResult register()
        {
            return View();

        }
        [HttpPost]
        public ActionResult register(admin b, string adminimg)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }
            if (c.admins.FirstOrDefault(x => x.adminname == b.adminname) != null)
            {
                ViewBag.message = "Bu adlı admin var";
                return View();
            }

            if (adminimg != null)
            {
                string filename = Path.GetFileName(Request.Files[0].FileName);
                string ex = Path.GetExtension(Request.Files[0].FileName);
                string path = "~/Content/img/adminpp/" + filename + ex;
                Request.Files[0].SaveAs(Server.MapPath(path));
                b.adminimg = "/Content/img/adminpp/" + filename + ex;
            }
            else if (adminimg is null)
            {
                b.adminimg = "/Content/img/pp/guest-user.jpg";

            }
            b.adminregister = DateTime.Now;
            c.admins.Add(b);
            c.SaveChanges();
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string adminname, string adminpass)
        {
            var values = c.admins.FirstOrDefault(x => x.adminname== adminname && x.adminpass == adminpass);
            //if (value == customerpass)
            //{
            //    HttpCookie cookie = new HttpCookie("users");
            //    cookie.Values.Add("islogin", "true");
            //    Response.Cookies.Add(cookie);
            //    return RedirectToAction("Index");

            //}
            if (values != null)
            {

                FormsAuthentication.SetAuthCookie(values.adminname, false);
                Session["adminname"] = values.adminname.ToString();
                return RedirectToAction("Index", "admin");

            }
            else
            {
                ViewBag.message = "ozunu tanimirsan  ?";
                return View();

            }
        }

        public ActionResult LogOff()
        {
            Session["adminname"] = string.Empty;
            return RedirectToAction("login");
        }


        public ActionResult settings()
        {
            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);
            if (values == null)
            {
                return RedirectToAction("login");
            }

            var value = c.admins.Find(values.adminid);
            return View(value);
        }
        [HttpPost]
        public ActionResult settings(string adminpss, admin a, string adminimg)
        {
            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);

            var value = c.admins.Find(values.adminid);
            if (adminpss == value.adminpass)
            {
                value.adminname = a.adminname;
                if (a.adminpass != null)
                {
                    value.adminpass= a.adminpass;
                    value.confirmpass = a.adminpass;

                }
                if (adminimg != null)
                {
                    string filename = Path.GetFileName(Request.Files[0].FileName);
                    string ex = Path.GetExtension(Request.Files[0].FileName);
                    string path = "~/Content/img/adminpp/" + filename + ex;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    value.adminimg = "/Content/img/adminpp/" + filename + ex;
                }
                c.SaveChanges();
                return View(value);
            }
            else
            {
                ViewBag.mesparol = "Parol duzgun daxil et";
                return View();
            }
        }


        //basliqlarin duymeleri 
        public ActionResult success(int id)
        {
            c.titles.FirstOrDefault(x => x.titleid == id).sorguid = 2;
            c.titles.FirstOrDefault(x => x.titleid == id).titleregister = DateTime.Now;
            //c.titles.FirstOrDefault(x => x.titleid == id).Customer.postcount++;
            c.titles.FirstOrDefault(x => x.titleid == id).Customer.titlecount++;
            c.SaveChanges();
            return RedirectToAction("basliqlar");
        }
        public ActionResult danger(int id)
        {
            c.titles.FirstOrDefault(x => x.titleid == id).sorguid = 3;
            c.SaveChanges();
            return RedirectToAction("basliqlar");
        }
    }
}