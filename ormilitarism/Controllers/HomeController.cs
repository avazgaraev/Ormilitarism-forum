using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using ormilitarism.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ormilitarism.Controllers
{

    public class HomeController : Controller
    {
        context c = new context();
        // GET: Home
        public ActionResult Index(string titlename, int? page, int say = 5)
        {
            //var key = c.customers.Where(x => x.postcount > 40 || x.likecount > 20).ToList();


            var value = from r in c.titles.Include("posts").Where(x=>x.sorguid==2).ToList().OrderByDescending(x => x.postcount) select r;


            //var Name = (from N in ObjList where N.titlename.Contains(p) select new { N.titlename });
            ViewBag.Message = TempData["Message"];

            return View(value.ToPagedList(page ?? 1, say));
        }
        public ActionResult reportpart()
        {
            var value = c.reports.ToList();
            return PartialView(value);
        }
        [Authorize]
        [HttpPost]
        public ActionResult likeinc(string id, like l, string likecount)
        {
            //session
            var mail = (string)Session["customername"];
            var values = c.customers.FirstOrDefault(x => x.customername == mail);
            if (values == null)
            {
                return View("Login");
            }
            else
            {
              
                var iid = int.Parse(id);
                ViewBag.custinfo = values.customerid;
                l.custname = values.customername;

                if (c.likes.Where(x => x.postid == iid).Count() == 0)
                {
                    l.postid = iid;
                    l.customerid = values.customerid;
                    c.likes.Add(l);
                    c.posts.Include("likes").FirstOrDefault(x => x.postid == iid).postlikecount++;
                    c.posts.First(x => x.postid == iid).Customer.likecount++;
                } 
                else if (c.likes.Where(x => x.postid == iid).Select(y=>y.customerid).Contains(values.customerid))
                {
                    l.likeid = c.likes.FirstOrDefault(x => x.postid == iid).likeid;
                    var del = c.likes.Find(l.likeid);
                    c.likes.Remove(del);
                    c.posts.Include("likes").FirstOrDefault(x => x.postid == iid).postlikecount--;
                    c.posts.First(x => x.postid == iid).Customer.likecount--;
                }
                else if (c.likes.Where(x => x.postid == iid).Select(y => y.customerid).Contains(values.customerid)==false)
                {
                    l.postid = iid;
                    l.customerid = values.customerid;
                    c.likes.Add(l);
                    c.posts.Include("likes").FirstOrDefault(x => x.postid == iid).postlikecount++;
                    c.posts.First(x => x.postid == iid).Customer.likecount++;

                }
                c.customers.Include("likes").Include("posts");
            c.SaveChanges();
            likecount = c.posts.FirstOrDefault(x => x.postid == iid).postlikecount.ToString();

            return Content(likecount);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult favinc(string id, favorit l, string favcount)
        {
            //session
            var mail = (string)Session["customername"];
            var values = c.customers.FirstOrDefault(x => x.customername == mail);
            if (values == null)
            {
                return View("Login");
            }
            else
            {

                var iid = int.Parse(id);
                ViewBag.custinfo = values.customerid;
                l.custname = values.customername;
                l.favdate = DateTime.Now;

                if (c.favorits.Where(x => x.postid == iid).Count() == 0)
                {
                    l.postid = iid;
                    l.custid = values.customerid;
                    c.favorits.Add(l);
                    c.posts.Include("favorits").FirstOrDefault(x => x.postid == iid).postfavcount++;
                    c.posts.First(x => x.postid == iid).Customer.favcount++;
                }
                else if (c.favorits.Where(x => x.postid == iid).Select(y => y.custid).Contains(values.customerid))
                {
                    l.favid = c.favorits.FirstOrDefault(x => x.postid == iid).favid;
                    var del = c.favorits.Find(l.favid);
                    c.favorits.Remove(del);
                    c.posts.Include("favorits").FirstOrDefault(x => x.postid == iid).postfavcount--;
                    c.posts.First(x => x.postid == iid).Customer.favcount--;
                }
                else if (c.favorits.Where(x => x.postid == iid).Select(y => y.custid).Contains(values.customerid) == false)
                {
                    l.postid = iid;
                    l.custid = values.customerid;
                    c.favorits.Add(l);
                    c.posts.Include("favorits").FirstOrDefault(x => x.postid == iid).postfavcount++;
                    c.posts.First(x => x.postid == iid).Customer.favcount++;

                }
                c.customers.Include("favorits").Include("posts");
                
                c.SaveChanges();
                favcount = c.posts.FirstOrDefault(x => x.postid == iid).postfavcount.ToString();
                return Content(favcount);
            }
        }
        [Authorize]
        public ActionResult favview(string sortBY, string p, int? page, string filter, int say = 5)
        {
            var mail = (string)Session["customername"];
            var values = c.customers.FirstOrDefault(x => x.customername == mail);
            var favlar = from r in c.favorits.Where(x => x.custid == values.customerid).ToList() select r;
            //var favlar = c.favorits.Where(x => x.custid == values.customerid).Select(y => y.Post.Title.titleid);
            //var son = c.titles.Contains(favlar);

            if (!string.IsNullOrEmpty(p))
            {
                //if (filter == "basliq")
                //{
                //    if (sortBY == "popular")
                //    {
                //        favlar = favlar.Where(y => y.Post.Title.titlename.Contains(p)).OrderByDescending(x => x.Post.Title.postcount) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqalebaxissayi)*/;
                //    }
                //    else
                //    {
                //        favlar = favlar.Where(y => y.titlename.Contains(p)).OrderByDescending(x => x.titleregister) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqaletarix)*/;
                //    }

                //}
                //if (filter == "postlar")
                //{
                //    foreach (var item in favlars.Posts)
                //    {

                //        if (sortBY == "popular")
                //        {
                //            favlar = favlar.Where(y => item.postmezmun.Contains(p)).OrderByDescending(x => x.postcount) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqalebaxissayi)*/;
                //        }
                //        else
                //        {
                //            favlar = favlar.Where(y => item.postmezmun.Contains(p)).OrderByDescending(x => x.titleregister) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqaletarix)*/;
                //        }
                //    }

                //}

                if (sortBY == "popular")
                {
                    favlar = favlar.Where(y => y.Post.Title.titlename.Contains(p)).OrderByDescending(x => x.Post.Title.postcount) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqalebaxissayi)*/;
                }
                else
                {
                    favlar = favlar.Where(y => y.Post.Title.titlename.Contains(p)).OrderByDescending(x => x.favdate) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqaletarix)*/;
                }

            }
            else
            {
              
                    if (sortBY == "popular")
                    {
                        favlar = favlar.OrderByDescending(x => x.Post.postmezmun) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqalebaxissayi)*/;
                    }
                    else
                    {
                        favlar = favlar.OrderByDescending(x => x.Post.posttime) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqaletarix)*/;
                    }
            }
            return View(favlar.ToPagedList(page ?? 1, say));
        }

        public PartialViewResult pagedview()
        {
            return PartialView();
        }

        public ActionResult searchview(string titlename, int? page, int say = 5)
        {
            var value = from r in c.titles.Include("posts").ToList().OrderByDescending(x => x.postcount) select r;


            //var Name = (from N in ObjList where N.titlename.Contains(p) select new { N.titlename });
            if (!string.IsNullOrEmpty(titlename))
            {
                value = value.Where(y => y.titlename.Contains(titlename));

            }
            return View(value.ToPagedList(page ?? 1, say));
        }

        public PartialViewResult daily()
        {
            var value = c.titles.Where(x => x.titleregister.Day == DateTime.Now.Day && x.sorguid==2).OrderByDescending(x=>x.titleregister).ToList();
            return PartialView( value);
        }
        [HttpPost]
        public ActionResult postdaily()
        {
            var value = c.titles.Where(x => x.titleregister.Day == DateTime.Now.Day).OrderByDescending(x=>x.titleregister).ToList();
            return PartialView(value);
            //return View(value);
        }

        public ActionResult GetData() //that's if you need the model
        {
            //do whatever
            var value = c.titles.Where(x => x.titleregister.Day == DateTime.Now.Day).OrderByDescending(x => x.titleregister).ToList();
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult dailyview(int? page, int say = 5)
        {
            var value = c.titles.Where(x => x.titleregister.Day == DateTime.Now.Day && x.sorguid == 2).Include("posts").OrderByDescending(x => x.titleregister).ToList();
            return View(value.ToPagedList(page ?? 1, say));
        }

        public PartialViewResult partnav()
        {
            return PartialView();
        }

        public JsonResult jsonres(string p)  
        {  
            List<title> ObjList = new List<title>();  
            var Name = (from N in ObjList where N.titlename.Contains(p) select new { N.titlename});  
            return Json(p, JsonRequestBehavior.AllowGet);  
        } 
        

        public ActionResult Register()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Register(customer b, string customerimg)
        {
            if (!ModelState.IsValid)
            {
                
                return View();
            }
            if (c.customers.FirstOrDefault(x => x.customername == b.customername) != null)
            {
                ViewBag.message = "Bu adlı istifaəçi var";
                return View();
            }

            if (customerimg !=null)
            {
                string filename = Path.GetFileName(Request.Files[0].FileName);
                string ex = Path.GetExtension(Request.Files[0].FileName);
                string path = "~/Content/img/pp/" + filename + ex;
                Request.Files[0].SaveAs(Server.MapPath(path));
                b.customerimg= "/Content/img/pp/" + filename + ex;
            }
            else if(customerimg is null)
            {
                b.customerimg= "/Content/img/pp/guest-user.jpg";
                
            }
            b.registertime =DateTime.Now;
            b.statusid = 1;
            c.customers.Add(b);
            c.SaveChanges();
            return RedirectToAction("Login");
        }
       
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string customername, string customerpass)
        {
            var values = c.customers.FirstOrDefault(x => x.customername == customername && x.customerpass==customerpass);
            //if (value == customerpass)
            //{
            //    HttpCookie cookie = new HttpCookie("users");
            //    cookie.Values.Add("islogin", "true");
            //    Response.Cookies.Add(cookie);
            //    return RedirectToAction("Index");

            //}
            if (values!=null)
            {

                FormsAuthentication.SetAuthCookie(values.customername, false);
                Session["customername"] = values.customername.ToString();
                return RedirectToAction("Index", "home");

            }
            else
            {
                ViewBag.message = "ozunu tanimirsan xiyar ?";
                return View();

            }
        }

        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("index");
        }
        [Authorize]
        public ActionResult newSinglePost()
        {
            return View();
        }
        [HttpPost]
        public ActionResult newSinglePost(title t, post p, postimgs pi, string imglar)
        {
            var mail = (string)Session["customername"];
            var values = c.customers.FirstOrDefault(x => x.customername == mail);

            
            t.titleregister = DateTime.Now;
            t.customerid = values.customerid;
             //values.customerid= t.customer.customerid;
            t.titleid = p.Titleid;
            t.postcount += 1;

            
            p.posttime= DateTime.Now;
            p.Customerid = values.customerid;
            //p.Customer.customerid= values.customerid;
            t.sorguid = 1;

            c.titles.Add(t);
            //values.titlecount += 1;
            c.posts.Add(p);
            //values.postcount += 1;

            if (values.statusid ==c.statuses.OrderByDescending(x=>x.statusid).First().statusid)
            {

            }
            else if (values.postcount > Convert.ToInt32(values.Status.postcount))
            {
                values.statusid += 1;
                TempData["shortMessage"] = (
                    "Tebrikler", "Siz artiq" + values.Status.statusad + "vezifesindesiniz");
            }
            


            c.SaveChanges();

            if (imglar!= null && Request.Files.Count==1)
            {


                string filename = Path.GetFileName(Request.Files[0].FileName);
                string ex = Path.GetExtension(Request.Files[0].FileName);
                string path = "~/Content/img/postimg/" + filename + ex;
                Request.Files[0].SaveAs(Server.MapPath(path));
                pi.imgname = "/Content/img/postimg/" + filename + ex;

                //var InputFileName = Path.GetFileName(file.FileName);
                //var ServerSavePath = Path.Combine(Server.MapPath("~/Content/img/postimg/") + InputFileName);
                ////Save file to server folder  
                //file.SaveAs(ServerSavePath);
                ////assigning file uploaded status to ViewBag for showing message to user.  
                //ViewBag.UploadStatus = imgname.Count().ToString() + " files uploaded successfully.";
                //pi.imgurl = ServerSavePath;
                //string filename = Path.GetFileName(Request.Files[0].FileName);
                //string ex = Path.GetExtension(Request.Files[0].FileName);
                //string path = "~/Content/img/postimg/" + filename + ex;
                //Request.Files[0].SaveAs(Server.MapPath(path));
                //pi.imgname = "/Content/img/postimg/" + filename + ex;
                    pi.postid = p.postid;
                c.postimgs.Add(pi);
                c.SaveChanges();
            }
            else if (Request.Files.Count >1)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    string filename = Path.GetFileName(Request.Files[i].FileName);
                    string ex = Path.GetExtension(Request.Files[i].FileName);
                    string path = "~/Content/img/postimg/" + filename + ex;
                    Request.Files[i].SaveAs(Server.MapPath(path));
                    pi.imgname = "/Content/img/postimg/" + filename + ex;
                    pi.postid = p.postid;
                    c.postimgs.Add(pi);
                    c.SaveChanges();
                }
            }
            else
            {

            }



            return RedirectToAction("myprofile", "customer");
        }

        public PartialViewResult mostpop()
        {
            var value = c.titles.OrderByDescending(x=>x.postcount).Take(12).ToList();
            return PartialView(value);
        }
        public ActionResult mostpopview(int? page, int say = 5)
        {
            var value = c.titles.OrderByDescending(x => x.postcount).ToList();
            return PartialView(value.ToPagedList(page ?? 1, say));
        }


        public ActionResult onetopic(int id, int? page, int say = 5)
        {
            var value = c.posts.Where(x => x.Titleid == id).ToList();
            //TempData["message"] = ViewBag.message;
            return View(value.ToPagedList(page ?? 1, say));
        }
        [Authorize]
        [HttpPost]
        public ActionResult onetopic(int id, post p, postimgs pi, string imglar)
        {
            var mail = (string)Session["customername"];
            var values = c.customers.FirstOrDefault(x => x.customername == mail);
            var basliq = c.titles.First(x => x.titleid == id);

            basliq.postcount += 1;

            p.Titleid = id;

            p.posttime = DateTime.Now;
            p.Customerid = values.customerid;


            c.posts.Add(p);
            values.postcount += 1;

            if (values.statusid == c.statuses.OrderByDescending(x => x.statusid).First().statusid)
            {

            }
            else if (values.postcount > Convert.ToInt32(values.Status.postcount))
            {
                values.statusid += 1;
                TempData["message"] = "Yazdiginiz post saylari " + values.Status.postcount + "-i otudyunden " + c.statuses.FirstOrDefault(x=>x.statusid==values.statusid).statusad + " vezifesindesiniz.";
                //ViewBag.message = "Tebrikler Siz artiq vezifesindesiniz";
            }
            c.SaveChanges();

            if (imglar != null && Request.Files.Count == 1)
            {


                string filename = Path.GetFileName(Request.Files[0].FileName);
                string ex = Path.GetExtension(Request.Files[0].FileName);
                string path = "~/Content/img/postimg/" + filename + ex;
                Request.Files[0].SaveAs(Server.MapPath(path));
                pi.imgname = "/Content/img/postimg/" + filename + ex;

                    pi.postid = p.postid;
                c.postimgs.Add(pi);
                c.SaveChanges();
            }
            else if (Request.Files.Count > 1)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    string filename = Path.GetFileName(Request.Files[i].FileName);
                    string ex = Path.GetExtension(Request.Files[i].FileName);
                    string path = "~/Content/img/postimg/" + filename + ex;
                    Request.Files[i].SaveAs(Server.MapPath(path));
                    pi.imgname = "/Content/img/postimg/" + filename + ex;
                    pi.postid = p.postid;
                    c.postimgs.Add(pi);
                    c.SaveChanges();
                }
            }
            else
            {

            }
            return RedirectToAction("onetopic", "home");
        }
    }
}