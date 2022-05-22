using ormilitarism.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ormilitarism.Controllers
{
    public class customerController : Controller
    {
        context c = new context();
        
        // GET: customer
        [Authorize]
        public ActionResult myprofile(string sortBY, string p, int? page, string filter, int say = 5)
        {
            var mail = (string)Session["customername"];
            var values = c.customers.FirstOrDefault(x => x.customername == mail);


            var value = from r in c.titles.Include("posts").Where(x => x.customerid == values.customerid).ToList().OrderByDescending(x => x.titleregister) select r;
            if (!string.IsNullOrEmpty(p))
            {
                if (filter=="basliq")
                {
                    if (sortBY == "popular")
                    {
                        value = value.Where(y => y.titlename.Contains(p)).OrderByDescending(x => x.postcount) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqalebaxissayi)*/;
                    }
                    else 
                    {
                        value = value.Where(y => y.titlename.Contains(p)).OrderByDescending(x => x.titleregister) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqaletarix)*/;
                    }

                }
                if (filter== "postlar")
                {
                    foreach (var item in values.Posts)
                    {

                        if (sortBY == "popular")
                        {
                            value = value.Where(y => item.postmezmun.Contains(p)).OrderByDescending(x => x.postcount) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqalebaxissayi)*/;
                        }
                        else 
                        {
                            value = value.Where(y => item.postmezmun.Contains(p)).OrderByDescending(x => x.titleregister) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqaletarix)*/;
                        }
                    }

                }


             
            }
            else
            {
                foreach (var item in values.Posts)
                {

                    if (sortBY == "popular")
                    {
                        value = value.OrderByDescending(x => item.postmezmun) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqalebaxissayi)*/;
                    }
                    else
                    {
                        value = value.OrderByDescending(x => item.posttime) /*|| y.basliq.Contains(p)).OrderByDescending(x => x.meqaletarix)*/;
                    }
                }
            }
            return View(value.ToList().ToPagedList(page ?? 1, say));
        }

        //Bildirisler
        public ActionResult bildirisler()
        {
            var mail = (string)Session["customername"];
            var values = c.customers.FirstOrDefault(x => x.customername == mail);
            var value = c.reports.Where(x=>x.statusid==values.statusid || x.statusid==null).OrderByDescending(x=>x.reportdate).ToList();
            return View(value);
        }



        
        public PartialViewResult custpart()
        {
            var mail = (string)Session["customername"];
            var values = c.customers.FirstOrDefault(x => x.customername == mail);
            return PartialView(values);
        }

        public PartialViewResult postt(int id)
        {
            var value = c.posts.Where(x=>x.Customerid==id);
            return PartialView(value);
        }

        //Status
        public ActionResult status()
        {
            var mail = (string)Session["adminname"];
            var values = c.admins.FirstOrDefault(x => x.adminname == mail);
            if (values == null)
            {
                return RedirectToAction("login", "admin");
            }
            var value = c.statuses.ToList();
            return View(value);
        }

        public ActionResult statusadd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult statusadd(status s)
        {
            c.statuses.Add(s);
            c.SaveChanges();
            return RedirectToAction("status");
        }

        public ActionResult statusindex(int id)
        {
            var value = c.statuses.Find(id);
            return View(value);
        }
        [HttpPost]
        public ActionResult statusindex(int id, status s)
        {
            var value = c.statuses.Find(id);
            value.likecount = s.likecount;
            value.titlecount = s.titlecount;
            value.postcount = s.postcount;
            value.statusad = s.statusad;
            c.SaveChanges();
            return RedirectToAction("status");
        }

        public ActionResult statusdel(int id)
        {
            var value = c.statuses.Find(id);
            c.statuses.Remove(value);
            c.SaveChanges();
            return RedirectToAction("status");
        }

        public PartialViewResult custsearch()
        {

            return PartialView();
        }

    }
}