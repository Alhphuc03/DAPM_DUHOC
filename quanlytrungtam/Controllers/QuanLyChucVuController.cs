using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using quanlytrungtam.Models;

namespace quanlytrungtam.Controllers
{
    public class QuanLyChucVuController : Controller
    {
        QUANLYTRUNGTAMDUHOCEntities db = new QUANLYTRUNGTAMDUHOCEntities();
        // GET: QuanLyChucVu
        public ActionResult Index()
        {
            return View(db.CHUCVUs.OrderBy(n=>n.MACV));
        }
        [HttpGet]
        public ActionResult ThemChucvu()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemChucvu(CHUCVU cv)
        {

            db.CHUCVUs.Add(cv);
            db.SaveChanges();
            return RedirectToAction("Index", "QuanLyChucVu");
        }

        public ActionResult editChucvu(int? id)
        {

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            CHUCVU cv = db.CHUCVUs.SingleOrDefault(n => n.MACV == id);
            if (cv == null) return HttpNotFound();
            return View(cv);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult editChucvu(CHUCVU cv)
        {

            db.Entry(cv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "QuanLyChucVu");
        }
        public ActionResult xoaChucvu(int? id)
        {

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            CHUCVU cv = db.CHUCVUs.SingleOrDefault(n => n.MACV == id);
            if (cv == null) return HttpNotFound();
            return View(cv);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult xoaChucvu(int id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            CHUCVU cv = db.CHUCVUs.SingleOrDefault(n => n.MACV == id);
            if (cv == null) return HttpNotFound();
            db.CHUCVUs.Remove(cv);
            db.SaveChanges();
            return RedirectToAction("Index", "QuanLyChucVu");
        }
    }
}