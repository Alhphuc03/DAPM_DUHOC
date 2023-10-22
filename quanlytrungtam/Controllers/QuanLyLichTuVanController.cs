    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using quanlytrungtam.Models;
    using System.IO;
    using System.Net;

    namespace quanlytrungtam.Controllers
    {
        public class QuanLyLichTuVanController : Controller
        {
            QUANLYTRUNGTAMDUHOCEntities db = new QUANLYTRUNGTAMDUHOCEntities();
            // GET: QuanLyLichTuVan
            public int pageSize = 5;
            public ActionResult Index(int? page,int? MANV)
            {
                if (TempData["result"] != null)
                {
                    ViewBag.SuccessMsg = TempData["result"];
                }

                if (page > 0)
                {
                    page = page;
                }
                else
                {
                    page = 1;
                }
                int start = (int)(page - 1) * pageSize;

                ViewBag.pageCurrent = page;
                int totalPage = db.LICHHENTUVANs.Count();
                float totalNumsize = (totalPage / (float)pageSize);
                int numSize = (int)Math.Ceiling(totalNumsize);
                ViewBag.numSize = numSize;
               /* if (MANV == 0)
                {
                    return View(db.LICHHENTUVANs.Where(n => n.MANV == null).OrderByDescending(x => x.MALH).Skip(start).Take(pageSize));
                }
                else if (MANV==1)
                {
                    return View(db.LICHHENTUVANs.Where(n => n.MANV != null).OrderByDescending(x => x.MALH).Skip(start).Take(pageSize));
                }*/

                return View(db.LICHHENTUVANs.OrderByDescending(x => x.MALH).Skip(start).Take(pageSize));
            }
            public ActionResult duyetlich(int? MANV)
            {
                if (MANV == 0)
                {
                    return View(db.LICHHENTUVANs.Where(n => n.MANV == null).OrderBy(n => n.MALH));
                }
                return View(db.LICHHENTUVANs.Where(n=>n.MANV!=null).OrderBy(n => n.MALH));
            }
            [HttpGet]
            public ActionResult xeplich (int? id)
            {
           
                if (id == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.MANV = new SelectList(db.NHANVIENs.OrderBy(n => n.MANV), "MANV", "HOTEN");
                LICHHENTUVAN lh = db.LICHHENTUVANs.SingleOrDefault(n => n.MALH == id);
                if (lh == null) return HttpNotFound();
           
           
                return View(lh);
            }
            [ValidateInput(false)]
            [HttpPost]
            public ActionResult xeplich(LICHHENTUVAN lh)
            {
                ViewBag.MANV = new SelectList(db.NHANVIENs.OrderBy(n => n.MANV), "MANV", "HOTEN");
                db.Entry(lh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["result"] = "Cập nhật nhân viên tư vấn thành công";
                return RedirectToAction("Index", "QuanLyLichTuVan");

            }
            public ActionResult xoalich(int? id)
            {

                if (id == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                ViewBag.MANV = new SelectList(db.NHANVIENs.OrderBy(n => n.MANV), "MANV", "HOTEN");
                LICHHENTUVAN lh = db.LICHHENTUVANs.SingleOrDefault(n => n.MALH == id);
                if (lh == null) return HttpNotFound();
                return View(lh);
            }
            [ValidateInput(false)]
            [HttpPost]
            public ActionResult xoalich(int id)
            {
                if (id == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                ViewBag.MANV = new SelectList(db.NHANVIENs.OrderBy(n => n.MANV), "MANV", "HOTEN");
                LICHHENTUVAN lh = db.LICHHENTUVANs.SingleOrDefault(n => n.MALH == id);
                if (lh == null) return HttpNotFound();
                db.LICHHENTUVANs.Remove(lh);
                db.SaveChanges();
                TempData["result"] = "Bạn đã xóa thành công !";
                return RedirectToAction("Index", "QuanLyLichTuVan");
            }



        [HttpPost]
        public ActionResult ChinhSuaTrangThai(int MALH, string TrangThai)
        {
            var lichHen = db.LICHHENTUVANs.Find(MALH);

            if (lichHen == null)
            {
                return HttpNotFound();
            }

            lichHen.TRANGTHAI = TrangThai;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}