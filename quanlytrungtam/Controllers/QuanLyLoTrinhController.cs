using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using quanlytrungtam.Models;
using System.IO;

namespace quanlytrungtam.Controllers
{
    public class QuanLyLoTrinhController : Controller
    {
        QUANLYTRUNGTAMDUHOCEntities db = new QUANLYTRUNGTAMDUHOCEntities();
        // GET: QuanLyLoTrinh
        public ActionResult Index()
        {
            return View(db.LOTRINHDUHOCs.OrderBy(n=>n.MALT).ToList());
        }
        [HttpGet]
        public ActionResult themLotrinh()
        {
         
            ViewBag.MATDH = new SelectList(db.TRUONGDAIHOCs.OrderBy(n => n.MATDH), "MATDH", "TENTDH");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themLotrinh(LOTRINHDUHOC ltdh)
        {
            // Check if ThoiGianHoc is less than 0
            if (ltdh.THOIGIANHOC < 0)
            {
                ModelState.AddModelError("THOIGIANHOC", "Thơi gian du học không được nhỏ hơn 0");
            }

            if (ModelState.IsValid)
            {
                // If the model is valid, proceed with adding it to the database
                ViewBag.MATDH = new SelectList(db.TRUONGDAIHOCs.OrderBy(n => n.MATDH), "MATDH", "TENTDH");
                db.LOTRINHDUHOCs.Add(ltdh);
                db.SaveChanges();
                return RedirectToAction("Index", "QuanLyLoTrinh");
            }

            // If ModelState is not valid, return to the view with validation errors
            ViewBag.MATDH = new SelectList(db.TRUONGDAIHOCs.OrderBy(n => n.MATDH), "MATDH", "TENTDH");
            return View(ltdh);
        }

        [HttpGet]
        public ActionResult editLotrinh(int? id)
        {

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MATDH = new SelectList(db.TRUONGDAIHOCs.OrderBy(n => n.MATDH), "MATDH", "TENTDH");
            LOTRINHDUHOC ltdh = db.LOTRINHDUHOCs.SingleOrDefault(n => n.MALT == id);
            if (ltdh == null) return HttpNotFound();
            return View(ltdh);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult editLotrinh(LOTRINHDUHOC ltdh)
        {
            ViewBag.MATDH = new SelectList(db.TRUONGDAIHOCs.OrderBy(n => n.MATDH), "MATDH", "TENTDH");
            db.Entry(ltdh).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "QuanLyLoTrinh");
        }
        [HttpGet]
        public ActionResult xoaLotrinh(int? id)
        {

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MATDH = new SelectList(db.TRUONGDAIHOCs.OrderBy(n => n.MATDH), "MATDH", "TENTDH");
            LOTRINHDUHOC ltdh = db.LOTRINHDUHOCs.SingleOrDefault(n => n.MALT == id);
            if (ltdh == null) return HttpNotFound();
            return View(ltdh);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult xoaLotrinh(int id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            LOTRINHDUHOC ltdh = db.LOTRINHDUHOCs.SingleOrDefault(n => n.MALT == id);
            if (ltdh == null) return HttpNotFound();
            db.LOTRINHDUHOCs.Remove(ltdh);
            db.SaveChanges();
            return RedirectToAction("Index", "QuanLyLoTrinh");
        }
        public ActionResult add(int id)
        {
            return View();
        }
     }
}